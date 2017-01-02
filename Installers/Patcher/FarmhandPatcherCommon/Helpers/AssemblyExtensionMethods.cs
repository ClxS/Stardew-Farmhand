namespace Farmhand.Installers.Patcher.Helpers
{
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    ///     Assembly extension methods.
    /// </summary>
    public static class AssemblyExtensionMethods
    {
        /// <summary>
        ///     Gets all types with custom attributes attached.
        /// </summary>
        /// <param name="assembly">
        ///     The assembly to search.
        /// </param>
        /// <param name="fullName">
        ///     The full name of the attribute to be searched for.
        /// </param>
        /// <returns>
        ///     The <see cref="Type" /> of types with the provided attribute.
        /// </returns>
        public static Type[] GetTypesWithCustomAttribute(this Assembly assembly, string fullName)
        {
            var types =
                assembly.GetTypes()
                    .Where(m => m.GetCustomAttributes(false).ToList().Any(n => n.GetType().FullName == fullName))
                    .ToArray();
            return types;
        }

        /// <summary>
        ///     Gets all methods with custom attributes attached.
        /// </summary>
        /// <param name="assembly">
        ///     The assembly to search.
        /// </param>
        /// <param name="fullName">
        ///     The full name of the attribute to be searched for.
        /// </param>
        /// <returns>
        ///     The <see cref="MethodInfo" /> of types with the provided attribute.
        /// </returns>
        public static MethodInfo[] GetMethodsWithCustomAttribute(this Assembly assembly, string fullName)
        {
            var methods =
                assembly.GetTypes()
                    .SelectMany(
                        t =>
                            t.GetMethods(
                                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance
                                | BindingFlags.Static))
                    .Where(m => m.GetCustomAttributes(false).ToList().Any(n => n.GetType().FullName == fullName))
                    .ToArray();
            return methods;
        }
    }
}