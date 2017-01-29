namespace Farmhand.Installers.Patcher.Injection
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Linq;

    using ILRepacking;

    /// <summary>
    ///     Merges assemblies together using Repack.
    /// </summary>
    [Export(typeof(IAssemblyMerger))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RepackAssemblyMerger : IAssemblyMerger
    {
        #region IAssemblyMerger Members

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
        public bool Merge(string mergeOutput, params string[] inputAssemblies)
        {
            var options = new RepackOptions();
            ILogger logger = new RepackLogger();
            try
            {
                options.InputAssemblies = string.IsNullOrEmpty(PatcherOptions.AssemblyDirectory)
                                              ? inputAssemblies
                                              : inputAssemblies.Select(
                                                  n =>
                                                      Path.IsPathRooted(n)
                                                          ? n
                                                          : Path.Combine(
                                                              PatcherOptions.AssemblyDirectory,
                                                              n)).ToArray();

                options.OutputFile = mergeOutput;
                options.DebugInfo = true;
                options.SearchDirectories = string.IsNullOrEmpty(PatcherOptions.AssemblyDirectory)
                                                ? new[] { Directory.GetCurrentDirectory() }
                                                : new[] { PatcherOptions.AssemblyDirectory };

                var repack = new ILRepack(options, logger);
                repack.Repack();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: ILRepack: {ex.Message}");
                throw;
            }
            finally
            {
                if (options.PauseBeforeExit)
                {
                    Console.WriteLine("Press Any Key To Continue");
                    Console.ReadKey(true);
                }
            }

            return true;
        }

        #endregion

        #region Nested type: RepackLogger

        internal class RepackLogger : ILogger
        {
            #region ILogger Members

            public bool ShouldLogVerbose
            {
                get
                {
                    return false;
                }

                set
                {
                }
            }

            public void DuplicateIgnored(string ignoredType, object ignoredObject)
            {
            }

            public void Error(string msg)
            {
            }

            public void Info(string msg)
            {
            }

            public void Log(object str)
            {
            }

            public void Verbose(string msg)
            {
            }

            public void Warn(string msg)
            {
            }

            #endregion
        }

        #endregion
    }
}