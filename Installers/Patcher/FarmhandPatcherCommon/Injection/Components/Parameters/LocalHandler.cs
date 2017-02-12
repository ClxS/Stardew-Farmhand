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
    public class LocalHandler<T> : IParameterHandler
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
            var index = attribute.ConstructorArguments[1].Value as int?;

            var inputParam =
                order.ReceivingProcessor.Body.Variables.FirstOrDefault(
                    p => p.Index == index && p.VariableType.FullName == type?.FullName);

            if (inputParam != null)
            {
                instructions.Add(order.ReceivingProcessor.Create(OpCodes.Ldloc, inputParam));
                return;
            }

            throw new Exception("Specified local variable was not found in method");
        }

        #endregion
    }
}