namespace Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters
{
    /// <summary>
    ///     The MakeObsoleteAttributeConverter interface.
    /// </summary>
    public interface IMakeObsoleteAttributeConverter : IAttributeConverter
    {
        /// <summary>
        ///     Gets the type name.
        /// </summary>
        string TypeName { get; }

        /// <summary>
        ///     Gets the element name.
        /// </summary>
        string ElementName { get; }

        /// <summary>
        ///     Gets a value indicating whether usage causes an error.
        /// </summary>
        bool IsError { get; }

        /// <summary>
        ///     Gets a value indicating whether element is a field.
        /// </summary>
        bool IsField { get; }

        /// <summary>
        ///     Gets a value indicating whether element is a property.
        /// </summary>
        bool IsProperty { get; }

        /// <summary>
        ///     Gets a value indicating whether element is a method.
        /// </summary>
        bool IsMethod { get; }

        /// <summary>
        ///     Gets the message to display.
        /// </summary>
        string Message { get; }
    }
}