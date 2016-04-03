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
        public override void PatchStardew(string path = null)
        {
            InjectRevolutionCoreClasses(PatcherConstants.PassTwoPackageResult, PatcherConstants.PassOneRevolutionExe, PatcherConstants.RevolutionUiDll);
            var cecilContext = new CecilContext(PatcherConstants.PassTwoPackageResult, true);
            RevolutionDllAssembly = Assembly.LoadFrom(PatcherConstants.RevolutionUiDll);

            HookApiEvents(cecilContext);
            HookApiProtectionAlterations<HookAlterBaseProtectionAttribute>(cecilContext);
            HookApiVirtualAlterations<HookForceVirtualBaseAttribute>(cecilContext);
            HookMakeBaseVirtualCallAlterations<HookMakeBaseVirtualCallAttribute>(cecilContext);
            HookConstructionRedirectors<HookRedirectConstructorFromBaseAttribute>(cecilContext);

            //TODO: Broken! Terrible performance
            //HookGlobalRouting(cecilContext);
           
            Console.WriteLine("Second Pass Installation Completed");

            path = path ?? PatcherConstants.RevolutionExe;
            cecilContext.WriteAssembly(path, true);
        }

        private void HookGlobalRouting(CecilContext cecilContext)
        {
            CecilHelper.HookAllGlobalRouteMethods(cecilContext);
        }

        protected override void AlterTypeBaseProtections(CecilContext context, Type type)
        {
            var attributeValue = type.GetCustomAttributes(typeof(HookAlterBaseProtectionAttribute), false).First() as HookAlterBaseProtectionAttribute;
            if (type.BaseType != null)
                CecilHelper.AlterProtectionOnTypeMembers(context, attributeValue != null && attributeValue.Protection == LowestProtection.Public, type.BaseType.FullName);
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
                    if (method.DeclaringType == null) continue;

                    var typeName = method.DeclaringType.FullName;
                    var methodName = method.Name;
                    var hookAttributes = method.GetCustomAttributes(typeof(HookAttribute), false).Cast<HookAttribute>();

                    foreach (var hook in hookAttributes)
                    {
                        var hookTypeName = hook.Type;
                        var hookMethodName = hook.Method;
                        try
                        {
                            switch (hook.HookType)
                            {
                                case HookType.Entry: CecilHelper.InjectEntryMethod<ParameterBindAttribute, ThisBindAttribute, InputBindAttribute, LocalBindAttribute>
                                        (cecilContext, hookTypeName, hookMethodName, typeName, methodName); break;
                                case HookType.Exit: CecilHelper.InjectExitMethod<ParameterBindAttribute, ThisBindAttribute, InputBindAttribute, LocalBindAttribute>
                                        (cecilContext, hookTypeName, hookMethodName, typeName, methodName); break;
                                default:
                                    throw new Exception("Unknown HookType");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to Inject {typeName}.{methodName} into {hookTypeName}.{hookMethodName}\n\t{ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void RedirectConstructorInMethod(CecilContext cecilContext, Type asmType)
        {
            var attributes = asmType.GetCustomAttributes(typeof (HookRedirectConstructorFromBaseAttribute), false).ToList().Cast<HookRedirectConstructorFromBaseAttribute>();
            foreach (var attribute in attributes)
            {
                CecilHelper.RedirectConstructorFromBase(cecilContext, asmType, attribute.Type, attribute.Method);
            }
        }

        protected override void SetVirtualCallOnMethod(CecilContext cecilContext, MethodInfo asmMethod)
        {
            var attributes = asmMethod.GetCustomAttributes(typeof (HookMakeBaseVirtualCallAttribute), false).ToList().Cast<HookMakeBaseVirtualCallAttribute>();
            foreach (var attribute in attributes.Where(attribute => asmMethod.DeclaringType?.BaseType != null))
            {
                if (asmMethod.DeclaringType?.BaseType != null)
                    CecilHelper.SetVirtualCallOnMethod(cecilContext, asmMethod.DeclaringType.BaseType.FullName, asmMethod.Name, attribute.Type, attribute.Method);
            }
        }
    }
}
