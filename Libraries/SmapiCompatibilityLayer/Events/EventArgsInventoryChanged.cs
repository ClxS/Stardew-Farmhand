using System;
using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI.Inheritance;
using StardewValley;

namespace StardewModdingAPI.Events
{
    public class EventArgsInventoryChanged : EventArgs
    {
        public EventArgsInventoryChanged(List<Item> inventory, List<ItemStackChange> changedItems)
        {
            Inventory = inventory;
            Added = changedItems.Where(n => n.ChangeType == ChangeType.Added).ToList();
            Removed = changedItems.Where(n => n.ChangeType == ChangeType.Removed).ToList();
            QuantityChanged = changedItems.Where(n => n.ChangeType == ChangeType.StackChange).ToList();
        }
        public List<Item> Inventory { get; private set; }
        public List<ItemStackChange> Added { get; private set; }
        public List<ItemStackChange> Removed { get; private set; }
        public List<ItemStackChange> QuantityChanged { get; private set; }
    }
}