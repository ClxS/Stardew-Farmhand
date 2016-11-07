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

namespace Farmhand
{
    internal static class ReferenceFixData
    {
        internal const bool WINDOWS = true;

        internal static AssemblyNameReference fixRef(string @ref) { return fixRef(AssemblyNameReference.Parse(@ref)); }
        internal static AssemblyNameReference fixRef(AssemblyNameReference @ref)
        {
            if (isStardew(@ref))
                return thisRef;

            if (!WINDOWS && isXna(@ref))
            {
                return monoRef;
            }
            else if (WINDOWS && isMono(@ref))
            {
                // Okay, I really have no clue what to do here, since the mapping for mono/xna is
                // MonoGame.Framework <=> Microsoft.Xna.Framework(|.Game|.Graphics)
                // I'm hoping the metadata resolver will help with this
                return xnaRef;
            }

            return @ref;
        }

        // If referencing vanilla Stardew, it wasn't built for Farmhand.
        // If on Windows and referencing Mono, it won't be compatible.
        // If not on Windows and referencing Microsoft XNA, it won't be compatible.
        // If none of these are true, everything is fine.
        internal static bool matchesPlatform(AssemblyNameReference asmRef)
        {
            return !(isStardew(asmRef) ||
                     WINDOWS && isMono(asmRef) ||
                     !WINDOWS && isXna(asmRef));
        }

        internal static bool matchesPlatform(TypeReference asmRef)
        {
            return (isStardew(asmRef.Namespace) ||
                    WINDOWS && isMono(asmRef.Namespace) ||
                    !WINDOWS && isXna(asmRef.Namespace));
        }

