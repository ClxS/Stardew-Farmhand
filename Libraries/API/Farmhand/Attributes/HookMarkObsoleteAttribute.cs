namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Marks game methods/fields/properties as obsolete.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class HookMarkObsoleteAttribute : FarmhandHook
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HookMarkObsoleteAttribute" /> class.
        /// </summary>
        /// <param name="type">
        ///     The type of element.
        /// </param>
        /// <param name="typeName">
        ///     The type name.
        /// </param>
        /// <param name="elementName">
        ///     The element name.
        /// </param>
        /// <param name="message">
        ///     The message to display
        /// </param>
        /// <param name="error">
        ///     Whether usage results in an error.
        /// </param>
        public HookMarkObsoleteAttribute(
            ObsoleteElementType type,
            string typeName,
            string elementName,
            string message = null,
            bool error = true)
        {
            this.Type = type;
            this.TypeName = typeName;
            this.ElementName = elementName;
            this.Message = message;
            this.Error = error;
        }

        /// <summary>
        ///     Gets the type of element.
        /// </summary>
        public ObsoleteElementType Type { get; }

        /// <summary>
        ///     Gets the type name.
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        ///     Gets the element name.
        /// </summary>
        public string ElementName { get; }

        /// <summary>
        ///     Gets a value indicating whether usage results in an error.
        /// </summary>
        public bool Error { get; }

        /// <summary>
        ///     Gets the message to display.
        /// </summary>
        public string Message { get; }
    }
}