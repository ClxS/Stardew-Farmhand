namespace Farmhand.Installers.Patcher.Cecil
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Cecil.Pdb;
    using Mono.Collections.Generic;

    /// <summary>
    ///     A utility class which helps with injecting via Cecil.
    /// </summary>
    public class CecilContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CecilContext" /> class.
        /// </summary>
        /// <param name="assembly">
        ///     The assembly to load.
        /// </param>
        /// <param name="loadPdb">
        ///     Whether we should also load PDBs
        /// </param>
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
                this.AssemblyDefinition = AssemblyDefinition.ReadAssembly(assembly, readerParameters);
            }
            else
            {
                this.AssemblyDefinition = AssemblyDefinition.ReadAssembly(assembly);
            }
        }

        private AssemblyDefinition AssemblyDefinition { get; }

        /// <summary>
        ///     Gets the IL Processor for a specific method.
        /// </summary>
        /// <param name="type">
        ///     The type containing the method.
        /// </param>
        /// <param name="method">
        ///     The method to get the processor for.
        /// </param>
        /// <returns>
        ///     The <see cref="ILProcessor" /> for the specified method..
        /// </returns>
        /// <exception cref="Exception">
        ///     Throws an exception if AssemblyDefinition is null, or if either a type of method is not specified.
        /// </exception>
        public ILProcessor GetMethodIlProcessor(string type, string method)
        {
            if (this.AssemblyDefinition == null)
            {
                throw new Exception("ERROR Assembly not properly read. Cannot parse");
            }

            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(method))
            {
                throw new Exception("Both type and method must be set");
            }

            ILProcessor processor = null;
            var typeDef = this.GetTypeDefinition(type);
            var methodDef = typeDef?.Methods.FirstOrDefault(m => m.Name == method)
                            ?? typeDef?.Methods.FirstOrDefault(m => m.FullName == method);

            if (methodDef != null && methodDef.HasBody)
            {
                processor = methodDef.Body.GetILProcessor();
            }

            return processor;
        }

        /// <summary>
        ///     Gets a Cecil type definition.
        /// </summary>
        /// <param name="type">
        ///     The type to get the definition for.
        /// </param>
        /// <param name="toCheck">
        ///     A collection of types to check for the provided them. (Defaults to null)
        /// </param>
        /// <returns>
        ///     The <see cref="TypeDefinition" /> for the provided type.
        /// </returns>
        /// <exception cref="Exception">
        ///     Throws an exception if AssemblyDefinition is null, or a type is not specified.
        /// </exception>
        public TypeDefinition GetTypeDefinition(string type, Collection<TypeDefinition> toCheck = null)
        {
            if (this.AssemblyDefinition == null)
            {
                throw new Exception("ERROR Assembly not properly read. Cannot parse");
            }

            if (string.IsNullOrWhiteSpace(type))
            {
                throw new Exception("Both type and method must be set");
            }

            this.AssemblyDefinition.MainModule.Import(typeof(void));

            if (toCheck == null)
            {
                toCheck = this.AssemblyDefinition.MainModule.Types;
            }

            var typeDef = default(TypeDefinition);
            foreach (var def in toCheck)
            {
                if (def.FullName == type)
                {
                    typeDef = def;
                    break;
                }

                if (type.StartsWith(def.FullName) && def.HasNestedTypes)
                {
                    typeDef = this.GetTypeDefinition(type, def.NestedTypes);
                    if (typeDef != null)
                    {
                        break;
                    }
                }
            }

            return typeDef;
        }

        /// <summary>
        ///     Gets the types in the loaded AssemblyDefinition.
        /// </summary>
        /// <returns>
        ///     An <see cref="IEnumerable&lt;TypeDefinition&gt;" /> of type definitions.
        /// </returns>
        public IEnumerable<TypeDefinition> GetTypes()
        {
            return this.AssemblyDefinition.MainModule.Types;
        }

        /// <summary>
        ///     Gets all methods in the loaded AssemblyDefinition
        /// </summary>
        /// <returns>
        ///     An <see cref="IEnumerable&lt;MethodDefinition&gt;" /> of method definitions.
        /// </returns>
        public IEnumerable<MethodDefinition> GetMethods()
        {
            return this.GetTypes().SelectMany(n => n.Methods);
        }

        /// <summary>
        ///     Imports a <see cref="TypeReference" /> for the provided type from the AssemblyDefinition.
        /// </summary>
        /// <param name="type">
        ///     The type to get a reference to.
        /// </param>
        /// <returns>
        ///     The <see cref="TypeReference" /> of the provided type.
        /// </returns>
        /// <exception cref="Exception">
        ///     Throws an exception if
        ///     <param name="type" />
        ///     is null
        /// </exception>
        public TypeReference GetTypeReference(Type type)
        {
            if (type == null)
            {
                throw new Exception("Both type must be set");
            }

            return this.AssemblyDefinition.MainModule.Import(type);
        }

        /// <summary>
        ///     Gets a method definition.
        /// </summary>
        /// <param name="type">
        ///     The type containing the method.
        /// </param>
        /// <param name="method">
        ///     The method to get a definition for.
        /// </param>
        /// <param name="selector">
        ///     A selector to filter certain methods. (Defaults to null)
        /// </param>
        /// <returns>
        ///     A <see cref="MethodDefinition" /> for the specified type.
        /// </returns>
        public MethodDefinition GetMethodDefinition(
            string type,
            string method,
            Func<MethodDefinition, bool> selector = null)
        {
            MethodDefinition methodDef = null;
            var typeDef = this.GetTypeDefinition(type);

            if (typeDef != null)
            {
                methodDef = selector == null
                                ? typeDef.Methods.FirstOrDefault(m => m.Name == method)
                                : typeDef.Methods.Where(m => m.Name == method).FirstOrDefault(selector);
            }

            return methodDef;
        }

        /// <summary>
        ///     Gets a method definition.
        /// </summary>
        /// <param name="type">
        ///     The type containing the method.
        /// </param>
        /// <param name="method">
        ///     The full name of the method to get a definition for.
        /// </param>
        /// <param name="selector">
        ///     A selector to filter certain methods. (Defaults to null)
        /// </param>
        /// <returns>
        ///     A <see cref="MethodDefinition" /> for the specified type.
        /// </returns>
        public MethodDefinition GetMethodDefinitionFullName(
            string type,
            string method,
            Func<MethodDefinition, bool> selector = null)
        {
            MethodDefinition methodDef = null;
            var typeDef = this.GetTypeDefinition(type);

            if (typeDef != null)
            {
                methodDef = selector == null
                                ? typeDef.Methods.FirstOrDefault(m => m.FullName == method)
                                : typeDef.Methods.Where(m => m.FullName == method).FirstOrDefault(selector);
            }

            return methodDef;
        }

        /// <summary>
        ///     Gets a reference to the first constructor of a type.
        /// </summary>
        /// <param name="typeDefinition">
        ///     The <see cref="TypeDefinition" /> of the type containing the constructor.
        /// </param>
        /// <param name="selector">
        ///     A selector to filter certain constructors. (Defaults to null)
        /// </param>
        /// <returns>
        ///     The <see cref="MethodReference" /> for the constructor.
        /// </returns>
        public MethodReference GetConstructorReference(
            TypeDefinition typeDefinition,
            Func<MethodDefinition, bool> selector = null)
        {
            return selector == null
                       ? typeDefinition.Methods.FirstOrDefault(m => m.IsConstructor)
                       : typeDefinition.Methods.Where(m => m.IsConstructor).FirstOrDefault(selector);
        }

        /// <summary>
        ///     Gets a reference to the first constructor of a type.
        /// </summary>
        /// <param name="typeDefinition">
        ///     The <see cref="TypeDefinition" /> of the type containing the constructor.
        /// </param>
        /// <param name="method">
        ///     The name of the constructor to get.
        /// </param>
        /// <returns>
        ///     The <see cref="MethodReference" /> for the constructor.
        /// </returns>
        public MethodReference GetConstructorReference(TypeDefinition typeDefinition, string method)
        {
            var methodDefinition = typeDefinition.Methods.FirstOrDefault(m => m.Name == method);
            return this.AssemblyDefinition.MainModule.Import(methodDefinition);
        }

        /// <summary>
        ///     Gets a <see cref="PropertyDefinition" /> for the specified property.
        /// </summary>
        /// <param name="type">
        ///     The type containing the property.
        /// </param>
        /// <param name="property">
        ///     The name of the property.
        /// </param>
        /// <returns>
        ///     The <see cref="PropertyDefinition" /> for the specified property.
        /// </returns>
        public PropertyDefinition GetPropertyDefinition(string type, string property)
        {
            PropertyDefinition propertyDefinition = null;
            var typeDef = this.GetTypeDefinition(type);

            if (typeDef != null)
            {
                propertyDefinition = typeDef.Properties.FirstOrDefault(m => m.Name == property);
            }

            return propertyDefinition;
        }

        /// <summary>
        ///     Gets a <see cref="FieldDefinition" /> for the specified field.
        /// </summary>
        /// <param name="type">
        ///     The type containing the field.
        /// </param>
        /// <param name="field">
        ///     The name of the field.
        /// </param>
        /// <returns>
        ///     The <see cref="FieldDefinition" /> for the specified field.
        /// </returns>
        public FieldDefinition GetFieldDefinition(string type, string field)
        {
            FieldDefinition fieldDefinition = null;
            var typeDef = this.GetTypeDefinition(type);

            if (typeDef != null)
            {
                fieldDefinition = typeDef.Fields.FirstOrDefault(m => m.Name == field);
            }

            return fieldDefinition;
        }

        /// <summary>
        ///     Imports a method into the AssemblyDefinition.
        /// </summary>
        /// <param name="method">
        ///     The <see cref="MethodBase" /> of the method to import.
        /// </param>
        /// <returns>
        ///     The <see cref="MethodReference" /> of the imported method.
        /// </returns>
        /// <exception cref="Exception">
        ///     Throws an exception if the AssemblyDefinition is null.
        /// </exception>
        public MethodReference ImportMethod(MethodBase method)
        {
            if (this.AssemblyDefinition == null)
            {
                throw new Exception("ERROR Assembly not properly read. Cannot parse");
            }

            MethodReference reference = null;
            if (method != null)
            {
                reference = this.AssemblyDefinition.MainModule.Import(method);
            }

            return reference;
        }

        /// <summary>
        ///     Imports a method into the AssemblyDefinition.
        /// </summary>
        /// <param name="method">
        ///     The <see cref="MethodReference" /> of the method to import.
        /// </param>
        /// <returns>
        ///     The <see cref="MethodReference" /> of the imported method.
        /// </returns>
        /// <exception cref="Exception">
        ///     Throws an exception if the AssemblyDefinition is null.
        /// </exception>
        public MethodReference ImportMethod(MethodReference method)
        {
            if (this.AssemblyDefinition == null)
            {
                throw new Exception("ERROR Assembly not properly read. Cannot parse");
            }

            MethodReference reference = null;
            if (method != null)
            {
                reference = this.AssemblyDefinition.MainModule.Import(method);
            }

            return reference;
        }

        /// <summary>
        ///     Writes the modified assembly to disk.
        /// </summary>
        /// <param name="file">
        ///     The output file.
        /// </param>
        /// <param name="writePdb">
        ///     Whether an updated PDB should also be written.
        /// </param>
        public void WriteAssembly(string file, bool writePdb = false)
        {
            if (writePdb)
            {
                var writerParameters = new WriterParameters
                                           {
                                               SymbolWriterProvider = new PdbWriterProvider(),
                                               WriteSymbols = true
                                           };
                this.AssemblyDefinition.Write(file, writerParameters);
            }
            else
            {
                this.AssemblyDefinition.Write(file);
            }
        }

        /// <summary>
        ///     Inserts a new type into the AssemblyDefinition
        /// </summary>
        /// <param name="type">
        ///     The <see cref="TypeDefinition" /> to insert.
        /// </param>
        public void InsertType(TypeDefinition type)
        {
            var sdvType = new TypeDefinition(type.Namespace, type.Name, type.Attributes);
            foreach (var md in type.Methods)
            {
                sdvType.Methods.Add(md);
            }

            this.AssemblyDefinition.MainModule.Types.Add(sdvType);
        }
    }
}