        private static FieldInfo typeRefField = typeof(TypeSpecification).GetField("scope", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo typeSpecField = typeof(TypeSpecification).GetField("element_type", BindingFlags.NonPublic | BindingFlags.Instance);
        internal static TypeReference fixType(ModuleDefinition def, TypeReference type)
        {
            string from = type.Scope.Name;

            TypeReference ret = type;
            if (ret.GetType() == typeof(TypeReference))
            {
                if (isStardew(from))
                    ret = new TypeReference(type.Namespace, type.Name, null, thisRef);
                else if (!WINDOWS && isXna(from))
                    ret = new TypeReference(type.Namespace, type.Name, null, monoRef);
                else if (WINDOWS && isMono(from))
                    ret = new TypeReference(type.Namespace, type.Name, null, findXnaRef(type));
                ret.IsValueType = type.IsValueType;
            }
            else if (ret.GetType() == typeof(GenericParameter))
                return fixGeneric(def, ret.DeclaringType, ret as GenericParameter);
            else if (ret is TypeSpecification)
                typeSpecField.SetValue(type, fixType(def, (type as TypeSpecification).ElementType));
            else if (!(ret is TypeDefinition)) // Meaning it is defined in this assembly. I think?
                Logging.Log.Warning("DON'T KNOW HOW TO DEAL WITH " + ret.GetType() + " " + ret);

            if (type is GenericInstanceType)
            {
                for (int i = 0; i < (type as GenericInstanceType).ElementType.GenericParameters.Count; ++i)
                {
                    var t = (type as GenericInstanceType).ElementType.GenericParameters[i];
                    (type as GenericInstanceType).ElementType.GenericParameters[i] = fixGeneric(def, type, t);
                }
                for (int i = 0; i < (type as GenericInstanceType).GenericArguments.Count; ++i)
                {
                    var t = (type as GenericInstanceType).GenericArguments[i];
                    (type as GenericInstanceType).GenericArguments[i] = fixType(def, t);
                }
            }
            else if ( type.HasGenericParameters )
            {
                for (int i = 0; i < type.GenericParameters.Count; ++i)
                    type.GenericParameters[i] = fixGeneric(def, type, type.GenericParameters[i]);
            }

            return def.Import( ret );
        }

        private static FieldInfo methSpecField = typeof(MethodSpecification).GetField("method", BindingFlags.NonPublic | BindingFlags.Instance);
        internal static MethodReference fixMethod(ModuleDefinition def, MethodReference meth)
        {
            foreach (var param in meth.Parameters)
                param.ParameterType = fixType(def, param.ParameterType);

            if (meth is GenericInstanceMethod)
            {
                var gim = (meth as GenericInstanceMethod);
                methSpecField.SetValue(meth, fixMethod(def, gim.ElementMethod));
                for (int i = 0; i < gim.GenericParameters.Count; ++i)
                    gim.GenericParameters[i] = fixGeneric(def, meth, gim.GenericParameters[i]);
                for (int i = 0; i < gim.GenericArguments.Count; ++i)
                {
                    gim.GenericArguments[i] = fixType(def, gim.GenericArguments[i]);
                }
            }
            else if (meth.HasGenericParameters)
            {
                for (int i = 0; i < meth.GenericParameters.Count; ++i)
                    meth.GenericParameters[i] = fixGeneric(def, meth, meth.GenericParameters[i]);
            }
            
            meth.ReturnType = fixType(def, meth.ReturnType);
            if (meth is MethodSpecification)
            {
                MethodReference elem = (methSpecField.GetValue(meth) as MethodReference);
                methSpecField.SetValue(meth, fixMethod(def, elem));
            }
            else
                meth.DeclaringType = fixType(def, meth.DeclaringType);
            return def.Import(meth);
        }

        internal static FieldReference fixField(ModuleDefinition def, FieldReference field)
        {
            field.DeclaringType = fixType(def, field.DeclaringType);
            field.FieldType = fixType(def, field.FieldType);
            return def.Import(field);
        }

        private static FieldInfo genDeclOwner = typeof(GenericParameter).GetField("owner", BindingFlags.NonPublic | BindingFlags.Instance);
        internal static GenericParameter fixGeneric( ModuleDefinition def, IGenericParameterProvider parent, GenericParameter param )
        {
            for (int i = 0; i < param.Constraints.Count; ++i)
                param.Constraints[i] = fixType(def, param.Constraints[i]);
            for (int i = 0; i < param.GenericParameters.Count; ++i)
                param.GenericParameters[i] = fixGeneric(def, param, param.GenericParameters[i]);
            /*
            if (param.DeclaringType != null)
                genDeclOwner.SetValue(param, fixType(def, param.DeclaringType));
            else if (param.DeclaringMethod != null)
                genDeclOwner.SetValue(param, fixMethod(def, param.DeclaringMethod));
            //*/
            return param;
        }

        private static IMetadataScope findXnaRef(TypeReference type)
        {
            string key = type.Namespace + "." + type.Name;
            if (!xnaTypes.ContainsKey(key))
                return xnaRef;
                //Logging.Log.Error("NO XNA KEY " + key + " " + type + " " + type.Scope + " " + type.DeclaringType + " " + Environment.StackTrace);

            return xnaTypes[ key ];
        }

        internal static bool isStardew(string @ref)
        {
           return @ref.StartsWith( "Stardew Valley" ) || @ref.StartsWith( "StardewValley" );
        }
        internal static bool isStardew(AssemblyNameReference @ref)
        {
            return @ref.Name == "Stardew Valley" || @ref.Name == "StardewValley";
        }

        // Can't compare to static vars because they are null for the opposite platform
        internal static bool isXna(string @ref)
        {
            return @ref.StartsWith("Microsoft.Xna.Framework");
        }
        internal static bool isXna( AssemblyNameReference @ref )
        {
            return @ref.Name == "Microsoft.Xna.Framework" || @ref.Name == "Microsoft.Xna.Framework.Game" || @ref.Name == "Microsoft.Xna.Framework.Graphics";
        }

        internal static bool isMono(string @ref)
        {
            return @ref.StartsWith("MonoGame.Framework");
        }
        internal static bool isMono( AssemblyNameReference @ref )
        {
            return @ref.Name == "MonoGame.Framework";
        }

        internal static AssemblyNameReference thisRef = AssemblyNameReference.Parse(Assembly.GetExecutingAssembly().FullName);
        internal static AssemblyNameReference monoRef = findMyRef("MonoGame.Framework");
        internal static AssemblyNameReference xnaRef = findMyRef("Microsoft.Xna.Framework");
        internal static AssemblyNameReference xnaGameRef = findMyRef("Microsoft.Xna.Framework.Game");
        internal static AssemblyNameReference xnaGraphicsRef = findMyRef("Microsoft.Xna.Framework.Graphics");

        internal static AssemblyNameReference findMyRef( string name )
        {
            foreach ( var @ref in Assembly.GetExecutingAssembly().GetReferencedAssemblies() )
            {
                if ( @ref.Name == name )
                {
                    return AssemblyNameReference.Parse(@ref.FullName);
                }
            }
            
            return null;
        }
        
        internal static Dictionary<string, AssemblyNameReference> xnaTypes = buildXnaTypeCache();
        private static Dictionary<string, AssemblyNameReference> buildXnaTypeCache()
        {
            if (!WINDOWS) return new Dictionary<string, AssemblyNameReference>();

            var core = ModuleDefinition.ReadModule(typeof(Microsoft.Xna.Framework.Vector2).Module.FullyQualifiedName);
            var game = ModuleDefinition.ReadModule(typeof(Microsoft.Xna.Framework.Game).Module.FullyQualifiedName);
            var graphics = ModuleDefinition.ReadModule(typeof(Microsoft.Xna.Framework.Graphics.SpriteBatch).Module.FullyQualifiedName);

            var ret = new Dictionary<string, AssemblyNameReference>();
            foreach (TypeDefinition type in core.GetTypes())
            {
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

    internal class ReferenceFixDefinition
    {
        internal static void fix( AssemblyDefinition def )
        {
            Logging.Log.Verbose("FIXING " + def);
            foreach ( ModuleDefinition mod in def.Modules )
            {
                foreach (var obj in mod.GetTypes())
                    fix(obj);

                TypeReference[] refs = (TypeReference[])mod.GetTypeReferences();
                for (int i = 0; i < refs.Count(); ++i)
                {
                    if (!ReferenceFixData.matchesPlatform(refs[i]))
                        refs[i] = ReferenceFixData.fixType(mod,refs[i]);
                }
            }
        }

        private static void fix( TypeDefinition type )
        {
            if (type.BaseType != null)
                type.BaseType = ReferenceFixData.fixType(type.Module, type.BaseType);
            for (int i = 0; i < type.Interfaces.Count; ++i)
                type.Interfaces[i] = ReferenceFixData.fixType(type.Module, type.Interfaces[i]);
            foreach (var obj in type.Events)
            {
                if (obj.AddMethod != null)
                    fix(obj.AddMethod);
                if (obj.InvokeMethod != null)
                    fix(obj.InvokeMethod);
                if (obj.RemoveMethod != null)
                    fix(obj.RemoveMethod);
                foreach (var evMeth in obj.OtherMethods)
                    fix(evMeth);
                obj.EventType = ReferenceFixData.fixType(type.Module, obj.EventType);
            }
            for (int i = 0; i < type.GenericParameters.Count; ++i)
                type.GenericParameters[i] = ReferenceFixData.fixGeneric(type.Module, type, type.GenericParameters[i]);
            foreach (var obj in type.NestedTypes)
                fix(obj);
            foreach (var obj in type.Methods)
                fix(obj);
            foreach (var obj in type.Fields)
                fix(obj);
        }

        private static void fix(MethodDefinition meth)
        {
            foreach (var obj in meth.Parameters)
                if(!ReferenceFixData.matchesPlatform(obj.ParameterType))
                    obj.ParameterType = ReferenceFixData.fixType(meth.Module, obj.ParameterType);
            if (!ReferenceFixData.matchesPlatform(meth.MethodReturnType.ReturnType))
                meth.MethodReturnType.ReturnType = ReferenceFixData.fixType(meth.Module, meth.MethodReturnType.ReturnType);
            for (int i = 0; i < meth.Overrides.Count; ++i)
                meth.Overrides[i] = ReferenceFixData.fixMethod(meth.Module, meth.Overrides[i]);
            
            if (meth.HasBody && meth.Body != null)
            {
                if (meth.Body.ThisParameter != null && !ReferenceFixData.matchesPlatform(meth.Body.ThisParameter.ParameterType))
                    meth.Body.ThisParameter.ParameterType = ReferenceFixData.fixType(meth.Module, meth.Body.ThisParameter.ParameterType);

                if (meth.Body.HasVariables)
                {
                    foreach (var v in meth.Body.Variables)
                        v.VariableType = ReferenceFixData.fixType(meth.Module, v.VariableType);
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
                        insn.Operand = ReferenceFixData.fixType(meth.Module, asType);
                    else if (asMeth != null)
                        insn.Operand = ReferenceFixData.fixMethod(meth.Module, asMeth);
                    else if (asField != null)
                        insn.Operand = ReferenceFixData.fixField(meth.Module, asField);
                    else if (asVar != null)
                        asVar.VariableType = ReferenceFixData.fixType(meth.Module, asVar.VariableType);
                    else if (asParam != null)
                        asParam.ParameterType = ReferenceFixData.fixType(meth.Module, asParam.ParameterType);
                    else if (asCall != null)
                    {
                        foreach (var param in asCall.Parameters)
                            param.ParameterType = ReferenceFixData.fixType(meth.Module, param.ParameterType);
                        asCall.ReturnType = ReferenceFixData.fixType(meth.Module, asCall.ReturnType);
                    }
                }
            }
        }

        private static void fix(FieldDefinition field)
        {
            if (!ReferenceFixData.matchesPlatform(field.FieldType))
                field.FieldType = ReferenceFixData.fixType(field.Module, field.FieldType);
        }
    }
}
