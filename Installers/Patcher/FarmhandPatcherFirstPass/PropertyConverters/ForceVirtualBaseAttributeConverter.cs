namespace Farmhand.Installers.Patcher.PropertyConverters
{
    using System;
    using System.ComponentModel.Composition;

    using Farmhand.Attributes;
    using Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters;

    [Export(typeof(IForceVirtualBaseAttributeConverter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class ForceVirtualBaseAttributeConverter : IForceVirtualBaseAttributeConverter
    {
        #region IForceVirtualBaseAttributeConverter Members

        public void FromAttribute(Attribute attribute)
        {
            throw new NotImplementedException();
        }

        public string FullName => typeof(HookForceVirtualBaseAttribute).FullName;

        #endregion
    }
}