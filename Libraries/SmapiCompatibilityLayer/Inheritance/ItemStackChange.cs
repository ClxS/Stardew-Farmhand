using StardewValley;

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