using ILRepacking;
using Farmhand.Cecil;
using Farmhand.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Farmhand
{
    public abstract class Patcher
    {
        protected Assembly FarmhandDllAssembly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Stardew Exe Path (Pass 1), or Farmhand Output Path (Pass 2)</param>
        public abstract void PatchStardew(string path = null);
        
        protected void HookConstructionRedirectors<T>(CecilContext cecilContext)
        {
            try
            {
                var types = FarmhandDllAssembly.GetTypesWithCustomAttribute(typeof (T).FullName).ToArray();

                foreach (var asmType in types)
                {
                    try
                    {
                        RedirectConstructorInMethod(cecilContext, asmType);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting protections for {asmType.FullName}: \n\t{ex.Message}");
                    }
                }
            }
            catch (System.Reflection.ReflectionTypeLoadException ex)
            {
                Console.WriteLine($"Error setting method/field protections: \n\t{ex.Message}\n\t\t{ex.LoaderExceptions[0].Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting method/field protections: \n\t{ex.Message}");
            }
        }
        protected void HookMakeBaseVirtualCallAlterations<T>(CecilContext cecilContext)
        {
            try
            {
                var methods = FarmhandDllAssembly.GetMethodsWithCustomAttribute(typeof(T).FullName).ToArray();
                                
                foreach (var asmMethod in methods)
                {
                    try
                    {
                        SetVirtualCallOnMethod(cecilContext, asmMethod);
                    }
                    catch (Exception ex)
                    {
                        if (asmMethod.DeclaringType != null)
                            Console.WriteLine($"Error setting protections for {asmMethod.DeclaringType.FullName}.{asmMethod.Name}: \n\t{ex.Message}");
                    }
                }
            }
            catch (System.Reflection.ReflectionTypeLoadException ex)
            {
                Console.WriteLine($"Error setting method/field protections: \n\t{ex.Message}\n\t\t{ex.LoaderExceptions[0].Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting method/field protections: \n\t{ex.Message}");
            }
        }

        protected abstract void SetVirtualCallOnMethod(CecilContext cecilContext, MethodInfo asmMethod);

        protected void HookApiVirtualAlterations<T>(CecilContext cecilContext)
        {
            try
            {
                var types = FarmhandDllAssembly.GetTypesWithCustomAttribute(typeof(T).FullName).ToArray();

                foreach (var asmType in types)
                {
                    try
                    {
                        if (asmType.BaseType != null)
                            CecilHelper.SetVirtualOnBaseMethods(cecilContext, asmType.BaseType.FullName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting protections for {asmType.FullName}: \n\t{ex.Message}");
                    }
                }
            }
            catch (System.Reflection.ReflectionTypeLoadException ex)
            {
                Console.WriteLine($"Error setting method/field protections: \n\t{ex.Message}\n\t\t{ex.LoaderExceptions[0].Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting method/field protections: \n\t{ex.Message}");
            }
        }

        protected void HookApiProtectionAlterations<T>(CecilContext cecilContext)
        {
            try
            {
                var types = FarmhandDllAssembly.GetTypesWithCustomAttribute(typeof(T).FullName).ToArray();
                foreach (var asmType in types)
                {
                    try
                    {
                        AlterTypeBaseProtections(cecilContext, asmType);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting protections for {asmType.FullName}: \n\t{ex.Message}");
                    }
                }
            }
            catch (System.Reflection.ReflectionTypeLoadException ex)
            {
                Console.WriteLine($"Error setting method/field protections: \n\t{ex.Message}\n\t\t{ex.LoaderExceptions[0].Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting method/field protections: \n\t{ex.Message}");
            }
        }

        protected abstract void RedirectConstructorInMethod(CecilContext cecilContext, Type asmType);

        protected abstract void AlterTypeBaseProtections(CecilContext context, Type type);

        protected abstract void HookApiEvents(CecilContext cecilContext);

        protected void InjectFarmhandCoreClasses(string output, params string[] inputs)
        {
            RepackOptions options = new RepackOptions();
            ILogger logger = new RepackLogger();
            try
            {
                options.InputAssemblies = inputs;
                options.OutputFile = output;
                options.DebugInfo = true;
                options.SearchDirectories = new[] { Directory.GetCurrentDirectory() };

                var repack = new ILRepack(options, logger);
                repack.Repack();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: ILRepack: {ex.Message}");
                throw new Exception("ILRepack Error", ex);
                // ignored
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

        public class RepackLogger : ILogger
        {
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
        }

    }
}
