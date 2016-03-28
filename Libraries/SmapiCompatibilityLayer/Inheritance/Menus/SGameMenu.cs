using System.Collections.Generic;
using System.Reflection;
using StardewValley.Menus;

namespace StardewModdingAPI.Inheritance.Menus
{
    public class SGameMenu : GameMenu
    {
        public GameMenu BaseGameMenu { get; private set; }

        public List<ClickableComponent> tabs
        {
            get { return (List<ClickableComponent>) GetBaseFieldInfo("tabs").GetValue(BaseGameMenu); }
            set { GetBaseFieldInfo("tabs").SetValue(BaseGameMenu, value); }
        }

        public List<IClickableMenu> pages
        {
            get { return (List<IClickableMenu>) GetBaseFieldInfo("pages").GetValue(BaseGameMenu); }
            set { GetBaseFieldInfo("pages").SetValue(BaseGameMenu, value); }
        }

        public static SGameMenu ConstructFromBaseClass(GameMenu baseClass)
        {
            var s = new SGameMenu();
            s.BaseGameMenu = baseClass;
            return s;
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            if (pages[currentTab] is InventoryPage)
            {
                Log.AsyncY("INV SCREEN");
            }
            base.receiveRightClick(x, y, playSound);
        }

        public static FieldInfo[] GetPrivateFields()
        {
            return typeof (GameMenu).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static FieldInfo GetBaseFieldInfo(string name)
        {
            return typeof (GameMenu).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }
}