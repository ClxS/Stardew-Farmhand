using Microsoft.Xna.Framework.Graphics;
using Farmhand.Registries.Containers;
using System.Collections.Generic;

namespace Farmhand.Registries
{
    /// <summary>
    /// Holds a reference to loaded textures. This class stores ordinary textures passed through but it primarily used to store mod textures
    /// </summary>
    public static class TextureRegistry
    {
        private static Registry<string, Texture2D> _textureRegistryInstance;
        private static Registry<string, Texture2D> RegistryInstance => _textureRegistryInstance ?? (_textureRegistryInstance = new Registry<string, Texture2D>());

        private static Registry<string, ModTexture> _modTextureRegistryInstance;
        private static Registry<string, ModTexture> ModTextureRegistryInstance => _modTextureRegistryInstance ?? (_modTextureRegistryInstance = new Registry<string, ModTexture>());

        #region Standard Texture Registry
    
        /// <summary>
        /// Returns all registered textures
        /// </summary>
        /// <returns>All registered textures</returns>
        public static IEnumerable<Texture2D> GetRegisteredTextures()
        {
            return RegistryInstance.GetRegisteredItems();
        }

        /// <summary>
        /// Returns item with matching id
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>Matching texture</returns>
        public static Texture2D GetItem(string itemId)
        {
            return RegistryInstance.GetItem(itemId);
        }
        
        /// <summary>
        /// Registers item with it
        /// </summary>
        /// <param name="itemId">Id of item to register</param>
        /// <param name="item">Texture to register</param>
        public static void RegisterItem(string itemId, Texture2D item)
        {
            RegistryInstance.RegisterItem(itemId, item);
        }

        /// <summary>
        /// Removes an item with id
        /// </summary>
        /// <param name="itemId">Id to remove</param>
        public static void UnregisterItem(string itemId)
        {
            RegistryInstance.UnregisterItem(itemId);
        }

        #endregion
        #region ModTexture Registry

        /// <summary>
        /// Gets all registered mod textures
        /// </summary>
        /// <returns>All registered mod textures</returns>
        public static IEnumerable<Texture2D> GetRegisteredModTextures()
        {
            return RegistryInstance.GetRegisteredItems();
        }

        /// <summary>
        /// Registers new mod texture
        /// </summary>
        /// <param name="mod">Mod which this texture belongs to</param>
        /// <param name="itemId">Id of texture</param>
        /// <param name="item">Texture to register</param>
        public static void RegisterItem(ModManifest mod, string itemId, ModTexture item)
        {
            ModTextureRegistryInstance.RegisterItem(GetModSpecificItemId(mod, itemId), item);
        }

        /// <summary>
        /// Removes a mod texture
        /// </summary>
        /// <param name="mod">Mod which this texture belongs to</param>
        /// <param name="itemId">Id of texture</param>
        public static void UnregisterItem(ModManifest mod, string itemId)
        {
            ModTextureRegistryInstance.UnregisterItem(GetModSpecificItemId(mod, itemId));
        }

        /// <summary>
        /// Gets a mod's texture
        /// </summary>
        /// <param name="mod">Mod which this texture belongs to</param>
        /// <param name="itemId">Id of texture</param>
        /// <returns>Matching mod texture</returns>
        public static ModTexture GetItem(ModManifest mod, string itemId)
        {
            return ModTextureRegistryInstance.GetItem(GetModSpecificItemId(mod, itemId));
        }

        #endregion
        #region Helper Functions
        private static string GetModSpecificPrefix(ModManifest mod)
        {
            return $"\\{mod.UniqueId}\\";
        }

        private static string GetModSpecificItemId(ModManifest mod, string itemId)
        {
            var modPrefix = GetModSpecificPrefix(mod);
            return $"{modPrefix}{itemId}";
        }
        #endregion
    }
}
