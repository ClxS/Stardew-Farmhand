namespace Farmhand.Installers.Patcher.Injection.Components.Modifiers
{
    // ReSharper disable StyleCop.SA1600
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;

    using Farmhand.Installers.Patcher.Cecil;
    using Farmhand.Installers.Patcher.Injection.Components.Hooks;
    using Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters;

    [Export(typeof(IHookHandler))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ForceVirtualBaseHandler : IHookHandler
    {
        private readonly CecilContext cecilContext;

        private readonly IForceVirtualBaseAttributeConverter propertyConverter;

        [ImportingConstructor]
        public ForceVirtualBaseHandler(
            IForceVirtualBaseAttributeConverter propertyConverter,
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
            var thisType = this.cecilContext.GetTypeDefinition(typeName);
            if (thisType.BaseType == null)
            {
                throw new Exception(
                    "ForceVirtualBase can only be applied to classes with a base type");
            }

            var type = this.cecilContext.GetTypeDefinition(thisType.BaseType.FullName);

            if (type.HasMethods)
            {
                foreach (var method in type.Methods.Where(n => !n.IsConstructor && !n.IsStatic))
                {
                    if (!method.IsVirtual)
                    {
                        method.IsVirtual = true;
                        method.IsNewSlot = true;
                        method.IsReuseSlot = false;
                        method.IsHideBySig = true;
                    }

                    method.IsFinal = false;
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