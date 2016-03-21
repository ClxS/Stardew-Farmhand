using Revolution.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution
{
    public static class Test
    {
        [Hook(HookType.Entry, "StardewValley.Menus.TitleMenu", "setUpIcons")]
        public static void Test3()
        {
            Console.WriteLine("Using default setup");
        }
    }
}
