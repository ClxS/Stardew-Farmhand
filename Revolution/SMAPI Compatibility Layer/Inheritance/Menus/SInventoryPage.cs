using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StardewModdingAPI.Inheritance.Menus
{
    public class SInventoryPage : InventoryPage
    {
        public InventoryPage BaseInventoryPage { get; private set; }

        public static SInventoryPage ConstructFromBaseClass(InventoryPage baseClass)
        {
            SInventoryPage s = new SInventoryPage(0, 0, 0, 0);
            s.BaseInventoryPage = baseClass;
            return s;
        }

        public SInventoryPage(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }
    }
}
