namespace Farmhand.Installers.Patcher.PropertyConverters
{
    using System;
    using System.ComponentModel.Composition;

    using Farmhand.Attributes;
    using Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters;

    [Export(typeof(IAlterProtectionAttributeConverter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class AlterProtectionAttributeConverter : IAlterProtectionAttributeConverter
    {
        public string FullName => typeof(HookAlterProtectionAttribute).FullName;

        #region IAlterProtectionAttributeConverter Members

        public int MinimumProtectionLevel { get; private set; }

        public string TypeName { get; private set; }

        #endregion

        public void FromAttribute(Attribute attribute)
        {
            var hookAttribute = attribute as HookAlterProtectionAttribute;
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

            this.TypeName = hookAttribute.ClassName;
        }
    }
}