using System;
using System.Linq;
using System.Reflection;

namespace Farmhand.Helpers
{
    public static class AssemblyExtensionMethods
    {
        public static Type[] GetTypesWithCustomAttribute(this Assembly assembly, string fullName)
        {
            var types = assembly.GetTypes()
                .Where(m => m.GetCustomAttributes(false).ToList()
                    .Any(n => n.GetType().FullName == fullName)).ToArray();
            return types;      
        }

        public static MethodInfo[] GetMethodsWithCustomAttribute(this Assembly assembly, string fullName)
        {
            var methods = assembly.GetTypes()
            .SelectMany(t => t.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            .Where(m => m.GetCustomAttributes(false).ToList().Any(n => n.GetType().FullName == fullName))
            .ToArray();
            return methods;
        }
    }
}
