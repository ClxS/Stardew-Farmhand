namespace Farmhand.Events
{
    using System;

    using Farmhand.API.Crops;
    using Farmhand.API.Items;
    using Farmhand.API.Tools;
    using Farmhand.Attributes;
    using Farmhand.Events.Arguments.SaveEvents;

    /// <summary>
    ///     Contains events related to saving/loading.
    /// </summary>
    public static class SaveEvents
    {
        /// <summary>
        ///     Triggered just prior to saving
        /// </summary>
        public static event EventHandler<BeforeSaveEventArgs> BeforeSave = delegate { };

        /// <summary>
        ///     Triggered prior to loading
        /// </summary>
        public static event EventHandler<BeforeLoadEventArgs> BeforeLoad = delegate { };

        /// <summary>
        ///     Triggered after progress towards saving is made. 100 is complete
        /// </summary>
        public static event EventHandler<ProgressEventArgs> AfterSaveProgress = delegate { };

        /// <summary>
        ///     Triggered after progress towards loading is made. 100 is complete
        /// </summary>
        public static event EventHandler<ProgressEventArgs> AfterLoadProgress = delegate { };

        /// <summary>
        ///     Triggered after loading is complete
        /// </summary>
        /// <remarks>
        ///     Just prior to firing this method, the API will fix up mismatched IDs and reload
        ///     mod configurations.
        /// </remarks>
        public static event EventHandler<AfterLoadEventArgs> AfterLoad = delegate { };

        /// <summary>
        ///     Triggered after loading is complete
        /// </summary>
        public static event EventHandler<AfterSaveEventArgs> AfterSave = delegate { };

        [Hook(HookType.Entry, "StardewValley.SaveGame", "Save")]
        internal static void OnBeforeSave()
        {
            EventCommon.SafeInvoke(BeforeSave, null, new BeforeSaveEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.SaveGame", "Load")]
        internal static bool OnBeforeLoad([InputBind(typeof(string), "filename")] string filename)
        {
            return EventCommon.SafeCancellableInvoke(BeforeLoad, null, new BeforeLoadEventArgs(filename));
        }

        // Triggered by PropertyWatcher
        internal static void OnAfterSaveProgress(int current)
        {
            EventCommon.SafeInvoke(AfterSaveProgress, null, new ProgressEventArgs(current));
        }

        // Triggered by PropertyWatcher
        internal static void OnAfterLoadProgress(int current)
        {
            EventCommon.SafeInvoke(AfterLoadProgress, null, new ProgressEventArgs(current));
        }

        // Triggered by PropertyWatcher
        internal static void OnAfterSave()
        {
            EventCommon.SafeInvoke(AfterSave, null, new AfterSaveEventArgs());
        }

        // Triggered by PropertyWatcher
        internal static void OnAfterLoad()
        {
            // Fix IDs after load, in all our ID based registries
            Item.FixupItemIds(null, null);
            BigCraftable.FixupBigCraftableIds(null, null);
            Crop.FixupCropIds(null, null);
            Weapon.FixupWeaponIds(null, null);
            ModLoader.ReloadConfigurations();

            EventCommon.SafeInvoke(AfterLoad, null, new AfterLoadEventArgs());
        }
    }
}