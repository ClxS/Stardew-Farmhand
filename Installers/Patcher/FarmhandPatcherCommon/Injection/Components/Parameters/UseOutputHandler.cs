namespace Farmhand.Installers.Patcher.Injection.Components.Parameters
{
    // ReSharper disable StyleCop.SA1600
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    using Farmhand.Installers.Patcher.Injection.Components;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    [Export(typeof(IParameterHandler))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class UseOutputHandler<T> : IParameterHandler
    {
        #region IParameterHandler Members

        public bool Equals(string fullName)
        {
            return typeof(T).FullName == fullName;
        }

        public void GetInstructions(
            InjectOrder order,
            CustomAttribute attribute,
            List<Instruction> instructions)
        {
            if (order.ReceivingProcessor.Body.Variables.All(n => n.Name != "UseReturnVal"))
            {
                var boolType = order.CecilContext.GetTypeReference(typeof(bool));
                order.ReceivingProcessor.Body.Variables.Add(new VariableDefinition("UseReturnVal", boolType));
            }

            var outputVar = order.ReceivingProcessor.Body.Variables.First(n => n.Name == "UseReturnVal");
            instructions.Add(order.ReceivingProcessor.Create(OpCodes.Ldloca, outputVar));
        }

        #endregion
    }
}