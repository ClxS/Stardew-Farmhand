using Farmhand.Registries.Containers;
using System.Collections.Generic;

namespace Farmhand.Registries
{
    /// <summary>
    /// Holds a reference to loaded textures. This class stores ordinary textures passed through but it primarily used to store mod textures
    /// </summary>
    public static class TextureRegistry
    {
        private static Registry<string, DiskTexture> _textureRegistryInstance;
        private static Registry<string, DiskTexture> RegistryInstance => _textureRegistryInstance ?? (_textureRegistryInstance = new Registry<string, DiskTexture>());
        
        /// <summary>
        /// Returns all registered textures
        /// </summary>
        /// <returns>All registered textures</returns>
        public static IEnumerable<DiskTexture> GetRegisteredTextures()
        {
            return RegistryInstance.GetRegisteredItems();
        }

        /// <summary>
        /// Returns item with matching id
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="mod">Owning mod, defaults to null</param>
        /// <returns>Matching texture</returns>
        public static DiskTexture GetItem(string itemId, ModManifest mod = null)
        {
            return RegistryInstance.GetItem(mod == null ? itemId : GetModSpecificId(mod, itemId));
        }

        /// <summary>
        /// Registers item with it
        /// </summary>
        /// <param name="itemId">Id of item to register</param>
        /// <param name="item">Texture to register</param>
        /// <param name="mod">Owning mod, defaults to null</param>
        public static void RegisterItem(string itemId, DiskTexture item, ModManifest mod = null)
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
