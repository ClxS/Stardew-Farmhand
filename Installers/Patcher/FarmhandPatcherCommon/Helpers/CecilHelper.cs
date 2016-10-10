using Mono.Cecil;
using Mono.Cecil.Cil;
using Farmhand.Cecil;
using System.Collections.Generic;
using System.Linq;
using System;
using Mono.Cecil.Rocks;

namespace Farmhand.Helpers
{
    public static class CecilHelper
    {
        private static void InjectMethod<TParam, TThis, TInput, TLocal>(CecilContext stardewContext, ILProcessor ilProcessor, Instruction target, MethodReference method, bool isExit = false, bool cancelable = false)
        {
            ilProcessor.Body.SimplifyMacros();
            
            var callEnterInstruction = ilProcessor.Create(OpCodes.Call, method);

            if (method.HasThis)
            {
                var loadObjInstruction = ilProcessor.Create(OpCodes.Ldarg_0);
                ilProcessor.InsertBefore(target, loadObjInstruction);
            }

            if (method.HasParameters)
            {
                Instruction prevInstruction = null;
                var paramLdInstruction = target;
                var first = true;
                foreach (var parameter in method.Parameters)
                {
                    paramLdInstruction = GetInstructionForParameter<TParam, TThis, TInput, TLocal, Patcher>(stardewContext, ilProcessor, parameter);
                    if (paramLdInstruction == null) throw new Exception($"Error parsing parameter setup on {parameter.Name}");

                    if (isExit)
                    {
                        if (first)
                        {
                            first = false;
                            ilProcessor.Replace(target, paramLdInstruction);
                        }
                        else
                        {
                            ilProcessor.InsertAfter(prevInstruction, paramLdInstruction);
                        }
                        prevInstruction = paramLdInstruction;
                    }
                    else
                    {
                        ilProcessor.InsertBefore(target, paramLdInstruction);
                    }
                }

                if (isExit)
                {
                    if (first)
                    {
                        ilProcessor.Replace(target, callEnterInstruction);
                        ilProcessor.InsertAfter(callEnterInstruction, ilProcessor.Create(OpCodes.Ret));
                    }
                    else
                    {
                        ilProcessor.InsertAfter(prevInstruction, callEnterInstruction);
                        ilProcessor.InsertAfter(callEnterInstruction, ilProcessor.Create(OpCodes.Ret));
                    }
                }
                else
                {
                    ilProcessor.InsertAfter(paramLdInstruction, callEnterInstruction);
                }
            }
            else
            {
                if (isExit)
                {
                    ilProcessor.Replace(target, callEnterInstruction);
                    ilProcessor.InsertAfter(callEnterInstruction, ilProcessor.Create(OpCodes.Ret));
                }
                else
                {
                    ilProcessor.InsertBefore(target, callEnterInstruction);
                }
            }

            if (cancelable)
            {
                var branch = ilProcessor.Create(OpCodes.Brtrue, ilProcessor.Body.Instructions.Last());
                ilProcessor.InsertAfter(callEnterInstruction, branch);
            }

            ilProcessor.Body.OptimizeMacros();
        }

