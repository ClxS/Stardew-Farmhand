using System.Collections.Generic;
using System.Linq;

namespace Farmhand.Registries
{
    /// <summary>
    /// A general use registry class. 
    /// </summary>
    /// <typeparam name="TKey">The UniqueID type</typeparam>
    /// <typeparam name="T">The type to store</typeparam>
    public class Registry<TKey, T> where T : class
    {        
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Registry() 
        {
            RegisteredItems = new Dictionary<TKey, T>();
        }
        /// <summary>
        /// Registered Items
        /// </summary>
        protected Dictionary<TKey, T> RegisteredItems { get; set; }

        /// <summary>
        /// Returns the value with the matching key
        /// </summary>
        /// <param name="key">Key to find</param>
        /// <returns></returns>
        public virtual T GetItem(TKey key)
        {
            var registryItem = RegisteredItems.ContainsKey(key) ? RegisteredItems[key] : null;
            return registryItem;
        }

        /// <summary>
        /// Registers a new item
        /// </summary>
        /// <param name="key">Key to register with</param>
        /// <param name="item">Item to register</param>
        public virtual void RegisterItem(TKey key, T item)
        {
            if (GetItem(key) == null)
            {
                RegisteredItems[key] = item;
            }
        }

        /// <summary>
        /// Gets all registered items
        /// </summary>
        /// <returns>All registered items</returns>
        public virtual IEnumerable<T> GetRegisteredItems()
        {
            return RegisteredItems.Select(n => n.Value);
        }

        /// <summary>
        /// Returns the internal dictionary object
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<TKey, T> GetAll()
        {
            return RegisteredItems;
        }

        /// <summary>
        /// Removes an item
        /// </summary>
        /// <param name="key">Key of item to remove</param>
        public virtual void UnregisterItem(TKey key)
        {
            if (GetItem(key) != null)
            {
                RegisteredItems.Remove(key);
            }
        }
    }
}
