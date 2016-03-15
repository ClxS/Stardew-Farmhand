using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.Cecil
{
    public class CecilContext
    {
        private AssemblyDefinition _assemblyDefinition { get; set; }

        public CecilContext(string assembly)
        {
            _assemblyDefinition = AssemblyDefinition.ReadAssembly(assembly);
        }

        public ILProcessor GetMethodILProcessor(string type, string method)
        {
            if (_assemblyDefinition == null)
                throw new Exception("ERROR Assembly not properly read. Cannot parse");

            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(method))
                throw new ArgumentNullException("Both type and method must be set");

            Mono.Cecil.Cil.ILProcessor ilProcessor = null;
            TypeDefinition typeDef = GetTypeDefinition(type);
            if (typeDef != null)
            {
                MethodDefinition methodDef = typeDef.Methods.FirstOrDefault(m => m.Name == method);
                if (methodDef != null)
                {
                    ilProcessor = methodDef.Body.GetILProcessor();
                }
            }

            return ilProcessor;
        }

        public TypeDefinition GetTypeDefinition(string type)
        {
            if (_assemblyDefinition == null)
                throw new Exception("ERROR Assembly not properly read. Cannot parse");

            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentNullException("Both type and method must be set");

            TypeDefinition typeDef = _assemblyDefinition.MainModule.Types.FirstOrDefault(n => n.FullName == type);
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
            if (_assemblyDefinition == null)
                throw new Exception("ERROR Assembly not properly read. Cannot parse");
            
            MethodReference reference = null;
            if (method != null)
            {
                reference = _assemblyDefinition.MainModule.Import(method);
            }
            return reference;
        }

        internal void WriteAssembly(string file)
        {
            _assemblyDefinition.Write(file);
        }

        public void InsertType(TypeDefinition type)
        {
            TypeDefinition sdvType = new TypeDefinition(type.Namespace, type.Name, type.Attributes);
            foreach (MethodDefinition md in type.Methods)
            {
                MethodDefinition newMethod = new MethodDefinition(md.Name, md.Attributes, md.ReturnType);
                sdvType.Methods.Add(md);
            }

            _assemblyDefinition.MainModule.Types.Add(sdvType);
        }

    }
}
