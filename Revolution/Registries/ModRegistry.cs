using Revolution.Registries.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Registries
{
    public class ModRegistry : Registry<ModInfo, string>
    {
        public override ModInfo GetItem(string key)
        {
            var registryItem = RegisteredItems.FirstOrDefault(n => n.UniqueId == key);
            return registryItem != null ? registryItem.Item : null;
        }

        public override void RegisterItem(string itemId, ModInfo item)
        {
            if (GetItem(itemId) != null)
            {
                RegisteredItems.Add(new RegistryItem<ModInfo, string>(itemId, item));
            }
        }

        public override void UnregisterItem(string itemId)
        {
            if (GetItem(itemId) != null)
            {
                RegisteredItems.Remove(RegisteredItems.FirstOrDefault(n => n.UniqueId == itemId));
            }
        }
    }
}
