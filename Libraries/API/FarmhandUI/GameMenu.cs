namespace Farmhand.UI
{
    using System.Collections.ObjectModel;

    using Farmhand.Attributes;

    using StardewValley.Menus;

    /// <summary>
    ///     Override for game's menu, providing methods to add custom pages/tabs
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
        /// <summary>
        ///     Initializes a new instance of the <see cref="GameMenu" /> class.
        /// </summary>
        public GameMenu()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameMenu" /> class.
        /// </summary>
        /// <param name="startingTab">
        ///     The starting tab.
        /// </param>
        /// <param name="extra">
        ///     Number of additional tabs.
        /// </param>
        public GameMenu(int startingTab, int extra = -1)
            : base(startingTab, extra)
        {
        }

        /// <summary>
        ///     The pages for this menu.
        /// </summary>
        public ReadOnlyCollection<IClickableMenu> Pages => this.pages.AsReadOnly();

        /// <summary>
        ///     The tabs for this menu.
        /// </summary>
        public ReadOnlyCollection<ClickableComponent> Tabs => this.tabs.AsReadOnly();

        /// <summary>
        ///     Adds a page to this menu.
        /// </summary>
        /// <param name="menu">
        ///     The page to add.
        /// </param>
        public void AddPage(IClickableMenu menu)
        {
            this.pages.Add(menu);
        }

        /// <summary>
        ///     Removes a page from this menu.
        /// </summary>
        /// <param name="menu">
        ///     The page to remove.
        /// </param>
        public void RemovePage(IClickableMenu menu)
        {
            this.pages.Remove(menu);
        }

        /// <summary>
        ///     Adds a tab to this menu.
        /// </summary>
        /// <param name="tab">
        ///     The tab to add.
        /// </param>
        public void AddTab(ClickableComponent tab)
        {
            this.tabs.Add(tab);
        }

        /// <summary>
        ///     Removes a tab from this menu.
        /// </summary>
        /// <param name="tab">
        ///     The tab to remove.
        /// </param>
        public void RemoveTab(ClickableComponent tab)
        {
            this.tabs.Remove(tab);
        }
    }
}