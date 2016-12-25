using System;
using Farmhand.Attributes;
using Farmhand.Events.Arguments.SaveEvents;
using System.Collections.Generic;
using Farmhand.API.Items;
using Farmhand.API.Crops;
using Farmhand.API.Tools;

namespace Farmhand.Events
{
    /// <summary>
    /// Contains events related to saving/loading
    /// </summary>
    public static class SaveEvents
    {
        /// <summary>
        /// Triggered prior to saving
        /// </summary>
        public static event EventHandler OnBeforeSave = delegate { };

        /// <summary>
        /// Triggered prior to loading
        /// </summary>
        public static event EventHandler<EventArgsOnBeforeLoad> OnBeforeLoad = delegate { };

        /// <summary>
        /// Triggered after progress towards saving is made. 100 is complete
        /// </summary>
        public static event EventHandler<EventArgsOnProgress> OnAfterSaveProgress = delegate { };

        /// <summary>
        /// Triggered after progress towards loading is made. 100 is complete
        /// </summary>
        public static event EventHandler<EventArgsOnProgress> OnAfterLoadProgress = delegate { };

        /// <summary>
        /// Triggered after loading is complete
        /// </summary>
        public static event EventHandler<EventArgsOnAfterLoad> OnAfterLoad = delegate { };

        /// <summary>
        /// Triggered after loading is complete
        /// </summary>
        public static event EventHandler<EventArgsOnAfterSave> OnAfterSave = delegate { };

        [Hook(HookType.Entry, "StardewValley.SaveGame", "Save")]
        internal static void InvokeOnBeforeSave()
        {
            EventCommon.SafeInvoke(OnBeforeSave, null);
        }
        
        [Hook(HookType.Entry, "StardewValley.SaveGame", "Load")]
        internal static bool InvokeOnBeforeLoad([InputBind(typeof(string), "filename")] string filename)
        {
            return EventCommon.SafeCancellableInvoke(OnBeforeLoad, null, new EventArgsOnBeforeLoad(filename));
        }

        // Triggered by PropertyWatcher
        internal static void InvokeOnAfterSaveProgress(int current)
        {
            EventCommon.SafeInvoke(OnAfterSaveProgress, null, new EventArgsOnProgress(current));
        }

        // Triggered by PropertyWatcher
        internal static void InvokeOnAfterLoadProgress(int current)
        {
            EventCommon.SafeInvoke(OnAfterLoadProgress, null, new EventArgsOnProgress(current));
        }

        // Triggered by PropertyWatcher
        internal static void InvokeOnAfterSave()
        {
            EventCommon.SafeInvoke(OnAfterSave, null, new EventArgsOnAfterSave());
        }

        // Triggered by PropertyWatcher
        internal static void InvokeOnAfterLoad()
        {
            // Fix IDs after load, in all our ID based registries
            Item.FixupItemIds(null, null);
            BigCraftable.FixupBigCraftableIds(null, null);
            Crop.FixupCropIds(null, null);
            Weapon.FixupWeaponIds(null, null);
            ModLoader.ReloadConfigurations();

            EventCommon.SafeInvoke(OnAfterLoad, null, new EventArgsOnAfterLoad());
        }
    }
}
