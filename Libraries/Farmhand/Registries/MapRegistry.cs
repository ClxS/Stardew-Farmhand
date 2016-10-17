using Farmhand.Registries.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xTile;

namespace Farmhand.Registries
{
    /// <summary>
    /// Holds a reference to loaded maps.
    /// </summary>
    public static class MapRegistry
    {
        private static Registry<string, ModMap> _mapRegistryInstance;
        private static Registry<string, ModMap> RegistryInstance => _mapRegistryInstance ?? (_mapRegistryInstance = new Registry<string, ModMap>());

        /// <summary>
        /// Returns all registered maps
        /// </summary>
        /// <returns>All registered maps</returns>
        public static IEnumerable<ModMap> GetRegisteredMaps()
        {
            return RegistryInstance.GetRegisteredItems();
        }

        /// <summary>
        /// Returns item with matching id
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="mod">Owning mod, defaults to null</param>
        /// <returns>Matching map</returns>
        public static ModMap GetItem(string itemId, ModManifest mod = null)
        {
            return RegistryInstance.GetItem(mod == null ? itemId : GetModSpecificId(mod, itemId));
        }

        /// <summary>
        /// Registers item with it
        /// </summary>
        /// <param name="itemId">Id of item to register</param>
        /// <param name="item">Map to register</param>
        /// <param name="mod">Owning mod, defaults to null</param>
        public static void RegisterItem(string itemId, ModMap item, ModManifest mod = null)
        {
            RegistryInstance.RegisterItem(mod == null ? itemId : GetModSpecificId(mod, itemId), item);
        }

        /// <summary>
        /// Removes an item with id
        /// </summary>
        /// <param name="itemId">Id to remove</param>
        /// <param name="mod">Owning mod, defaults to null</param>
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