        private static void InjectReturnableMethod<TParam, TThis, TInput, TLocal, TUseOutput>(CecilContext stardewContext, ILProcessor ilProcessor, Instruction target, MethodReference method, bool isExit = false)
        {
            ilProcessor.Body.SimplifyMacros();

            // Add UseReturnVal variable
            if (ilProcessor.Body.Variables.All(n => n.Name != "UseReturnVal"))
            {
                var boolType = stardewContext.GetTypeReference(typeof(bool));
                ilProcessor.Body.Variables.Add(new VariableDefinition("UseReturnVal", boolType));
            }
            var useOutputVariable = ilProcessor.Body.Variables.First(n => n.Name == "UseReturnVal");

            if (ilProcessor.Body.Variables.All(n => n.Name != "ReturnContainer-" + method.ReturnType.Name))
            {
                ilProcessor.Body.Variables.Add(new VariableDefinition("ReturnContainer-" + method.ReturnType.Name, method.ReturnType));
            }
            if (isExit)
            {
                if (ilProcessor.Body.Variables.All(n => n.Name != "OldReturnContainer-" + method.ReturnType.Name))
                {
                    ilProcessor.Body.Variables.Add(new VariableDefinition("OldReturnContainer-" + method.ReturnType.Name, method.ReturnType));
                }
            }
            var containerVariable = ilProcessor.Body.Variables.First(n => n.Name == "ReturnContainer-" + method.ReturnType.Name);
            var oldContainerVariable = ilProcessor.Body.Variables.FirstOrDefault(n => n.Name == "OldReturnContainer-" + method.ReturnType.Name);
            
            List<Instruction> instructions = new List<Instruction>();
            if (isExit)
            {
                instructions.Add(ilProcessor.Create(OpCodes.Stloc, oldContainerVariable));
            }

            if (method.HasParameters)
            {
                foreach (var parameter in method.Parameters)
                {
                    var paramLdInstruction = GetInstructionForParameter<TParam, TThis, TInput, TLocal, TUseOutput>(stardewContext, ilProcessor, parameter);
                    if (paramLdInstruction == null) throw new Exception($"Error parsing parameter setup on {parameter.Name}");

                    instructions.Add(paramLdInstruction);
                }
            }
            
            instructions.Add(ilProcessor.Create(OpCodes.Call, method));
            instructions.Add(ilProcessor.Create(OpCodes.Stloc, containerVariable));
            instructions.Add(ilProcessor.Create(OpCodes.Ldloc, useOutputVariable));
            if (!isExit)
            {
                instructions.Add(ilProcessor.Create(OpCodes.Brfalse, target));
                instructions.Add(ilProcessor.Create(OpCodes.Ldloc, containerVariable));
                instructions.Add(ilProcessor.Create(OpCodes.Br, ilProcessor.Body.Instructions.Last()));
            }
            else
            {
                var brToRet = ilProcessor.Create(OpCodes.Br, target);
                var loadOld = ilProcessor.Create(OpCodes.Ldloc, oldContainerVariable);
                var loadNew = ilProcessor.Create(OpCodes.Ldloc, containerVariable);
                instructions.Add(ilProcessor.Create(OpCodes.Brfalse, loadOld));
                instructions.Add(loadNew);
                instructions.Add(brToRet);
                instructions.Add(loadOld);
            }

            foreach (var inst in instructions)
            {
                ilProcessor.InsertBefore(target, inst);
            }

            ilProcessor.Body.OptimizeMacros();
        }

        private static Instruction GetInstructionForParameter<TParam, TThis, TInput, TLocal, TUseOutput>(CecilContext stardewContext, ILProcessor ilProcessor, ParameterDefinition parameter)
        {
            if (!parameter.HasCustomAttributes) return null;

            var attribute = parameter.CustomAttributes.FirstOrDefault(n => n.AttributeType.IsDefinition && n.AttributeType.Resolve().BaseType?.FullName == typeof(TParam).FullName);

            if (attribute == null) return null;

            Instruction instruction = null;
            if (typeof(TThis).FullName == attribute.AttributeType.FullName)
            {
                instruction = ilProcessor.Create(OpCodes.Ldarg, ilProcessor.Body.ThisParameter);
            }
            else if (typeof(TInput).FullName == attribute.AttributeType.FullName)
            {
                var type = attribute.ConstructorArguments[0].Value as TypeReference;
                var name = attribute.ConstructorArguments[1].Value as string;

                var inputParam =
                    ilProcessor.Body.Method.Parameters.FirstOrDefault(
                        p => p.Name == name && p.ParameterType.FullName == type?.FullName);

                if (inputParam != null)
                {
                    instruction = ilProcessor.Create(OpCodes.Ldarg, inputParam);
                }
            }
            else if (typeof(TLocal).FullName == attribute.AttributeType.FullName)
            {
                var type = attribute.ConstructorArguments[0].Value as TypeReference;
                var index = attribute.ConstructorArguments[1].Value as int?;

                var inputParam =
                    ilProcessor.Body.Variables.FirstOrDefault(
                        p => p.Index == index && p.VariableType.FullName == type?.FullName);

                if (inputParam != null)
                {
                    instruction = ilProcessor.Create(OpCodes.Ldloc, inputParam);
                }
            }
            else if (typeof(TUseOutput).FullName == attribute.AttributeType.FullName)
            {
                var outputVar = ilProcessor.Body.Variables.First(n => n.Name == "UseReturnVal");
                instruction = ilProcessor.Create(OpCodes.Ldloca, outputVar);
            }
            else
            {


                throw new Exception("Unhandled parameter bind type");
            }

            return instruction;
        }

