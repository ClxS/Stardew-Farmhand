using Farmhand.Registries.Containers;
using System.Collections.Generic;
using Farmhand.Helpers;

namespace Farmhand.Registries
{
    /// <summary>
    /// Holds a reference to every loaded mod manifest
    /// </summary>
    public static class ModRegistry
    {
        private static Registry<UniqueId<string>, IModManifest> _modRegistryInstance;
        private static Registry<UniqueId<string>, IModManifest> RegistryInstance => _modRegistryInstance ?? (_modRegistryInstance = new Registry<UniqueId<string>, IModManifest>());

        /// <summary>
        /// Returns a selected mod manifest
        /// </summary>
        /// <param name="key">The Unique ID of the mod</param>
        /// <returns></returns>
        public static IModManifest GetItem(UniqueId<string> key)
        {
            return RegistryInstance.GetItem(key);
        }

        /// <summary>
        /// Returns all registered mods
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IModManifest> GetRegisteredItems()
        {
            return RegistryInstance.GetRegisteredItems();
        }

        /// <summary>
        /// Registers a mod
        /// </summary>
        /// <param name="itemId">The UniqueID of the mod</param>
        /// <param name="item">The mod manifest</param>
        public static void RegisterItem(UniqueId<string> itemId, IModManifest item)
        {
            RegistryInstance.RegisterItem(itemId, item);
        }

        /// <summary>
        /// Unregisters a mod
        /// </summary>
        /// <param name="itemId">The Unique ID of the mod</param>
        public static void UnregisterItem(UniqueId<string> itemId)
        {
            RegistryInstance.UnregisterItem(itemId);
        }
    }
}
