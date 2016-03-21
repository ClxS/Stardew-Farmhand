using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Registries
{
    public class TextureRegistry
    {
        private static Registry<string, Texture2D> _modRegistryInstance;
        private static Registry<string, Texture2D> RegistryInstance
        {
            get
            {
                if (_modRegistryInstance == null)
                {
                    _modRegistryInstance = new Registry<string, Texture2D>();
                }
                return _modRegistryInstance;
            }
        }

        public static Texture2D GetItem(string key)
        {
            return RegistryInstance.GetItem(key);
        }

        public static IEnumerable<Texture2D> GetRegisteredItems()
        {
            return RegistryInstance.GetRegisteredItems();
        }

        public static void RegisterItem(string itemId, Texture2D item)
        {
            RegistryInstance.RegisterItem(itemId, item);
        }

        public static void UnregisterItem(string itemId)
        {
            RegistryInstance.UnregisterItem(itemId);
        }
    }
}
