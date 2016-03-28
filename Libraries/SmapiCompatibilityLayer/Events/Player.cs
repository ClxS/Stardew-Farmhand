using System;
using System.Collections.Generic;
using Revolution.Events.Arguments.PlayerEvents;
using StardewModdingAPI.Inheritance;
using StardewValley;

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
            FarmerChanged.Invoke(null, new EventArgsFarmerChanged(null, Game1.player));
        }

        internal static void InvokeInventoryChanged(object sender, EventArgsOnItemAddedToInventory eventArgsOnItemAddedToInventory)
        {
            InventoryChanged.Invoke(null, new EventArgsInventoryChanged(null, null));
        }

        internal static void InvokeLeveledUp(object sender, EventArgsOnLevelUp eventArgsOnLevelUp)
        {
            LeveledUp.Invoke(null, new EventArgsLevelUp(eventArgsOnLevelUp.Which, eventArgsOnLevelUp.NewLevel));
        }

        internal static void InvokeLoadedGame(EventArgsLoadedGameChanged loaded)
        {
            LoadedGame.Invoke(null, loaded);
        }
    }
}