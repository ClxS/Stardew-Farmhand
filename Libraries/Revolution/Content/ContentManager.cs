using Revolution.Attributes;
using Revolution.Logging;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Log.Verbose("Rewriting ContentManager with our own");
            Game1.game1.Content = new ContentManager(Game1.game1.Content.ServiceProvider, Game1.game1.Content.RootDirectory);
        }

        public ContentManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            Log.Success("test1");
        }
        public ContentManager(IServiceProvider serviceProvider, string rootDirectory)
            : base(serviceProvider, rootDirectory)
        {
            Log.Success("test2");
        }

        public override T Load<T>(string assetName)
        {
            var item = XnbRegistry.GetItem(assetName);
            if(item != null && item.OwningMod != null && item.OwningMod.ModState == ModState.Loaded)
            {
                try
                {
                    Uri relRoot = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + this.RootDirectory + "\\", UriKind.Absolute);
                    Uri fullPath = new Uri(Path.GetDirectoryName(item.AbsoluteFilePath), UriKind.Absolute);                    
                    string relPath = relRoot.MakeRelativeUri(fullPath).ToString() + "/" + item.File;
                    
                    Log.Verbose($"Using own asset replacement: {assetName} = {relPath}");
                    return base.Load<T>(relPath);
                }
                catch (System.Exception ex)
                {
                    Log.Exception("Error reading own file", ex);
                    return base.Load<T>(assetName);
                }
            }
            else
            {
                return base.Load<T>(assetName);
            }
        }
    }
}

    
    