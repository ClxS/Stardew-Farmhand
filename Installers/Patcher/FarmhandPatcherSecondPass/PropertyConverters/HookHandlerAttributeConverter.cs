namespace Farmhand.Installers.Patcher.PropertyConverters
{
    using System;
    using System.ComponentModel.Composition;

    using Farmhand.Attributes;
    using Farmhand.Installers.Patcher.Injection.Components.Hooks.Converters;

    [Export(typeof(IHookHandlerAttributeConverter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class HookHandlerAttributeConverter : IHookHandlerAttributeConverter
    {
        #region IHookHandlerAttributeConverter Members

        public void FromAttribute(Attribute attribute)
        {
            var hookAttribute = attribute as HookAttribute;
            if (hookAttribute == null)
            {
                throw new ArgumentException("This attribute not valid for this property provider.");
            }

            this.IsExit = hookAttribute.HookType == HookType.Exit;
            this.Type = hookAttribute.Type;
            this.Method = hookAttribute.Method;
        }

        public bool IsExit { get; private set; }

        public string Type { get; private set; }

        public string Method { get; private set; }

        public string FullName => typeof(HookAttribute).FullName;

        #endregion
    }
}