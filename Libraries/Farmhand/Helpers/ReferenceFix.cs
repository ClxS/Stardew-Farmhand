namespace Farmhand.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Farmhand.Logging;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    internal static class ReferenceHelper
    {
        internal static AssemblyNameReference ThisRef { get; } =
            AssemblyNameReference.Parse(Assembly.GetExecutingAssembly().FullName);

        internal static AssemblyNameReference MonoRef { get; } = FindMyRef("MonoGame.Framework");

        internal static AssemblyNameReference XnaRef { get; } = FindMyRef("Microsoft.Xna.Framework");

        internal static AssemblyNameReference XnaGameRef { get; } = FindMyRef("Microsoft.Xna.Framework.Game");

        internal static AssemblyNameReference XnaGraphicsRef { get; } = FindMyRef("Microsoft.Xna.Framework.Graphics");

        // Since Mono <-> (Xna,Graphics,Game), we can't just replace every Mono reference
        // with XNA directly. So this will be the cache of which types go to which assembly.
        internal static Dictionary<string, AssemblyNameReference> XnaTypes { get; } = BuildXnaTypeCache();

        // If referencing vanilla Stardew, it wasn't built for Farmhand.
        // If on Windows and referencing Mono, it won't be compatible.
        // If not on Windows and referencing Microsoft XNA, it won't be compatible.
        // If none of these are true, everything is fine.
        internal static bool MatchesPlatform(AssemblyNameReference asmRef)
        {
#if WINDOWS
            return !(IsStardew(asmRef.Name) || IsMono(asmRef.Name));
#else
            return !(IsStardew(asmRef.Name) || IsXna(asmRef.Name));
#endif
        }

        internal static bool MatchesPlatform(TypeReference asmRef)
        {
#if WINDOWS
            return !(IsStardew(asmRef.Namespace) || IsMono(asmRef.Namespace));
#else
            return !(IsStardew(asmRef.Namespace) || IsXna(asmRef.Namespace));
#endif
        }

        internal static bool IsStardew(string @ref)
        {
            // "Stardew Valley" on Windows, "StardewValley" otherwise.
            // However, both end up being wrong since they should be referencing Farmhand.
            return @ref.StartsWith("Stardew Valley") || @ref.StartsWith("StardewValley");
        }

        // Can't compare to static vars because they are null for the opposite platform
        internal static bool IsXna(string @ref)
        {
            return @ref.StartsWith("Microsoft.Xna.Framework");
        }

        internal static bool IsMono(string @ref)
        {
            return @ref.StartsWith("MonoGame.Framework");
        }

        // Find the references that we use. Technically could have hard-coded these to
        // what Farmhand uses right now, but this is a bit more future-proof.
        internal static AssemblyNameReference FindMyRef(string name)
        {
            var assembly = Assembly.GetExecutingAssembly().GetReferencedAssemblies().FirstOrDefault(r => r.Name == name);
            if (assembly != null)
            {
                return AssemblyNameReference.Parse(assembly.FullName);
            }

            return null;
        }

        // This will return which assembly an XNA type is in. (See BuildXnaTypeCache())
        internal static IMetadataScope FindXnaRef(TypeReference type)
        {
            var key = type.Namespace + "." + type.Name;
            return !XnaTypes.ContainsKey(key) ? XnaRef : XnaTypes[key];
        }

        private static Dictionary<string, AssemblyNameReference> BuildXnaTypeCache()
        {
            // All XNA is replaced with the single Mono assembly on Linux/Mac anyways, so this won't be used.
#if !WINDOWS
                return new Dictionary<string, AssemblyNameReference>();
#else
            var core = ModuleDefinition.ReadModule(typeof(Vector2).Module.FullyQualifiedName);
            var game = ModuleDefinition.ReadModule(typeof(Game).Module.FullyQualifiedName);
            var graphics = ModuleDefinition.ReadModule(typeof(SpriteBatch).Module.FullyQualifiedName);

            var ret = new Dictionary<string, AssemblyNameReference>();
            foreach (var type in core.GetTypes())
            {
                // If they aren't public then nobody should be referencing them, so
                // we don't need to fix any of those references.
                // I can't remember why the '<' check was needed. I think compiler
                // generated stuff caused duplicate key problems?
                if (!type.IsPublic || type.Namespace.IndexOf("<", StringComparison.Ordinal) != -1)
                {
                    continue;
                }

                ret.Add(type.Namespace + "." + type.Name, XnaRef);
            }

            foreach (var type in game.GetTypes())
            {
                if (!type.IsPublic || type.Namespace.IndexOf("<", StringComparison.Ordinal) != -1)
                {
                    continue;
                }

                ret.Add(type.Namespace + "." + type.Name, XnaGameRef);
            }

            foreach (var type in graphics.GetTypes())
            {
                if (!type.IsPublic || type.Namespace.IndexOf("<", StringComparison.Ordinal) != -1)
                {
                    continue;
                }

                ret.Add(type.Namespace + "." + type.Name, XnaGraphicsRef);
            }

            return ret;
#endif
        }

        // Fix references to other things. The ones we fix are the external references,
        // but everything still needs to be checked, even if the top-level is fine.
        // Stupid generics.
        internal class ReferenceResolver
        {
            private static readonly FieldInfo TypeSpecField = typeof(TypeSpecification).GetField(
                "element_type",
                BindingFlags.NonPublic | BindingFlags.Instance);

            private static readonly FieldInfo MethSpecField = typeof(MethodSpecification).GetField(
                "method",
                BindingFlags.NonPublic | BindingFlags.Instance);

            internal static TypeReference Fix(ModuleDefinition def, TypeReference type)
            {
                var from = type.Scope.Name;

                var ret = type;
                if (ret.GetType() == typeof(TypeReference))
                {
                    if (IsStardew(from))
                    {
                        ret = new TypeReference(type.Namespace, type.Name, null, ThisRef);
                    }
                    else
                    {
#if WINDOWS
                        if (IsMono(from))
                        {
                            ret = new TypeReference(type.Namespace, type.Name, null, FindXnaRef(type));
                        }

#else
                        if (IsXna(from))
                        {
                            ret = new TypeReference(type.Namespace, type.Name, null, MonoRef);
                        }
#endif
                    }

                    ret.IsValueType = type.IsValueType;
                }
                else if (ret is GenericParameter)
                {
                    return Fix(def, ret.DeclaringType, (GenericParameter)ret);
                }
                else if (ret is TypeSpecification)
                {
                    TypeSpecField.SetValue(type, Fix(def, ((TypeSpecification)type).ElementType));
                }
                else if (!(ret is TypeDefinition))
                {
                    // Meaning it is defined in this assembly. I think?
                    Log.Warning("Unable to fix unhandled type " + ret.GetType() + " " + ret);
                }

                var genericInstanceType = type as GenericInstanceType;
                if (genericInstanceType != null)
                {
                    for (var i = 0; i < genericInstanceType.ElementType.GenericParameters.Count; ++i)
                    {
                        genericInstanceType.ElementType.GenericParameters[i] = Fix(
                            def,
                            genericInstanceType,
                            genericInstanceType.ElementType.GenericParameters[i]);
                    }

                    for (var i = 0; i < genericInstanceType.GenericArguments.Count; ++i)
                    {
                        genericInstanceType.GenericArguments[i] = Fix(def, genericInstanceType.GenericArguments[i]);
                    }
                }
                else if (type.HasGenericParameters)
                {
                    for (var i = 0; i < type.GenericParameters.Count; ++i)
                    {
                        type.GenericParameters[i] = Fix(def, type, type.GenericParameters[i]);
                    }
                }

                return def.Import(ret);
            }

            internal static MethodReference Fix(ModuleDefinition def, MethodReference meth)
            {
                foreach (var param in meth.Parameters)
                {
                    param.ParameterType = Fix(def, param.ParameterType);
                }

                var instanceMethod = meth as GenericInstanceMethod;
                if (instanceMethod != null)
                {
                    MethSpecField.SetValue(instanceMethod, Fix(def, instanceMethod.ElementMethod));
                    for (var i = 0; i < instanceMethod.GenericParameters.Count; ++i)
                    {
                        instanceMethod.GenericParameters[i] = Fix(def, instanceMethod, instanceMethod.GenericParameters[i]);
                    }

                    for (var i = 0; i < instanceMethod.GenericArguments.Count; ++i)
                    {
                        instanceMethod.GenericArguments[i] = Fix(def, instanceMethod.GenericArguments[i]);
                    }
                }
                else if (meth.HasGenericParameters)
                {
                    for (var i = 0; i < meth.GenericParameters.Count; ++i)
                    {
                        meth.GenericParameters[i] = Fix(def, meth, meth.GenericParameters[i]);
                    }
                }

                meth.ReturnType = Fix(def, meth.ReturnType);
                if (meth is MethodSpecification)
                {
                    var elem = MethSpecField.GetValue(meth) as MethodReference;
                    MethSpecField.SetValue(meth, Fix(def, elem));
                }
                else
                {
                    meth.DeclaringType = Fix(def, meth.DeclaringType);
                }

                return def.Import(meth);
            }

            internal static FieldReference Fix(ModuleDefinition def, FieldReference field)
            {
                field.DeclaringType = Fix(def, field.DeclaringType);
                field.FieldType = Fix(def, field.FieldType);
                return def.Import(field);
            }

            internal static GenericParameter Fix(
                ModuleDefinition def,
                IGenericParameterProvider parent,
                GenericParameter param)
            {
                for (var i = 0; i < param.Constraints.Count; ++i)
                {
                    param.Constraints[i] = Fix(def, param.Constraints[i]);
                }

                for (var i = 0; i < param.GenericParameters.Count; ++i)
                {
                    param.GenericParameters[i] = Fix(def, param, param.GenericParameters[i]);
                }

                return param;
            }

            #region Nested type: DefinitionResolver

            // Fix the actual definitions of things, mainly to go down and find all
            // of the references that need fixing.
            internal class DefinitionResolver
            {
                internal static void Fix(AssemblyDefinition def)
                {
                    foreach (var mod in def.Modules)
                    {
                        foreach (var obj in mod.GetTypes())
                        {
                            Fix(obj);
                        }

                        var refs = (TypeReference[])mod.GetTypeReferences();
                        for (var i = 0; i < refs.Length; ++i)
                        {
                            if (!MatchesPlatform(refs[i]))
                            {
                                refs[i] = ReferenceResolver.Fix(mod, refs[i]);
                            }
                        }
                    }
                }

                private static void Fix(TypeDefinition type)
                {
                    if (type.BaseType != null)
                    {
                        type.BaseType = ReferenceResolver.Fix(type.Module, type.BaseType);
                    }

                    for (var i = 0; i < type.Interfaces.Count; ++i)
                    {
                        type.Interfaces[i] = ReferenceResolver.Fix(type.Module, type.Interfaces[i]);
                    }

                    foreach (var obj in type.Events)
                    {
                        if (obj.AddMethod != null)
                        {
                            Fix(obj.AddMethod);
                        }

                        if (obj.InvokeMethod != null)
                        {
                            Fix(obj.InvokeMethod);
                        }

                        if (obj.RemoveMethod != null)
                        {
                            Fix(obj.RemoveMethod);
                        }

                        foreach (var method in obj.OtherMethods)
                        {
                            Fix(method);
                        }

                        obj.EventType = ReferenceResolver.Fix(type.Module, obj.EventType);
                    }

                    for (var i = 0; i < type.GenericParameters.Count; ++i)
                    {
                        type.GenericParameters[i] = ReferenceResolver.Fix(type.Module, type, type.GenericParameters[i]);
                    }

                    foreach (var obj in type.NestedTypes)
                    {
                        Fix(obj);
                    }

                    foreach (var obj in type.Methods)
                    {
                        Fix(obj);
                    }

                    foreach (var obj in type.Fields)
                    {
                        Fix(obj);
                    }
                }

                private static void Fix(MethodDefinition meth)
                {
                    foreach (var obj in meth.Parameters)
                    {
                        if (!MatchesPlatform(obj.ParameterType))
                        {
                            obj.ParameterType = ReferenceResolver.Fix(meth.Module, obj.ParameterType);
                        }
                    }

                    if (!MatchesPlatform(meth.MethodReturnType.ReturnType))
                    {
                        meth.MethodReturnType.ReturnType = ReferenceResolver.Fix(
                            meth.Module,
                            meth.MethodReturnType.ReturnType);
                    }

                    for (var i = 0; i < meth.Overrides.Count; ++i)
                    {
                        meth.Overrides[i] = ReferenceResolver.Fix(meth.Module, meth.Overrides[i]);
                    }

                    if (!meth.HasBody || meth.Body == null)
                    {
                        return;
                    }

                    if (meth.Body.ThisParameter != null && !MatchesPlatform(meth.Body.ThisParameter.ParameterType))
                    {
                        meth.Body.ThisParameter.ParameterType = ReferenceResolver.Fix(
                            meth.Module,
                            meth.Body.ThisParameter.ParameterType);
                    }

                    if (meth.Body.HasVariables)
                    {
                        foreach (var v in meth.Body.Variables)
                        {
                            v.VariableType = ReferenceResolver.Fix(meth.Module, v.VariableType);
                        }
                    }

                    foreach (var insn in meth.Body.Instructions)
                    {
                        var oper = insn.Operand;
                        var asType = oper as TypeReference;
                        var asMeth = oper as MethodReference;
                        var asField = oper as FieldReference;
                        var asVar = oper as VariableDefinition;
                        var asParam = oper as ParameterDefinition;
                        var asCall = oper as CallSite;
                        if (asType != null)
                        {
                            insn.Operand = ReferenceResolver.Fix(meth.Module, asType);
                        }
                        else if (asMeth != null)
                        {
                            insn.Operand = ReferenceResolver.Fix(meth.Module, asMeth);
                        }
                        else if (asField != null)
                        {
                            insn.Operand = ReferenceResolver.Fix(meth.Module, asField);
                        }
                        else if (asVar != null)
                        {
                            asVar.VariableType = ReferenceResolver.Fix(meth.Module, asVar.VariableType);
                        }
                        else if (asParam != null)
                        {
                            asParam.ParameterType = ReferenceResolver.Fix(meth.Module, asParam.ParameterType);
                        }
                        else if (asCall != null)
                        {
                            foreach (var param in asCall.Parameters)
                            {
                                param.ParameterType = ReferenceResolver.Fix(meth.Module, param.ParameterType);
                            }

                            asCall.ReturnType = ReferenceResolver.Fix(meth.Module, asCall.ReturnType);
                        }
                    }
                }

                private static void Fix(FieldReference field)
                {
                    if (!MatchesPlatform(field.FieldType))
                    {
                        field.FieldType = ReferenceResolver.Fix(field.Module, field.FieldType);
                    }
                }
            }

            #endregion
        }
    }
}