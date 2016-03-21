using ILRepacking;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Revolution.Cecil;
using Revolution.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Revolution
{
    public abstract class Patcher
    {
        protected Assembly RevolutionDllAssembly { get; set; }

        public abstract void PatchStardew();
        
        protected void HookConstructionRedirectors<T>(CecilContext cecilContext)
        {
            try
            {
                var types = RevolutionDllAssembly.GetTypesWithCustomAttribute(typeof(T).FullName).ToArray();

                foreach (var asmType in types)
                {
                    try
                    {
                        RedirectConstructorInMethod(cecilContext, asmType);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine("Error setting protections for {0}: \n\t{1}", asmType.FullName, ex.Message);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error setting method/field protections: \n\t{0}", ex.Message);
            }
        }
        protected void HookMakeBaseVirtualCallAlterations<T>(CecilContext cecilContext)
        {
            try
            {
                var methods = RevolutionDllAssembly.GetMethodsWithCustomAttribute(typeof(T).FullName).ToArray();
                                
                foreach (var asmMethod in methods)
                {
                    try
                    {
                        SetVirtualCallOnMethod(cecilContext, asmMethod);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine("Error setting protections for {0}.{1}: \n\t{1}", asmMethod.DeclaringType.FullName, asmMethod.Name, ex.Message);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error setting method/field protections: \n\t{0}", ex.Message);
            }
        }

        protected abstract void SetVirtualCallOnMethod(CecilContext cecilContext, MethodInfo asmMethod);

        protected void HookApiVirtualAlterations<T>(CecilContext cecilContext)
        {
            try
            {
                var types = RevolutionDllAssembly.GetTypesWithCustomAttribute(typeof(T).FullName).ToArray();

                foreach (var asmType in types)
                {
                    try
                    {
                        CecilHelper.SetVirtualOnBaseMethods(cecilContext, asmType.BaseType.FullName);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine("Error setting protections for {0}: \n\t{1}", asmType.FullName, ex.Message);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error setting method/field protections: \n\t{0}", ex.Message);
            }
        }

        protected void HookApiProtectionAlterations<T>(CecilContext cecilContext)
        {
            try
            {
                var types = RevolutionDllAssembly.GetTypesWithCustomAttribute(typeof(T).FullName).ToArray();
                foreach (var asmType in types)
                {
                    try
                    {
                        AlterTypeBaseProtections(cecilContext, asmType);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine("Error setting protections for {0}: \n\t{1}", asmType.FullName, ex.Message);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error setting method/field protections: \n\t{0}", ex.Message);
            }
        }

        protected abstract void RedirectConstructorInMethod(CecilContext cecilContext, Type asmType);

        protected abstract void AlterTypeBaseProtections(CecilContext context, Type type);

        protected abstract void HookApiEvents(CecilContext cecilContext);

        protected void InjectRevolutionCoreClasses(string output, params string[] inputs)
        {
            RepackOptions options = new RepackOptions();
            ILogger logger = new RepackLogger();
            try
            {
                options.InputAssemblies = inputs;
                options.OutputFile = output; 
                options.SearchDirectories = new string[] { System.IO.Path.GetDirectoryName(Constants.CurrentAssemblyPath) };

                ILRepack repack = new ILRepack(options, logger);
                repack.Repack();
            }            
            catch (Exception e)
            {
                
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
