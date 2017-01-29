namespace Farmhand.UI.Internal_Menus
{
    using System;

    using Farmhand.Attributes;
    using Farmhand.Events;
    using Farmhand.Events.Arguments.GraphicsEvents;

    using StardewValley;

    internal static class SettingsFlyoutHandler
    {
        private static bool isFlyoutVisible = false;

        private static bool isFlyoutSettingsWindowOpen = false;

        [Hook(HookType.Entry, "StardewValley.Game1", ".ctor")]
        internal static void AttachListeners()
        {
            GraphicsEvents.PostRenderGuiEventNoCheck += GraphicsEvents_PostRenderGuiEventNoCheck;
        }

        private static void GraphicsEvents_PostRenderGuiEventNoCheck(object sender, DrawEventArgs e)
        {
            if (isFlyoutVisible && isFlyoutSettingsWindowOpen)
            {
                return;
            }

            var mouseX = Game1.getMouseX();
            var mouseY = Game1.getMouseY();
        }
    }
}