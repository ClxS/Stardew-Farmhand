using System.Collections.ObjectModel;
using Farmhand.Attributes;
using StardewValley.Menus;

namespace Farmhand.UI
{
    /// <summary>
    /// Override for Stardew's Game Menu, providing methods to add custom pages/tabs
    /// </summary>
    [HookRedirectConstructorFromBase("StardewValley.Game1", "Window_ClientSizeChanged")]
    [HookRedirectConstructorFromBase("StardewValley.Game1", "updateActiveMenu")]
    [HookRedirectConstructorFromBase("StardewValley.Game1", "checkForEscapeKeys")]
    [HookRedirectConstructorFromBase("StardewValley.Game1", "UpdateControlInput")]
    [HookRedirectConstructorFromBase("StardewValley.Game1", "UpdateControlInput")]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "openCraftingMenu")]
    [HookRedirectConstructorFromBase("StardewValley.Menus.GameMenu", "receiveGamePadButton")]
    [HookRedirectConstructorFromBase("StardewValley.Menus.MenuHUDButtons", "receiveLeftClick")]
    [HookRedirectConstructorFromBase("StardewValley.Menus.OptionsInputListener", "receiveLeftClick")]
    [HookRedirectConstructorFromBase("StardewValley.Options", "setWindowedOption")]
    [HookRedirectConstructorFromBase("StardewValley.Options", "changeDropDownOption")]
    public class GameMenu : StardewValley.Menus.GameMenu
    {
        public ReadOnlyCollection<IClickableMenu> Pages => pages.AsReadOnly();
        public ReadOnlyCollection<ClickableComponent> Tabs => tabs.AsReadOnly();

        public GameMenu()
        {
        }

        public GameMenu(int startingTab, int extra = -1)
        : base(startingTab, extra)
        { }

        public void AddPage(IClickableMenu menu)
        {
            this.pages.Add(menu);
        }

        public void RemovePage(IClickableMenu menu)
        {
            this.pages.Remove(menu);
        }

        public void AddTab(ClickableComponent tab)
        {
            this.tabs.Add(tab);
        }

        public void RemoveTab(ClickableComponent tab)
        {
            this.tabs.Remove(tab);
        }
    }
}
