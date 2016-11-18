using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Pdb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Farmhand.Cecil
{
    public class CecilContext
    {
        private AssemblyDefinition AssemblyDefinition { get; }

        public CecilContext(string assembly, bool loadPdb = false)
        {
            var pdbPath = Path.GetDirectoryName(assembly) + Path.GetFileNameWithoutExtension(assembly) + ".pdb";
            if (loadPdb && File.Exists(pdbPath))
            {
                var readerParameters = new ReaderParameters
                {
                    SymbolReaderProvider = new PdbReaderProvider(),
                    ReadSymbols = true
                };
                AssemblyDefinition = AssemblyDefinition.ReadAssembly(assembly, readerParameters);
            }
            else
            {
                AssemblyDefinition = AssemblyDefinition.ReadAssembly(assembly);
            }
        }

        public ILProcessor GetMethodIlProcessor(string type, string method)
        {
            if (AssemblyDefinition == null)
                throw new Exception("ERROR Assembly not properly read. Cannot parse");

            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(method))
                throw new Exception("Both type and method must be set");

            ILProcessor ilProcessor = null;
            var typeDef = GetTypeDefinition(type);
            var methodDef = typeDef?.Methods.FirstOrDefault(m => m.Name == method);
            if (methodDef == null)
            {
                methodDef = typeDef?.Methods.FirstOrDefault(m => m.FullName == method);
            }

            if (methodDef != null && methodDef.HasBody)
            {
                ilProcessor = methodDef.Body.GetILProcessor();
            }

            return ilProcessor;
        }

        public TypeDefinition GetTypeDefinition(string type, Mono.Collections.Generic.Collection<TypeDefinition> toCheck = null)
        {
            if (AssemblyDefinition == null)
                throw new Exception("ERROR Assembly not properly read. Cannot parse");

            if (string.IsNullOrWhiteSpace(type))
                throw new Exception("Both type and method must be set");
            AssemblyDefinition.MainModule.Import(typeof(void));

            if (toCheck == null)
                toCheck = AssemblyDefinition.MainModule.Types;
            TypeDefinition typeDef = default(TypeDefinition);
            foreach (TypeDefinition def in toCheck)
            {
                if (def.FullName == type)
                {
                    typeDef = def;
                    break;
                }
                else if (type.StartsWith(def.FullName) && def.HasNestedTypes)
                {
                    typeDef = GetTypeDefinition(type, def.NestedTypes);
                    if (typeDef != null)
                        break;
                }
            }

            return typeDef;
        }
        
        public IEnumerable<TypeDefinition> GetTypes()
        {
            return AssemblyDefinition.MainModule.Types;
        }

        public IEnumerable<MethodDefinition> GetMethods()
        {
            return GetTypes().SelectMany(n => n.Methods);
        }

        public TypeReference GetTypeReference(Type type)
        {
            if (type == null)
                throw new Exception("Both type must be set");
            return AssemblyDefinition.MainModule.Import(type);
        }

        public MethodDefinition GetMethodDefinition(string type, string method, Func<MethodDefinition, bool> selector = null)
        {
            MethodDefinition methodDef = null;
            TypeDefinition typeDef = GetTypeDefinition(type);

            if (typeDef != null)
            {
                if(selector == null)
                {
                    methodDef = typeDef.Methods.FirstOrDefault(m => m.Name == method);
                }
                else
                {
                    methodDef = typeDef.Methods.Where(m => m.Name == method).FirstOrDefault(selector);
                }
            }

            return methodDef;
        }

        public MethodDefinition GetMethodDefinitionFullName(string type, string method, Func<MethodDefinition, bool> selector = null)
        {
            MethodDefinition methodDef = null;
            TypeDefinition typeDef = GetTypeDefinition(type);

            if (typeDef != null)
            {
                if (selector == null)
                {
                    methodDef = typeDef.Methods.FirstOrDefault(m => m.FullName == method);
                }
                else
                {
                    methodDef = typeDef.Methods.Where(m => m.FullName == method).FirstOrDefault(selector);
                }
            }

            return methodDef;
        }

        public MethodReference GetConstructorReference(TypeDefinition typeDefinition, Func<MethodDefinition, bool> selector = null)
        {
            return selector == null 
                ? typeDefinition.Methods.FirstOrDefault(m => m.IsConstructor) 
                : typeDefinition.Methods.Where(m => m.IsConstructor).FirstOrDefault(selector);
        }

        public MethodReference GetConstructorReference(TypeDefinition typeDefinition, string method)
        {
            var methodDefinition = typeDefinition.Methods.FirstOrDefault(m => m.Name == method);
            return AssemblyDefinition.MainModule.Import(methodDefinition);
        }

        public PropertyDefinition GetPropertyDefinition(string type, string property)
        {
            PropertyDefinition propertyDefinition = null;
            TypeDefinition typeDef = GetTypeDefinition(type);

            if (typeDef != null)
            {
                propertyDefinition = typeDef.Properties.FirstOrDefault(m => m.Name == property);
            }

            return propertyDefinition;
        }

        public FieldDefinition GetFieldDefinition(string type, string field)
        {
            FieldDefinition fieldDefinition = null;
            TypeDefinition typeDef = GetTypeDefinition(type);

            if (typeDef != null)
            {
                fieldDefinition = typeDef.Fields.FirstOrDefault(m => m.Name == field);
            }

            return fieldDefinition;
        }

        public MethodReference ImportMethod(MethodBase method)
        {
            if (AssemblyDefinition == null)
                throw new Exception("ERROR Assembly not properly read. Cannot parse");
            
            MethodReference reference = null;
            if (method != null)
            {
                reference = AssemblyDefinition.MainModule.Import(method);
            }
            return reference;
        }

        public MethodReference ImportMethod(MethodReference method)
        {
            if (AssemblyDefinition == null)
                throw new Exception("ERROR Assembly not properly read. Cannot parse");

            MethodReference reference = null;
            if (method != null)
            {
                reference = AssemblyDefinition.MainModule.Import(method);
            }
            return reference;
        }

        public void WriteAssembly(string file, bool writePdb = false)
        {
            if(writePdb)
            {
                var writerParameters = new WriterParameters
                {
                    SymbolWriterProvider = new PdbWriterProvider(),
                    WriteSymbols = true
                };
                AssemblyDefinition.Write(file, writerParameters);
            }
            else
            {
                AssemblyDefinition.Write(file);
            }
        }

        public void InsertType(TypeDefinition type)
        {
            var sdvType = new TypeDefinition(type.Namespace, type.Name, type.Attributes);
            foreach (var md in type.Methods)
            {
                sdvType.Methods.Add(md);
            }

            AssemblyDefinition.MainModule.Types.Add(sdvType);
        }

    }
}
