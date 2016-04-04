using Farmhand.Attributes;
using Farmhand.Cecil;
using Farmhand.Helpers;
using System;
using System.Linq;
using System.Reflection;

namespace Farmhand
{
    public class PatcherFirstPass : Patcher
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Stardew EXE Path</param>
        public override void PatchStardew(string path = null)
        {
            path = path ?? PatcherConstants.StardewExe;
            InjectFarmhandCoreClasses(PatcherConstants.PassOnePackageResult, path, PatcherConstants.FarmhandDll, PatcherConstants.JsonLibrary);
            var cecilContext = new CecilContext(PatcherConstants.PassOnePackageResult, true);
            FarmhandDllAssembly = Assembly.LoadFrom(PatcherConstants.FarmhandDll);
            
            HookApiEvents(cecilContext);
            HookApiProtectionAlterations<HookAlterBaseProtectionAttribute>(cecilContext);
            HookApiVirtualAlterations<HookForceVirtualBaseAttribute>(cecilContext);
            HookMakeBaseVirtualCallAlterations<HookMakeBaseVirtualCallAttribute>(cecilContext);
            HookConstructionRedirectors<HookRedirectConstructorFromBaseAttribute>(cecilContext);

            Console.WriteLine("First Pass Installation Completed");

            cecilContext.WriteAssembly(PatcherConstants.PassOneFarmhandExe, true);
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
                var methods = FarmhandDllAssembly.GetTypes()
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
                        string hookTypeName = hook.Type;
                        string hookMethodName = hook.Method;
                        try
                        {
                            switch (hook.HookType)
                            {
                                case HookType.Entry: CecilHelper.InjectEntryMethod<ParameterBindAttribute, ThisBindAttribute, InputBindAttribute, LocalBindAttribute>
                                        (cecilContext, hookTypeName, hookMethodName, typeName, methodName); break;
                                case HookType.Exit: CecilHelper.InjectExitMethod<ParameterBindAttribute, ThisBindAttribute, InputBindAttribute, LocalBindAttribute>
                                        (cecilContext, hookTypeName, hookMethodName, typeName, methodName); break;
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
