namespace Farmhand.Installers.Patcher.Injection.Components.Parameters
{
    // ReSharper disable StyleCop.SA1600
    using System.Collections.Generic;

    using Farmhand.Installers.Patcher.Injection.Components;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    public interface IParameterHandler
    {
        void GetInstructions(InjectOrder order, CustomAttribute attribute, List<Instruction> instructions);

        bool Equals(string fullName);
    }
}