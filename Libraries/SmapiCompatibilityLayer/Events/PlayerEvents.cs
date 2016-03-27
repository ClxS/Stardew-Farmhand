using StardewModdingAPI.Inheritance;
using StardewValley;
using System;
using System.Collections.Generic;
using Revolution.Events.Arguments.PlayerEvents;

// ReSharper disable CheckNamespace
namespace StardewModdingAPI.Events
{
    public static class PlayerEvents
    {
        public static event EventHandler<EventArgsFarmerChanged> FarmerChanged = delegate { };
        public static event EventHandler<EventArgsInventoryChanged> InventoryChanged = delegate { };
        public static event EventHandler<EventArgsLevelUp> LeveledUp = delegate { };

        public static void InvokeFarmerChanged(object sender, EventArgs eventArgs)
        {
            //FarmerChanged.Invoke(null, new EventArgsFarmerChanged(priorFarmer, newFarmer));
        }

        public static void InvokeInventoryChanged(object sender, EventArgsOnItemAddedToInventory eventArgsOnItemAddedToInventory)
        {
            //InventoryChanged.Invoke(null, new EventArgsInventoryChanged(inventory, changedItems));
        }

        public static void InvokeLeveledUp(object sender, EventArgsOnLevelUp eventArgsOnLevelUp)
        {
           // LeveledUp.Invoke(null, new EventArgsLevelUp(type, newLevel));
        }
        
    }
}
