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
    public class HookHandler<TParam> : IHookHandler
        where TParam : Attribute
    {
        private readonly CecilContext cecilContext;

        private readonly IEnumerable<IParameterHandler> parameterHandlers;

        private readonly IHookHandlerAttributeConverter propertyConverter;
        
        [ImportingConstructor]
        public HookHandler(
            [ImportMany] IEnumerable<IParameterHandler> parameterHandlers,
            IHookHandlerAttributeConverter propertyProvider,
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

            List<InjectOrder> orders = new List<InjectOrder>();
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

        private void GatherInjectionOrders(string injectedType, string injectedMethod, List<InjectOrder> orders)
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
                foreach (
                    var retInst in
                    processor.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret))
                {
                    orders.Add(new InjectOrder()
                    {
                        CecilContext = this.cecilContext,
                        InsertedMethod = method,
                        IsCancellable = method.ReturnType != null
                        && method.ReturnType.FullName == typeof(bool).FullName,
                        IsExit = this.propertyConverter.IsExit,
                        ReceivingProcessor = processor,
                        Target = retInst
                    });
                }
            }
            else
            {
                orders.Add(new InjectOrder()
                {
                    CecilContext = this.cecilContext,
                    InsertedMethod = method,
                    IsCancellable = method.ReturnType != null
                        && method.ReturnType.FullName == typeof(bool).FullName,
                    IsExit = this.propertyConverter.IsExit,
                    ReceivingProcessor = processor,
                    Target = processor.Body.Instructions.First()
                });
            }
        }
        
        private void InjectMethod(
            InjectOrder order)
        {
            order.ReceivingProcessor.Body.SimplifyMacros();

            var instructions = new List<Instruction>();

            if (order.InsertedMethod.HasThis)
            {
                instructions.Add(order.ReceivingProcessor.Create(OpCodes.Ldarg_0));
            }

            if (order.InsertedMethod.HasParameters)
            {
                foreach (var parameter in order.InsertedMethod.Parameters)
                {
                    this.GetParameterInstructions(order, parameter, instructions);
                }
            }

            instructions.Add(order.ReceivingProcessor.Create(OpCodes.Call, order.InsertedMethod));

            if (order.IsCancellable)
            {
                if (order.ReceivingProcessor.Body.Method.MethodReturnType.ReturnType.FullName
                    != this.cecilContext.GetTypeReference(typeof(void)).FullName)
                {
                    throw new InvalidOperationException(
                        "Cancellable hooks are only supported for methods returning void");
                }

                instructions.Add(
                    order.ReceivingProcessor.Create(OpCodes.Brtrue, order.ReceivingProcessor.Body.Instructions.Last()));
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