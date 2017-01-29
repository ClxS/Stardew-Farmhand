namespace Farmhand.Installers.Patcher.Injection.Components.Hooks.Converters
{
    /// <summary>
    ///     The HookHandlerPropertyProvider interface.
    /// </summary>
    public interface IHookHandlerAttributeConverter : IAttributeConverter
    {
        /// <summary>
        ///     Gets a value indicating whether is exit.
        /// </summary>
        bool IsExit { get; }

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