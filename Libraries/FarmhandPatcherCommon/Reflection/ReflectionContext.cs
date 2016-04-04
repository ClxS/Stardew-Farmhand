using System;
using System.Reflection;
using Mono.Cecil;

namespace Farmhand.Reflection
{
    public class ReflectionContext
    {
        private AssemblyDefinition AssemblyDefinition { get; }

        public ReflectionContext(string assemblyPath)
        {
            AssemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyPath);
            //_assemblyDefinition = AssemblyDefinition.ReadAssembly(Constants.StardewExePath);
        }
        
        public ConstructorInfo GetSmapiTypeContructor(string type)
        {
            if (AssemblyDefinition == null)
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
            if (AssemblyDefinition == null)
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
