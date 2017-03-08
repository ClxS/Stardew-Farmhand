namespace Farmhand.Installers.Patcher.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Farmhand.Installers.Patcher.Cecil;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Cecil.Rocks;

    /// <summary>
    ///     A utility class which provides various common methods related to IL manipulation.
    /// </summary>
    public static class CecilHelper
    {
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
            if (listenedMethodField == null)
            {
                throw new NullReferenceException(
                    "Failed to get field definition for (Farmhand.Events.GlobalRouteManager.ListenedMethods)");
            }

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
    }
}