        private static void InjectMethod<TParam, TThis, TInput, TLocal>(CecilContext stardewContext, ILProcessor ilProcessor, IEnumerable<Instruction> targets, MethodReference method, bool isExit = false)
        {
            foreach (var target in targets.ToList())
            {
                InjectMethod<TParam, TThis, TInput, TLocal>(stardewContext, ilProcessor, target, method, isExit);
            }
        }

        private static void InjectReturnableMethod<TParam, TThis, TInput, TLocal, TUseOutput>(CecilContext stardewContext, ILProcessor ilProcessor, IEnumerable<Instruction> targets, MethodReference method, bool isExit = false)
        {
            foreach (var target in targets.ToList())
            {
                InjectReturnableMethod<TParam, TThis, TInput, TLocal, TUseOutput>(stardewContext, ilProcessor, target, method, isExit);
            }
        }

        public static void InjectEntryMethod<TParam, TThis, TInput, TLocal>(CecilContext stardewContext, string injecteeType, string injecteeMethod,
            string injectedType, string injectedMethod)
        {
            var methodDefinition = stardewContext.GetMethodDefinition(injectedType, injectedMethod);
            if (methodDefinition.HasGenericParameters)
            {
                var injecteeMethodDef = stardewContext.GetMethodDefinition(injecteeType, injecteeMethod);

                var inst = new GenericInstanceMethod(methodDefinition);
                for (var i = 0; i < methodDefinition.GenericParameters.Count; ++i)
                {
                    inst.GenericArguments.Add(injecteeMethodDef.DeclaringType.GenericParameters[i]);
                }

                var ilProcessor = stardewContext.GetMethodIlProcessor(injecteeType, injecteeMethod);
                InjectMethod<TParam, TThis, TInput, TLocal>(stardewContext, ilProcessor, ilProcessor.Body.Instructions.First(), inst, false, methodDefinition.ReturnType != null && methodDefinition.ReturnType.FullName == typeof(bool).FullName);
            }
            else
            {
                var ilProcessor = stardewContext.GetMethodIlProcessor(injecteeType, injecteeMethod);
                InjectMethod<TParam, TThis, TInput, TLocal>(stardewContext, ilProcessor, ilProcessor.Body.Instructions.First(), methodDefinition, false, methodDefinition.ReturnType != null && methodDefinition.ReturnType.FullName == typeof(bool).FullName);
            }
        }

        public static void InjectReturnableEntryMethod<TParam, TThis, TInput, TLocal, TUseOutput>(CecilContext stardewContext, string injecteeType, string injecteeMethod,
            string injectedType, string injectedMethod)
        {
            var methodDefinition = stardewContext.GetMethodDefinition(injectedType, injectedMethod);
            if (methodDefinition.HasGenericParameters)
            {
                var injecteeMethodDef = stardewContext.GetMethodDefinition(injecteeType, injecteeMethod);

                var inst = new GenericInstanceMethod(methodDefinition);
                for (var i = 0; i < methodDefinition.GenericParameters.Count; ++i)
                {
                    inst.GenericArguments.Add(injecteeMethodDef.DeclaringType.GenericParameters[i]);
                }

                var ilProcessor = stardewContext.GetMethodIlProcessor(injecteeType, injecteeMethod);
                InjectReturnableMethod<TParam, TThis, TInput, TLocal, TUseOutput>(stardewContext, ilProcessor, ilProcessor.Body.Instructions.First(), inst, false);
            }
            else
            {
                var ilProcessor = stardewContext.GetMethodIlProcessor(injecteeType, injecteeMethod);
                InjectReturnableMethod<TParam, TThis, TInput, TLocal, TUseOutput>(stardewContext, ilProcessor, ilProcessor.Body.Instructions.First(), methodDefinition, false);
            }
        }

