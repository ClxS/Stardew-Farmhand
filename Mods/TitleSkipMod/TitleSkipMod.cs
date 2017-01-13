namespace TitleSkipMod
{
    using Farmhand;
    using Farmhand.Events;

    internal class TitleSkipMod : Mod
    {
        public override void Entry()
        {
            MenuEvents.MenuChanged += this.MenuEvents_OnMenuChanged;
        }

        private void MenuEvents_OnMenuChanged(object sender, Farmhand.Events.Arguments.MenuEvents.EventArgsOnMenuChanged e)
        {
            var menu = e.NewMenu as Farmhand.UI.TitleMenu;
            if (menu == null)
            {
                return;
            }

            menu.SkipToTitleButtons();
        }
    }
}