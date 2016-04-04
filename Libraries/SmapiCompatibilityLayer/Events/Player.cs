using System;
using System.Collections.Generic;
using Farmhand.Events.Arguments.PlayerEvents;
using StardewModdingAPI.Inheritance;
using StardewValley;
using Farmhand.Events;

namespace StardewModdingAPI.Events
{
    public static class PlayerEvents
    {
        public static event EventHandler<EventArgsFarmerChanged> FarmerChanged = delegate { };
        public static event EventHandler<EventArgsInventoryChanged> InventoryChanged = delegate { };
        public static event EventHandler<EventArgsLevelUp> LeveledUp = delegate { };
        public static event EventHandler<EventArgsLoadedGameChanged> LoadedGame = delegate { };

        internal static void InvokeFarmerChanged(object sender, EventArgs eventArgs)
        {
            EventCommon.SafeInvoke(FarmerChanged, sender, new EventArgsFarmerChanged(null, Game1.player));
        }

        internal static void InvokeInventoryChanged(object sender, EventArgsOnItemAddedToInventory eventArgsOnItemAddedToInventory)
        {
            EventCommon.SafeInvoke(InventoryChanged, sender, new EventArgsInventoryChanged(null, null));
        }

        internal static void InvokeLeveledUp(object sender, EventArgsOnLevelUp eventArgsOnLevelUp)
        {
            EventCommon.SafeInvoke(LeveledUp, sender, new EventArgsLevelUp(eventArgsOnLevelUp.Which, eventArgsOnLevelUp.NewLevel));
        }

        internal static void InvokeLoadedGame(EventArgsLoadedGameChanged loaded)
        {
            EventCommon.SafeInvoke(LoadedGame, null, loaded);
        }
    }
}