        public static void InjectExitMethod<TParam, TThis, TInput, TLocal>(CecilContext stardewContext, string injecteeType, string injecteeMethod,
            string injectedType, string injectedMethod)
        {
            var methodDefinition = stardewContext.GetMethodDefinition(injectedType, injectedMethod);
            if (methodDefinition.HasGenericParameters)
            {
                var injecteeMethodDef = stardewContext.GetMethodDefinition(injecteeType, injecteeMethod);

                var inst = new GenericInstanceMethod(methodDefinition);
                for (var i = 0; i < methodDefinition.GenericParameters.Count; ++i)
                {
                    inst.GenericArguments.Add(injecteeMethodDef.DeclaringType.GenericParameters[i]);
                }

                var ilProcessor = stardewContext.GetMethodIlProcessor(injecteeType, injecteeMethod);
                InjectMethod<TParam, TThis, TInput, TLocal>(stardewContext, ilProcessor, ilProcessor.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret), methodDefinition, true);
            }
            else
            {
                var ilProcessor = stardewContext.GetMethodIlProcessor(injecteeType, injecteeMethod);
                InjectMethod<TParam, TThis, TInput, TLocal>(stardewContext, ilProcessor, ilProcessor.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret), methodDefinition, true);
            }
        }

        public static void InjectReturnableExitMethod<TParam, TThis, TInput, TLocal, TUseOutput>(CecilContext stardewContext, string injecteeType, string injecteeMethod,
            string injectedType, string injectedMethod)
        {
            var methodDefinition = stardewContext.GetMethodDefinition(injectedType, injectedMethod);
            if (methodDefinition.HasGenericParameters)
            {
                var injecteeMethodDef = stardewContext.GetMethodDefinition(injecteeType, injecteeMethod);

                var inst = new GenericInstanceMethod(methodDefinition);
                for (var i = 0; i < methodDefinition.GenericParameters.Count; ++i)
                {
                    inst.GenericArguments.Add(injecteeMethodDef.DeclaringType.GenericParameters[i]);
                }

                var ilProcessor = stardewContext.GetMethodIlProcessor(injecteeType, injecteeMethod);
                InjectReturnableMethod<TParam, TThis, TInput, TLocal, TUseOutput>(stardewContext, ilProcessor, ilProcessor.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret), methodDefinition, true);
            }
            else
            {
                var ilProcessor = stardewContext.GetMethodIlProcessor(injecteeType, injecteeMethod);
                InjectReturnableMethod<TParam, TThis, TInput, TLocal, TUseOutput>(stardewContext, ilProcessor, ilProcessor.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret), methodDefinition, true);
            }
        }

        public static void InjectGlobalRoutePreMethod(CecilContext stardewContext, string injecteeType, string injecteeMethod, int index)
        {
            var fieldDefinition = stardewContext.GetFieldDefinition("Farmhand.Events.GlobalRouteManager", "IsEnabled");
            var methodIsListenedTo = stardewContext.GetMethodDefinition("Farmhand.Events.GlobalRouteManager", "IsBeingPreListenedTo");
            var methodDefinition = stardewContext.GetMethodDefinition("Farmhand.Events.GlobalRouteManager", "GlobalRoutePreInvoke");
            var ilProcessor = stardewContext.GetMethodIlProcessor(injecteeType, injecteeMethod);

            if (ilProcessor == null || methodDefinition == null || fieldDefinition == null)
                return;

            var method = ilProcessor.Body.Method;
            var hasThis = method.HasThis;
            var argIndex = 0;

            VariableDefinition outputVar = null;

            Instruction first = ilProcessor.Body.Instructions.First();
            Instruction last = ilProcessor.Body.Instructions.Last();
            Instruction prelast;
            if (ilProcessor.Body.Instructions.Count > 1)
            {
                prelast = ilProcessor.Body.Instructions[ilProcessor.Body.Instructions.Count - 2];
            }
            else
            {
                prelast = first;
            }
            var objectType = stardewContext.GetTypeReference(typeof(object));
            var voidType = stardewContext.GetTypeReference(typeof(void));

            var newInstructions = new List<Instruction>();
            newInstructions.Add(ilProcessor.Create(OpCodes.Ldsfld, fieldDefinition));
            newInstructions.Add(ilProcessor.Create(OpCodes.Brfalse, first));

            newInstructions.Add(ilProcessor.Create(OpCodes.Ldc_I4, index));
            newInstructions.Add(ilProcessor.Create(OpCodes.Call, methodIsListenedTo));
            newInstructions.Add(ilProcessor.Create(OpCodes.Brfalse, first));

            newInstructions.Add(ilProcessor.Create(OpCodes.Ldc_I4, index));
            newInstructions.Add(ilProcessor.Create(OpCodes.Ldstr, injecteeType));
            newInstructions.Add(ilProcessor.Create(OpCodes.Ldstr, injecteeMethod));

            outputVar = new VariableDefinition("GlobalRouteOutput", objectType);
            ilProcessor.Body.Variables.Add(outputVar);
            newInstructions.Add(ilProcessor.Create(OpCodes.Ldloca, outputVar));
            
            newInstructions.Add(ilProcessor.Create(OpCodes.Ldc_I4, method.Parameters.Count + (hasThis ? 1 : 0)));
            newInstructions.Add(ilProcessor.Create(OpCodes.Newarr, objectType));

            if (hasThis)
            {
                newInstructions.Add(ilProcessor.Create(OpCodes.Dup));
                newInstructions.Add(ilProcessor.Create(OpCodes.Ldc_I4, argIndex++));
                newInstructions.Add(ilProcessor.Create(OpCodes.Ldarg, ilProcessor.Body.ThisParameter));
                newInstructions.Add(ilProcessor.Create(OpCodes.Stelem_Ref));
            }

            foreach (var param in method.Parameters)
            {
                newInstructions.Add(ilProcessor.Create(OpCodes.Dup));
                newInstructions.Add(ilProcessor.Create(OpCodes.Ldc_I4, argIndex++));
                newInstructions.Add(ilProcessor.Create(OpCodes.Ldarg, param));
                if (param.ParameterType.IsPrimitive)
                    newInstructions.Add(ilProcessor.Create(OpCodes.Box, param.ParameterType));
                newInstructions.Add(ilProcessor.Create(OpCodes.Stelem_Ref));
            }

            newInstructions.Add(ilProcessor.Create(OpCodes.Call, methodDefinition));
            newInstructions.Add(ilProcessor.Create(OpCodes.Brfalse, first));

            if (method.ReturnType != null && method.ReturnType.FullName != voidType.FullName)
            {
                if (outputVar == null)
                    throw new Exception("outputVar is null");

                newInstructions.Add(ilProcessor.Create(OpCodes.Ldloc, outputVar));
                if (method.ReturnType.IsPrimitive || method.ReturnType.IsGenericParameter)
                {
                    newInstructions.Add(ilProcessor.Create(OpCodes.Unbox_Any, method.ReturnType));
                }
                else
                {
                    newInstructions.Add(ilProcessor.Create(OpCodes.Castclass, method.ReturnType));
                }
                newInstructions.Add(ilProcessor.Create(OpCodes.Br, last));
            }
            else
            {
                //newInstructions.Add(ilProcessor.Create(OpCodes.Br, prelast));
            }

            ilProcessor.Body.SimplifyMacros();
            if (newInstructions.Any())
            {
                var previousInstruction = newInstructions.First();
                ilProcessor.InsertBefore(first, previousInstruction);
                for (var i = 1; i < newInstructions.Count; ++i)
                {
                    ilProcessor.InsertAfter(previousInstruction, newInstructions[i]);
                    previousInstruction = newInstructions[i];
                }
            }
            ilProcessor.Body.OptimizeMacros();

        }

        public static void InjectGlobalRoutePostMethod(CecilContext stardewContext, string injecteeType, string injecteeMethod, int index)
        {
            var ilProcessor = stardewContext.GetMethodIlProcessor(injecteeType, injecteeMethod);

            if (ilProcessor == null)
                return;

            var objectType = stardewContext.GetTypeReference(typeof(object));
            var voidType = stardewContext.GetTypeReference(typeof(void));
            var boolType = stardewContext.GetTypeReference(typeof(bool));
            var method = ilProcessor.Body.Method;
            var hasThis = method.HasThis;
            var returnsValue = method.ReturnType != null && method.ReturnType.FullName != voidType.FullName;
            
            VariableDefinition outputVar = null;

            var isEnabledField = stardewContext.GetFieldDefinition("Farmhand.Events.GlobalRouteManager", "IsEnabled");
            var methodIsListenedTo = stardewContext.GetMethodDefinition("Farmhand.Events.GlobalRouteManager", "IsBeingPostListenedTo");
            MethodDefinition methodDefinition;

            methodDefinition = stardewContext.GetMethodDefinition("Farmhand.Events.GlobalRouteManager", "GlobalRoutePostInvoke", 
                    n => n.Parameters.Count == (returnsValue ? 5 : 4));
            
            if (methodDefinition == null || isEnabledField == null)
                return;

            var retInstructions = ilProcessor.Body.Instructions.Where(n => n.OpCode == OpCodes.Ret).ToArray();
            
            foreach (var ret in retInstructions)
            {
                var newInstructions = new List<Instruction>();
                
                newInstructions.Add(ilProcessor.PushFieldToStack(isEnabledField));
                newInstructions.Add(ilProcessor.BranchIfFalse(ret));
                
                newInstructions.Add(ilProcessor.PushInt32ToStack(index));
                newInstructions.Add(ilProcessor.Call(methodIsListenedTo));
                newInstructions.Add(ilProcessor.BranchIfFalse(ret));
                
                if (returnsValue)
                {
                    outputVar = new VariableDefinition("GlobalRouteOutput", objectType);
                    ilProcessor.Body.Variables.Add(outputVar);
                    if (method.ReturnType.IsPrimitive || method.ReturnType.IsGenericParameter)
                    {
                        newInstructions.Add(ilProcessor.Create(OpCodes.Box, method.ReturnType));
                    }
                    newInstructions.Add(ilProcessor.Create(OpCodes.Stloc, outputVar));
                }

                newInstructions.Add(ilProcessor.PushInt32ToStack(index));
                newInstructions.Add(ilProcessor.PushStringToStack(injecteeType));
                newInstructions.Add(ilProcessor.PushStringToStack(injecteeMethod));

                if (returnsValue)
                {
                    newInstructions.Add(ilProcessor.Create(OpCodes.Ldloca, outputVar));
                }

                int argIndex = 0;
                newInstructions.AddRange(ilProcessor.CreateArray(objectType, method.Parameters.Count + (hasThis ? 1 : 0)));

                if (method.HasThis)
                {
                    newInstructions.AddRange(ilProcessor.InsertParameterIntoArray(ilProcessor.Body.ThisParameter, argIndex++));
                }
                    
                foreach (var param in method.Parameters)
                {
                    newInstructions.AddRange(ilProcessor.InsertParameterIntoArray(param, argIndex++));
                }

                newInstructions.Add(ilProcessor.Call(methodDefinition));

                if (returnsValue)
                {
                    newInstructions.Add(ilProcessor.Create(OpCodes.Ldloc, outputVar));
                    if (method.ReturnType.IsPrimitive || method.ReturnType.IsGenericParameter)
                    {
                        newInstructions.Add(ilProcessor.Create(OpCodes.Unbox_Any, method.ReturnType));
                    }
                    else
                    {
                        newInstructions.Add(ilProcessor.Create(OpCodes.Castclass, method.ReturnType));
                    }
                }

                ilProcessor.Body.SimplifyMacros();
                if (newInstructions.Any())
                {
                    var previousInstruction = newInstructions.First();
                    ilProcessor.InsertBefore(ret, previousInstruction);
                    for (var i = 1; i < newInstructions.Count; ++i)
                    {
                        ilProcessor.InsertAfter(previousInstruction, newInstructions[i]);
                        previousInstruction = newInstructions[i];
                    }
                }
                ilProcessor.Body.OptimizeMacros();
            }            
        }

        public static void RedirectConstructorFromBase(CecilContext stardewContext, Type asmType, Type[] test, string type, string method, Type[] parameters)
        {
            var types = test.Select(n => stardewContext.GetTypeReference(n)).ToArray();
            var typeDef = stardewContext.GetTypeDefinition(asmType.Namespace + "." + asmType.Name);
            var typeDefBase = stardewContext.GetTypeDefinition(asmType.BaseType.Namespace + "." + asmType.BaseType.Name);

            var newConstructorReference = stardewContext.GetConstructorReference(typeDef);
            var oldConstructorReference = stardewContext.GetConstructorReference(typeDefBase);

            var concreteFromConstructor = MakeHostInstanceGeneric(oldConstructorReference, types);
            var concreteToConstructor = MakeHostInstanceGeneric(newConstructorReference, types);
            
            ILProcessor ilProcessor = stardewContext.GetMethodIlProcessor(type, method);

            var instructions = ilProcessor.Body.Instructions.Where(n => n.OpCode == OpCodes.Newobj && n.Operand is MethodReference && 
                ((MethodReference)n.Operand).FullName == concreteFromConstructor.FullName).ToList();
            
            foreach (var instruction in instructions)
            {
                ilProcessor.Replace(instruction, ilProcessor.Create(OpCodes.Newobj, concreteToConstructor));
            }
        }

        public static MethodReference MakeHostInstanceGeneric(this MethodReference self, params TypeReference[] arguments)
        {
            var reference = new MethodReference(self.Name, self.ReturnType, self.DeclaringType.MakeGenericInstanceType(arguments))
            {
                HasThis = self.HasThis,
                ExplicitThis = self.ExplicitThis,
                CallingConvention = self.CallingConvention
            };

            foreach (var parameter in self.Parameters)
                reference.Parameters.Add(new ParameterDefinition(parameter.ParameterType));

            foreach (var generic_parameter in self.GenericParameters)
                reference.GenericParameters.Add(new GenericParameter(generic_parameter.Name, reference));

            return reference;
        }

        public static MethodReference MakeGenericInstanceMethod(this MethodReference self, TypeReference[] arguments)
        {
            if (self == null)
                throw new ArgumentException("self");
            if (arguments == null)
                throw new ArgumentNullException("arguments");
            if (!arguments.Any())
                throw new ArgumentException("{0} cannot be empty", "arguments");

            var refernce = new GenericInstanceMethod(self);
            foreach (var methodArg in arguments)
                refernce.GenericArguments.Add(methodArg);
            return refernce;
        }

        public static void RedirectConstructorFromBase(CecilContext stardewContext, Type asmType, string type, string method, Type[] parameters)
        {
            // Get a single string containing the full names of all parameters, comma separated
            string parametersString = "";
            for(int i=0; i<parameters.Length; i++)
            {
                parametersString += parameters[i].FullName;
                if(i != parameters.Length-1)
                {
                    parametersString += ",";
                }
            }

            var newConstructor = asmType.GetConstructor(parameters);

            if (asmType.BaseType == null) return;
            var oldConstructor = asmType.BaseType.GetConstructor(parameters);

            if (newConstructor == null) return;
            // Build a FullName version of newConstructor.Name
            string newConstructorFullName = "System.Void " + asmType.FullName + "::.ctor" + "(" + parametersString + ")";
            var newConstructorReference = stardewContext.GetMethodDefinitionFullName(asmType.FullName, newConstructorFullName);

            if (oldConstructor == null) return;
            // Build a FullName version of oldConstructor.Name
            string oldConstructorFullName = "System.Void " + asmType.BaseType.FullName + "::.ctor" + "(" + parametersString + ")";
            var oldConstructorReference = stardewContext.GetMethodDefinitionFullName(asmType.BaseType.FullName ?? asmType.BaseType.Namespace + "." + asmType.BaseType.Name, oldConstructorFullName);


            ILProcessor ilProcessor = stardewContext.GetMethodIlProcessor(type, method);

            var instructions = ilProcessor.Body.Instructions.Where(n => n.OpCode == OpCodes.Newobj && n.Operand == oldConstructorReference).ToList();
            foreach(var instruction in instructions)
            {
                ilProcessor.Replace(instruction, ilProcessor.Create(OpCodes.Newobj, newConstructorReference));
            }
        }

        public static void SetVirtualCallOnMethod(CecilContext cecilContext, string fullName, string name, string type, string method)
        {
            MethodDefinition methodDefinition = cecilContext.GetMethodDefinition(fullName, name);
            ILProcessor ilProcessor = cecilContext.GetMethodIlProcessor(type, method);

            var instructions = ilProcessor.Body.Instructions.Where(n => n.OpCode == OpCodes.Call && n.Operand == methodDefinition).ToList();
            foreach (var instruction in instructions)
            {
                ilProcessor.Replace(instruction, ilProcessor.Create(OpCodes.Callvirt, methodDefinition));
            }
        }

        public static void SetVirtualOnBaseMethods(CecilContext stardewContext, string typeName)
        {
            var type = stardewContext.GetTypeDefinition(typeName);
            
            if (type.HasMethods)
            {
                foreach (MethodDefinition method in type.Methods.Where(n => !n.IsConstructor && !n.IsStatic))
                {
                    if (!method.IsVirtual)
                    {
                        method.IsVirtual = true;
                        method.IsNewSlot = true;
                        method.IsReuseSlot = false;
                        method.IsHideBySig = true;
                    }
                    method.IsFinal = false;
                }
            }
        }

        public static void AlterProtectionOnTypeMembers(CecilContext stardewContext, bool @public, string typeName)
        {
            var type = stardewContext.GetTypeDefinition(typeName);

            if (type.HasMethods)
            {
                foreach (MethodDefinition method in type.Methods)
                {
                    if (!@public)
                    {
                        if (method.IsPrivate)
                        {
                            method.IsPrivate = false;
                            method.IsFamily = true;
                        }
                    }
                    else
                    {
                        if (method.IsPrivate || method.IsFamily)
                        {
                            method.IsPrivate = false;
                            method.IsPublic = false;
                        }
                    }
                }
            }

            if (type.HasFields)
            {
                foreach (FieldDefinition field in type.Fields)
                {
                    if (!@public)
                    {
                        if (field.IsPrivate)
                        {
                            field.IsPrivate = false;
                            field.IsFamily = true;
                        }
                    }
                    else
                    {
                        if (field.IsPrivate || field.IsFamily)
                        {
                            field.IsPrivate = false;
                            field.IsPublic = true;
                        }
                    }
                }
            }
        }

        public static void HookAllGlobalRouteMethods(CecilContext stardewContext)
        {
            var methods = stardewContext.GetMethods().Where(n => n.DeclaringType.Namespace.StartsWith("StardewValley")).ToArray();

            var ilProcessor2 = stardewContext.GetMethodIlProcessor("Farmhand.Test", "TesttttgetNewID");
            ilProcessor2.Body.SimplifyMacros();


            {
                var listenedMethodField = stardewContext.GetFieldDefinition("Farmhand.Events.GlobalRouteManager", "ListenedMethods");
                var ilProcessor = stardewContext.GetMethodIlProcessor("Farmhand.Events.GlobalRouteManager", ".cctor");
                var loadInt = ilProcessor.Create(OpCodes.Ldc_I4, methods.Length);
                var setValue = ilProcessor.Create(OpCodes.Stsfld, listenedMethodField);
                ilProcessor.InsertAfter(ilProcessor.Body.Instructions[ilProcessor.Body.Instructions.Count - 2], loadInt);
                ilProcessor.InsertAfter(loadInt, setValue);                
            }

            for (int i = 0; i < methods.Length; ++i)
            {
                //InjectGlobalRoutePreMethod(stardewContext, methods[i].DeclaringType.FullName, methods[i].Name, i);
                InjectGlobalRoutePostMethod(stardewContext, methods[i].DeclaringType.FullName, methods[i].Name, i);
                InjectMappingInformation(stardewContext, methods[i].DeclaringType.FullName, methods[i].Name, i);
            }            
        }

        private static void InjectMappingInformation(CecilContext stardewContext, string className, string methodName, int index)
        {
            var mapMethodDefinition = stardewContext.GetMethodDefinition("Farmhand.Events.GlobalRouteManager", "MapIndex");
            var ilProcessor = stardewContext.GetMethodIlProcessor("Farmhand.Events.GlobalRouteManager", "InitialiseMappings");

            if (ilProcessor == null || mapMethodDefinition == null)
                return;

            var newInstructions = new List<Instruction>();

            newInstructions.Add(ilProcessor.Create(OpCodes.Ldstr, className));
            newInstructions.Add(ilProcessor.Create(OpCodes.Ldstr, methodName));
            newInstructions.Add(ilProcessor.Create(OpCodes.Ldc_I4, index));
            newInstructions.Add(ilProcessor.Create(OpCodes.Call, mapMethodDefinition));

            ilProcessor.Body.SimplifyMacros();
            if (newInstructions.Any())
            {
                var previousInstruction = newInstructions.First();
                if (ilProcessor.Body.Instructions.Count > 1)
                {
                    ilProcessor.InsertAfter(ilProcessor.Body.Instructions[ilProcessor.Body.Instructions.Count - 2],
                        previousInstruction);
                }
                else
                {
                    ilProcessor.InsertBefore(ilProcessor.Body.Instructions[0], previousInstruction);
                }

                for (var i = 1; i < newInstructions.Count; ++i)
                {
                    ilProcessor.InsertAfter(previousInstruction, newInstructions[i]);
                    previousInstruction = newInstructions[i];
                }
            }
            ilProcessor.Body.OptimizeMacros();
        }

        public static void AlterProtectionOnType(CecilContext stardewContext, bool isPublic, string typeName)
        {
            var type = stardewContext.GetTypeDefinition(typeName);
            type.IsPublic = isPublic;
            type.IsNotPublic = !isPublic;
        }
    }
}
