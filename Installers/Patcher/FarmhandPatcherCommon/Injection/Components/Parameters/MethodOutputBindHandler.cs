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
    public class MethodOutputBindHandler<T> : IParameterHandler
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
            if (order.IsExit)
            {
                if (order.ReceivingProcessor.Body.Variables.All(n => n.Name != "OldReturnContainer"))
                {
                    var outVarContainer = new VariableDefinition(
                        "OldReturnContainer",
                        order.ReceivingProcessor.Body.Method.ReturnType);
                    order.ReceivingProcessor.Body.Variables.Add(outVarContainer);
                    instructions.Insert(0, order.ReceivingProcessor.Create(OpCodes.Dup));
                    instructions.Insert(
                        1,
                        order.ReceivingProcessor.Create(OpCodes.Stloc, outVarContainer));
                }
            }
            else
            {
                throw new Exception("MethodOutputBind can only be used on exit methods");
            }

            var outputVar =
                order.ReceivingProcessor.Body.Variables.First(n => n.Name == "OldReturnContainer");
            instructions.Add(order.ReceivingProcessor.Create(OpCodes.Ldloc, outputVar));
        }

        #endregion
    }
}