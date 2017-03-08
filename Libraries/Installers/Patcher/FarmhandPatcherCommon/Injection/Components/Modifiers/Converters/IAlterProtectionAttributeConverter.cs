namespace Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters
{
    /// <summary>
    ///     The AlterProtectionAttributeConverter interface.
    /// </summary>
    public interface IAlterProtectionAttributeConverter : IAttributeConverter
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

        /// <summary>
        ///     Gets the type name.
        /// </summary>
        string TypeName { get; }
    }
}