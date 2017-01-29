namespace Farmhand.Installers.Patcher.Injection.Components.Modifiers
{
    // ReSharper disable StyleCop.SA1600
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;

    using Farmhand.Installers.Patcher.Cecil;
    using Farmhand.Installers.Patcher.Injection.Components.Hooks;
    using Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters;

    using Mono.Cecil.Cil;

    [Export(typeof(IHookHandler))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MakeVirtualBaseCallHandler : IHookHandler
    {
        private readonly CecilContext cecilContext;

        private readonly IMakeVirtualBaseCallAttributeConverter propertyConverter;

        [ImportingConstructor]
        public MakeVirtualBaseCallHandler(
            IMakeVirtualBaseCallAttributeConverter propertyConverter,
            IInjectionContext injectionContext)
        {
            this.propertyConverter = propertyConverter;
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

        public void PerformAlteration(Attribute attribute, string typeName, string methodName)
        {
            this.propertyConverter.FromAttribute(attribute);

            var asmTypeSet =
                this.cecilContext.LoadedAssemblies.Select(a => a.GetType(typeName)).Where(n => n != null).ToArray();
            if (!asmTypeSet.Any())
            {
                throw new Exception("No types found matching name " + typeName);
            }

            if (asmTypeSet.Length > 1)
            {
                throw new Exception("Multiple types found matching name " + typeName);
            }

            var asmType = asmTypeSet.First();

            if (asmType?.BaseType == null)
            {
                throw new Exception("Invalid type provided.");
            }

            var methodDefinition = this.cecilContext.GetMethodDefinition(asmType.BaseType.FullName, methodName);
            var processor = this.cecilContext.GetMethodIlProcessor(this.propertyConverter.Type, this.propertyConverter.Method);

            var instructions =
                processor.Body.Instructions.Where(n => n.OpCode == OpCodes.Call && n.Operand == methodDefinition)
                    .ToList();
            foreach (var instruction in instructions)
            {
                processor.Replace(instruction, processor.Create(OpCodes.Callvirt, methodDefinition));
            }
        }

        public bool Equals(string fullName)
        {
            return this.propertyConverter.FullName == fullName;
        }

        #endregion
    }
}