namespace Farmhand.Registries
{
    using System.Collections.Generic;
    using System.Linq;

    using Farmhand.Helpers;
    using Farmhand.Registries.Containers;

    /// <summary>
    ///     Holds a reference to every loaded mod manifest
    /// </summary>
    public static class ModRegistry
    {
        private static Registry<UniqueId<string>, IModManifest> modRegistryInstance;

        private static Registry<UniqueId<string>, IModManifest> RegistryInstance
            => modRegistryInstance ?? (modRegistryInstance = new Registry<UniqueId<string>, IModManifest>());

        /// <summary>
        ///     Returns item with matching id
        /// </summary>
        /// <param name="key">
        ///     The unique ID for this item.
        /// </param>
        /// <returns>
        ///     The <see cref="IModManifest" /> for this ID
        /// </returns>
        public static IModManifest GetItem(UniqueId<string> key)
        {
            return RegistryInstance.GetItem(key);
        }

        /// <summary>
        ///     Returns item with matching id
        /// </summary>
        /// <param name="key">ID of the item to return</param>
        /// <returns>
        ///     The <see cref="IModManifest" /> for this ID
        /// </returns>
        public static IModManifest GetItem(string key)
        {
            return RegistryInstance.GetRegisteredItems().FirstOrDefault(m => m.UniqueId.Equals(key));
        }

        /// <summary>
        ///     Returns all registered textures
        /// </summary>
        /// <returns>
        ///     An <see cref="IEnumerable{IModManifest}" /> of all registered items.
        /// </returns>
        public static IEnumerable<IModManifest> GetRegisteredItems()
        {
            return RegistryInstance.GetRegisteredItems();
        }

        /// <summary>
        ///     Registers a mod
        /// </summary>
        /// <param name="itemId">The UniqueID of the mod</param>
        /// <param name="item">The mod manifest</param>
        public static void RegisterItem(UniqueId<string> itemId, IModManifest item)
        {
            RegistryInstance.RegisterItem(itemId, item);
        }

        /// <summary>
        ///     Unregisters a mod
        /// </summary>
        /// <param name="itemId">The Unique ID of the mod</param>
        public static void UnregisterItem(UniqueId<string> itemId)
        {
            RegistryInstance.UnregisterItem(itemId);
        }
    }
}