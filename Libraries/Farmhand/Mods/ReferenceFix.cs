using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

// Since WINDOWS is hardcoded to true right now, this is necessary to prevent
// "unreachable code detected" from keeping the thing from compiling.
#pragma warning disable 0162

using static Farmhand.ReferenceFix.Data;

namespace Farmhand.ReferenceFix
{
    internal static class Data
    {
        internal const bool WINDOWS = true;

        // If referencing vanilla Stardew, it wasn't built for Farmhand.
        // If on Windows and referencing Mono, it won't be compatible.
        // If not on Windows and referencing Microsoft XNA, it won't be compatible.
        // If none of these are true, everything is fine.
        internal static bool MatchesPlatform(AssemblyNameReference asmRef)
        {
            return !(IsStardew(asmRef.Name) ||
                     WINDOWS && IsMono(asmRef.Name) ||
                     !WINDOWS && IsXna(asmRef.Name));
        }

        internal static bool MatchesPlatform(TypeReference asmRef)
        {
            return (IsStardew(asmRef.Namespace) ||
                    WINDOWS && IsMono(asmRef.Namespace) ||
                    !WINDOWS && IsXna(asmRef.Namespace));
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

        internal static AssemblyNameReference thisRef = AssemblyNameReference.Parse(Assembly.GetExecutingAssembly().FullName);
        internal static AssemblyNameReference monoRef = FindMyRef("MonoGame.Framework");
        internal static AssemblyNameReference xnaRef = FindMyRef("Microsoft.Xna.Framework");
        internal static AssemblyNameReference xnaGameRef = FindMyRef("Microsoft.Xna.Framework.Game");
        internal static AssemblyNameReference xnaGraphicsRef = FindMyRef("Microsoft.Xna.Framework.Graphics");

        // Find the references that we use. Technically could have hard-coded these to
        // what Farmhand uses right now, but this is a bit more future-proof.
        internal static AssemblyNameReference FindMyRef(string name)
        {
            foreach (var @ref in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                if (@ref.Name == name)
                {
                    return AssemblyNameReference.Parse(@ref.FullName);
                }
            }
            
            return null;
        }

        // This will return which assembly an XNA type is in. (See BuildXnaTypeCache())
        internal static IMetadataScope FindXnaRef(TypeReference type)
        {
            string key = type.Namespace + "." + type.Name;
            if (!xnaTypes.ContainsKey(key))
                return xnaRef;

            return xnaTypes[key];
        }

        // Since Mono <-> (Xna,Graphics,Game), we can't just replace every Mono reference
        // with XNA directly. So this will be the cache of which types go to which assembly.
        internal static Dictionary<string, AssemblyNameReference> xnaTypes = BuildXnaTypeCache();
        private static Dictionary<string, AssemblyNameReference> BuildXnaTypeCache()
        {
            // All XNA is replaced with the single Mono assembly on Linux/Mac anyways, so this won't be used.
            if (!WINDOWS)
                return new Dictionary<string, AssemblyNameReference>();

            var core = ModuleDefinition.ReadModule(typeof(Microsoft.Xna.Framework.Vector2).Module.FullyQualifiedName);
            var game = ModuleDefinition.ReadModule(typeof(Microsoft.Xna.Framework.Game).Module.FullyQualifiedName);
            var graphics = ModuleDefinition.ReadModule(typeof(Microsoft.Xna.Framework.Graphics.SpriteBatch).Module.FullyQualifiedName);

            var ret = new Dictionary<string, AssemblyNameReference>();
            foreach (TypeDefinition type in core.GetTypes())
            {
                // If they aren't public then nobody should be referencing them, so
                // we don't need to fix any of those references.
                // I can't remember why the '<' check was needed. I think compiler
                // generated stuff caused duplicate key problems?
                if (!type.IsPublic || type.Namespace.IndexOf("<") != -1) continue;
                ret.Add(type.Namespace + "." + type.Name, xnaRef);
            }
            foreach (TypeDefinition type in game.GetTypes())
            {
                if (!type.IsPublic || type.Namespace.IndexOf("<") != -1) continue;
                ret.Add(type.Namespace + "." + type.Name, xnaGameRef);
            }
            foreach (TypeDefinition type in graphics.GetTypes())
            {
                if (!type.IsPublic || type.Namespace.IndexOf("<") != -1) continue;
                ret.Add(type.Namespace + "." + type.Name, xnaGraphicsRef);
            }

            return ret;
        }
    }

