namespace Farmhand.Installers.Patcher.Injection.Components.Modifiers
{
    // ReSharper disable StyleCop.SA1600
    using System;
    using System.ComponentModel.Composition;

    using Farmhand.Installers.Patcher.Cecil;
    using Farmhand.Installers.Patcher.Injection.Components.Hooks;
    using Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters;

    [Export(typeof(IHookHandler))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class AlterProtectionHandler : IHookHandler
    {
        private readonly CecilContext cecilContext;

        private readonly IAlterProtectionAttributeConverter propertyConverter;

        [ImportingConstructor]
        public AlterProtectionHandler(
            IAlterProtectionAttributeConverter propertyConverter,
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

            bool isPublic = this.propertyConverter.MinimumProtectionLevel == 2;
            var type = this.cecilContext.GetTypeDefinition(this.propertyConverter.TypeName);
            type.IsPublic = isPublic;
            type.IsNotPublic = !isPublic;
        }

        public bool Equals(string fullName)
        {
            return this.propertyConverter.FullName == fullName;
        }

        #endregion
    }
}