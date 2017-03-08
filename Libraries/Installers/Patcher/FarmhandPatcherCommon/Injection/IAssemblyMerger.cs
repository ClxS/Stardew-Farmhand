namespace Farmhand.Installers.Patcher.Injection
{
    /// <summary>
    ///     The AssemblyMerger interface.
    /// </summary>
    public interface IAssemblyMerger
    {
        /// <summary>
        ///     Merges together multiple assemblies.
        /// </summary>
        /// <param name="mergeOutput">
        ///     The output path for the merged assemblies.
        /// </param>
        /// <param name="inputAssemblies">
        ///     The assemblies to merge together.
        /// </param>
        /// <returns>
        ///     Whether the operation succeeded.
        /// </returns>
        bool Merge(string mergeOutput, params string[] inputAssemblies);
    }
}