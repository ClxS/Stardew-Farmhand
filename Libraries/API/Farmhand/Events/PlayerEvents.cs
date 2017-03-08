namespace Farmhand.Events
{
    using System;
    using System.ComponentModel;

    using Farmhand.Attributes;
    using Farmhand.Events.Arguments;
    using Farmhand.Events.Arguments.PlayerEvents;

    using Microsoft.Xna.Framework;

    using StardewValley;

    /// <summary>
    ///     Contains events relating to players.
    /// </summary>
    public static class PlayerEvents
    {
        /// <summary>
        ///     Fires just before a player takes damage.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, which will prevent damage being applied.
        /// </remarks>
        public static event EventHandler<BeforePlayerTakesDamageEventArgs> BeforePlayerTakesDamage = delegate { };

        /// <summary>
        ///     Fires just after a player takes damage.
        /// </summary>
        public static event EventHandler<AfterPlayerTakesDamageEventArgs> AfterPlayerTakesDamage = delegate { };

        /// <summary>
        ///     Fires after a player has finished eating.
        /// </summary>
        public static event EventHandler<PlayerDoneEatingEventArgs> PlayerDoneEating = delegate { };

        /// <summary>
        ///     Fires after an item has been added to the inventory.
        /// </summary>
        /// <remarks>
        ///     This event is returnable, allowing you to override the item added to the inventory.
        /// </remarks>
        public static event EventHandler<ItemAddedToInventoryEventArgs> ItemAddedToInventory = delegate { };

        /// <summary>
        ///     Fires just prior to gaining experience.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, allowing you to prevent experience gain.
        /// </remarks>
        public static event EventHandler<CancelEventArgs> BeforeGainExperience = delegate { };

        /// <summary>
        ///     Fires when the Farmer changes.
        /// </summary>
        /// <remarks>
        ///     This is invoked by the property watcher, so may occur a full frame after the
        ///     change actually occurred.
        /// </remarks>
        public static event EventHandler<FarmerChangedEventArgs> FarmerChanged = delegate { };

        /// <summary>
        ///     Fires just after leveling up a skill.
        /// </summary>
        public static event EventHandler<LevelUpEventArgs> LevelUp = delegate { };

        /// <summary>
        ///     Fires when the player changes shirt.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, allowing you to clothing changes.
        /// </remarks>
        public static event EventHandler<CancelEventArgs> ChangeShirt = delegate { };

        /// <summary>
        ///     Fires when the player changes hair style.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, allowing you to clothing changes.
        /// </remarks>
        public static event EventHandler<CancelEventArgs> ChangeHairStyle = delegate { };

        /// <summary>
        ///     Fires when the player changes shoe colour.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, allowing you to clothing changes.
        /// </remarks>
        public static event EventHandler<CancelEventArgs> ChangeShoeColour = delegate { };

        /// <summary>
        ///     Fires when the player changes hair colour.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, allowing you to clothing changes.
        /// </remarks>
        public static event EventHandler<CancelEventArgs> ChangeHairColour = delegate { };

        /// <summary>
        ///     Fires when the player changes pants colour.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, allowing you to clothing changes.
        /// </remarks>
        public static event EventHandler<CancelEventArgs> ChangePantsColour = delegate { };

        /// <summary>
        ///     Fires when the player changes hat.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, allowing you to clothing changes.
        /// </remarks>
        public static event EventHandler<CancelEventArgs> ChangeHat = delegate { };

        /// <summary>
        ///     Fires when the player changes accessory.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, allowing you to clothing changes.
        /// </remarks>
        public static event EventHandler<CancelEventArgs> ChangeAccessory = delegate { };

        /// <summary>
        ///     Fires when the player changes skin colour.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, allowing you to clothing changes.
        /// </remarks>
        public static event EventHandler<CancelEventArgs> ChangeSkinColour = delegate { };

        /// <summary>
        ///     Fires when the player changes eye colour.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, allowing you to clothing changes.
        /// </remarks>
        public static event EventHandler<CancelEventArgs> ChangeEyeColour = delegate { };

        /// <summary>
        ///     Fires when the player changes gender.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, allowing you to clothing changes.
        /// </remarks>
        public static event EventHandler<CancelEventArgs> ChangeGender = delegate { };

        [Hook(HookType.Entry, "StardewValley.Game1", "farmerTakeDamage")]
        internal static bool OnBeforePlayerTakesDamage()
        {
            return EventCommon.SafeCancellableInvoke(
                BeforePlayerTakesDamage,
                null,
                new BeforePlayerTakesDamageEventArgs());
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "farmerTakeDamage")]
        internal static void OnAfterPlayerTakesDamage()
        {
            EventCommon.SafeInvoke(AfterPlayerTakesDamage, null, new AfterPlayerTakesDamageEventArgs());
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "doneEating")]
        internal static void OnPlayerDoneEating()
        {
            EventCommon.SafeInvoke(PlayerDoneEating, null, new PlayerDoneEatingEventArgs());
        }

        internal static void OnFarmerChanged(Farmer priorFarmer, Farmer newFarmer)
        {
            EventCommon.SafeInvoke(FarmerChanged, newFarmer, new FarmerChangedEventArgs(priorFarmer, newFarmer));
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeShirt")]
        internal static bool OnPlayerChangeShirt(
            [ThisBind] object @this,
            [InputBind(typeof(int), "whichShirt")] int which)
        {
            return EventCommon.SafeCancellableInvoke(ChangeShirt, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeHairStyle")]
        internal static bool OnPlayerChangeHairStyle(
            [ThisBind] object @this,
            [InputBind(typeof(int), "whichHair")] int which)
        {
            return EventCommon.SafeCancellableInvoke(ChangeHairStyle, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeShoeColor")]
        internal static bool OnPlayerChangeShoeColour(
            [ThisBind] object @this,
            [InputBind(typeof(int), "which")] int which)
        {
            return EventCommon.SafeCancellableInvoke(ChangeShoeColour, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeHairColor")]
        internal static bool OnPlayerChangeHairColour(
            [ThisBind] object @this,
            [InputBind(typeof(Color), "c")] Color which)
        {
            return EventCommon.SafeCancellableInvoke(ChangeHairColour, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changePants")]
        internal static bool OnPlayerChangePants(
            [ThisBind] object @this,
            [InputBind(typeof(Color), "color")] Color which)
        {
            return EventCommon.SafeCancellableInvoke(ChangePantsColour, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeHat")]
        internal static bool OnPlayerChangeHat([ThisBind] object @this, [InputBind(typeof(int), "newHat")] int which)
        {
            return EventCommon.SafeCancellableInvoke(ChangeHat, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeAccessory")]
        internal static bool OnPlayerChangeAccessory(
            [ThisBind] object @this,
            [InputBind(typeof(int), "which")] int which)
        {
            return EventCommon.SafeCancellableInvoke(ChangeAccessory, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeSkinColor")]
        internal static bool OnPlayerChangeSkinColour(
            [ThisBind] object @this,
            [InputBind(typeof(int), "which")] int which)
        {
            return EventCommon.SafeCancellableInvoke(ChangeSkinColour, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeEyeColor")]
        internal static bool OnPlayerChangeEyeColour(
            [ThisBind] object @this,
            [InputBind(typeof(Color), "c")] Color which)
        {
            return EventCommon.SafeCancellableInvoke(ChangeEyeColour, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeGender")]
        internal static bool OnPlayerChangeGender([ThisBind] object @this, [InputBind(typeof(bool), "male")] bool male)
        {
            return EventCommon.SafeCancellableInvoke(ChangeGender, @this, new CancelEventArgs());
        }

        [HookReturnable(HookType.Exit, "StardewValley.Farmer", "addItemToInventory")]
        internal static Item OnItemAddedToInventory(
            [UseOutputBind] out bool useOutput,
            [ThisBind] object @this,
            [InputBind(typeof(Item), "item")] Item item)
        {
            var eventArgs = new ItemAddedToInventoryEventArgs(item);
            EventCommon.SafeInvoke(ItemAddedToInventory, @this, eventArgs);
            useOutput = eventArgs.IsHandled;
            return eventArgs.Item;
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "gainExperience")]
        internal static bool OnBeforeGainExperience(
            [ThisBind] object @this,
            [InputBind(typeof(int), "which")] int which,
            [InputBind(typeof(int), "howMuch")] int howMuch)
        {
            return EventCommon.SafeCancellableInvoke(BeforeGainExperience, @this, new CancelEventArgs());
        }

        [Hook(HookType.Exit, "StardewValley.Farmer", "gainExperience")]
        internal static void OnLevelUp(
            [ThisBind] object @this,
            [InputBind(typeof(int), "which")] int which,
            [LocalBind(typeof(int), 1)] int originalLevel,
            [LocalBind(typeof(int), 0)] int newLevel)
        {
            if (originalLevel != newLevel)
            {
                EventCommon.SafeInvoke(LevelUp, @this, new LevelUpEventArgs(which, newLevel, originalLevel));
            }
        }
    }
}