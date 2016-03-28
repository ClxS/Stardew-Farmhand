using Mono.Cecil;
using Mono.Cecil.Cil;
using Revolution.Cecil;
using System.Collections.Generic;
using System.Linq;
using System;
using Mono.Cecil.Rocks;

namespace Revolution.Helpers
{
    public static class CecilHelper
    {
        //System.Void StardewValley.Game1::.ctor()

        private static void InjectMethod<TParam, TThis, TInput, TLocal>(ILProcessor ilProcessor, Instruction target, MethodReference method, bool cancelable = false)
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
                var paramLdInstruction = target;
                foreach (var parameter in method.Parameters)
                {
                    paramLdInstruction = GetInstructionForParameter<TParam, TThis, TInput, TLocal>(ilProcessor, parameter);
                    if (paramLdInstruction == null) throw new Exception($"Error parsing parameter setup on {parameter.Name}");
                    ilProcessor.InsertBefore(target, paramLdInstruction);
                }

                ilProcessor.InsertAfter(paramLdInstruction, callEnterInstruction);
            }
            else
            {
                ilProcessor.InsertBefore(target, callEnterInstruction);
            }

            if (cancelable)
            {
                var branch = ilProcessor.Create(OpCodes.Brtrue, ilProcessor.Body.Instructions.Last());
                ilProcessor.InsertAfter(callEnterInstruction, branch);
            }

            ilProcessor.Body.OptimizeMacros();
        }

        private static Instruction GetInstructionForParameter<TParam, TThis, TInput, TLocal>(ILProcessor ilProcessor, ParameterDefinition parameter)
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
            else 
            {
                

                throw new Exception("Unhandled parameter bind type");
            }

            return instruction;
        }

        private static void InjectMethod<TParam, TThis, TInput, TLocal>(ILProcessor ilProcessor, IEnumerable<Instruction> targets, MethodReference method)
        {
            foreach (var target in targets.ToList())
            {
                InjectMethod<TParam, TThis, TInput, TLocal>(ilProcessor, target, method);
            }
        }

        public static void InjectEntryMethod<TParam, TThis, TInput, TLocal>(CecilContext stardewContext, string injecteeType, string injecteeMethod,
            string injectedType, string injectedMethod)
        {
            var methodDefinition = stardewContext.GetMethodDefinition(injectedType, injectedMethod);
            var ilProcessor = stardewContext.GetMethodIlProcessor(injecteeType, injecteeMethod);
            InjectMethod<TParam, TThis, TInput, TLocal>(ilProcessor, ilProcessor.Body.Instructions.First(), methodDefinition, methodDefinition.ReturnType != null && methodDefinition.ReturnType.FullName == typeof(bool).FullName);
        }

        public static void InjectExitMethod<TParam, TThis, TInput, TLocal>(CecilContext stardewContext, string injecteeType, string injecteeMethod,
            string injectedType, string injectedMethod)
        {
            MethodDefinition methodDefinition = stardewContext.GetMethodDefinition(injectedType, injectedMethod);
            ILProcessor ilProcessor = stardewContext.GetMethodIlProcessor(injecteeType, injecteeMethod);
            InjectMethod<TParam, TThis, TInput, TLocal>(ilProcessor, ilProcessor.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret), methodDefinition);
        }

        public static void RedirectConstructorFromBase(CecilContext stardewContext, Type asmType, string type, string method)
        {
            var test = stardewContext.GetMethodIlProcessor("Revolution.Test", "Test1");
            test.Body.SimplifyMacros();
            test.Body.OptimizeMacros();
            var newConstructor = asmType.GetConstructor(new Type[] { });

            if (asmType.BaseType == null) return;
            var oldConstructor = asmType.BaseType.GetConstructor(new Type[] { });

            if (newConstructor == null) return;
            var newConstructorReference = stardewContext.GetMethodDefinition(asmType.FullName, newConstructor.Name);

            if (oldConstructor == null) return;
            var oldConstructorReference = stardewContext.GetMethodDefinition(asmType.BaseType.FullName, oldConstructor.Name);

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
                            field.IsPublic = false;
                        }
                    }
                }
            }
        }
    }
}
