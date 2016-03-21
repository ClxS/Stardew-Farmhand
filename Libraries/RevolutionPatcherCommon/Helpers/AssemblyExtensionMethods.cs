using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Revolution.Helpers
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
    }
}
