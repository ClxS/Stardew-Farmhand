namespace Farmhand.Installers.Patcher.Injection.Components.Parameters
{
    // ReSharper disable StyleCop.SA1600
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    using Farmhand.Installers.Patcher.Injection.Components;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    [Export(typeof(IParameterHandler))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class InputHandler<T> : IParameterHandler
    {
        #region IAttributeHandler Members

        public bool Equals(string fullName)
        {
            return typeof(T).FullName == fullName;
        }

        public void GetInstructions(
            InjectOrder order,
            CustomAttribute attribute,
            List<Instruction> instructions)
        {
            var type = attribute.ConstructorArguments[0].Value as TypeReference;
            var name = attribute.ConstructorArguments[1].Value as string;

            var inputParam =
                order.ReceivingProcessor.Body.Method.Parameters.FirstOrDefault(
                    p => p.Name == name && p.ParameterType.FullName == type?.FullName);

            if (inputParam != null)
            {
                if ((bool)attribute.ConstructorArguments[2].Value)
                {
                    instructions.Add(order.ReceivingProcessor.Create(OpCodes.Ldarga, inputParam));
                }

                instructions.Add(order.ReceivingProcessor.Create(OpCodes.Ldarg, inputParam));
                return;
            }

            throw new Exception("The specified parameter was not found on this method");
        }

        #endregion
    }
}