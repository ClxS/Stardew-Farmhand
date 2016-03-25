using StardewValley.Menus;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
namespace StardewModdingAPI.Inheritance.Menus
{
    public class SInventoryPage : InventoryPage
    {
        public InventoryPage BaseInventoryPage { get; private set; }

        public static SInventoryPage ConstructFromBaseClass(InventoryPage baseClass)
        {
            return new SInventoryPage(0, 0, 0, 0) {BaseInventoryPage = baseClass};
        }

        public SInventoryPage(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }
    }
}