    // Fix references to other things. The ones we fix are the external references,
    // but everything still needs to be checked, even if the top-level is fine.
    // Stupid generics.
    internal class Reference
    {
        private static FieldInfo typeRefField = typeof(TypeSpecification).GetField("scope", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo typeSpecField = typeof(TypeSpecification).GetField("element_type", BindingFlags.NonPublic | BindingFlags.Instance);
        internal static TypeReference Fix(ModuleDefinition def, TypeReference type)
        {
            string from = type.Scope.Name;

            TypeReference ret = type;
            if (ret.GetType() == typeof(TypeReference))
            {
                if (IsStardew(from))
                    ret = new TypeReference(type.Namespace, type.Name, null, thisRef);
                else if (!WINDOWS && IsXna(from))
                    ret = new TypeReference(type.Namespace, type.Name, null, monoRef);
                else if (WINDOWS && IsMono(from))
                    ret = new TypeReference(type.Namespace, type.Name, null, FindXnaRef(type));
                ret.IsValueType = type.IsValueType;
            }
            else if (ret.GetType() == typeof(GenericParameter))
                return Fix(def, ret.DeclaringType, ret as GenericParameter);
            else if (ret is TypeSpecification)
                typeSpecField.SetValue(type, Fix(def, (type as TypeSpecification).ElementType));
            else if (!(ret is TypeDefinition)) // Meaning it is defined in this assembly. I think?
                Logging.Log.Warning("DON'T KNOW HOW TO DEAL WITH " + ret.GetType() + " " + ret);

            if (type is GenericInstanceType)
            {
                for (int i = 0; i < (type as GenericInstanceType).ElementType.GenericParameters.Count; ++i)
                {
                    var t = (type as GenericInstanceType).ElementType.GenericParameters[i];
                    (type as GenericInstanceType).ElementType.GenericParameters[i] = Fix(def, type, t);
                }
                for (int i = 0; i < (type as GenericInstanceType).GenericArguments.Count; ++i)
                {
                    var t = (type as GenericInstanceType).GenericArguments[i];
                    (type as GenericInstanceType).GenericArguments[i] = Fix(def, t);
                }
            }
            else if (type.HasGenericParameters)
            {
                for (int i = 0; i < type.GenericParameters.Count; ++i)
                    type.GenericParameters[i] = Fix(def, type, type.GenericParameters[i]);
            }

            return def.Import(ret);
        }

        private static FieldInfo methSpecField = typeof(MethodSpecification).GetField("method", BindingFlags.NonPublic | BindingFlags.Instance);
        internal static MethodReference Fix(ModuleDefinition def, MethodReference meth)
        {
            foreach (var param in meth.Parameters)
                param.ParameterType = Fix(def, param.ParameterType);

            if (meth is GenericInstanceMethod)
            {
                var gim = (meth as GenericInstanceMethod);
                methSpecField.SetValue(meth, Fix(def, gim.ElementMethod));
                for (int i = 0; i < gim.GenericParameters.Count; ++i)
                    gim.GenericParameters[i] = Fix(def, meth, gim.GenericParameters[i]);
                for (int i = 0; i < gim.GenericArguments.Count; ++i)
                {
                    gim.GenericArguments[i] = Fix(def, gim.GenericArguments[i]);
                }
            }
            else if (meth.HasGenericParameters)
            {
                for (int i = 0; i < meth.GenericParameters.Count; ++i)
                    meth.GenericParameters[i] = Fix(def, meth, meth.GenericParameters[i]);
            }

            meth.ReturnType = Fix(def, meth.ReturnType);
            if (meth is MethodSpecification)
            {
                MethodReference elem = (methSpecField.GetValue(meth) as MethodReference);
                methSpecField.SetValue(meth, Fix(def, elem));
            }
            else
                meth.DeclaringType = Fix(def, meth.DeclaringType);
            return def.Import(meth);
        }

        internal static FieldReference Fix(ModuleDefinition def, FieldReference field)
        {
            field.DeclaringType = Fix(def, field.DeclaringType);
            field.FieldType = Fix(def, field.FieldType);
            return def.Import(field);
        }

        private static FieldInfo genDeclOwner = typeof(GenericParameter).GetField("owner", BindingFlags.NonPublic | BindingFlags.Instance);
        internal static GenericParameter Fix(ModuleDefinition def, IGenericParameterProvider parent, GenericParameter param)
        {
            for (int i = 0; i < param.Constraints.Count; ++i)
                param.Constraints[i] = Fix(def, param.Constraints[i]);
            for (int i = 0; i < param.GenericParameters.Count; ++i)
                param.GenericParameters[i] = Fix(def, param, param.GenericParameters[i]);
            return param;
        }
    }

