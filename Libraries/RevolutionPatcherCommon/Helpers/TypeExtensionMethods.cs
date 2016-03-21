using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Helpers
{
    public static class TypeExtensionMethods
    {
        public static object[] GetTypesWithCustomAttribute(this Type type, string fullName)
        {
            return type.GetCustomAttributes(false).ToList().Where(n => n.GetType().FullName == fullName).ToArray();
        }
    }
}
