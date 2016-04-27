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
        /// <param name="itemId">Id of ModXnb to return</param>
        /// <param name="mod">Mod this ModXnb belongs to</param>
        /// <returns>Matching ModXnb</returns>
        public static ModXnb GetItem(string itemId, ModManifest mod = null)
        {
            return RegistryInstance.GetItem(mod == null ? itemId : GetModSpecificId(mod, itemId));
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
        /// <param name="mod">Mod this ModXnb belongs to</param>
        public static void RegisterItem(string itemId, ModXnb item, ModManifest mod = null)
        {
            RegistryInstance.RegisterItem(mod == null ? itemId : GetModSpecificId(mod, itemId), item);
        }

        /// <summary>
        /// Remove ModXnb
        /// </summary>
        /// <param name="itemId">Id of ModXnb to remove</param>
        /// <param name="mod">Mod this ModXnb belongs to</param>
        public static void UnregisterItem(string itemId, ModManifest mod = null)
        {
            RegistryInstance.UnregisterItem(mod == null ? itemId : GetModSpecificId(mod, itemId));
        }

        #region Helper Functions
        private static string GetModSpecificPrefix(ModManifest mod)
        {
            return $"\\{mod.UniqueId.ThisId}\\";
        }

        public static string GetModSpecificId(ModManifest mod, string itemId)
        {
            var modPrefix = GetModSpecificPrefix(mod);
            return $"{modPrefix}{itemId}";
        }
        #endregion
    }
}
