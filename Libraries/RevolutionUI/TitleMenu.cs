using Revolution.Attributes;
using System;

namespace RevolutionUI
{
    [HookRedirectConstructorFromBase("StardewValley.Game1", "setGameMode")]
    public class TitleMenu : StardewValley.Menus.TitleMenu
    {
        public TitleMenu()
        {
            Console.WriteLine("Using Overloaded Title Menu!");
        }
    }
}
