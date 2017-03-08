namespace Farmhand.Installers.Patcher.PropertyConverters
{
    using System;
    using System.ComponentModel.Composition;

    using Farmhand.Attributes;
    using Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters;

    [Export(typeof(IRedirectConstructorToMethodAttributeConverter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class RedirectConstructorToMethodAttributeConverter :
        IRedirectConstructorToMethodAttributeConverter
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

        /// <summary>
        ///     Gets the parameters.
        /// </summary>
        public Type[] Parameters { get; private set; }

        /// <summary>
        ///     Gets the generic arguments.
        /// </summary>
        public Type[] GenericArguments { get; private set; }

        public string FullName => typeof(HookRedirectConstructorToMethodAttribute).FullName;

        public void FromAttribute(Attribute attribute)
        {
            var hookAttribute = attribute as HookRedirectConstructorToMethodAttribute;
            if (hookAttribute == null)
            {
                throw new ArgumentException("This attribute not valid for this property provider.");
            }

            this.Type = hookAttribute.Type;
            this.Method = hookAttribute.Method;
            this.GenericArguments = hookAttribute.GenericArguments;
        }

        #endregion
    }
}