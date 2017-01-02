namespace Farmhand.Installers.Patcher.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Farmhand.Installers.Patcher;
    using Farmhand.Installers.Patcher.Cecil;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Cecil.Rocks;

    /// <summary>
    ///     A utility class which provides various common methods related to IL manipulation.
    /// </summary>
    public static class CecilHelper
    {
        private static void InjectMethod<TParam, TThis, TInput, TLocal>(
            CecilContext stardewContext,
            ILProcessor processor,
            Instruction target,
            MethodReference method,
            bool isExit = false,
            bool cancelable = false)
        {
            processor.Body.SimplifyMacros();

            var callEnterInstruction = processor.Create(OpCodes.Call, method);

            if (method.HasThis)
            {
                var loadObjInstruction = processor.Create(OpCodes.Ldarg_0);
                processor.InsertBefore(target, loadObjInstruction);
            }

            if (method.HasParameters)
            {
                Instruction previousInstruction = null;
                var paramLdInstruction = target;
                var first = true;
                foreach (var parameter in method.Parameters)
                {
                    paramLdInstruction =
                        GetInstructionForParameter<TParam, TThis, TInput, TLocal, Patcher, Patcher>(
                            processor,
                            parameter);
                    if (paramLdInstruction == null)
                    {
                        throw new Exception($"Error parsing parameter setup on {parameter.Name}");
                    }

                    if (isExit)
                    {
                        if (first)
                        {
                            first = false;
                            processor.Replace(target, paramLdInstruction);
                        }
                        else
                        {
                            processor.InsertAfter(previousInstruction, paramLdInstruction);
                        }

                        previousInstruction = paramLdInstruction;
                    }
                    else
                    {
                        processor.InsertBefore(target, paramLdInstruction);
                    }
                }

                if (isExit)
                {
                    if (first)
                    {
                        processor.Replace(target, callEnterInstruction);
                        processor.InsertAfter(callEnterInstruction, processor.Create(OpCodes.Ret));
                    }
                    else
                    {
                        processor.InsertAfter(previousInstruction, callEnterInstruction);
                        processor.InsertAfter(callEnterInstruction, processor.Create(OpCodes.Ret));
                    }
                }
                else
                {
                    processor.InsertAfter(paramLdInstruction, callEnterInstruction);
                }
            }
            else
            {
                if (isExit)
                {
                    processor.Replace(target, callEnterInstruction);
                    processor.InsertAfter(callEnterInstruction, processor.Create(OpCodes.Ret));
                }
                else
                {
                    processor.InsertBefore(target, callEnterInstruction);
                }
            }

            if (cancelable)
            {
                if (processor.Body.Method.MethodReturnType.ReturnType.FullName
                    != stardewContext.GetTypeReference(typeof(void)).FullName)
                {
                    throw new InvalidOperationException(
                        "Cancelable hooks are only supported for methods returning void");
                }

                var branch = processor.Create(OpCodes.Brtrue, processor.Body.Instructions.Last());
                processor.InsertAfter(callEnterInstruction, branch);
            }

            processor.Body.OptimizeMacros();
        }

        private static void InjectReturnableMethod<TParam, TThis, TInput, TLocal, TUseOutput, TMethodOutputBind>(
            CecilContext stardewContext,
            ILProcessor processor,
            Instruction target,
            MethodReference method,
            bool isExit = false)
        {
            processor.Body.SimplifyMacros();

            // Add UseReturnVal variable
            if (processor.Body.Variables.All(n => n.Name != "UseReturnVal"))
            {
                var boolType = stardewContext.GetTypeReference(typeof(bool));
                processor.Body.Variables.Add(new VariableDefinition("UseReturnVal", boolType));
            }

            var useOutputVariable = processor.Body.Variables.First(n => n.Name == "UseReturnVal");

            if (processor.Body.Variables.All(n => n.Name != "ReturnContainer-" + method.ReturnType.Name))
            {
                processor.Body.Variables.Add(
                    new VariableDefinition("ReturnContainer-" + method.ReturnType.Name, method.ReturnType));
            }

            if (isExit)
            {
                if (processor.Body.Variables.All(n => n.Name != "OldReturnContainer"))
                {
                    processor.Body.Variables.Add(new VariableDefinition("OldReturnContainer", method.ReturnType));
                }
            }

            var containerVariable =
                processor.Body.Variables.First(n => n.Name == "ReturnContainer-" + method.ReturnType.Name);
            var oldContainerVariable = processor.Body.Variables.FirstOrDefault(n => n.Name == "OldReturnContainer");

            var instructions = new List<Instruction>();
            if (isExit)
            {
                instructions.Add(processor.Create(OpCodes.Stloc, oldContainerVariable));
            }

            if (method.HasParameters)
            {
                foreach (var parameter in method.Parameters)
                {
                    var paramLdInstruction =
                        GetInstructionForParameter<TParam, TThis, TInput, TLocal, TUseOutput, TMethodOutputBind>(
                            processor,
                            parameter);
                    if (paramLdInstruction == null)
                    {
                        throw new Exception($"Error parsing parameter setup on {parameter.Name}");
                    }

                    instructions.Add(paramLdInstruction);
                }
            }

            instructions.Add(processor.Create(OpCodes.Call, method));
            instructions.Add(processor.Create(OpCodes.Stloc, containerVariable));
            instructions.Add(processor.Create(OpCodes.Ldloc, useOutputVariable));
            if (!isExit)
            {
                instructions.Add(processor.Create(OpCodes.Brfalse, target));
                instructions.Add(processor.Create(OpCodes.Ldloc, containerVariable));
                instructions.Add(processor.Create(OpCodes.Br, processor.Body.Instructions.Last()));
            }
            else
            {
                var branchToRet = processor.Create(OpCodes.Br, target);
                var loadOld = processor.Create(OpCodes.Ldloc, oldContainerVariable);
                var loadNew = processor.Create(OpCodes.Ldloc, containerVariable);
                instructions.Add(processor.Create(OpCodes.Brfalse, loadOld));
                instructions.Add(loadNew);
                instructions.Add(branchToRet);
                instructions.Add(loadOld);
            }

            foreach (var inst in instructions)
            {
                processor.InsertBefore(target, inst);
            }

            processor.Body.OptimizeMacros();
        }

        private static Instruction GetInstructionForParameter<TParam, TThis, TInput, TLocal, TUseOutput, TMethodOutputBind>(
                ILProcessor processor,
                ParameterDefinition parameter)
        {
            if (!parameter.HasCustomAttributes)
            {
                return null;
            }

            var attribute =
                parameter.CustomAttributes.FirstOrDefault(
                    n =>
                        n.AttributeType.IsDefinition
                        && n.AttributeType.Resolve().BaseType?.FullName == typeof(TParam).FullName);

            if (attribute == null)
            {
                return null;
            }

            Instruction instruction = null;
            if (typeof(TThis).FullName == attribute.AttributeType.FullName)
            {
                instruction = processor.Create(OpCodes.Ldarg, processor.Body.ThisParameter);
            }
            else if (typeof(TInput).FullName == attribute.AttributeType.FullName)
            {
                var type = attribute.ConstructorArguments[0].Value as TypeReference;
                var name = attribute.ConstructorArguments[1].Value as string;

                var inputParam =
                    processor.Body.Method.Parameters.FirstOrDefault(
                        p => p.Name == name && p.ParameterType.FullName == type?.FullName);

                if (inputParam != null)
                {
                    if ((bool)attribute.ConstructorArguments[2].Value)
                    {
                        instruction = processor.Create(OpCodes.Ldarga, inputParam);
                    }
                    else
                    {
                        instruction = processor.Create(OpCodes.Ldarg, inputParam);
                    }
                }
            }
            else if (typeof(TLocal).FullName == attribute.AttributeType.FullName)
            {
                var type = attribute.ConstructorArguments[0].Value as TypeReference;
                var index = attribute.ConstructorArguments[1].Value as int?;

                var inputParam =
                    processor.Body.Variables.FirstOrDefault(
                        p => p.Index == index && p.VariableType.FullName == type?.FullName);

                if (inputParam != null)
                {
                    instruction = processor.Create(OpCodes.Ldloc, inputParam);
                }
            }
            else if (typeof(TUseOutput).FullName == attribute.AttributeType.FullName)
            {
                var outputVar = processor.Body.Variables.First(n => n.Name == "UseReturnVal");
                instruction = processor.Create(OpCodes.Ldloca, outputVar);
            }
            else if (typeof(TMethodOutputBind).FullName == attribute.AttributeType.FullName)
            {
                var outputVar = processor.Body.Variables.First(n => n.Name == "OldReturnContainer");
                instruction = processor.Create(OpCodes.Ldloc, outputVar);
            }
            else
            {
                throw new Exception("Unhandled parameter bind type");
            }

            return instruction;
        }

        private static void InjectMethod<TParam, TThis, TInput, TLocal>(
            CecilContext stardewContext,
            ILProcessor processor,
            IEnumerable<Instruction> targets,
            MethodReference method,
            bool isExit = false)
        {
            foreach (var target in targets.ToList())
            {
                InjectMethod<TParam, TThis, TInput, TLocal>(stardewContext, processor, target, method, isExit);
            }
        }

        private static void InjectReturnableMethod<TParam, TThis, TInput, TLocal, TUseOutput, TMethodOutputBind>(
            CecilContext stardewContext,
            ILProcessor processor,
            IEnumerable<Instruction> targets,
            MethodReference method,
            bool isExit = false)
        {
            foreach (var target in targets.ToList())
            {
                InjectReturnableMethod<TParam, TThis, TInput, TLocal, TUseOutput, TMethodOutputBind>(
                    stardewContext,
                    processor,
                    target,
                    method,
                    isExit);
            }
        }

        /// <summary>
        ///     Injects a call to method into the start of another method.
        /// </summary>
        /// <param name="stardewContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <param name="injectReceiverType">
        ///     The type containing the method to inject into.
        /// </param>
        /// <param name="injectReceiverMethod">
        ///     The method to inject into.
        /// </param>
        /// <param name="injectedType">
        ///     The type containing the method being injected.
        /// </param>
        /// <param name="injectedMethod">
        ///     The method to inject.
        /// </param>
        /// <typeparam name="TParam">
        ///     The type of the attribute specifying the attribute base class.
        /// </typeparam>
        /// <typeparam name="TThis">
        ///     The type of the attribute binding to the this value.
        /// </typeparam>
        /// <typeparam name="TInput">
        ///     The type of the attribute binding to input parameters.
        /// </typeparam>
        /// <typeparam name="TLocal">
        ///     The type of the attribute binding to local variables.
        /// </typeparam>
        public static void InjectEntryMethod<TParam, TThis, TInput, TLocal>(
            CecilContext stardewContext,
            string injectReceiverType,
            string injectReceiverMethod,
            string injectedType,
            string injectedMethod)
        {
            var methodDefinition = stardewContext.GetMethodDefinition(injectedType, injectedMethod);
            if (methodDefinition.HasGenericParameters)
            {
                var injectReceiverMethodDef = stardewContext.GetMethodDefinition(
                    injectReceiverType,
                    injectReceiverMethod);

                var inst = new GenericInstanceMethod(methodDefinition);
                for (var i = 0; i < methodDefinition.GenericParameters.Count; ++i)
                {
                    inst.GenericArguments.Add(injectReceiverMethodDef.DeclaringType.GenericParameters[i]);
                }

                var processor = stardewContext.GetMethodIlProcessor(injectReceiverType, injectReceiverMethod);
                InjectMethod<TParam, TThis, TInput, TLocal>(
                    stardewContext,
                    processor,
                    processor.Body.Instructions.First(),
                    inst,
                    false,
                    methodDefinition.ReturnType != null && methodDefinition.ReturnType.FullName == typeof(bool).FullName);
            }
            else
            {
                var processor = stardewContext.GetMethodIlProcessor(injectReceiverType, injectReceiverMethod);
                InjectMethod<TParam, TThis, TInput, TLocal>(
                    stardewContext,
                    processor,
                    processor.Body.Instructions.First(),
                    methodDefinition,
                    false,
                    methodDefinition.ReturnType != null && methodDefinition.ReturnType.FullName == typeof(bool).FullName);
            }
        }

        /// <summary>
        ///     Injects a call to method into the start of another method, and supports returning early.
        /// </summary>
        /// <param name="stardewContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <param name="injectReceiverType">
        ///     The type containing the method to inject into.
        /// </param>
        /// <param name="injectReceiverMethod">
        ///     The method to inject into.
        /// </param>
        /// <param name="injectedType">
        ///     The type containing the method being injected.
        /// </param>
        /// <param name="injectedMethod">
        ///     The method to inject.
        /// </param>
        /// <typeparam name="TParam">
        ///     The type of the attribute specifying the attribute base class.
        /// </typeparam>
        /// <typeparam name="TThis">
        ///     The type of the attribute binding to the this value.
        /// </typeparam>
        /// <typeparam name="TInput">
        ///     The type of the attribute binding to input parameters.
        /// </typeparam>
        /// <typeparam name="TLocal">
        ///     The type of the attribute binding to local variables.
        /// </typeparam>
        /// <typeparam name="TUseOutput">
        ///     The type of the attribute binding the to local UseOutput variable.
        /// </typeparam>
        /// <typeparam name="TMethodOutputBind">
        ///     The type of the attribute binding to the method return setter attribute.
        /// </typeparam>
        public static void InjectReturnableEntryMethod<TParam, TThis, TInput, TLocal, TUseOutput, TMethodOutputBind>(
            CecilContext stardewContext,
            string injectReceiverType,
            string injectReceiverMethod,
            string injectedType,
            string injectedMethod)
        {
            var methodDefinition = stardewContext.GetMethodDefinition(injectedType, injectedMethod);
            if (methodDefinition.HasGenericParameters)
            {
                var injectReceiverMethodDef = stardewContext.GetMethodDefinition(
                    injectReceiverType,
                    injectReceiverMethod);

                var inst = new GenericInstanceMethod(methodDefinition);
                for (var i = 0; i < methodDefinition.GenericParameters.Count; ++i)
                {
                    inst.GenericArguments.Add(injectReceiverMethodDef.DeclaringType.GenericParameters[i]);
                }

                var processor = stardewContext.GetMethodIlProcessor(injectReceiverType, injectReceiverMethod);
                InjectReturnableMethod<TParam, TThis, TInput, TLocal, TUseOutput, TMethodOutputBind>(
                    stardewContext,
                    processor,
                    processor.Body.Instructions.First(),
                    inst);
            }
            else
            {
                var processor = stardewContext.GetMethodIlProcessor(injectReceiverType, injectReceiverMethod);
                InjectReturnableMethod<TParam, TThis, TInput, TLocal, TUseOutput, TMethodOutputBind>(
                    stardewContext,
                    processor,
                    processor.Body.Instructions.First(),
                    methodDefinition);
            }
        }

        /// <summary>
        ///     Injects a call to method into the end of another method.
        /// </summary>
        /// <param name="stardewContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <param name="injectReceiverType">
        ///     The type containing the method to inject into.
        /// </param>
        /// <param name="injectReceiverMethod">
        ///     The method to inject into.
        /// </param>
        /// <param name="injectedType">
        ///     The type containing the method being injected.
        /// </param>
        /// <param name="injectedMethod">
        ///     The method to inject.
        /// </param>
        /// <typeparam name="TParam">
        ///     The type of the attribute specifying the attribute base class.
        /// </typeparam>
        /// <typeparam name="TThis">
        ///     The type of the attribute binding to the this value.
        /// </typeparam>
        /// <typeparam name="TInput">
        ///     The type of the attribute binding to input parameters.
        /// </typeparam>
        /// <typeparam name="TLocal">
        ///     The type of the attribute binding to local variables.
        /// </typeparam>
        public static void InjectExitMethod<TParam, TThis, TInput, TLocal>(
            CecilContext stardewContext,
            string injectReceiverType,
            string injectReceiverMethod,
            string injectedType,
            string injectedMethod)
        {
            var methodDefinition = stardewContext.GetMethodDefinition(injectedType, injectedMethod);
            if (methodDefinition.HasGenericParameters)
            {
                var injectReceiverMethodDef = stardewContext.GetMethodDefinition(
                    injectReceiverType,
                    injectReceiverMethod);

                var inst = new GenericInstanceMethod(methodDefinition);
                for (var i = 0; i < methodDefinition.GenericParameters.Count; ++i)
                {
                    inst.GenericArguments.Add(injectReceiverMethodDef.DeclaringType.GenericParameters[i]);
                }

                var processor = stardewContext.GetMethodIlProcessor(injectReceiverType, injectReceiverMethod);
                InjectMethod<TParam, TThis, TInput, TLocal>(
                    stardewContext,
                    processor,
                    processor.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret),
                    methodDefinition,
                    true);
            }
            else
            {
                var processor = stardewContext.GetMethodIlProcessor(injectReceiverType, injectReceiverMethod);
                InjectMethod<TParam, TThis, TInput, TLocal>(
                    stardewContext,
                    processor,
                    processor.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret),
                    methodDefinition,
                    true);
            }
        }

        /// <summary>
        ///     Injects a call to method into the end of another method, and supports returning a value from the called method.
        /// </summary>
        /// <param name="stardewContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <param name="injectReceiverType">
        ///     The type containing the method to inject into.
        /// </param>
        /// <param name="injectReceiverMethod">
        ///     The method to inject into.
        /// </param>
        /// <param name="injectedType">
        ///     The type containing the method being injected.
        /// </param>
        /// <param name="injectedMethod">
        ///     The method to inject.
        /// </param>
        /// <typeparam name="TParam">
        ///     The type of the attribute specifying the attribute base class.
        /// </typeparam>
        /// <typeparam name="TThis">
        ///     The type of the attribute binding to the this value.
        /// </typeparam>
        /// <typeparam name="TInput">
        ///     The type of the attribute binding to input parameters.
        /// </typeparam>
        /// <typeparam name="TLocal">
        ///     The type of the attribute binding to local variables.
        /// </typeparam>
        /// <typeparam name="TUseOutput">
        ///     The type of the attribute binding the to local UseOutput variable.
        /// </typeparam>
        /// <typeparam name="TMethodOutputBind">
        ///     The type of the attribute binding to the method return setter attribute.
        /// </typeparam>
        public static void InjectReturnableExitMethod<TParam, TThis, TInput, TLocal, TUseOutput, TMethodOutputBind>(
            CecilContext stardewContext,
            string injectReceiverType,
            string injectReceiverMethod,
            string injectedType,
            string injectedMethod)
        {
            var methodDefinition = stardewContext.GetMethodDefinition(injectedType, injectedMethod);
            if (methodDefinition.HasGenericParameters)
            {
                var injectReceiverMethodDef = stardewContext.GetMethodDefinition(
                    injectReceiverType,
                    injectReceiverMethod);

                var inst = new GenericInstanceMethod(methodDefinition);
                for (var i = 0; i < methodDefinition.GenericParameters.Count; ++i)
                {
                    inst.GenericArguments.Add(injectReceiverMethodDef.DeclaringType.GenericParameters[i]);
                }

                var processor = stardewContext.GetMethodIlProcessor(injectReceiverType, injectReceiverMethod);
                InjectReturnableMethod<TParam, TThis, TInput, TLocal, TUseOutput, TMethodOutputBind>(
                    stardewContext,
                    processor,
                    processor.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret),
                    methodDefinition,
                    true);
            }
            else
            {
                var processor = stardewContext.GetMethodIlProcessor(injectReceiverType, injectReceiverMethod);
                InjectReturnableMethod<TParam, TThis, TInput, TLocal, TUseOutput, TMethodOutputBind>(
                    stardewContext,
                    processor,
                    processor.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret),
                    methodDefinition,
                    true);
            }
        }

        /// <summary>
        ///     Injects a call to the Global Route Manager at the start of the method.
        /// </summary>
        /// <param name="stardewContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <param name="injectReceiverType">
        ///     The type containing the method to be injected into.
        /// </param>
        /// <param name="injectReceiverMethod">
        ///     The method being injected into.
        /// </param>
        /// <param name="index">
        ///     The unique index for this method.
        /// </param>
        public static void InjectGlobalRoutePreMethod(
            CecilContext stardewContext,
            string injectReceiverType,
            string injectReceiverMethod,
            int index)
        {
            var fieldDefinition = stardewContext.GetFieldDefinition("Farmhand.Events.GlobalRouteManager", "IsEnabled");
            var methodIsListenedTo = stardewContext.GetMethodDefinition(
                "Farmhand.Events.GlobalRouteManager",
                "IsBeingPreListenedTo");
            var methodDefinition = stardewContext.GetMethodDefinition(
                "Farmhand.Events.GlobalRouteManager",
                "GlobalRoutePreInvoke");
            var processor = stardewContext.GetMethodIlProcessor(injectReceiverType, injectReceiverMethod);

            if (processor == null || methodDefinition == null || fieldDefinition == null)
            {
                return;
            }

            var method = processor.Body.Method;
            var hasThis = method.HasThis;
            var argIndex = 0;

            var first = processor.Body.Instructions.First();
            var last = processor.Body.Instructions.Last();
            var objectType = stardewContext.GetTypeReference(typeof(object));
            var voidType = stardewContext.GetTypeReference(typeof(void));

            var newInstructions = new List<Instruction>
                                      {
                                          processor.Create(OpCodes.Ldsfld, fieldDefinition),
                                          processor.Create(OpCodes.Brfalse, first),
                                          processor.Create(OpCodes.Ldc_I4, index),
                                          processor.Create(OpCodes.Call, methodIsListenedTo),
                                          processor.Create(OpCodes.Brfalse, first),
                                          processor.Create(OpCodes.Ldc_I4, index),
                                          processor.Create(OpCodes.Ldstr, injectReceiverType),
                                          processor.Create(OpCodes.Ldstr, injectReceiverMethod)
                                      };

            var outputVar = new VariableDefinition("GlobalRouteOutput", objectType);
            processor.Body.Variables.Add(outputVar);
            newInstructions.Add(processor.Create(OpCodes.Ldloca, outputVar));

            newInstructions.Add(processor.Create(OpCodes.Ldc_I4, method.Parameters.Count + (hasThis ? 1 : 0)));
            newInstructions.Add(processor.Create(OpCodes.Newarr, objectType));

            if (hasThis)
            {
                newInstructions.Add(processor.Create(OpCodes.Dup));
                newInstructions.Add(processor.Create(OpCodes.Ldc_I4, argIndex++));
                newInstructions.Add(processor.Create(OpCodes.Ldarg, processor.Body.ThisParameter));
                newInstructions.Add(processor.Create(OpCodes.Stelem_Ref));
            }

            foreach (var param in method.Parameters)
            {
                newInstructions.Add(processor.Create(OpCodes.Dup));
                newInstructions.Add(processor.Create(OpCodes.Ldc_I4, argIndex++));
                newInstructions.Add(processor.Create(OpCodes.Ldarg, param));
                if (param.ParameterType.IsPrimitive)
                {
                    newInstructions.Add(processor.Create(OpCodes.Box, param.ParameterType));
                }

                newInstructions.Add(processor.Create(OpCodes.Stelem_Ref));
            }

            newInstructions.Add(processor.Create(OpCodes.Call, methodDefinition));
            newInstructions.Add(processor.Create(OpCodes.Brfalse, first));

            if (method.ReturnType != null && method.ReturnType.FullName != voidType.FullName)
            {
                newInstructions.Add(processor.Create(OpCodes.Ldloc, outputVar));
                if (method.ReturnType.IsPrimitive || method.ReturnType.IsGenericParameter)
                {
                    newInstructions.Add(processor.Create(OpCodes.Unbox_Any, method.ReturnType));
                }
                else
                {
                    newInstructions.Add(processor.Create(OpCodes.Castclass, method.ReturnType));
                }

                newInstructions.Add(processor.Create(OpCodes.Br, last));
            }

            processor.Body.SimplifyMacros();
            if (newInstructions.Any())
            {
                var previousInstruction = newInstructions.First();
                processor.InsertBefore(first, previousInstruction);
                for (var i = 1; i < newInstructions.Count; ++i)
                {
                    processor.InsertAfter(previousInstruction, newInstructions[i]);
                    previousInstruction = newInstructions[i];
                }
            }

            processor.Body.OptimizeMacros();
        }

        /// <summary>
        ///     Injects a call to the Global Route Manager at the end of the method.
        /// </summary>
        /// <param name="stardewContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <param name="injectReceiverType">
        ///     The type containing the method to be injected into.
        /// </param>
        /// <param name="injectReceiverMethod">
        ///     The method being injected into.
        /// </param>
        /// <param name="index">
        ///     The unique index for this method.
        /// </param>
        public static void InjectGlobalRoutePostMethod(
            CecilContext stardewContext,
            string injectReceiverType,
            string injectReceiverMethod,
            int index)
        {
            var processor = stardewContext.GetMethodIlProcessor(injectReceiverType, injectReceiverMethod);

            if (processor == null)
            {
                return;
            }

            var objectType = stardewContext.GetTypeReference(typeof(object));
            var voidType = stardewContext.GetTypeReference(typeof(void));
            var method = processor.Body.Method;
            var hasThis = method.HasThis;
            var returnsValue = method.ReturnType != null && method.ReturnType.FullName != voidType.FullName;

            VariableDefinition outputVar = null;

            var isEnabledField = stardewContext.GetFieldDefinition("Farmhand.Events.GlobalRouteManager", "IsEnabled");
            var methodIsListenedTo = stardewContext.GetMethodDefinition(
                "Farmhand.Events.GlobalRouteManager",
                "IsBeingPostListenedTo");

            var methodDefinition = stardewContext.GetMethodDefinition(
                "Farmhand.Events.GlobalRouteManager",
                "GlobalRoutePostInvoke",
                n => n.Parameters.Count == (returnsValue ? 5 : 4));

            if (methodDefinition == null || isEnabledField == null)
            {
                return;
            }

            var retInstructions = processor.Body.Instructions.Where(n => n.OpCode == OpCodes.Ret).ToArray();

            foreach (var ret in retInstructions)
            {
                var newInstructions = new List<Instruction>
                                          {
                                              processor.PushFieldToStack(isEnabledField),
                                              processor.BranchIfFalse(ret),
                                              processor.PushInt32ToStack(index),
                                              processor.Call(methodIsListenedTo),
                                              processor.BranchIfFalse(ret)
                                          };

                if (returnsValue)
                {
                    outputVar = new VariableDefinition("GlobalRouteOutput", objectType);
                    processor.Body.Variables.Add(outputVar);
                    if (method.ReturnType.IsPrimitive || method.ReturnType.IsGenericParameter)
                    {
                        newInstructions.Add(processor.Create(OpCodes.Box, method.ReturnType));
                    }

                    newInstructions.Add(processor.Create(OpCodes.Stloc, outputVar));
                }

                newInstructions.Add(processor.PushInt32ToStack(index));
                newInstructions.Add(processor.PushStringToStack(injectReceiverType));
                newInstructions.Add(processor.PushStringToStack(injectReceiverMethod));

                if (returnsValue)
                {
                    newInstructions.Add(processor.Create(OpCodes.Ldloca, outputVar));
                }

                var argIndex = 0;
                newInstructions.AddRange(processor.CreateArray(objectType, method.Parameters.Count + (hasThis ? 1 : 0)));

                if (method.HasThis)
                {
                    newInstructions.AddRange(
                        processor.InsertParameterIntoArray(processor.Body.ThisParameter, argIndex++));
                }

                foreach (var param in method.Parameters)
                {
                    newInstructions.AddRange(processor.InsertParameterIntoArray(param, argIndex++));
                }

                newInstructions.Add(processor.Call(methodDefinition));

                if (returnsValue)
                {
                    newInstructions.Add(processor.Create(OpCodes.Ldloc, outputVar));
                    if (method.ReturnType.IsPrimitive || method.ReturnType.IsGenericParameter)
                    {
                        newInstructions.Add(processor.Create(OpCodes.Unbox_Any, method.ReturnType));
                    }
                    else
                    {
                        newInstructions.Add(processor.Create(OpCodes.Castclass, method.ReturnType));
                    }
                }

                processor.Body.SimplifyMacros();
                if (newInstructions.Any())
                {
                    var previousInstruction = newInstructions.First();
                    processor.InsertBefore(ret, previousInstruction);
                    for (var i = 1; i < newInstructions.Count; ++i)
                    {
                        processor.InsertAfter(previousInstruction, newInstructions[i]);
                        previousInstruction = newInstructions[i];
                    }
                }

                processor.Body.OptimizeMacros();
            }
        }

        /// <summary>
        ///     Redirects a constructor from base.
        /// </summary>
        /// <param name="stardewContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <param name="newConstructorType">
        ///     The <see cref="Type" /> containing the new constructor.
        /// </param>
        /// <param name="oldConstructorTypes">
        ///     The <see cref="Type" /> of the old constructors to be redirected.
        /// </param>
        /// <param name="type">
        ///     The type containing the method to perform the redirections.
        /// </param>
        /// <param name="method">
        ///     The method to redirect the constructors.
        /// </param>
        /// <param name="parameters">
        ///     The parameters of the constructor.
        /// </param>
        public static void RedirectConstructorFromBase(
            CecilContext stardewContext,
            Type newConstructorType,
            Type[] oldConstructorTypes,
            string type,
            string method,
            Type[] parameters)
        {
            var types = oldConstructorTypes.Select(stardewContext.GetTypeReference).ToArray();
            var typeDef = stardewContext.GetTypeDefinition(newConstructorType.Namespace + "." + newConstructorType.Name);
            if (newConstructorType.BaseType != null)
            {
                var typeDefBase =
                    stardewContext.GetTypeDefinition(
                        newConstructorType.BaseType.Namespace + "." + newConstructorType.BaseType.Name);

                var newConstructorReference = stardewContext.GetConstructorReference(typeDef);
                var oldConstructorReference = stardewContext.GetConstructorReference(typeDefBase);

                var concreteFromConstructor = MakeHostInstanceGeneric(oldConstructorReference, types);
                var concreteToConstructor = MakeHostInstanceGeneric(newConstructorReference, types);

                var processor = stardewContext.GetMethodIlProcessor(type, method);

                var instructions =
                    processor.Body.Instructions.Where(
                        n =>
                            n.OpCode == OpCodes.Newobj && n.Operand is MethodReference
                            && ((MethodReference)n.Operand).FullName == concreteFromConstructor.FullName).ToList();

                foreach (var instruction in instructions)
                {
                    processor.Replace(instruction, processor.Create(OpCodes.Newobj, concreteToConstructor));
                }
            }
        }

        private static MethodReference MakeHostInstanceGeneric(
            this MethodReference self,
            params TypeReference[] arguments)
        {
            var reference = new MethodReference(
                                self.Name,
                                self.ReturnType,
                                self.DeclaringType.MakeGenericInstanceType(arguments))
                                {
                                    HasThis = self.HasThis,
                                    ExplicitThis =
                                        self.ExplicitThis,
                                    CallingConvention =
                                        self.CallingConvention
                                };

            foreach (var parameter in self.Parameters)
            {
                reference.Parameters.Add(new ParameterDefinition(parameter.ParameterType));
            }

            foreach (var genericParameter in self.GenericParameters)
            {
                reference.GenericParameters.Add(new GenericParameter(genericParameter.Name, reference));
            }

            return reference;
        }

        /*
        private static MethodReference MakeGenericInstanceMethod(this MethodReference self, TypeReference[] arguments)
        {
            if (self == null)
            {
                throw new ArgumentException("self");
            }

            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            if (!arguments.Any())
            {
                throw new ArgumentException("{0} cannot be empty", "arguments");
            }

            var reference = new GenericInstanceMethod(self);
            foreach (var methodArg in arguments)
            {
                reference.GenericArguments.Add(methodArg);
            }

            return reference;
        }
*/

        /// <summary>
        ///     Redirects a constructor from base.
        /// </summary>
        /// <param name="stardewContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <param name="asmType">
        ///     The <see cref="Type" /> containing the new constructor.
        /// </param>
        /// <param name="type">
        ///     The type containing the method to perform the redirections.
        /// </param>
        /// <param name="method">
        ///     The method to redirect the constructors.
        /// </param>
        /// <param name="parameters">
        ///     The parameters of the constructor.
        /// </param>
        public static void RedirectConstructorFromBase(
            CecilContext stardewContext,
            Type asmType,
            string type,
            string method,
            Type[] parameters)
        {
            var parametersString = string.Empty;
            for (var i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].FullName.Contains("`"))
                {
                    parametersString += ConvertGenericTypeFullNameToGenericTypeParameterFormat(parameters[i].FullName);
                }
                else
                {
                    parametersString += parameters[i].FullName;
                }

                if (i != parameters.Length - 1)
                {
                    parametersString += ",";
                }
            }

            var newConstructor = asmType.GetConstructor(parameters);

            if (asmType.BaseType == null)
            {
                return;
            }

            var oldConstructor = asmType.BaseType.GetConstructor(parameters);

            if (newConstructor == null)
            {
                return;
            }

            // Build a FullName version of newConstructor.Name
            var newConstructorFullName = "System.Void " + asmType.FullName + "::.ctor" + "(" + parametersString + ")";
            var newConstructorReference = stardewContext.GetMethodDefinitionFullName(
                asmType.FullName,
                newConstructorFullName);

            if (oldConstructor == null)
            {
                return;
            }

            // Build a FullName version of oldConstructor.Name
            var oldConstructorFullName = "System.Void " + asmType.BaseType.FullName + "::.ctor" + "(" + parametersString
                                         + ")";
            var oldConstructorReference =
                stardewContext.GetMethodDefinitionFullName(
                    asmType.BaseType.FullName ?? asmType.BaseType.Namespace + "." + asmType.BaseType.Name,
                    oldConstructorFullName);

            var processor = stardewContext.GetMethodIlProcessor(type, method);

            var instructions =
                processor.Body.Instructions.Where(
                    n => n.OpCode == OpCodes.Newobj && n.Operand == oldConstructorReference).ToList();
            foreach (var instruction in instructions)
            {
                processor.Replace(instruction, processor.Create(OpCodes.Newobj, newConstructorReference));
            }
        }

        /// <summary>
        ///     Redirects a constructor to a method, such as a factory method.
        /// </summary>
        /// <param name="stardewContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <param name="asmType">
        ///     The <see cref="Type" /> containing the old constructor.
        /// </param>
        /// <param name="type">
        ///     The type containing the method to perform the redirections.
        /// </param>
        /// <param name="method">
        ///     The method to redirect the constructors.
        /// </param>
        /// <param name="methodType">
        ///     The type containing the method to replace the constructor with.
        /// </param>
        /// <param name="methodName">
        ///     The method to replace the constructor with.
        /// </param>
        /// <param name="parameters">
        ///     The parameters to the replaced constructor.
        /// </param>
        public static void RedirectConstructorToMethod(
            CecilContext stardewContext,
            Type asmType,
            string type,
            string method,
            string methodType,
            string methodName,
            Type[] parameters)
        {
            // Get a single string containing the full names of all parameters, comma separated
            var parametersString = string.Empty;
            for (var i = 0; i < parameters.Length; i++)
            {
                parametersString += parameters[i].FullName;
                if (i != parameters.Length - 1)
                {
                    parametersString += ",";
                }
            }

            if (asmType == null)
            {
                return;
            }

            var oldConstructor = asmType.GetConstructor(parameters);

            if (oldConstructor == null)
            {
                return;
            }

            // Build a FullName version of oldConstructor.Name
            var oldConstructorFullName = "System.Void " + asmType.FullName + "::.ctor" + "(" + parametersString + ")";
            var oldConstructorReference =
                stardewContext.GetMethodDefinitionFullName(
                    asmType.FullName ?? asmType.Namespace + "." + asmType.Name,
                    oldConstructorFullName);

            // Build a FullName version of the new method
            string methodFullName = $"{asmType} {methodType}::{methodName}({parametersString})";
            var newMethod = stardewContext.GetMethodDefinitionFullName(methodType, methodFullName);
            if (newMethod == null)
            {
                return;
            }

            var processor = stardewContext.GetMethodIlProcessor(type, method);

            var instructions =
                processor.Body.Instructions.Where(
                    n => n.OpCode == OpCodes.Newobj && n.Operand == oldConstructorReference).ToList();
            foreach (var instruction in instructions)
            {
                processor.Body.SimplifyMacros();
                processor.Replace(instruction, processor.Call(newMethod));
                processor.Body.OptimizeMacros();
            }
        }

        /// <summary>
        ///     Replaces an IL 'call' instruction with a 'call virtual' instruction.
        /// </summary>
        /// <param name="cecilContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <param name="fullName">
        ///     The name of the type containing the method whose call is to be replaced
        /// </param>
        /// <param name="name">
        ///     The name of the method whose call is being replaced.
        /// </param>
        /// <param name="type">
        ///     The type containing the method where the replacements are to be done.
        /// </param>
        /// <param name="method">
        ///     The method where the replacements are to be done.
        /// </param>
        public static void SetVirtualCallOnMethod(
            CecilContext cecilContext,
            string fullName,
            string name,
            string type,
            string method)
        {
            var methodDefinition = cecilContext.GetMethodDefinition(fullName, name);
            var processor = cecilContext.GetMethodIlProcessor(type, method);

            var instructions =
                processor.Body.Instructions.Where(n => n.OpCode == OpCodes.Call && n.Operand == methodDefinition)
                    .ToList();
            foreach (var instruction in instructions)
            {
                processor.Replace(instruction, processor.Create(OpCodes.Callvirt, methodDefinition));
            }
        }

        /// <summary>
        ///     Sets all methods of a type to be marked as virtual.
        /// </summary>
        /// <param name="stardewContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <param name="typeName">
        ///     The name of the type to perform the operation on.
        /// </param>
        public static void SetVirtualOnBaseMethods(CecilContext stardewContext, string typeName)
        {
            var type = stardewContext.GetTypeDefinition(typeName);

            if (type.HasMethods)
            {
                foreach (var method in type.Methods.Where(n => !n.IsConstructor && !n.IsStatic))
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

        /// <summary>
        ///     Changes the protection on all members of a type.
        /// </summary>
        /// <param name="stardewContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <param name="public">
        ///     Whether the methods should be marked as public.
        /// </param>
        /// <param name="typeName">
        ///     The name of the type to perform the operation on.
        /// </param>
        public static void AlterProtectionOnTypeMembers(CecilContext stardewContext, bool @public, string typeName)
        {
            var type = stardewContext.GetTypeDefinition(typeName);

            if (type.HasMethods)
            {
                foreach (var method in type.Methods)
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
                foreach (var field in type.Fields)
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

        /// <summary>
        ///     Injects all Global Route Manager hooks.
        /// </summary>
        /// <param name="stardewContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        public static void HookAllGlobalRouteMethods(CecilContext stardewContext)
        {
            var methods =
                stardewContext.GetMethods().Where(n => n.DeclaringType.Namespace.StartsWith("StardewValley")).ToArray();

            var listenedMethodField = stardewContext.GetFieldDefinition(
                "Farmhand.Events.GlobalRouteManager",
                "ListenedMethods");
            var processor = stardewContext.GetMethodIlProcessor("Farmhand.Events.GlobalRouteManager", ".cctor");
            var loadInt = processor.Create(OpCodes.Ldc_I4, methods.Length);
            var setValue = processor.Create(OpCodes.Stsfld, listenedMethodField);
            processor.InsertAfter(processor.Body.Instructions[processor.Body.Instructions.Count - 2], loadInt);
            processor.InsertAfter(loadInt, setValue);

            for (var i = 0; i < methods.Length; ++i)
            {
                // InjectGlobalRoutePreMethod(stardewContext, methods[i].DeclaringType.FullName, methods[i].Name, i);
                InjectGlobalRoutePostMethod(stardewContext, methods[i].DeclaringType.FullName, methods[i].Name, i);
                InjectMappingInformation(stardewContext, methods[i].DeclaringType.FullName, methods[i].Name, i);
            }
        }

        private static void InjectMappingInformation(
            CecilContext stardewContext,
            string className,
            string methodName,
            int index)
        {
            var mapMethodDefinition = stardewContext.GetMethodDefinition(
                "Farmhand.Events.GlobalRouteManager",
                "MapIndex");
            var processor = stardewContext.GetMethodIlProcessor(
                "Farmhand.Events.GlobalRouteManager",
                "InitialiseMappings");

            if (processor == null || mapMethodDefinition == null)
            {
                return;
            }

            var newInstructions = new List<Instruction>
                                      {
                                          processor.Create(OpCodes.Ldstr, className),
                                          processor.Create(OpCodes.Ldstr, methodName),
                                          processor.Create(OpCodes.Ldc_I4, index),
                                          processor.Create(OpCodes.Call, mapMethodDefinition)
                                      };

            processor.Body.SimplifyMacros();
            if (newInstructions.Any())
            {
                var previousInstruction = newInstructions.First();
                if (processor.Body.Instructions.Count > 1)
                {
                    processor.InsertAfter(
                        processor.Body.Instructions[processor.Body.Instructions.Count - 2],
                        previousInstruction);
                }
                else
                {
                    processor.InsertBefore(processor.Body.Instructions[0], previousInstruction);
                }

                for (var i = 1; i < newInstructions.Count; ++i)
                {
                    processor.InsertAfter(previousInstruction, newInstructions[i]);
                    previousInstruction = newInstructions[i];
                }
            }

            processor.Body.OptimizeMacros();
        }

        /// <summary>
        ///     Alters the protection on a type.
        /// </summary>
        /// <param name="stardewContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <param name="isPublic">
        ///     Whether the type should be named public.
        /// </param>
        /// <param name="typeName">
        ///     The name of the type to perform the operation on.
        /// </param>
        public static void AlterProtectionOnType(CecilContext stardewContext, bool isPublic, string typeName)
        {
            var type = stardewContext.GetTypeDefinition(typeName);
            type.IsPublic = isPublic;
            type.IsNotPublic = !isPublic;
        }

        /// <summary>
        ///     Takes the Type.FullName property of a generic type, and converts it to a format suitable for identifying generic
        ///     types.
        /// </summary>
        /// <param name="genericTypeFullName">
        ///     The generic type full name.
        /// </param>
        /// <returns>
        ///     The modified type name.
        /// </returns>
        public static string ConvertGenericTypeFullNameToGenericTypeParameterFormat(string genericTypeFullName)
        {
            // Start with an empty string we'll build on
            var convertedString = string.Empty;

            // Everything before the first [[ can be directly lifted into the new string
            convertedString += genericTypeFullName.Split(new[] { "[[" }, StringSplitOptions.None)[0];

            // Open symbol
            convertedString += "<";

            // Spit the string after the first [[ by commas
            var commaSeparated = genericTypeFullName.Split(new[] { "[[" }, StringSplitOptions.None)[1].Split(',');

            // Iterate over the comma separated array. Every 5 commas is the string value we need, the rest is useless to us
            for (var c = 0; c < commaSeparated.Length; c += 5)
            {
                // Iterate over that string we got even further, in order to clean it up character by character
                for (var characterNum = 0; characterNum < commaSeparated[c].Length; characterNum++)
                {
                    // The only garbage symbol we should have to deal with is stray '[', but we can't get rid of them all, since we may need open and close brackets for arrays
                    // So, if a '[' doesn't have a matching ']' after it, it's garbage and can be ignored, while everything else gets tacked on our converted string
                    if (!commaSeparated[c][characterNum].Equals('[')
                        || (characterNum != commaSeparated[c].Length - 1
                        && commaSeparated[c][characterNum + 1].Equals(']')))
                    {
                        convertedString += commaSeparated[c][characterNum];
                    }
                }

                // Comma separation between the generic parameters
                if (c != commaSeparated.Length - 5)
                {
                    convertedString += ",";
                }
            }

            // Close symbol
            convertedString += ">";

            return convertedString;
        }
    }
}