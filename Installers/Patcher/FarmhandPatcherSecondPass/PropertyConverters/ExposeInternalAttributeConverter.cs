namespace Farmhand.Installers.Patcher.PropertyConverters
{
    using System;
    using System.ComponentModel.Composition;

    using Farmhand.Attributes;
    using Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters;

    [Export(typeof(IExposeInternalAttributeConverter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class ExposeInternalAttributeConverter : IExposeInternalAttributeConverter
    {
        #region IExposeInternalAttributeConverter Members

        public string FullName => typeof(HookAlterProtectionAttribute).FullName;

        public void FromAttribute(Attribute attribute)
        {
            var hookAttribute = attribute as HookExposeInternal;
            if (hookAttribute == null)
            {
                throw new ArgumentException("This attribute not valid for this property provider.");
            }

            this.AssemblyName = hookAttribute.AssemblyName;
        }

        public string AssemblyName { get; private set; }

        #endregion
    }
}