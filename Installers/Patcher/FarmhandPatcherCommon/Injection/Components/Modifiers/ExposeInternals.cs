namespace Farmhand.Installers.Patcher.Injection.Components.Modifiers
{
    // ReSharper disable StyleCop.SA1600
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using Farmhand.Installers.Patcher.Cecil;
    using Farmhand.Installers.Patcher.Injection.Components.Hooks;
    using Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters;

    using Mono.Cecil;

    [Export(typeof(IHookHandler))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ExposeInternals : IHookHandler
    {
        private readonly CecilContext cecilContext;

        private readonly IExposeInternalAttributeConverter propertyConverter;

        [ImportingConstructor]
        public ExposeInternals(
            IExposeInternalAttributeConverter propertyConverter,
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
        public void PerformAlteration(Attribute attribute, string type, string method)
        {
            this.propertyConverter.FromAttribute(attribute);

            if (string.IsNullOrEmpty(this.propertyConverter.AssemblyName))
            {
                return;
            }
            
            var stringType = this.cecilContext.GetTypeReference(typeof(string));
            var generatedCodeCtor =
                this.cecilContext.ImportMethod(
                    typeof(InternalsVisibleToAttribute).GetConstructor(new[] { typeof(string) }));
           
            var result = new CustomAttribute(generatedCodeCtor);
            result.ConstructorArguments.Add(new CustomAttributeArgument(stringType, this.propertyConverter.AssemblyName));

            this.cecilContext.AssemblyDefinition.CustomAttributes.Add(result);
        }

        public bool Equals(string fullName)
        {
            return this.propertyConverter.FullName == fullName;
        }

        #endregion
    }
}