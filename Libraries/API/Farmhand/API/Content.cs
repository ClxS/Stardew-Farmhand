namespace Farmhand.API
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Farmhand.Content;
    using Farmhand.Registries.Containers;

    using StardewValley;

    /// <summary>
    ///     Content-related API functionality.
    /// </summary>
    public static class Content
    {
        private static readonly List<LocalizedContentManager> ModManagers;

        static Content()
        {
            ModManagers = new List<LocalizedContentManager>();
        }

        /// <summary>
        ///     Gets the main game's <see cref="ContentManager" />.
        /// </summary>
        public static ContentManager ContentManager => Game1.content as ContentManager;

        /// <summary>
        ///     Gets the mod manager for the provided mods content directory.
        /// </summary>
        /// <param name="mod">
        ///     The mod whose content manager to retrieve.
        /// </param>
        /// <returns>
        ///     The <see cref="LocalizedContentManager" /> for this mod.
        /// </returns>
        public static LocalizedContentManager GetContentManagerForMod(Mod mod)
        {
            return GetContentManagerForMod(mod.ModSettings);
        }

        /// <summary>
        ///     Gets the mod manager for the provided mods content directory.
        /// </summary>
        /// <param name="mod">
        ///     The manifest of the mod whose content manager to retrieve.
        /// </param>
        /// <returns>
        ///     The <see cref="LocalizedContentManager" /> for this mod.
        /// </returns>
        public static LocalizedContentManager GetContentManagerForMod(ModManifest mod)
        {
            if (mod == null)
            {
                return ContentManager;
            }

            var path = Path.Combine(mod.ModDirectory, "Content");
            var manager = ModManagers.FirstOrDefault(m => m.RootDirectory.Equals(path));

            if (manager == null)
            {
                manager = ContentManager.CreateContentManager(path);
                ModManagers.Add(manager);
            }

            return manager;
        }
    }
}