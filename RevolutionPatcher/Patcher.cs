using ILRepacking;
using Mono.Cecil;
using Revolution.Attributes;
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
    public class Patcher
    {
        public static void PatchStardew(string stardewExe, string revolutionDll)
        {
            InjectRevolutionCoreClasses(stardewExe, revolutionDll);
            CecilContext cecilContext = new CecilContext(Constants.IntermediateRevolutionExe);
            HookApiEvents(cecilContext);
            HookApiProtectionAlterations(cecilContext);
            HookApiVirtualAlterations(cecilContext);
            Console.WriteLine("Methods injected");
            
            cecilContext.WriteAssembly(Constants.RevolutionExe);          
        }

        private static void HookApiVirtualAlterations(CecilContext cecilContext)
        {
            try
            {
                var revolutionAssembly = typeof(HookForceVirtualBase).Assembly;
                var attribute = revolutionAssembly.GetModules()[0].GetType("Revolution.Attributes.HookForceVirtualBase");
                var types = revolutionAssembly.GetTypes().Where(m => m.GetCustomAttributes(attribute, false).Any()).ToArray();

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

        private static void HookApiProtectionAlterations(CecilContext cecilContext)
        {
            try
            {
                var revolutionAssembly = typeof(HookAlterBaseProtection).Assembly;
                var attribute = revolutionAssembly.GetModules()[0].GetType("Revolution.Attributes.HookAlterBaseProtection");
                var types = revolutionAssembly.GetTypes().Where(m => m.GetCustomAttributes(attribute, false).Any()).ToArray();

                foreach (var asmType in types)
                {
                    try
                    {
                        var attributeValue = asmType.GetCustomAttributes(attribute, false).First() as HookAlterBaseProtection;
                        CecilHelper.AlterProtectionOnTypeMembers(cecilContext, attributeValue.Protection, asmType.BaseType.FullName);
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

        static void HookApiEvents(CecilContext cecilContext)
        {
            try
            {
                var revolutionAssembly = typeof(HookAttribute).Assembly;
                var attribute = revolutionAssembly.GetModules()[0].GetType("Revolution.Attributes.HookAttribute");
                var methods = revolutionAssembly.GetTypes()
                            .SelectMany(t => t.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                            .Where(m => m.GetCustomAttributes(attribute, false).Any() )
                            .ToArray();

                foreach (var method in methods)
                {
                    string typeName = method.DeclaringType.FullName;
                    string methodName = method.Name;
                    var hookAttributes = method.GetCustomAttributes(typeof(HookAttribute), false).Cast<HookAttribute>();

                    foreach (var hook in hookAttributes)
                    {
                        string hookTypeName = hook.Type;
                        string hookMethodName = hook.Method;
                        try
                        {
                            switch (hook.HookType)
                            {
                                case HookType.Entry: CecilHelper.InjectEntryMethod(cecilContext, hookTypeName, hookMethodName, typeName, methodName); break;
                                case HookType.Exit: CecilHelper.InjectExitMethod(cecilContext, hookTypeName, hookMethodName, typeName, methodName); break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine("Failed to Inject {0}.{1} into {2}.{3}\n\t{4}", typeName, methodName, hookTypeName, hookMethodName, ex.Message);
                        }    
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
                
        }

        static void InjectRevolutionCoreClasses(string stardewExe, string revolutionDll)
        {
            RepackOptions options = new RepackOptions();
            ILogger logger = new RepackLogger();
            try
            {
                options.InputAssemblies = new string[] 
                {
                    stardewExe,
                    revolutionDll,
                    Constants.JsonLibrary
                };
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
