using StardewValley.Menus;

namespace StardewModdingAPI.Inheritance.Menus
{
    public class SInventoryPage : InventoryPage
    {
        public SInventoryPage(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }

        public InventoryPage BaseInventoryPage { get; private set; }

        public static SInventoryPage ConstructFromBaseClass(InventoryPage baseClass)
        {
            var s = new SInventoryPage(0, 0, 0, 0);
            s.BaseInventoryPage = baseClass;
            return s;
        }
    }
}