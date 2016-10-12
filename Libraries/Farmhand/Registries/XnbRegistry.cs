using System.Collections.Generic;
using Farmhand.Registries.Containers;
using System.Linq;

namespace Farmhand.Registries
{
    /// <summary>
    /// Stores registered XNB overloads
    /// </summary>
    public static class XnbRegistry
    {
        private static Registry<string, List<ModXnb>> _modRegistryInstance;
        private static Registry<string, List<ModXnb>> RegistryInstance => _modRegistryInstance ?? (_modRegistryInstance = new Registry<string, List<ModXnb>>());

        /// <summary>
        /// Get ModXnb with key
        /// </summary>
        /// <param name="itemId">Id of ModXnb to return</param>
        /// <param name="mod">Mod this ModXnb belongs to</param>
        /// <param name="ignoreModPrefixes">Whether this search should ignore mod-filtering prefixes</param>
        /// <returns>Matching ModXnb</returns>
        public static IEnumerable<ModXnb> GetItem(string itemId, ModManifest mod = null, bool ignoreModPrefixes = false)
        {
            return ignoreModPrefixes 
                ? GetRegisteredItems().Where(n => n.Original == itemId) 
                : RegistryInstance.GetItem(mod == null ? itemId : GetModSpecificId(mod, itemId));
        }

        /// <summary>
        /// Returns all registered ModXnbs
        /// </summary>
        /// <returns>All ModXnbs</returns>
        public static IEnumerable<ModXnb> GetRegisteredItems()
        {
            return RegistryInstance.GetRegisteredItems().SelectMany(x => x);
        }

        /// <summary>
        /// Register ModXnb with key
        /// </summary>
        /// <param name="itemId">Id of ModXnb to return</param>
        /// <param name="item">ModXnb to register</param>
        /// <param name="mod">Mod this ModXnb belongs to</param>
        public static void RegisterItem(string itemId, ModXnb item, ModManifest mod = null)
        {
            var xnb = RegistryInstance.GetItem(mod == null ? itemId : GetModSpecificId(mod, itemId));
            if (xnb != null)
            {
                xnb.Add(item);
            }
            else
            {
                RegistryInstance.RegisterItem(mod == null ? itemId : GetModSpecificId(mod, itemId), new List<ModXnb> { item });
            }
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

        public static bool IsDirty(string itemId, ModManifest mod = null, bool ignoreModPrefixes = false)
        {
            return GetItem(itemId, mod, ignoreModPrefixes).Any(n => n.IsDirty);
        }

        public static void ClearDirtyFlag(string itemId, ModManifest mod = null, bool ignoreModPrefixes = false)
        {
            var items = GetItem(itemId, mod, ignoreModPrefixes);
            foreach (var item in items)
            {
                item.IsDirty = false;
            }
        }
    }
}
