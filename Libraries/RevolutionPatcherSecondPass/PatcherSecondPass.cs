using System;
using System.Linq;
using Revolution.Cecil;
using System.Reflection;
using Revolution.Helpers;
using Revolution.Attributes;

namespace Revolution
{
    public class PatcherSecondPass : Patcher
    {
        public override void PatchStardew()
        {
            CecilContext cecilContext;

            InjectRevolutionCoreClasses(Constants.PassTwoPackageResult, Constants.PassOneRevolutionExe, Constants.RevolutionUIDll);
            cecilContext = new CecilContext(Constants.PassTwoPackageResult);
            RevolutionDllAssembly = Assembly.LoadFrom(Constants.RevolutionUIDll);
            
            HookApiEvents(cecilContext);
            HookApiProtectionAlterations<HookAlterBaseProtectionAttribute>(cecilContext);
            HookApiVirtualAlterations<HookForceVirtualBaseAttribute>(cecilContext);
            HookMakeBaseVirtualCallAlterations<HookMakeBaseVirtualCallAttribute>(cecilContext);
            HookConstructionRedirectors<HookRedirectConstructorFromBaseAttribute>(cecilContext);
            
            Console.WriteLine("Second Pass Installation Completed");
            
            cecilContext.WriteAssembly(Constants.RevolutionExe);
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

        protected override void RedirectConstructorInMethod(CecilContext cecilContext, Type asmType)
        {
            var attributes = asmType.GetCustomAttributes(typeof(HookRedirectConstructorFromBaseAttribute), false).ToList().Cast<HookRedirectConstructorFromBaseAttribute>();
            foreach(var attribute in attributes)
            {
                CecilHelper.RedirectConstructorFromBase(cecilContext, asmType, attribute.Type, attribute.Method);
            }
        }

        protected override void SetVirtualCallOnMethod(CecilContext cecilContext, MethodInfo asmMethod)
        {
            var attributes = asmMethod.GetCustomAttributes(typeof(HookMakeBaseVirtualCallAttribute), false).ToList().Cast<HookMakeBaseVirtualCallAttribute>();
            foreach (var attribute in attributes)
            {
                CecilHelper.SetVirtualCallOnMethod(cecilContext, asmMethod.DeclaringType.BaseType.FullName, asmMethod.Name, attribute.Type, attribute.Method);
            }
        }
    }
}
