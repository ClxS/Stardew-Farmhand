using Microsoft.Xna.Framework.Graphics;
using Revolution.Registries.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Registries
{
    public class TextureRegistry
    {
        private static Registry<string, Texture2D> _textureRegistryInstance;
        private static Registry<string, Texture2D> RegistryInstance
        {
            get
            {
                if (_textureRegistryInstance == null)
                {
                    _textureRegistryInstance = new Registry<string, Texture2D>();
                }
                return _textureRegistryInstance;
            }
        }

        private static Registry<string, ModTexture> _modTextureRegistryInstance;
        private static Registry<string, ModTexture> ModTextureRegistryInstance
        {
            get
            {
                if (_modTextureRegistryInstance == null)
                {
                    _modTextureRegistryInstance = new Registry<string, ModTexture>();
                }
                return _modTextureRegistryInstance;
            }
        }
        
        #region Standard Texture Registry

        public static IEnumerable<Texture2D> GetRegisteredTextures()
        {
            return RegistryInstance.GetRegisteredItems();
        }

        public static Texture2D GetItem(string itemId)
        {
            return RegistryInstance.GetItem(itemId);
        }
        
        public static void RegisterItem(string itemId, Texture2D item)
        {
            RegistryInstance.RegisterItem(itemId, item);
        }

        public static void UnregisterItem(string itemId)
        {
            RegistryInstance.UnregisterItem(itemId);
        }

        #endregion
        #region ModTexture Registry

        public static IEnumerable<Texture2D> GetRegisteredModTextures()
        {
            return RegistryInstance.GetRegisteredItems();
        }

        public static void RegisterItem(ModInfo mod, string itemId, ModTexture item)
        {
            ModTextureRegistryInstance.RegisterItem(GetModSpecificItemId(mod, itemId), item);
        }

        public static void UnregisterItem(ModInfo mod, string itemId)
        {
            ModTextureRegistryInstance.UnregisterItem(GetModSpecificItemId(mod, itemId));
        }

        public static ModTexture GetItem(ModInfo mod, string itemId)
        {
            return ModTextureRegistryInstance.GetItem(GetModSpecificItemId(mod, itemId));
        }

        #endregion
        #region Helper Functions
        private static string GetModSpecificPrefix(ModInfo mod)
        {
            return $"\\{mod.UniqueId}\\";
        }

        private static string GetModSpecificItemId(ModInfo mod, string itemId)
        {
            var modPrefix = GetModSpecificPrefix(mod);
            return $"{modPrefix}{itemId}";
        }
        #endregion
    }
}
