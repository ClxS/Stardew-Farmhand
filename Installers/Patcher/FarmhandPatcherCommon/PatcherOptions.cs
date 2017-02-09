namespace Farmhand.Installers.Patcher
{
    /// <summary>
    ///     The Patch options.
    /// </summary>
    public static class PatcherOptions
    {
        /// <summary>
        ///     Gets or sets a value indicating whether the Global Route Manager is disabled.
        /// </summary>
        public static bool DisableGrm { get; set; } = false;

        /// <summary>
        ///     Gets or sets a value indicating whether the attribute to make things obsolete should be skipped.
        /// </summary>
        public static bool NoObsolete { get; set; } = false;

        /// <summary>
        ///     Gets or sets the output override. This can be used to redirect the patching output binary.
        /// </summary>
        public static string OutputOverride { get; set; } = null;

        /// <summary>
        ///     Gets or sets the assembly directory.
        /// </summary>
        public static string AssemblyDirectory { get; set; } = string.Empty;
    }
}