using Farmhand.Attributes;
using Farmhand.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Farmhand.Content
{
    /// <summary>
    /// An override for the XNA ContentManager which deals with loading custom XNBs when mods have registered custom overrides. Can also be used by mods
    /// to load their own XNB data
    /// </summary>
    [HookRedirectConstructorFromBase("StardewValley.Game1", ".ctor", new[]{ typeof(IServiceProvider), typeof(System.String) })]
    [HookRedirectConstructorFromBase("StardewValley.Game1", "dummyLoad", new[] { typeof(IServiceProvider), typeof(System.String) })]
    [HookRedirectConstructorFromBase("StardewValley.Game1", "LoadContent", new[] { typeof(IServiceProvider), typeof(System.String) })]
    [HookRedirectConstructorFromBase("StardewValley.LocalizedContentManager", "CreateTemporary", new[] { typeof(IServiceProvider), typeof(System.String), typeof(CultureInfo), typeof(string) } )]
    public class ContentManager : StardewValley.LocalizedContentManager
    {
        public static List<IContentInjector> ContentInjectors = new List<IContentInjector>
        {
            new ModXnbInjector(),
            new BlueprintInjector(),
            new MonsterLoader(),
            new MonsterInjector(),
            new CropInjector(),
            new WeaponInjector(),
            new BigCraftableInjector()
        };

        public ContentManager(IServiceProvider serviceProvider, string rootDirectory, System.Globalization.CultureInfo currentCulture, string languageCodeOverride)
            : base(serviceProvider, rootDirectory, currentCulture, languageCodeOverride)
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
        public StardewValley.LocalizedContentManager CreateContentManager(string rootDirectory)
        {
            return new StardewValley.LocalizedContentManager(ServiceProvider, rootDirectory, CurrentCulture, LanguageCodeOverride);
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

    
    