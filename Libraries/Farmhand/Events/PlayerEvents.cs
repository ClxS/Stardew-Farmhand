using Farmhand.Attributes;
using Farmhand.Events.Arguments;
using System;
using System.ComponentModel;
using Farmhand.Events.Arguments.PlayerEvents;
using Microsoft.Xna.Framework;
using StardewValley;

namespace Farmhand.Events
{
    /// <summary>
    /// Contains events relating to players
    /// </summary>
    public static class PlayerEvents
    {
        public static event EventHandler<EventArgsOnBeforePlayerTakesDamage> OnBeforePlayerTakesDamage = delegate { };
        public static event EventHandler<EventArgsOnAfterPlayerTakesDamage> OnAfterPlayerTakesDamage = delegate { };
        public static event EventHandler<EventArgsOnPlayerDoneEating> OnPlayerDoneEating = delegate { };
        public static event EventHandler<EventArgsOnItemAddedToInventory> OnItemAddedToInventory = delegate { };
        public static event EventHandler<CancelEventArgs> OnBeforeGainExperience = delegate { };
        public static event EventHandler<EventArgsOnFarmerChanged> OnFarmerChanged = delegate { };
        public static event EventHandler<EventArgsOnLevelUp> OnLevelUp = delegate { };
        public static event EventHandler<CancelEventArgs> OnChangeShirt = delegate { };
        public static event EventHandler<CancelEventArgs> OnChangeHairStyle = delegate { };
        public static event EventHandler<CancelEventArgs> OnChangeShoeColour = delegate { };
        public static event EventHandler<CancelEventArgs> OnChangeHairColour = delegate { };
        public static event EventHandler<CancelEventArgs> OnChangePantsColour = delegate { };
        public static event EventHandler<CancelEventArgs> OnChangeHat = delegate { };
        public static event EventHandler<CancelEventArgs> OnChangeAccessory = delegate { };
        public static event EventHandler<CancelEventArgs> OnChangeSkinColour = delegate { };
        public static event EventHandler<CancelEventArgs> OnChangeEyeColour = delegate { };
        public static event EventHandler<CancelEventArgs> OnChangeGender = delegate { };
        
        [Hook(HookType.Entry, "StardewValley.Game1", "farmerTakeDamage")]
        internal static bool InvokeBeforePlayerTakesDamage()
        {
            return EventCommon.SafeCancellableInvoke(OnBeforePlayerTakesDamage, null, new EventArgsOnBeforePlayerTakesDamage());
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "farmerTakeDamage")]
        internal static void InvokeAfterPlayerTakesDamage()
        {
            EventCommon.SafeInvoke(OnAfterPlayerTakesDamage, null, new EventArgsOnAfterPlayerTakesDamage());
        }
        
        [Hook(HookType.Exit, "StardewValley.Game1", "doneEating")]
        internal static void InvokeOnPlayerDoneEating()
        {
            EventCommon.SafeInvoke(OnPlayerDoneEating, null, new EventArgsOnPlayerDoneEating());
        }
        
        internal static void InvokeFarmerChanged(Farmer priorFarmer, Farmer newFarmer)
        {
            EventCommon.SafeInvoke(OnFarmerChanged, newFarmer, new EventArgsOnFarmerChanged(priorFarmer, newFarmer));
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeShirt")]
        internal static bool InvokeOnPlayerChangeShirt([ThisBind] object @this,
            [InputBind(typeof(int), "whichShirt")] int which)
        {
            return EventCommon.SafeCancellableInvoke(OnChangeShirt, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeHairStyle")]
        internal static bool InvokeOnPlayerChangeHairStyle([ThisBind] object @this,
            [InputBind(typeof(int), "whichHair")] int which)
        {
            return EventCommon.SafeCancellableInvoke(OnChangeHairStyle, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeShoeColor")]
        internal static bool InvokeOnPlayerChangeShoeColour([ThisBind] object @this,
            [InputBind(typeof(int), "which")] int which)
        {
            return EventCommon.SafeCancellableInvoke(OnChangeShoeColour, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeHairColor")]
        internal static bool InvokeOnPlayerChangeHairColour([ThisBind] object @this,
            [InputBind(typeof(Color), "c")] Color which)
        {
            return EventCommon.SafeCancellableInvoke(OnChangeHairColour, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changePants")]
        internal static bool InvokeOnPlayerChangePants([ThisBind] object @this,
            [InputBind(typeof(Color), "color")] Color which)
        {
            return EventCommon.SafeCancellableInvoke(OnChangePantsColour, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeHat")]
        internal static bool InvokeOnPlayerChangeHat([ThisBind] object @this,
            [InputBind(typeof(int), "newHat")] int which)
        {
            return EventCommon.SafeCancellableInvoke(OnChangeHat, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeAccessory")]
        internal static bool InvokeOnPlayerChangeAccessory([ThisBind] object @this,
            [InputBind(typeof(int), "which")] int which)
        {
            return EventCommon.SafeCancellableInvoke(OnChangeAccessory, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeSkinColor")]
        internal static bool InvokeOnPlayerChangeSkinColour([ThisBind] object @this,
            [InputBind(typeof(int), "which")] int which)
        {
            return EventCommon.SafeCancellableInvoke(OnChangeSkinColour, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeEyeColor")]
        internal static bool InvokeOnPlayerChangeEyeColour([ThisBind] object @this,
            [InputBind(typeof(Color), "c")] Color which)
        {
            return EventCommon.SafeCancellableInvoke(OnChangeEyeColour, @this, new CancelEventArgs());
        }

        [Hook(HookType.Entry, "StardewValley.Farmer", "changeGender")]
        internal static bool InvokeOnPlayerChangeGender([ThisBind] object @this,
            [InputBind(typeof(bool), "male")] bool male)
        {
            return EventCommon.SafeCancellableInvoke(OnChangeGender, @this, new CancelEventArgs());
        }

        //[Hook(HookType.Exit, "StardewValley.Farmer", "addItemToInventory")]
        //internal static bool InvokeItemAddedToInventory(
        //    [ThisBind] object @this,
        //    [InputBind(typeof(Item), "item")] Item item)
        //{
        //    return EventCommon.SafeCancellableInvoke(OnItemAddedToInventory, @this, new EventArgsOnItemAddedToInventory(item));
        //}

        [Hook(HookType.Entry, "StardewValley.Farmer", "gainExperience")]
        internal static bool InvokeOnBeforeGainExperience([ThisBind] object @this,
            [InputBind(typeof(int), "which")] int which,
            [InputBind(typeof(int), "howMuch")] int howMuch)
        {
            return EventCommon.SafeCancellableInvoke(OnBeforeGainExperience, @this, new CancelEventArgs());
        }

        [Hook(HookType.Exit, "StardewValley.Farmer", "gainExperience")]
        internal static void InvokeLevelUp([ThisBind] object @this,
            [InputBind(typeof(int), "which")] int which,
            [LocalBind(typeof(int), 1)] int originalLevel,
            [LocalBind(typeof(int), 0)] int newLevel
        )
        {
            if (originalLevel != newLevel)
            {
                EventCommon.SafeInvoke(OnLevelUp, @this, new EventArgsOnLevelUp(which, newLevel, originalLevel));
            }
        }

    }
}
