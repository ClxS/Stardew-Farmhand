namespace Farmhand.Registries
{
    using System.Collections.Generic;
    using System.Linq;

    using Farmhand.Registries.Containers;

    /// <summary>
    ///     Stores registered XNB overloads
    /// </summary>
    public static class XnbRegistry
    {
        private static Registry<string, List<ModXnb>> modRegistryInstance;

        private static Registry<string, List<ModXnb>> RegistryInstance
            => modRegistryInstance ?? (modRegistryInstance = new Registry<string, List<ModXnb>>());

        /// <summary>
        ///     Get ModXnb with key
        /// </summary>
        /// <param name="itemId">ID of the item to return</param>
        /// <param name="mod">Mod this item belongs to</param>
        /// <param name="ignoreModPrefixes">Whether this search should ignore mod-filtering prefixes</param>
        /// <returns>
        ///     The <see cref="IEnumerable{ModXnb}" /> for this ID
        /// </returns>
        public static IEnumerable<ModXnb> GetItem(string itemId, ModManifest mod = null, bool ignoreModPrefixes = false)
        {
            return ignoreModPrefixes
                       ? GetRegisteredItems().Where(n => n.Original == itemId)
                       : RegistryInstance.GetItem(mod == null ? itemId : GetModSpecificId(mod, itemId));
        }

        /// <summary>
        ///     Returns all registered textures
        /// </summary>
        /// <returns>
        ///     An <see cref="IEnumerable{ModXnb}" /> of all registered items.
        /// </returns>
        public static IEnumerable<ModXnb> GetRegisteredItems()
        {
            return RegistryInstance.GetRegisteredItems().SelectMany(x => x);
        }

        /// <summary>
        ///     Register ModXnb with key
        /// </summary>
        /// <param name="itemId">ID of the item to return</param>
        /// <param name="item">Item to register</param>
        /// <param name="mod">Mod this item belongs to</param>
        public static void RegisterItem(string itemId, ModXnb item, ModManifest mod = null)
        {
            var xnb = RegistryInstance.GetItem(mod == null ? itemId : GetModSpecificId(mod, itemId));
            if (xnb != null)
            {
                xnb.Add(item);
            }
            else
            {
                RegistryInstance.RegisterItem(
                    mod == null ? itemId : GetModSpecificId(mod, itemId),
                    new List<ModXnb> { item });
            }
        }

        /// <summary>
        ///     Remove ModXnb
        /// </summary>
        /// <param name="itemId">ID of the item to remove</param>
        /// <param name="mod">Mod this item belongs to</param>
        public static void UnregisterItem(string itemId, ModManifest mod = null)
        {
            RegistryInstance.UnregisterItem(mod == null ? itemId : GetModSpecificId(mod, itemId));
        }

        /// <summary>
        ///     Gets whether the specified XNB is marked as dirty.
        /// </summary>
        /// <param name="itemId">
        ///     The id of the XNB to check.
        /// </param>
        /// <param name="mod">
        ///     The owning mod of the XNB to check.
        /// </param>
        /// <param name="ignoreModPrefixes">
        ///     Whether this check should ignore mod prefixes.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsDirty(string itemId, ModManifest mod = null, bool ignoreModPrefixes = false)
        {
            return GetItem(itemId, mod, ignoreModPrefixes).Any(n => n.IsDirty);
        }

        /// <summary>
        ///     Clears the dirty flag on the specified XNB.
        /// </summary>
        /// <param name="itemId">
        ///     The id of the XNB to check.
        /// </param>
        /// <param name="mod">
        ///     The owning mod of the XNB to check.
        /// </param>
        /// <param name="ignoreModPrefixes">
        ///     Whether this check should ignore mod prefixes.
        /// </param>
        public static void ClearDirtyFlag(string itemId, ModManifest mod = null, bool ignoreModPrefixes = false)
        {
            var items = GetItem(itemId, mod, ignoreModPrefixes);
            foreach (var item in items)
            {
                item.IsDirty = false;
            }
        }

        #region Helper Functions

        private static string GetModSpecificPrefix(ModManifest mod)
        {
            return $"\\{mod.UniqueId.ThisId}\\";
        }

        /// <summary>
        ///     Gets a mod specific ID
        /// </summary>
        /// <param name="mod">
        ///     The owning mod.
        /// </param>
        /// <param name="itemId">
        ///     The ID of the item to get the modified ID for.
        /// </param>
        /// <returns>
        ///     The mod specific ID.
        /// </returns>
        public static string GetModSpecificId(ModManifest mod, string itemId)
        {
            var modPrefix = GetModSpecificPrefix(mod);
            return $"{modPrefix}{itemId}";
        }

        #endregion
    }
}