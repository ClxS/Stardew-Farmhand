namespace Farmhand.Registries
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     A general use registry class.
    /// </summary>
    /// <typeparam name="TKey">The UniqueID type</typeparam>
    /// <typeparam name="T">The type to store</typeparam>
    public class Registry<TKey, T>
        where T : class
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Registry{TKey,T}" /> class.
        /// </summary>
        public Registry()
        {
            this.RegisteredItems = new Dictionary<TKey, T>();
        }

        /// <summary>
        ///     Gets or sets the registered items.
        /// </summary>
        protected Dictionary<TKey, T> RegisteredItems { get; set; }

        /// <summary>
        ///     Returns the value with the matching key
        /// </summary>
        /// <param name="key">Key to find</param>
        /// <returns>
        ///     The item specified by the key.
        /// </returns>
        public virtual T GetItem(TKey key)
        {
            var registryItem = this.RegisteredItems.ContainsKey(key) ? this.RegisteredItems[key] : null;
            return registryItem;
        }

        /// <summary>
        ///     Registers a new item
        /// </summary>
        /// <param name="key">Key to register with</param>
        /// <param name="item">Item to register</param>
        public virtual void RegisterItem(TKey key, T item)
        {
            if (this.GetItem(key) == null)
            {
                this.RegisteredItems[key] = item;
            }
        }

        /// <summary>
        ///     Gets all registered items
        /// </summary>
        /// <returns>All registered items</returns>
        public virtual IEnumerable<T> GetRegisteredItems()
        {
            return this.RegisteredItems.Select(n => n.Value);
        }

        /// <summary>
        ///     Returns the internal dictionary object
        /// </summary>
        /// <returns>
        ///     A <see cref="Registry{TKey,T}" /> of all items.
        /// </returns>
        public virtual Dictionary<TKey, T> GetAll()
        {
            return this.RegisteredItems;
        }

        /// <summary>
        ///     Removes an item
        /// </summary>
        /// <param name="key">Key of item to remove</param>
        public virtual void UnregisterItem(TKey key)
        {
            if (this.GetItem(key) != null)
            {
                this.RegisteredItems.Remove(key);
            }
        }
    }
}