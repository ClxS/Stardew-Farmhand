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
    public class AlterBaseProtectionHandler : IHookHandler
    {
        private readonly CecilContext cecilContext;

        private readonly IAlterBaseFieldProtectionAttributeConverter propertyConverter;

        [ImportingConstructor]
        public AlterBaseProtectionHandler(
            IAlterBaseFieldProtectionAttributeConverter propertyConverter,
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

            var thisType = this.cecilContext.GetTypeDefinition(typeName);
            if (thisType.BaseType == null)
            {
                throw new Exception("AlterBaseTypeProtection can only be applied to classes with a base type");    
            }

            var type = this.cecilContext.GetTypeDefinition(thisType.BaseType.FullName);

            if (type.HasMethods)
            {
                foreach (var method in type.Methods)
                {
                    switch (this.propertyConverter.MinimumProtectionLevel)
                    {
                        case 1:
                            if (method.IsPrivate)
                            {
                                method.IsPrivate = false;
                                method.IsFamily = true;
                            }

                            break;
                        case 2:
                            if (method.IsPrivate || method.IsFamily)
                            {
                                method.IsPrivate = false;
                                method.IsPublic = false;
                            }

                            break;
                    }
                }
            }

            if (type.HasFields)
            {
                foreach (var field in type.Fields)
                {
                    switch (this.propertyConverter.MinimumProtectionLevel)
                    {
                        case 1:
                            if (field.IsPrivate)
                            {
                                field.IsPrivate = false;
                                field.IsFamily = true;
                            }

                            break;
                        case 2:
                            if (field.IsPrivate || field.IsFamily)
                            {
                                field.IsPrivate = false;
                                field.IsPublic = true;
                            }

                            break;
                    }
                }
            }
        }

        public bool Equals(string fullName)
        {
            return this.propertyConverter.FullName == fullName;
        }

        #endregion
    }
}