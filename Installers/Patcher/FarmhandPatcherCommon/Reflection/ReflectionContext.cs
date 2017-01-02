namespace Farmhand.Installers.Patcher.Reflection
{
    using System;
    using System.Reflection;

    using Mono.Cecil;

    internal class ReflectionContext
    {
        public ReflectionContext(string assemblyPath)
        {
            this.AssemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyPath);

            // _assemblyDefinition = AssemblyDefinition.ReadAssembly(Constants.StardewExePath);
        }

        private AssemblyDefinition AssemblyDefinition { get; }

        public ConstructorInfo GetSmapiTypeConstructor(string type)
        {
            if (this.AssemblyDefinition == null)
            {
                throw new Exception("ERROR Assembly not properly read. Cannot parse");
            }

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
            if (this.AssemblyDefinition == null)
            {
                throw new Exception("ERROR Assembly not properly read. Cannot parse");
            }

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