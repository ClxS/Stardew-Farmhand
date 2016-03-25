using StardewModdingAPI.Inheritance;
using StardewValley;
using System;
using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace StardewModdingAPI.Events
{
    public static class PlayerEvents
    {
        public static event EventHandler<EventArgsFarmerChanged> FarmerChanged = delegate { };
        public static event EventHandler<EventArgsInventoryChanged> InventoryChanged = delegate { };
        public static event EventHandler<EventArgsLevelUp> LeveledUp = delegate { };

        public static void InvokeFarmerChanged(Farmer priorFarmer, Farmer newFarmer)
        {
            FarmerChanged.Invoke(null, new EventArgsFarmerChanged(priorFarmer, newFarmer));
        }

        public static void InvokeInventoryChanged(List<Item> inventory, List<ItemStackChange> changedItems)
        {
            InventoryChanged.Invoke(null, new EventArgsInventoryChanged(inventory, changedItems));
        }

        public static void InvokeLeveledUp(EventArgsLevelUp.LevelType type, int newLevel)
        {
            LeveledUp.Invoke(null, new EventArgsLevelUp(type, newLevel));
        }
    }
}
