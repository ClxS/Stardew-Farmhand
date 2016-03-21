using Mono.Cecil;
using System;
using System.Reflection;

namespace Revolution.Cecil
{
    public class ReflectionContext
    {
        private AssemblyDefinition _assemblyDefinition { get; set; }

        public ReflectionContext(string assemblyPath)
        {
            //_assemblyDefinition = AssemblyDefinition.ReadAssembly(Assembly.GetExecutingAssembly().Location);
            //_assemblyDefinition = AssemblyDefinition.ReadAssembly(Constants.StardewExePath);
        }
        
        public ConstructorInfo GetSMAPITypeContructor(string type)
        {
            if (_assemblyDefinition == null)
                throw new Exception("ERROR Assembly not properly read. Cannot parse");
            
            ConstructorInfo methodInfo = null;

            var reflectionType = Assembly.GetExecutingAssembly().GetType(type);
            if (reflectionType != null)
            {
                methodInfo = reflectionType.GetConstructor(Type.EmptyTypes);
            }

            return methodInfo;
        }

        public MethodInfo GetMethodReference(string type, string method)
        {
            if (_assemblyDefinition == null)
                throw new Exception("ERROR Assembly not properly read. Cannot parse");

            MethodInfo methodInfo = null;
            var reflectionType = Assembly.GetExecutingAssembly().GetType(type);
            if (reflectionType != null)
            {
                methodInfo = reflectionType.GetMethod(method);
            }

            return methodInfo;
        }        
    }
}
