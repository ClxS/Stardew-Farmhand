namespace Farmhand.Installers.Patcher.Injection.Components.Hooks
{
    // ReSharper disable StyleCop.SA1600
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    using Farmhand.Installers.Patcher.Cecil;
    using Farmhand.Installers.Patcher.Injection.Components.Hooks.Converters;
    using Farmhand.Installers.Patcher.Injection.Components.Parameters;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Cecil.Rocks;

    [Export(typeof(IHookHandler))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ReturnableHookHandler<TParam> : IHookHandler
        where TParam : Attribute
    {
        private readonly CecilContext cecilContext;

        private readonly IEnumerable<IParameterHandler> parameterHandlers;

        private readonly IReturnableHookHandlerAttributeConverter propertyConverter;

        [ImportingConstructor]
        public ReturnableHookHandler(
            [ImportMany] IEnumerable<IParameterHandler> parameterHandlers,
            IReturnableHookHandlerAttributeConverter propertyProvider,
            IInjectionContext injectionContext)
        {
            this.parameterHandlers = parameterHandlers;
            this.propertyConverter = propertyProvider;
            var context = injectionContext as CecilContext;
            if (context != null)
            {
                this.cecilContext = context;
            }
            else
            {
                throw new Exception(
                    $"CecilInjectionProcessor is only compatible with {nameof(CecilContext)}.");
            }
        }

        #region IHookHandler Members

        public void PerformAlteration(Attribute attribute, string type, string method)
        {
            this.propertyConverter.FromAttribute(attribute);

            var orders = new List<InjectOrder>();
            this.GatherInjectionOrders(type, method, orders);

            foreach (var order in orders)
            {
                this.InjectMethod(order);
            }
        }

        public bool Equals(string fullName)
        {
            return this.propertyConverter.FullName == fullName;
        }

        #endregion

        private void GatherInjectionOrders(
            string injectedType,
            string injectedMethod,
            List<InjectOrder> orders)
        {
            MethodReference method = this.cecilContext.GetMethodDefinition(
                injectedType,
                injectedMethod);
            if (method.HasGenericParameters)
            {
                var injectReceiverMethodDef =
                    this.cecilContext.GetMethodDefinition(
                        this.propertyConverter.Type,
                        this.propertyConverter.Method);

                var inst = new GenericInstanceMethod(method);
                for (var i = 0; i < method.GenericParameters.Count; ++i)
                {
                    inst.GenericArguments.Add(
                        injectReceiverMethodDef.DeclaringType.GenericParameters[i]);
                }

                method = inst;
            }

            var processor = this.cecilContext.GetMethodIlProcessor(
                this.propertyConverter.Type,
                this.propertyConverter.Method);

            if (this.propertyConverter.IsExit)
            {
                foreach (var retInst in
                    processor.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret))
                {
                    orders.Add(
                        new InjectOrder
                            {
                                CecilContext = this.cecilContext,
                                InsertedMethod = method,
                                IsCancellable =
                                    method.ReturnType != null
                                    && method.ReturnType.FullName
                                    == typeof(bool).FullName,
                                IsExit = this.propertyConverter.IsExit,
                                ReceivingProcessor = processor,
                                Target = retInst
                            });
                }
            }
            else
            {
                orders.Add(
                    new InjectOrder
                        {
                            CecilContext = this.cecilContext,
                            InsertedMethod = method,
                            IsCancellable =
                                method.ReturnType != null
                                && method.ReturnType.FullName == typeof(bool).FullName,
                            IsExit = this.propertyConverter.IsExit,
                            ReceivingProcessor = processor,
                            Target = processor.Body.Instructions.First()
                        });
            }
        }

        private void InjectMethod(InjectOrder order)
        {
            order.ReceivingProcessor.Body.SimplifyMacros();

            // Add UseReturnVal variable
            if (order.ReceivingProcessor.Body.Variables.All(n => n.Name != "UseReturnVal"))
            {
                var boolType = order.CecilContext.GetTypeReference(typeof(bool));
                order.ReceivingProcessor.Body.Variables.Add(
                    new VariableDefinition("UseReturnVal", boolType));
            }

            var useOutputVariable =
                order.ReceivingProcessor.Body.Variables.First(n => n.Name == "UseReturnVal");

            if (
                order.ReceivingProcessor.Body.Variables.All(
                    n => n.Name != "ReturnContainer-" + order.InsertedMethod.ReturnType.Name))
            {
                order.ReceivingProcessor.Body.Variables.Add(
                    new VariableDefinition(
                        "ReturnContainer-" + order.InsertedMethod.ReturnType.Name,
                        order.InsertedMethod.ReturnType));
            }

            if (order.IsExit)
            {
                if (order.ReceivingProcessor.Body.Variables.All(n => n.Name != "OldReturnContainer"))
                {
                    order.ReceivingProcessor.Body.Variables.Add(
                        new VariableDefinition(
                            "OldReturnContainer",
                            order.InsertedMethod.ReturnType));
                }
            }

            var containerVariable =
                order.ReceivingProcessor.Body.Variables.First(
                    n => n.Name == "ReturnContainer-" + order.InsertedMethod.ReturnType.Name);
            var oldContainerVariable =
                order.ReceivingProcessor.Body.Variables.FirstOrDefault(
                    n => n.Name == "OldReturnContainer");

            var instructions = new List<Instruction>();
            if (order.IsExit)
            {
                instructions.Add(
                    order.ReceivingProcessor.Create(OpCodes.Stloc, oldContainerVariable));
            }

            if (order.InsertedMethod.HasParameters)
            {
                foreach (var parameter in order.InsertedMethod.Parameters)
                {
                    this.GetParameterInstructions(order, parameter, instructions);
                }
            }

            instructions.Add(order.ReceivingProcessor.Create(OpCodes.Call, order.InsertedMethod));
            instructions.Add(order.ReceivingProcessor.Create(OpCodes.Stloc, containerVariable));
            instructions.Add(order.ReceivingProcessor.Create(OpCodes.Ldloc, useOutputVariable));
            if (!order.IsExit)
            {
                instructions.Add(order.ReceivingProcessor.Create(OpCodes.Brfalse, order.Target));
                instructions.Add(order.ReceivingProcessor.Create(OpCodes.Ldloc, containerVariable));
                instructions.Add(
                    order.ReceivingProcessor.Create(
                        OpCodes.Br,
                        order.ReceivingProcessor.Body.Instructions.Last()));
            }
            else
            {
                var branchToRet = order.ReceivingProcessor.Create(OpCodes.Br, order.Target);
                var loadOld = order.ReceivingProcessor.Create(OpCodes.Ldloc, oldContainerVariable);
                var loadNew = order.ReceivingProcessor.Create(OpCodes.Ldloc, containerVariable);
                instructions.Add(order.ReceivingProcessor.Create(OpCodes.Brfalse, loadOld));
                instructions.Add(loadNew);
                instructions.Add(branchToRet);
                instructions.Add(loadOld);
            }

            foreach (var inst in instructions)
            {
                order.ReceivingProcessor.InsertBefore(order.Target, inst);
            }

            order.ReceivingProcessor.Body.OptimizeMacros();
        }

        private void GetParameterInstructions(InjectOrder order, ParameterDefinition parameter, List<Instruction> instruction)
        {
            var attribute =
                parameter.CustomAttributes.FirstOrDefault(
                    n =>
                        n.AttributeType.IsDefinition
                        && n.AttributeType.Resolve().BaseType?.FullName == typeof(TParam).FullName);
            if (attribute == null)
            {
                throw new Exception("Non API parameter type encountered");
            }

            var paramMatches =
                this.parameterHandlers.Where(p => p.Equals(attribute.AttributeType.FullName)).ToArray();

            if (!paramMatches.Any())
            {
                throw new Exception(
                    $"Found no parameter handlers for {attribute.AttributeType.FullName}");
            }

            if (paramMatches.Length > 1)
            {
                throw new Exception(
                    $"Encountered multiple parameter handlers for {attribute.AttributeType.FullName}");
            }

            paramMatches.First().GetInstructions(order, attribute, instruction);
        }
    }
}