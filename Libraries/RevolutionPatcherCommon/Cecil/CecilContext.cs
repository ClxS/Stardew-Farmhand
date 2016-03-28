using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Pdb;
using System;
using System.Linq;
using System.Reflection;

namespace Revolution.Cecil
{
    public class CecilContext
    {
        private AssemblyDefinition AssemblyDefinition { get; }

        public CecilContext(string assembly, bool loadPdb = false)
        {
            if (loadPdb)
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
            if (methodDef != null)
            {
                ilProcessor = methodDef.Body.GetILProcessor();
            }

            return ilProcessor;
        }

        public TypeDefinition GetTypeDefinition(string type)
        {
            if (AssemblyDefinition == null)
                throw new Exception("ERROR Assembly not properly read. Cannot parse");

            if (string.IsNullOrWhiteSpace(type))
                throw new Exception("Both type and method must be set");

            TypeDefinition typeDef = AssemblyDefinition.MainModule.Types.FirstOrDefault(n => n.FullName == type);
            return typeDef;
        }

        public MethodDefinition GetMethodDefinition(string type, string method)
        {
            MethodDefinition methodDef = null;
            TypeDefinition typeDef = GetTypeDefinition(type);

            if (typeDef != null)
            {
                methodDef = typeDef.Methods.FirstOrDefault(m => m.Name == method);
            }

            return methodDef;
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

        public void WriteAssembly(string file, bool writePdb = false)
        {
            if(writePdb)
            {
                var writerParameters = new WriterParameters
                {
                    SymbolWriterProvider = new PdbWriterProvider(),
                    WriteSymbols = true,
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
