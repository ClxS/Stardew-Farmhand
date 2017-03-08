namespace Farmhand.Installers.Patcher.PropertyConverters
{
    using System;
    using System.ComponentModel.Composition;

    using Farmhand.Attributes;
    using Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters;

    [Export(typeof(IMakeObsoleteAttributeConverter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class MakeObsoleteAttributeConverter : IMakeObsoleteAttributeConverter
    {
        #region IMakeObsoleteAttributeConverter Members

        public void FromAttribute(Attribute attribute)
        {
            var hookAttribute = attribute as HookMarkObsoleteAttribute;
            if (hookAttribute == null)
            {
                throw new ArgumentException("This attribute not valid for this property provider.");
            }

            this.TypeName = hookAttribute.TypeName;
            this.ElementName = hookAttribute.ElementName;
            this.IsError = hookAttribute.Error;
            this.IsField = hookAttribute.Type == ObsoleteElementType.Field;
            this.IsProperty = hookAttribute.Type == ObsoleteElementType.Property;
            this.IsMethod = hookAttribute.Type == ObsoleteElementType.Method;
            this.Message = hookAttribute.Message;
        }

        public string FullName => typeof(HookMarkObsoleteAttribute).FullName;

        public string TypeName { get; private set; }

        public string ElementName { get; private set; }

        public bool IsError { get; private set; }

        public bool IsField { get; private set; }

        public bool IsProperty { get; private set; }

        public bool IsMethod { get; private set; }

        public string Message { get; private set; }

        #endregion
    }
}