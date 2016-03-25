using System.Collections.Generic;
using Revolution.Registries.Containers;

namespace Revolution.Registries
{
    public static class XnbRegistry
    {
        private static Registry<string, ModXnb> _modRegistryInstance;
        private static Registry<string, ModXnb> RegistryInstance => _modRegistryInstance ?? (_modRegistryInstance = new Registry<string, ModXnb>());

        public static ModXnb GetItem(string key)
        {
            return RegistryInstance.GetItem(key);
        }

        public static IEnumerable<ModXnb> GetRegisteredItems()
        {
            return RegistryInstance.GetRegisteredItems();
        }

        public static void RegisterItem(string itemId, ModXnb item)
        {
            RegistryInstance.RegisterItem(itemId, item);
        }

        public static void UnregisterItem(string itemId)
        {
            RegistryInstance.UnregisterItem(itemId);
        }
    }
}
