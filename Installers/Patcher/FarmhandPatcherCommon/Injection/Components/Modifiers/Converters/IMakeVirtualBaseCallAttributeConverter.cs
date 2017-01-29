namespace Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters
{
    using System;

    /// <summary>
    ///     The IMakeVirtualBaseCallAttributeConverter interface.
    /// </summary>
    public interface IMakeVirtualBaseCallAttributeConverter : IAttributeConverter
    {
        /// <summary>
        ///     Gets the type.
        /// </summary>
        string Type { get; }

        /// <summary>
        ///     Gets the method.
        /// </summary>
        string Method { get; }
    }
}
