using System;
using System.IO;
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
            var repackOutput = this.GetAssemblyPath(PatcherConstants.PassTwoPackageResult);
            InjectFarmhandCoreClasses(
                repackOutput,
                PatcherConstants.PassOneFarmhandExe,
                PatcherConstants.FarmhandUiDll,
                PatcherConstants.FarmhandGameDll,
                PatcherConstants.FarmhandCharacterDll);
            var cecilContext = new CecilContext(repackOutput, true);
            FarmhandAssemblies.Add(Assembly.LoadFrom(this.GetAssemblyPath(PatcherConstants.FarmhandUiDll)));
            FarmhandAssemblies.Add(Assembly.LoadFrom(this.GetAssemblyPath(PatcherConstants.FarmhandGameDll)));
            FarmhandAssemblies.Add(Assembly.LoadFrom(this.GetAssemblyPath(PatcherConstants.FarmhandCharacterDll)));

            HookApiEvents(cecilContext);
            HookOutputableApiEvents(cecilContext);
            HookApiFieldProtectionAlterations<HookAlterBaseFieldProtectionAttribute>(cecilContext);
            HookApiTypeProtectionAlterations<HookAlterProtectionAttribute>(cecilContext);
            HookApiVirtualAlterations<HookForceVirtualBaseAttribute>(cecilContext);
            HookMakeBaseVirtualCallAlterations<HookMakeBaseVirtualCallAttribute>(cecilContext);
            HookConstructionRedirectors<HookRedirectConstructorFromBaseAttribute>(cecilContext);
            HookConstructionToMethodRedirectors(cecilContext);
            
            HookGlobalRouting(cecilContext);
           
            Console.WriteLine("Second Pass Installation Completed");
            
            path = path ?? PatcherConstants.FarmhandExe;
            var directory = Path.GetDirectoryName(path); 

            if (directory == null)
            {
                throw new Exception("Path.GetDirectoryName(path) returned null");
            }

            Directory.CreateDirectory(directory);

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
            if (type.BaseType != null && attributeValue != null)
                CecilHelper.AlterProtectionOnType(context, attributeValue.Protection == LowestProtection.Public, attributeValue.ClassName);
        }

        protected override void HookApiEvents(CecilContext cecilContext)
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
                        throw ex;
                    }
                }
            }
        }

        private void HookOutputableApiEvents(CecilContext cecilContext)
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
                        throw;
                    }
                }
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

        protected override void HookConstructionToMethodRedirectors(CecilContext cecilContext)
        {
            var methods = FarmhandAssemblies.SelectMany(a => a.GetTypes())
                        .SelectMany(t => t.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                        .Where(m => m.GetCustomAttributes(typeof(HookRedirectConstructorToMethodAttribute), false).Any())
                        .ToArray();

            foreach (var method in methods)
            {
                // Check if this method has any properties that would immediately disqualify it from using this hook
                if (method.ReturnType == typeof(void))
                {
                    Logging.Log.Warning($"{method.Name} in {method.DeclaringType.FullName} cannot be used in a hook because it does not return a value!");
                    continue;
                }
                if (!method.IsStatic)
                {
                    Logging.Log.Warning($"{method.Name} in {method.DeclaringType.FullName} cannot be used in a hook because it is not static!");
                    continue;
                }

                // Get the type that the method returns
                var typeName = method.ReturnType.FullName;
                // Get the name of the method
                var methodName = method.Name;
                // Get the type name of the method
                var methodDeclaringType = method.DeclaringType.FullName;
                // Get an array of parameters that the method returns
                var methodParameterInfo = method.GetParameters();
                Type[] methodParamters = new Type[methodParameterInfo.Length];
                for (int i = 0; i < methodParamters.Length; i++)
                {
                    methodParamters[i] = methodParameterInfo[i].ParameterType;
                }

                // Get all the hooks for this method
                var hookAttributes = method.GetCustomAttributes(typeof(HookRedirectConstructorToMethodAttribute), false).Cast<HookRedirectConstructorToMethodAttribute>();

                foreach (var hook in hookAttributes)
                {
                    // Get the type name that contains the method we're hooking in
                    var hookTypeName = hook.Type;
                    // Get the name of the method we're hooking in
                    var hookMethodName = hook.Method;
                    try
                    {
                        CecilHelper.RedirectConstructorToMethod(cecilContext, method.ReturnType, hookTypeName, hookMethodName, methodDeclaringType, methodName, methodParamters);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to Inject {typeName}.{methodName} into {hookTypeName}.{hookMethodName}\n\t{ex.Message}");
                        throw ex;
                    }
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
