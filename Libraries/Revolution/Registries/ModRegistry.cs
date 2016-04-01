using Revolution.Registries.Containers;
using System.Collections.Generic;

namespace Revolution.Registries
{
    public static class ModRegistry
    {
        private static Registry<string, ModManifest> _modRegistryInstance;
        private static Registry<string, ModManifest> RegistryInstance => _modRegistryInstance ?? (_modRegistryInstance = new Registry<string, ModManifest>());

        public static ModManifest GetItem(string key)
        {
            return RegistryInstance.GetItem(key);
        }

        public static IEnumerable<ModManifest> GetRegisteredItems()
        {
            return RegistryInstance.GetRegisteredItems();
        }

        public static void RegisterItem(string itemId, ModManifest item)
        {
            RegistryInstance.RegisterItem(itemId, item);
        }

        public static void UnregisterItem(string itemId)
        {
            RegistryInstance.UnregisterItem(itemId);
        }
    }
}
