namespace Farmhand.Installers.Patcher.Injection.Components.Parameters
{
    // ReSharper disable StyleCop.SA1600
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    using Farmhand.Installers.Patcher.Injection.Components;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    [Export(typeof(IParameterHandler))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ThisHandler<T> : IParameterHandler
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
            instructions.Add(order.ReceivingProcessor.Create(OpCodes.Ldarg, order.ReceivingProcessor.Body.ThisParameter));
        }

        #endregion
    }
}