using Revolution.Attributes;
using Revolution.Logging;
using System;
using System.Collections.Generic;
using Revolution.Registries;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using Revolution.Registries.Containers;

namespace Revolution.Content
{
    public class ContentManager : Microsoft.Xna.Framework.Content.ContentManager
    {
        private static List<Microsoft.Xna.Framework.Content.ContentManager> _modManagers;

        private readonly Dictionary<string, Texture2D> _cachedAlteredTextures = new Dictionary<string, Texture2D>();

        //TODO Do not redirect this way. There are so many (pointless) separate instances of ContentManager, and we'll need to override them all as soon as they're 
        //created. It's something more suited to the ConstructionRedirect hook.
        [Hook(HookType.Entry, "StardewValley.Game1", "LoadContent")]
        internal static void ConstructionHook()
        {
            Log.Verbose("Using Revolution's ContentManager");
            Game1.game1.Content = new ContentManager(Game1.game1.Content.ServiceProvider, Game1.game1.Content.RootDirectory);
        }

        public ContentManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            LoadModManagers();
        }
        public ContentManager(IServiceProvider serviceProvider, string rootDirectory)
            : base(serviceProvider, rootDirectory)
        {
            LoadModManagers();
        }
        
        private void LoadModManagers()
        {
            if (_modManagers != null) return;

            _modManagers = new List<Microsoft.Xna.Framework.Content.ContentManager>();
            foreach (var modPath in ModLoader.ModPaths)
            {
                _modManagers.Add(new Microsoft.Xna.Framework.Content.ContentManager(this.ServiceProvider, modPath));
            }
        }

        private Microsoft.Xna.Framework.Content.ContentManager GetContentManagerForMod(ModXnb mod)
        {
            //;mod.OwningMod.ModDirectory
            return _modManagers.FirstOrDefault(n => mod.OwningMod.ModDirectory.Contains(n.RootDirectory));
        }

        public override T Load<T>(string assetName)
        {
            var item = XnbRegistry.GetItem(assetName);
            if (item?.OwningMod?.ModState == null || item.OwningMod.ModState != ModState.Loaded) return base.Load<T>(assetName);
            
            try
            {
                if (item.IsXnb)
                {
                    var currentDirectory = Path.GetDirectoryName(item.AbsoluteFilePath);
                    var modContentManager = GetContentManagerForMod(item);
                    var relPath = modContentManager.RootDirectory + "\\";
                    if (currentDirectory != null)
                    {
                        var relRootUri = new Uri(relPath, UriKind.Absolute);
                        var fullPath = new Uri(currentDirectory, UriKind.Absolute);
                        var relUri = relRootUri.MakeRelativeUri(fullPath) + "/" + item.File;

                        Log.Verbose($"Using own asset replacement: {assetName} = {relUri}");
                        return modContentManager.Load<T>(relUri);
                    }
                }
                else if (item.IsTexture && typeof(T) == typeof(Texture2D))
                {
                    return (T)(LoadTexture(assetName, item));
                }
            }
            catch (Exception ex)
            {
                Log.Exception("Error reading own file", ex);
                return base.Load<T>(assetName);
            }
            return base.Load<T>(assetName);
        }

        private object LoadTexture(string assetName, ModXnb item)
        {
            var obj = TextureRegistry.GetItem(item.OwningMod, item.Texture).Texture;

            if (obj == null) return null;

            Log.Success(item.Destination.ToString());

            if (item.Destination != null)
            {
                //TODO, Error checking on this.
                //TODO, Multiple mods should be able to edit this
                var originalTexture = base.Load<Texture2D>(assetName);

                Log.Verbose("Is A Constructed Texture");
                string assetKey = $"{assetName}-\u2764-modified";
                if (_cachedAlteredTextures.ContainsKey(assetKey))
                {
                    Log.Verbose("Which we already had cached");
                    return _cachedAlteredTextures[assetKey];
                }
                else
                {
                    Log.Verbose("Trying to construct texture from scratch");
                    var originalData = new Color[originalTexture.Width * originalTexture.Height];
                    Color[] modData;
                    originalTexture.GetData<Color>(originalData);

                    if (item.Source == null)
                    {
                        modData = new Color[obj.Width * obj.Height];
                        obj.GetData<Color>(modData);
                    }
                    else
                    {
                        modData = new Color[item.Source.Width * item.Source.Height];
                        obj.GetData<Color>(0, item.Source, modData, 0, item.Source.Width * item.Source.Height);
                    }

                    var newObject = new Texture2D(Game1.graphics.GraphicsDevice, originalTexture.Width, originalTexture.Height);
                    newObject.SetData<Color>(originalData);
                    newObject.SetData<Color>(0, item.Destination, modData, 0, obj.Width * obj.Height);

                    _cachedAlteredTextures[assetKey] = newObject;
                    obj = newObject;
                }
            }

            Log.Verbose($"Using own asset replacement: {assetName} = {item.OwningMod.Name}.{item.Texture}");
            Log.Success("I've done it mooom!");
            return obj;
        }
        
    }
}

    
    