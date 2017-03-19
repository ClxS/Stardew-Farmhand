namespace FarmhandInstaller.Core.Patcher
{
    /// <summary>
    ///     Possible patcher passes.
    /// </summary>
    internal enum PatcherPass
    {
        /// <summary>
        ///     The core library injection pass.
        /// </summary>
        PassOne,

        /// <summary>
        ///     The secondary library injection pass.
        /// </summary>
        PassTwo
    }
}