namespace Farmhand.Installers.Patcher.PropertyConverters
{
    using System;
    using System.ComponentModel.Composition;

    using Farmhand.Attributes;
    using Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters;

    [Export(typeof(IAlterBaseFieldProtectionAttributeConverter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class AlterBaseFieldProtectionAttributeConverter :
        IAlterBaseFieldProtectionAttributeConverter
    {
        #region IAlterBaseFieldProtectionAttributeConverter Members

        public void FromAttribute(Attribute attribute)
        {
            var hookAttribute = attribute as HookAlterBaseFieldProtectionAttribute;
            if (hookAttribute == null)
            {
                throw new ArgumentException("This attribute not valid for this property provider.");
            }

            switch (hookAttribute.Protection)
            {
                case LowestProtection.Private:
                    this.MinimumProtectionLevel = 0;
                    break;
                case LowestProtection.Protected:
                    this.MinimumProtectionLevel = 1;
                    break;
                case LowestProtection.Public:
                    this.MinimumProtectionLevel = 2;
                    break;
            }
        }

        public string FullName => typeof(HookAlterBaseFieldProtectionAttribute).FullName;

        public int MinimumProtectionLevel { get; private set; }

        #endregion
    }
}