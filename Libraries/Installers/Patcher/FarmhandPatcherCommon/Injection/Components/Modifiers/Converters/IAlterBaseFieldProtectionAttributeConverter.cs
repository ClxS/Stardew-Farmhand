namespace Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters
{
    /// <summary>
    ///     The HookHandlerPropertyProvider interface.
    /// </summary>
    public interface IAlterBaseFieldProtectionAttributeConverter : IAttributeConverter
    {
        /// <summary>
        ///     Gets the minimum protection level.
        /// </summary>
        /// <remarks>
        ///     0 = Private
        ///     1 = Protected
        ///     2 = Public
        /// </remarks>
        int MinimumProtectionLevel { get; }
    }
}