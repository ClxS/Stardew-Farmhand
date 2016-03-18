using Revolution.Registries.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Registries
{
    public class ModRegistry
    {
        private static Registry<string, ModInfo> _modRegistryInstance;
        private static Registry<string, ModInfo> RegistryInstance
        {
            get
            {
                if(_modRegistryInstance == null)
                {
                    _modRegistryInstance = new Registry<string, ModInfo>();
                }
                return _modRegistryInstance;
            }
        }

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
