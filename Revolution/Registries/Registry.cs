using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Registries
{
    public abstract class Registry<T, TKey>
    {
        public Registry() 
        {
            RegisteredItems = new List<RegistryItem<T, TKey>>();
        }
        protected List<RegistryItem<T, TKey>> RegisteredItems { get; set; }

        public abstract T GetItem(TKey key);
        public abstract void UnregisterItem(TKey key);

        public virtual void RegisterItem(TKey itemId, T item)
        {
            if (GetItem(itemId) != null)
            {
                RegisteredItems.Add(new RegistryItem<T, TKey>(itemId, item));
            }
        }        
    }
}
