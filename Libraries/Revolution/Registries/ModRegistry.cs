using Revolution.Registries.Containers;
using System.Collections.Generic;

namespace Revolution.Registries
{
    public class ModRegistry
    {
        private static Registry<string, ModInfo> _modRegistryInstance;
        private static Registry<string, ModInfo> RegistryInstance => _modRegistryInstance ?? (_modRegistryInstance = new Registry<string, ModInfo>());

        public static ModInfo GetItem(string key)
        {
            return RegistryInstance.GetItem(key);
        }

        public static IEnumerable<ModInfo> GetRegisteredItems()
        {
            return RegistryInstance.GetRegisteredItems();
        }

        public static void RegisterItem(string itemId, ModInfo item)
        {
            RegistryInstance.RegisterItem(itemId, item);
        }

        public static void UnregisterItem(string itemId)
        {
            RegistryInstance.UnregisterItem(itemId);
        }
    }
}
