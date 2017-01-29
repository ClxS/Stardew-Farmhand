namespace Farmhand.Installers.Patcher.Injection.Components
{
    using Farmhand.Installers.Patcher.Cecil;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    public struct InjectOrder
    {
        public MethodReference InsertedMethod;

        public ILProcessor ReceivingProcessor;

        public Instruction Target;

        public bool IsExit;

        public bool IsCancellable;

        public CecilContext CecilContext;
    }
}