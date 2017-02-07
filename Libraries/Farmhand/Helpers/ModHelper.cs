namespace Farmhand.Helpers
{
    using System.Linq;
    using System.Reflection;

    using Farmhand.Registries;
    using Farmhand.Registries.Containers;

    internal static class ModHelper
    {
        public static IModManifest GetModForAssembly(Assembly assembly)
        {
            return
                ModRegistry.GetRegisteredItems()
                    .FirstOrDefault(n => n is ModManifest && ((ModManifest)n).ModAssembly == assembly);
        }
    }
}