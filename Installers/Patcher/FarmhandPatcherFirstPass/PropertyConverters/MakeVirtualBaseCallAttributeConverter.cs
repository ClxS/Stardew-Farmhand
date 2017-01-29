namespace Farmhand.Installers.Patcher.PropertyConverters
{
    using System;
    using System.ComponentModel.Composition;

    using Farmhand.Attributes;
    using Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters;

    [Export(typeof(IMakeVirtualBaseCallAttributeConverter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class MakeVirtualBaseCallAttributeConverter :
        IMakeVirtualBaseCallAttributeConverter
    {
        #region IRedirectConstructorFromBaseAttributeConverter Members

        /// <summary>
        ///     Gets the type.
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        ///     Gets the method.
        /// </summary>
        public string Method { get; private set; }

        public string FullName => typeof(HookMakeBaseVirtualCallAttribute).FullName;

        public void FromAttribute(Attribute attribute)
        {
            var hookAttribute = attribute as HookMakeBaseVirtualCallAttribute;
            if (hookAttribute == null)
            {
                throw new ArgumentException("This attribute not valid for this property provider.");
            }

            this.Type = hookAttribute.Type;
            this.Method = hookAttribute.Method;
        }

        #endregion
    }
}