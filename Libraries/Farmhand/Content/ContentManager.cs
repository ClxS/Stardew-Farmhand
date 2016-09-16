using Farmhand.Attributes;
using Farmhand.Logging;
using System;
using System.Collections.Generic;
using Farmhand.Registries;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using Farmhand.Registries.Containers;

namespace Farmhand.Content
{
    /// <summary>
    /// An override for the XNA ContentManager which deals with loading custom XNBs when mods have registered custom overrides. Can also be used by mods
    /// to load their own XNB data
    /// </summary>
    public class ContentManager : Microsoft.Xna.Framework.Content.ContentManager
    {
        // A tracker, so we can check if the current Game1.temporaryContent is our override, to prevent uneccesary overriding
        protected static Microsoft.Xna.Framework.Content.ContentManager _tempManagerTracker = null;

        public static List<IContentInjector> ContentInjectors = new List<IContentInjector>()
        {
            new ModXnbInjector(),
            new BlueprintInjector(),
            new MonsterLoader(),
            new MonsterInjector(),
            new CropInjector(),
            new WeaponInjector()
        };

        //TODO Do not redirect this way. There are so many (pointless) separate instances of ContentManager, and we'll need to override them all as soon as they're 
        //created. It's something more suited to the ConstructionRedirect hook.
        [Hook(HookType.Entry, "StardewValley.Game1", "LoadContent")]
        internal static void ConstructionHook()
        {
            Log.Verbose("Using Farmhand's ContentManager");
            Game1.game1.Content = new ContentManager(Game1.game1.Content.ServiceProvider, Game1.game1.Content.RootDirectory);
        }

        [Hook(HookType.Entry, "StardewValley.Object", "performObjectDropInAction")]
        internal static void ConstructionHookObject()
        {
            if (Game1.temporaryContent != _tempManagerTracker)
            {
                Log.Verbose("Using Farmhand's ContentManager on Temporary Content Manager");
                Game1.temporaryContent = new ContentManager(Game1.game1.Content.ServiceProvider, Game1.game1.Content.RootDirectory);
                _tempManagerTracker = Game1.temporaryContent;
            }
        }

        public ContentManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
        public ContentManager(IServiceProvider serviceProvider, string rootDirectory)
            : base(serviceProvider, rootDirectory)
        {
        }
        
        public T LoadDirect<T>(string assetName)
        {
            return base.Load<T>(assetName);
        }

        /// <summary>
        /// Load an asset by via a relative (extensionless) path
        /// </summary>
        /// <typeparam name="T">Type of content to return</typeparam>
        /// <param name="assetName">Content to load</param>
        /// <returns>Loaded content</returns>
        public override T Load<T>(string assetName)
        {
            T output = default(T);

            var loaders =
                ContentInjectors.Where(n => n.IsLoader && n.HandlesAsset(typeof (T), assetName)).ToArray();
            var injectors =
                ContentInjectors.Where(n => !n.IsLoader && n.HandlesAsset(typeof (T), assetName)).ToArray();

            if (loaders.Length > 1)
            {
                Log.Warning($"Multiple loading injectors found for {assetName} - {typeof (T).FullName} - Using first");
            }

            if (loaders.Length > 0)
            {
                foreach (var loader in loaders)
                {
                    output = loader.Load<T>(this, assetName);
                    if (output != null)
                        break;
                }
                
                if (output == null)
                {
                    output = base.Load<T>(assetName);
                }
            }
            else
            {
                output = base.Load<T>(assetName);
            }

            foreach (var injector in injectors)
            {
                injector.Inject(output, assetName);
            }
            
            return output;
        }
    }
}

    
    