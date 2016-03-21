using ILRepacking;
using Mono.Cecil;
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

        public abstract void PatchStardew(string stardewExe, string revolutionDll);
        
        protected void HookApiVirtualAlterations<T>(CecilContext cecilContext)
        {
            try
            {
                var types = RevolutionDllAssembly.GetTypes().Where(m => m.GetCustomAttributes(typeof(T), false).Any()).ToArray();

                foreach (var asmType in types)
                {
                    try
                    {
                        CecilHelper.SetVirtualOnBaseMethods(cecilContext, asmType.FullName);
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
                var types = RevolutionDllAssembly.GetTypes().Where(m => m.GetCustomAttributes(typeof(T), false).Any()).ToArray();

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

        protected abstract void AlterTypeBaseProtections(CecilContext context, Type type);

        protected abstract void HookApiEvents(CecilContext cecilContext);

        protected void InjectRevolutionCoreClasses(params string[] inputs)
        {
            RepackOptions options = new RepackOptions();
            ILogger logger = new RepackLogger();
            try
            {
                options.InputAssemblies = inputs;
                options.OutputFile = Constants.IntermediateRevolutionExe;
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