    // Fix the actual definitions of things, mainly to go down and find all
    // of the references that need fixing.
    internal class Definition
    {
        internal static void Fix(AssemblyDefinition def)
        {
            foreach (ModuleDefinition mod in def.Modules)
            {
                foreach (var obj in mod.GetTypes())
                    Fix(obj);

                TypeReference[] refs = (TypeReference[])mod.GetTypeReferences();
                for (int i = 0; i < refs.Count(); ++i)
                {
                    if (!MatchesPlatform(refs[i]))
                        refs[i] = Reference.Fix(mod,refs[i]);
                }
            }
        }

        private static void Fix(TypeDefinition type)
        {
            if (type.BaseType != null)
                type.BaseType = Reference.Fix(type.Module, type.BaseType);
            for (int i = 0; i < type.Interfaces.Count; ++i)
                type.Interfaces[i] = Reference.Fix(type.Module, type.Interfaces[i]);
            foreach (var obj in type.Events)
            {
                if (obj.AddMethod != null)
                    Fix(obj.AddMethod);
                if (obj.InvokeMethod != null)
                    Fix(obj.InvokeMethod);
                if (obj.RemoveMethod != null)
                    Fix(obj.RemoveMethod);
                foreach (var evMeth in obj.OtherMethods)
                    Fix(evMeth);
                obj.EventType = Reference.Fix(type.Module, obj.EventType);
            }
            for (int i = 0; i < type.GenericParameters.Count; ++i)
                type.GenericParameters[i] = Reference.Fix(type.Module, type, type.GenericParameters[i]);
            foreach (var obj in type.NestedTypes)
                Fix(obj);
            foreach (var obj in type.Methods)
                Fix(obj);
            foreach (var obj in type.Fields)
                Fix(obj);
        }

        private static void Fix(MethodDefinition meth)
        {
            foreach (var obj in meth.Parameters)
                if(!MatchesPlatform(obj.ParameterType))
                    obj.ParameterType = Reference.Fix(meth.Module, obj.ParameterType);
            if (!MatchesPlatform(meth.MethodReturnType.ReturnType))
                meth.MethodReturnType.ReturnType = Reference.Fix(meth.Module, meth.MethodReturnType.ReturnType);
            for (int i = 0; i < meth.Overrides.Count; ++i)
                meth.Overrides[i] = Reference.Fix(meth.Module, meth.Overrides[i]);
            
            if (meth.HasBody && meth.Body != null)
            {
                if (meth.Body.ThisParameter != null && !MatchesPlatform(meth.Body.ThisParameter.ParameterType))
                    meth.Body.ThisParameter.ParameterType = Reference.Fix(meth.Module, meth.Body.ThisParameter.ParameterType);

                if (meth.Body.HasVariables)
                {
                    foreach (var v in meth.Body.Variables)
                        v.VariableType = Reference.Fix(meth.Module, v.VariableType);
                }

                foreach (Instruction insn in meth.Body.Instructions)
                {
                    object oper = insn.Operand;
                    var asType = oper as TypeReference;
                    var asMeth = oper as MethodReference;
                    var asField = oper as FieldReference;
                    var asVar = oper as VariableDefinition;
                    var asParam = oper as ParameterDefinition;
                    var asCall = oper as CallSite;
                    if (asType != null)
                        insn.Operand = Reference.Fix(meth.Module, asType);
                    else if (asMeth != null)
                        insn.Operand = Reference.Fix(meth.Module, asMeth);
                    else if (asField != null)
                        insn.Operand = Reference.Fix(meth.Module, asField);
                    else if (asVar != null)
                        asVar.VariableType = Reference.Fix(meth.Module, asVar.VariableType);
                    else if (asParam != null)
                        asParam.ParameterType = Reference.Fix(meth.Module, asParam.ParameterType);
                    else if (asCall != null)
                    {
                        foreach (var param in asCall.Parameters)
                            param.ParameterType = Reference.Fix(meth.Module, param.ParameterType);
                        asCall.ReturnType = Reference.Fix(meth.Module, asCall.ReturnType);
                    }
                }
            }
        }

        private static void Fix(FieldDefinition field)
        {
            if (!MatchesPlatform(field.FieldType))
                field.FieldType = Reference.Fix(field.Module, field.FieldType);
        }
    }
}
