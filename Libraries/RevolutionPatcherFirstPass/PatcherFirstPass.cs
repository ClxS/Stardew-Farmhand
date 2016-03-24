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
        public override void PatchStardew()
        {
            CecilContext cecilContext;
            
            InjectRevolutionCoreClasses(PatcherConstants.PassOnePackageResult, PatcherConstants.StardewExe, PatcherConstants.RevolutionDll, PatcherConstants.JsonLibrary);
            cecilContext = new CecilContext(PatcherConstants.PassOnePackageResult);
            RevolutionDllAssembly = Assembly.LoadFrom(PatcherConstants.RevolutionDll);
                                    
            HookApiEvents(cecilContext);
            HookApiProtectionAlterations<HookAlterBaseProtectionAttribute>(cecilContext);
            HookApiVirtualAlterations<HookForceVirtualBaseAttribute>(cecilContext);
            HookMakeBaseVirtualCallAlterations<HookMakeBaseVirtualCallAttribute>(cecilContext);
            HookConstructionRedirectors<HookRedirectConstructorFromBaseAttribute>(cecilContext);

            Console.WriteLine("First Pass Installation Completed");

            cecilContext.WriteAssembly(PatcherConstants.PassOneRevolutionExe);
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
                            Console.WriteLine($"Failed to Inject {typeName}.{methodName} into {hookTypeName}.{hookMethodName}\n\t{ex.Message}");
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void RedirectConstructorInMethod(CecilContext cecilContext, Type asmType)
        {
            var attributes = asmType.GetCustomAttributes(typeof(HookRedirectConstructorFromBaseAttribute), false).ToList().Cast<HookRedirectConstructorFromBaseAttribute>();
            foreach (var attribute in attributes)
            {
                CecilHelper.RedirectConstructorFromBase(cecilContext, asmType, attribute.Type, attribute.Method);
            }
        }

        protected override void SetVirtualCallOnMethod(CecilContext cecilContext, MethodInfo asmMethod)
        {
            throw new NotImplementedException();
        }
    }
}
