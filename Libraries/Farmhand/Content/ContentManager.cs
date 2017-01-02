namespace Farmhand.Content
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Farmhand.Attributes;
    using Farmhand.Content.Injectors;
    using Farmhand.Content.Injectors.Blueprints;
    using Farmhand.Content.Injectors.Items;
    using Farmhand.Content.Injectors.Mail;
    using Farmhand.Content.Injectors.Maps;
    using Farmhand.Content.Injectors.NPCs;
    using Farmhand.Content.Injectors.NPCs.Monsters;
    using Farmhand.Content.Injectors.Other;
    using Farmhand.Logging;

    using StardewValley;

    /// <summary>
    ///     An override for the XNA ContentManager which deals with loading custom XNBs when mods have registered custom
    ///     overrides. Can also be used by mods
    ///     to load their own XNB data
    /// </summary>
    [HookRedirectConstructorFromBase("StardewValley.Game1", ".ctor", new[] { typeof(IServiceProvider), typeof(string) })
    ]
    [HookRedirectConstructorFromBase("StardewValley.Game1", "dummyLoad",
        new[] { typeof(IServiceProvider), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.Game1", "LoadContent",
        new[] { typeof(IServiceProvider), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.LocalizedContentManager", "CreateTemporary",
        new[] { typeof(IServiceProvider), typeof(string), typeof(CultureInfo), typeof(string) })]
    public class ContentManager : LocalizedContentManager
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ContentManager" /> class.
        /// </summary>
        /// <param name="serviceProvider">
        ///     The service provider.
        /// </param>
        /// <param name="rootDirectory">
        ///     The root directory.
        /// </param>
        /// <param name="currentCulture">
        ///     The current culture.
        /// </param>
        /// <param name="languageCodeOverride">
        ///     The language code override.
        /// </param>
        public ContentManager(
            IServiceProvider serviceProvider,
            string rootDirectory,
            CultureInfo currentCulture,
            string languageCodeOverride)
            : base(serviceProvider, rootDirectory, currentCulture, languageCodeOverride)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ContentManager" /> class.
        /// </summary>
        /// <param name="serviceProvider">
        ///     The service provider.
        /// </param>
        /// <param name="rootDirectory">
        ///     The root directory.
        /// </param>
        public ContentManager(IServiceProvider serviceProvider, string rootDirectory)
            : base(serviceProvider, rootDirectory)
        {
        }

        /// <summary>
        ///     Gets a <see cref="List{T}" /> which contains the injectors used for loading assets.
        /// </summary>
        public static List<IContentInjector> ContentInjectors { get; } = new List<IContentInjector>
                                                                             {
                                                                                 new ModXnbInjector(),
                                                                                 new BlueprintInjector(),
                                                                                 new MonsterLoader(),
                                                                                 new MonsterInjector(),
                                                                                 new CropInjector(),
                                                                                 new WeaponInjector(),
                                                                                 new BigCraftableInjector(),
                                                                                 new DelegatedContentInjector(),
                                                                                 new MapInjector(),
                                                                                 new MailInjector(),
                                                                                 new QuestInjector(),
                                                                                 /* Begin NPC Injectors */
                                                                                 new DialogueLoader(),
                                                                                 new GiftTastesInjector(),
                                                                                 new NpcDispositionsInjector(),
                                                                                 new NpcLoader(),
                                                                                 new RainyDialogueInjector(),
                                                                                 new ScheduleLoader()
                                                                                 /* End NPC Injectors */
                                                                             };

        /// <summary>
        ///     Gets a list of expected problematic assets. The game has quite a lot of missing XNBs that throw exceptions.
        ///     This is used to filter those exceptions from the error log.
        /// </summary>
        public static List<string> ExpectedFailures { get; } = new List<string>
                                                                   {
                                                                       @"Characters\Dialogue\Bouncer",
                                                                       @"Characters\Dialogue\Gunther",
                                                                       @"Characters\Dialogue\Marlon",
                                                                       @"Characters\Dialogue\Henchman",
                                                                       @"Characters\schedules\Wizard",
                                                                       @"Characters\schedules\Dwarf",
                                                                       @"Characters\schedules\Mister Qi",
                                                                       @"Characters\schedules\Sandy",
                                                                       @"Characters\schedules\Bouncer",
                                                                       @"Characters\schedules\Gunther",
                                                                       @"Characters\schedules\Marlon",
                                                                       @"Characters\schedules\Henchman",
                                                                       @"Data\Festivals\spring1"
                                                                   };

        /// <summary>
        ///     Loads an asset directly using the base <see cref="LocalizedContentManager" />, skipping any injection.
        /// </summary>
        /// <param name="assetName">
        ///     The asset name.
        /// </param>
        /// <typeparam name="T">
        ///     The type of asset to load
        /// </typeparam>
        /// <returns>
        ///     The asset as an instance of type T.
        /// </returns>
        public T LoadDirect<T>(string assetName)
        {
            return base.Load<T>(assetName);
        }

        /// <summary>
        ///     Creates a new <see cref="LocalizedContentManager" /> with the specified root directory.
        /// </summary>
        /// <param name="rootDirectory">
        ///     The root directory.
        /// </param>
        /// <returns>
        ///     The new <see cref="LocalizedContentManager" />.
        /// </returns>
        public LocalizedContentManager CreateContentManager(string rootDirectory)
        {
            return new LocalizedContentManager(
                this.ServiceProvider,
                rootDirectory,
                this.CurrentCulture,
                this.LanguageCodeOverride);
        }

        /// <summary>
        ///     Load an asset by via a relative (extensionless) path
        /// </summary>
        /// <typeparam name="T">Type of content to return</typeparam>
        /// <param name="assetName">Content to load</param>
        /// <returns>Loaded content</returns>
        public override T Load<T>(string assetName)
        {
            var output = default(T);

            try
            {
                var loaders = ContentInjectors.Where(n => n.IsLoader && n.HandlesAsset(typeof(T), assetName)).ToArray();
                var injectors =
                    ContentInjectors.Where(n => !n.IsLoader && n.HandlesAsset(typeof(T), assetName)).ToArray();

                if (loaders.Length > 1)
                {
                    Log.Warning(
                        $"Multiple loading injectors found for {assetName} - {typeof(T).FullName} - Using first");
                }

                if (loaders.Length > 0)
                {
                    foreach (var loader in loaders)
                    {
                        output = loader.Load<T>(this, assetName);
                        if (output != null)
                        {
                            break;
                        }
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
                    object refOutput = null;
                    injector.Inject(output, assetName, ref refOutput);

                    if (refOutput != null)
                    {
                        output = (T)refOutput;
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ExpectedFailures.Contains(assetName))
                {
                    Log.Exception("Failed to load asset: " + assetName, ex);
                }

                throw;
            }

            return output;
        }
    }
}