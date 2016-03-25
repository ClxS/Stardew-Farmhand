using System.Collections.Generic;
using System.Linq;

namespace Revolution.Registries
{
    public class Registry<TKey, T> where T : class
    {        
        public Registry() 
        {
            RegisteredItems = new Dictionary<TKey, T>();
        }
        protected Dictionary<TKey, T> RegisteredItems { get; set; }

        public virtual T GetItem(TKey key)
        {
            var registryItem = RegisteredItems.ContainsKey(key) ? RegisteredItems[key] : null;
            return registryItem;
        }

        public virtual void RegisterItem(TKey key, T item)
        {
            if (GetItem(key) == null)
            {
                RegisteredItems[key] = item;
            }
        }

        public virtual IEnumerable<T> GetRegisteredItems()
        {
            return RegisteredItems.Select(n => n.Value);
        }

        public virtual Dictionary<TKey, T> GetAll()
        {
            return RegisteredItems;
        }

        public virtual void UnregisterItem(TKey key)
        {
            if (GetItem(key) != null)
            {
                RegisteredItems.Remove(key);
            }
        }
    }
}
