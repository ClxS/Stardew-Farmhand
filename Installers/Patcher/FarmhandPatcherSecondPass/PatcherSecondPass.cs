using System;
using System.Linq;
using Farmhand.Cecil;
using System.Reflection;
using Farmhand.Helpers;
using Farmhand.Attributes;

namespace Farmhand
{
    public class PatcherSecondPass : Patcher
    {
        public override void PatchStardew(string path = null)
        {
            InjectFarmhandCoreClasses(PatcherConstants.PassTwoPackageResult, PatcherConstants.PassOneFarmhandExe, PatcherConstants.FarmhandUiDll, PatcherConstants.FarmhandGameDll, PatcherConstants.FarmhandCharacterDll);
            var cecilContext = new CecilContext(PatcherConstants.PassTwoPackageResult, true);
            FarmhandAssemblies.Add(Assembly.LoadFrom(PatcherConstants.FarmhandUiDll));
            FarmhandAssemblies.Add(Assembly.LoadFrom(PatcherConstants.FarmhandGameDll));
            FarmhandAssemblies.Add(Assembly.LoadFrom(PatcherConstants.FarmhandCharacterDll));

            HookApiEvents(cecilContext);
            HookOutputableApiEvents(cecilContext);
            HookApiFieldProtectionAlterations<HookAlterBaseFieldProtectionAttribute>(cecilContext);
            HookApiTypeProtectionAlterations<HookAlterProtectionAttribute>(cecilContext);
            HookApiVirtualAlterations<HookForceVirtualBaseAttribute>(cecilContext);
            HookMakeBaseVirtualCallAlterations<HookMakeBaseVirtualCallAttribute>(cecilContext);
            HookConstructionRedirectors<HookRedirectConstructorFromBaseAttribute>(cecilContext);
            
            HookGlobalRouting(cecilContext);
           
            Console.WriteLine("Second Pass Installation Completed");

            path = path ?? PatcherConstants.FarmhandExe;
            cecilContext.WriteAssembly(path, true);
        }
        
        private void HookGlobalRouting(CecilContext cecilContext)
        {
            if (!Options.DisableGrm)
            {
                CecilHelper.HookAllGlobalRouteMethods(cecilContext);
            }
            else
            {
                Console.WriteLine("Skipping GRM injection");
            }
        }

        protected override void AlterTypeBaseFieldProtections(CecilContext context, Type type)
        {
            var attributeValue = type.GetCustomAttributes(typeof(HookAlterBaseFieldProtectionAttribute), false).First() as HookAlterBaseFieldProtectionAttribute;
            if (type.BaseType != null)
                CecilHelper.AlterProtectionOnTypeMembers(context, attributeValue != null && attributeValue.Protection == LowestProtection.Public, type.BaseType.FullName);
        }

        protected override void AlterTypeProtections(CecilContext context, Type type)
        {
            var attributeValue = type.GetCustomAttributes(typeof(HookAlterProtectionAttribute), false).First() as HookAlterProtectionAttribute;
            if (type.BaseType != null)
                CecilHelper.AlterProtectionOnType(context, attributeValue != null && attributeValue.Protection == LowestProtection.Public, attributeValue.ClassName);
        }

        protected override void HookApiEvents(CecilContext cecilContext)
        {
            try
            {
                var methods = FarmhandAssemblies.SelectMany(a => a.GetTypes())
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
                                case HookType.Entry:
                                    CecilHelper.InjectEntryMethod<ParameterBindAttribute, ThisBindAttribute, InputBindAttribute, LocalBindAttribute>
                                        (cecilContext, hookTypeName, hookMethodName, typeName, methodName); break;
                                case HookType.Exit:
                                    CecilHelper.InjectExitMethod<ParameterBindAttribute, ThisBindAttribute, InputBindAttribute, LocalBindAttribute>
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

        private void HookOutputableApiEvents(CecilContext cecilContext)
        {
            try
            {
                var methods = FarmhandAssemblies.SelectMany(a => a.GetTypes())
                            .SelectMany(t => t.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                            .Where(m => m.GetCustomAttributes(typeof(HookReturnableAttribute), false).Any())
                            .ToArray();

                foreach (var method in methods)
                {
                    if (method.DeclaringType == null) continue;

                    var typeName = method.DeclaringType.FullName;
                    var methodName = method.Name;
                    var hookAttributes = method.GetCustomAttributes(typeof(HookReturnableAttribute), false).Cast<HookReturnableAttribute>();

                    foreach (var hook in hookAttributes)
                    {
                        var hookTypeName = hook.Type;
                        var hookMethodName = hook.Method;
                        try
                        {
                            switch (hook.HookType)
                            {
                                case HookType.Entry:
                                    CecilHelper.InjectReturnableEntryMethod<ParameterBindAttribute, ThisBindAttribute, InputBindAttribute, LocalBindAttribute,
                                        UseOutputBindAttribute, MethodOutputBindAttribute>
                                        (cecilContext, hookTypeName, hookMethodName, typeName, methodName); break;
                                case HookType.Exit:
                                    CecilHelper.InjectReturnableExitMethod<ParameterBindAttribute, ThisBindAttribute, InputBindAttribute, LocalBindAttribute,
                                        UseOutputBindAttribute, MethodOutputBindAttribute>
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
                if (attribute.GenericArguments != null && attribute.GenericArguments.Any())
                {
                    CecilHelper.RedirectConstructorFromBase(cecilContext, asmType, attribute.GenericArguments, attribute.Type, attribute.Method, attribute.Parameters);
                }
                else
                {
                    CecilHelper.RedirectConstructorFromBase(cecilContext, asmType, attribute.Type, attribute.Method, attribute.Parameters);
                }
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
