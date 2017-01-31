namespace Farmhand.UI.Internal_Menus
{
    using System;

    using Farmhand.Attributes;
    using Farmhand.Events;
    using Farmhand.Events.Arguments.GraphicsEvents;

    using Microsoft.Xna.Framework;

    using StardewValley;

    internal static class SettingsFlyoutHandler
    {
        // private static bool isFlyoutVisible = false;

        // private static bool isFlyoutSettingsWindowOpen = false;

        private static FrameworkMenu Menu;

        [Hook(HookType.Entry, "StardewValley.Game1", ".ctor")]
        internal static void AttachListeners()
        {
            //GraphicsEvents.PostRenderGuiEventNoCheck += GraphicsEvents_PostRenderGuiEventNoCheck;
            //GameEvents.AfterLoadedContent += GameEvents_AfterLoadedContent;
        }

        private static void GameEvents_AfterLoadedContent(object sender, EventArgs e)
        {
            Menu = new FrameworkMenu(new Rectangle(0, 200, 50, 50), false, true);
        }

        private static void GraphicsEvents_PostRenderGuiEventNoCheck(object sender, DrawEventArgs e)
        {
            var mouseX = Game1.getMouseX();
            var mouseY = Game1.getMouseY();

            if (mouseX < 25)
            {
                ShowMenu(e);
            }
            else
            {
                HideMenu(e);
            }
        }

        private static void HideMenu(DrawEventArgs e)
        {
        }

        private static void ShowMenu(DrawEventArgs e)
        {
            Menu.draw(e.SpriteBatch);
        }
    }
}