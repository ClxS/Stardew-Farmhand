namespace Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters
{
    /// <summary>
    ///     The ExposeInternalAttributeConverter interface.
    /// </summary>
    public interface IExposeInternalAttributeConverter : IAttributeConverter
    {
        /// <summary>
        ///     Gets the name of the to provide access to assembly.
        /// </summary>
        string AssemblyName { get; }
    }
}