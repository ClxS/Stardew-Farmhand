using System;
using System.Linq;

namespace Farmhand.Helpers
{
    public static class TypeExtensionMethods
    {
        public static object[] GetTypesWithCustomAttribute(this Type type, string fullName)
        {
            return type.GetCustomAttributes(false).ToList().Where(n => n.GetType().FullName == fullName).ToArray();
        }
    }
}
