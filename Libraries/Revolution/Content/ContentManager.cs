using Revolution.Attributes;
using Revolution.Logging;
using StardewValley;
using System;
using Revolution.Registries;
using System.IO;
using System.Reflection;
using Revolution.Registries.Containers;

namespace Revolution.Content
{
    class ContentManager : Microsoft.Xna.Framework.Content.ContentManager
    {
        [Hook(HookType.Entry, "StardewValley.Game1", "LoadContent")]
        internal static void ConstructionHook()
        {
            Log.Verbose("Using Revolution's ContentManager");
            Game1.game1.Content = new ContentManager(Game1.game1.Content.ServiceProvider, Game1.game1.Content.RootDirectory);
        }

        public ContentManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
        public ContentManager(IServiceProvider serviceProvider, string rootDirectory)
            : base(serviceProvider, rootDirectory)
        {
        }

        public override T Load<T>(string assetName)
        {
            var item = XnbRegistry.GetItem(assetName);
            if (item?.OwningMod?.ModState == null || item.OwningMod.ModState != ModState.Loaded) return base.Load<T>(assetName);

            try
            {
                var currentDirectory = Path.GetDirectoryName(item.AbsoluteFilePath);
                var relPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" +
                              RootDirectory + "\\";
                if (currentDirectory != null)
                {
                    var relRootUri = new Uri(relPath, UriKind.Absolute);
                    var fullPath = new Uri(currentDirectory, UriKind.Absolute);
                    var relUri = relRootUri.MakeRelativeUri(fullPath) + "/" + item.File;

                    Log.Verbose($"Using own asset replacement: {assetName} = {relPath}");
                    return base.Load<T>(relUri);
                }
            }
            catch (Exception ex)
            {
                Log.Exception("Error reading own file", ex);
                return base.Load<T>(assetName);
            }
            return base.Load<T>(assetName);
        }
    }
}

    
    