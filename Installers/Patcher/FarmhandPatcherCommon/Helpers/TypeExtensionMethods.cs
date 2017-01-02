namespace Farmhand.Installers.Patcher.Helpers
{
    using System;
    using System.Linq;

    /// <summary>
    ///     Extension methods for <see cref="Type" />
    /// </summary>
    public static class TypeExtensionMethods
    {
        /// <summary>
        ///     Gets custom attributes of the specified type.
        /// </summary>
        /// <param name="type">
        ///     The <see cref="Type" />.
        /// </param>
        /// <param name="fullName">
        ///     The name of the type to find
        /// </param>
        /// <returns>
        ///     An array of <see cref="object" /> of the matching attributes.
        /// </returns>
        public static object[] GetTypesWithCustomAttribute(this Type type, string fullName)
        {
            return type.GetCustomAttributes(false).ToList().Where(n => n.GetType().FullName == fullName).ToArray();
        }
    }
}