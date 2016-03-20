using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StardewModdingAPI.Inheritance
{
    public enum ChangeType
    {
        Removed,
        Added,
        StackChange
    }

    public class ItemStackChange
    {
        public Item Item { get; set; }
        public int StackChange { get; set; }
        public ChangeType ChangeType { get; set; }
    }
}
