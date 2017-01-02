namespace Farmhand.Installers.Patcher
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Farmhand.Installers.Patcher.Cecil;
    using Farmhand.Installers.Patcher.Helpers;

    using ILRepacking;

    /// <summary>
    ///     Used by the installers to modify the game executable
    /// </summary>
    public abstract class Patcher
    {
        /// <summary>
        ///     Gets or sets the farmhand assemblies.
        /// </summary>
        protected List<Assembly> FarmhandAssemblies { get; set; } = new List<Assembly>();

        /// <summary>
        ///     Gets the options.
        /// </summary>
        public PatcherOptions Options { get; } = new PatcherOptions();

        /// <summary>
        ///     Patches the games executable, injecting our libraries and making changes found via attributes.
        /// </summary>
        /// <param name="path">
        ///     Exe Path (Pass 1), or Farmhand Output Path (Pass 2)
        /// </param>
        public abstract void PatchStardew(string path = null);

        /// <summary>
        ///     Gets the assembly path. If an AssemblyDirectory is defined, it will use that as the root assembly directory,
        ///     otherwise it returns the same value as was input.
        /// </summary>
        /// <param name="assembly">
        ///     The assembly to get the path to
        /// </param>
        /// <returns>
        ///     The path to the assembly..
        /// </returns>
        public string GetAssemblyPath(string assembly)
        {
            return string.IsNullOrEmpty(this.Options.AssemblyDirectory)
                       ? assembly
                       : Path.Combine(this.Options.AssemblyDirectory, assembly);
        }

        /// <summary>
        ///     Performs changes required to redirect constructors.
        /// </summary>
        /// <param name="cecilContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the construction redirect hook to find
        /// </typeparam>
        protected void HookConstructionRedirectors<T>(CecilContext cecilContext)
        {
            try
            {
                var types =
                    this.FarmhandAssemblies.SelectMany(a => a.GetTypesWithCustomAttribute(typeof(T).FullName)).ToArray();

                foreach (var asmType in types)
                {
                    try
                    {
                        this.RedirectConstructorInMethod(cecilContext, asmType);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting protections for {asmType.FullName}: \n\t{ex.Message}");
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine(
                    $"Error setting method/field protections: \n\t{ex.Message}\n\t\t{ex.LoaderExceptions[0].Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting method/field protections: \n\t{ex.Message}");
            }
        }

        /// <summary>
        ///     Performs changes required to for base methods to be virtual.
        /// </summary>
        /// <param name="cecilContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the virtual base hook to find
        /// </typeparam>
        protected void HookMakeBaseVirtualCallAlterations<T>(CecilContext cecilContext)
        {
            try
            {
                var methods =
                    this.FarmhandAssemblies.SelectMany(a => a.GetMethodsWithCustomAttribute(typeof(T).FullName))
                        .ToArray();

                foreach (var asmMethod in methods)
                {
                    try
                    {
                        this.SetVirtualCallOnMethod(cecilContext, asmMethod);
                    }
                    catch (Exception ex)
                    {
                        if (asmMethod.DeclaringType != null)
                        {
                            Console.WriteLine(
                                $"Error setting protections for {asmMethod.DeclaringType.FullName}.{asmMethod.Name}: \n\t{ex.Message}");
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine(
                    $"Error setting method/field protections: \n\t{ex.Message}\n\t\t{ex.LoaderExceptions[0].Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting method/field protections: \n\t{ex.Message}");
            }
        }

        /// <summary>
        ///     Edits a method to be marked as virtual.
        /// </summary>
        /// <param name="cecilContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <param name="asmMethod">
        ///     The method to edit.
        /// </param>
        protected abstract void SetVirtualCallOnMethod(CecilContext cecilContext, MethodInfo asmMethod);

        /// <summary>
        ///     Performs changes required to for methods to be virtual.
        /// </summary>
        /// <param name="cecilContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the virtual hook to find
        /// </typeparam>
        protected void HookApiVirtualAlterations<T>(CecilContext cecilContext)
        {
            try
            {
                var types =
                    this.FarmhandAssemblies.SelectMany(a => a.GetTypesWithCustomAttribute(typeof(T).FullName)).ToArray();

                foreach (var asmType in types)
                {
                    try
                    {
                        if (asmType.BaseType != null)
                        {
                            CecilHelper.SetVirtualOnBaseMethods(
                                cecilContext,
                                asmType.BaseType.FullName ?? asmType.BaseType.Namespace + "." + asmType.BaseType.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting protections for {asmType.FullName}: \n\t{ex.Message}");
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine(
                    $"Error setting method/field protections: \n\t{ex.Message}\n\t\t{ex.LoaderExceptions[0].Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting method/field protections: \n\t{ex.Message}");
            }
        }

        /// <summary>
        ///     Performs changes required to alter the protection on fields.
        /// </summary>
        /// <param name="cecilContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the field protection hook to find
        /// </typeparam>
        protected void HookApiFieldProtectionAlterations<T>(CecilContext cecilContext)
        {
            try
            {
                var types =
                    this.FarmhandAssemblies.SelectMany(a => a.GetTypesWithCustomAttribute(typeof(T).FullName)).ToArray();
                foreach (var asmType in types)
                {
                    try
                    {
                        this.AlterTypeBaseFieldProtections(cecilContext, asmType);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting protections for {asmType.FullName}: \n\t{ex.Message}");
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine(
                    $"Error setting method/field protections: \n\t{ex.Message}\n\t\t{ex.LoaderExceptions[0].Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting method/field protections: \n\t{ex.Message}");
            }
        }

        /// <summary>
        ///     Performs changes required to alter the protection on types.
        /// </summary>
        /// <param name="cecilContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the type protection hook to find
        /// </typeparam>
        protected void HookApiTypeProtectionAlterations<T>(CecilContext cecilContext)
        {
            try
            {
                var types =
                    this.FarmhandAssemblies.SelectMany(a => a.GetTypesWithCustomAttribute(typeof(T).FullName)).ToArray();
                foreach (var asmType in types)
                {
                    try
                    {
                        this.AlterTypeProtections(cecilContext, asmType);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting protections for {asmType.FullName}: \n\t{ex.Message}");
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine(
                    $"Error setting type protections: \n\t{ex.Message}\n\t\t{ex.LoaderExceptions[0].Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting type protections: \n\t{ex.Message}");
            }
        }

        /// <summary>
        ///     Redirects a constructor.
        /// </summary>
        /// <param name="cecilContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <param name="asmType">
        ///     The type whose constructor to redirect
        /// </param>
        protected abstract void RedirectConstructorInMethod(CecilContext cecilContext, Type asmType);

        /// <summary>
        ///     Alters the field protections in a type
        /// </summary>
        /// <param name="context">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <param name="type">
        ///     The type whose fields to edit
        /// </param>
        protected abstract void AlterTypeBaseFieldProtections(CecilContext context, Type type);

        /// <summary>
        ///     Alters the protection of a type.
        /// </summary>
        /// <param name="context">
        ///     The <see cref="CecilContext" />.
        /// </param>
        /// <param name="type">
        ///     The type to edit.
        /// </param>
        protected abstract void AlterTypeProtections(CecilContext context, Type type);

        /// <summary>
        ///     Inserts a callback to invoke API events.
        /// </summary>
        /// <param name="cecilContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        protected abstract void HookApiEvents(CecilContext cecilContext);

        /// <summary>
        ///     Inserts a construction redirect instruction
        /// </summary>
        /// <param name="cecilContext">
        ///     The <see cref="CecilContext" />.
        /// </param>
        protected abstract void HookConstructionToMethodRedirectors(CecilContext cecilContext);

        /// <summary>
        ///     Injects Farmhand assemblies into the game's executable
        /// </summary>
        /// <param name="output">
        ///     The output path
        /// </param>
        /// <param name="inputs">
        ///     The assemblies to merge
        /// </param>
        /// <exception cref="Exception">
        ///     Throws an exception if ILRepack fails.
        /// </exception>
        protected void InjectFarmhandCoreClasses(string output, params string[] inputs)
        {
            var options = new RepackOptions();
            ILogger logger = new RepackLogger();
            try
            {
                options.InputAssemblies = string.IsNullOrEmpty(this.Options.AssemblyDirectory)
                                              ? inputs
                                              : inputs.Select(
                                                  n =>
                                                      Path.IsPathRooted(n)
                                                          ? n
                                                          : Path.Combine(this.Options.AssemblyDirectory, n)).ToArray();

                options.OutputFile = output;
                options.DebugInfo = true;
                options.SearchDirectories = string.IsNullOrEmpty(this.Options.AssemblyDirectory)
                                                ? new[] { Directory.GetCurrentDirectory() }
                                                : new[] { this.Options.AssemblyDirectory };

                var repack = new ILRepack(options, logger);
                repack.Repack();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: ILRepack: {ex.Message}");
                throw new Exception("ILRepack Error", ex);
            }
            finally
            {
                if (options.PauseBeforeExit)
                {
                    Console.WriteLine("Press Any Key To Continue");
                    Console.ReadKey(true);
                }
            }
        }

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