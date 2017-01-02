namespace Farmhand.Installers.Patcher
{
    /// <summary>
    ///     The Patch options.
    /// </summary>
    public class PatcherOptions
    {
        /// <summary>
        ///     Gets or sets a value indicating whether the Global Route Manager is disabled.
        /// </summary>
        public bool DisableGrm { get; set; } = false;

        /// <summary>
        ///     Gets or sets the assembly directory.
        /// </summary>
        public string AssemblyDirectory { get; set; } = string.Empty;
    }
}