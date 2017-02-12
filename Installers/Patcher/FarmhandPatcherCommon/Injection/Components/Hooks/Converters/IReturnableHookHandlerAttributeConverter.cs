namespace Farmhand.Installers.Patcher.Injection.Components.Hooks.Converters
{
    /// <summary>
    ///     The IReturnableHookHandlerAttributeConverter interface.
    /// </summary>
    public interface IReturnableHookHandlerAttributeConverter : IAttributeConverter
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