using Revolution;
using Revolution.Attributes;
using Revolution.Cecil;
using Revolution.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Revolution
{
    public class PatcherFirstPass : Patcher
    {
        public override void PatchStardew(string stardewExe, string revolutionDll)
        {
            CecilContext cecilContext;
            
            InjectRevolutionCoreClasses(stardewExe, revolutionDll, Constants.JsonLibrary);
            cecilContext = new CecilContext(Constants.IntermediateRevolutionExe);
            RevolutionDllAssembly = Assembly.LoadFrom("Revolution.dll");
                        
            HookApiEvents(cecilContext);
            HookApiProtectionAlterations<HookAlterBaseProtectionAttribute>(cecilContext);
            HookApiVirtualAlterations<HookForceVirtualBaseAttribute>(cecilContext);
            Console.WriteLine("Methods injected");

            cecilContext.WriteAssembly(Constants.PassOneRevolutionExe);
        }

        protected override void AlterTypeBaseProtections(CecilContext context, Type type)
        {
            var attributeValue = type.GetCustomAttributes(typeof(HookAlterBaseProtectionAttribute), false).First() as HookAlterBaseProtectionAttribute;
            CecilHelper.AlterProtectionOnTypeMembers(context, attributeValue.Protection == LowestProtection.Public, type.BaseType.FullName); 
        }

        protected override void HookApiEvents(CecilContext cecilContext)
        {
            try
            {
                var methods = RevolutionDllAssembly.GetTypes()
                            .SelectMany(t => t.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                            .Where(m => m.GetCustomAttributes(typeof(HookAttribute), false).Any())
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
    }
}
