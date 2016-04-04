using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Farmhand.Registries.Containers;

namespace Farmhand.Registries
{
    /// <summary>
    /// Stores registered XNB overloads
    /// </summary>
    public static class XnbRegistry
    {
        private static Registry<string, ModXnb> _modRegistryInstance;
        private static Registry<string, ModXnb> RegistryInstance => _modRegistryInstance ?? (_modRegistryInstance = new Registry<string, ModXnb>());

        /// <summary>
        /// Get ModXnb with key
        /// </summary>
        /// <param name="key">Id of ModXnb to return</param>
        /// <returns>Matching ModXnb</returns>
        public static ModXnb GetItem(string key)
        {
            return RegistryInstance.GetItem(key);
        }

        /// <summary>
        /// Returns all registered ModXnbs
        /// </summary>
        /// <returns>All ModXnbs</returns>
        public static IEnumerable<ModXnb> GetRegisteredItems()
        {
            return RegistryInstance.GetRegisteredItems();
        }

        /// <summary>
        /// Register ModXnb with key
        /// </summary>
        /// <param name="itemId">Id of ModXnb to return</param>
        /// <param name="item">ModXnb to register</param>
        public static void RegisterItem(string itemId, ModXnb item)
        {
            RegistryInstance.RegisterItem(itemId, item);
        }

        /// <summary>
        /// Remove ModXnb
        /// </summary>
        /// <param name="itemId">Id of ModXnb to remove</param>
        public static void UnregisterItem(string itemId)
        {
            RegistryInstance.UnregisterItem(itemId);
        }
    }
}
