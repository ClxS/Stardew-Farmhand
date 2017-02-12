namespace Farmhand.Installers.Patcher.Injection
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    ///     The InjectionContext interface.
    /// </summary>
    public interface IInjectionContext
    {
        /// <summary>
        ///     Gets the loaded assemblies.
        /// </summary>
        IEnumerable<Assembly> LoadedAssemblies { get; }

        /// <summary>
        ///     Loads an assembly.
        /// </summary>
        /// <param name="file">
        ///     The path of the assembly to load.
        /// </param>
        void LoadAssembly(string file);

        /// <summary>
        ///     Sets the primary assembly.
        /// </summary>
        /// <param name="file">
        ///     The path of the assembly.
        /// </param>
        /// <param name="loadDebugInformation">
        ///     Whether debug symbols should also be loaded.
        /// </param>
        void SetPrimaryAssembly(string file, bool loadDebugInformation);

        /// <summary>
        ///     Writes the modified assembly to disk.
        /// </summary>
        /// <param name="file">
        ///     The output file.
        /// </param>
        /// <param name="writePdb">
        ///     Whether an updated PDB should also be written.
        /// </param>
        void WriteAssembly(string file, bool writePdb = false);
    }
}