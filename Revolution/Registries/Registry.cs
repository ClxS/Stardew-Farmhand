using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Registries
{
    public class Registry<T, TKey> where T : class
    {        
        public Registry() 
        {
            RegisteredItems = new List<RegistryItem<T, TKey>>();
        }
        protected List<RegistryItem<T, TKey>> RegisteredItems { get; set; }

        public virtual T GetItem(TKey key)
        {
            var registryItem = RegisteredItems.FirstOrDefault(n => EqualityComparer<TKey>.Default.Equals(n.UniqueId, key));
            return registryItem != null ? registryItem.Item : null;
        }

        public virtual void RegisterItem(TKey itemId, T item)
        {
            if (GetItem(itemId) != null)
            {
                RegisteredItems.Add(new RegistryItem<T, TKey>(itemId, item));
            }
        }

        public virtual void UnregisterItem(TKey itemId)
        {
            if (GetItem(itemId) != null)
            {
                RegisteredItems.Remove(RegisteredItems.FirstOrDefault(n => EqualityComparer<TKey>.Default.Equals(n.UniqueId, itemId)));
            }
        }
    }
}
