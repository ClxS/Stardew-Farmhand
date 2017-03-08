namespace Farmhand.UI.Internal_Menus.Configuration
{
    using System;

    using Farmhand.Attributes;
    using Farmhand.Events;
    using Farmhand.Events.Arguments.GameEvents;
    using Farmhand.Events.Arguments.GraphicsEvents;
    using Farmhand.Events.Arguments.Menus.TitleMenuEvents;
    using Farmhand.Events.Menus;
    using Farmhand.Registries;
    using Farmhand.UI.Components.Interfaces;
    using Farmhand.UI.Internal_Menus.Configuration.Menus;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    using StardewValley;
    using StardewValley.Menus;

    using ClickableTextureComponent = Farmhand.UI.Components.Controls.ClickableTextureComponent;
    using Game = Farmhand.API.Game;
    using TitleMenu = Farmhand.UI.TitleMenu;

    internal static class ConfigurationMenuHandler
    {
        private static FrameworkMenu flyoutMenu;

        private static FarmhandConfig apiConfigMenu;

        private static ModConfig modConfigMenu;

        private static FlyoutState currentFlyoutState = FlyoutState.Closed;

        private static float flyoutOffset = 1.0f;

        private static bool removeOnNextFrame;

        [Hook(HookType.Entry, "StardewValley.Game1", ".ctor")]
        internal static void AttachListeners()
        {
            // Add your entry logic here
            GraphicsEvents.PostRenderGuiEventNoCheck += GraphicsEvents_PostRenderGuiEventNoCheck;
            GameEvents.AfterLoadedContent += GameEvents_AfterLoadedContent;
            GameEvents.BeforeUpdateTick += GameEvents_BeforeUpdateTick;
            TitleMenuEvents.BeforeReceiveLeftClick += TitleMenuEvents_BeforeReceiveLeftClick;
            TitleMenuEvents.BeforeHoverAction += TitleMenuEvents_BeforeHoverAction;
        }

        private static bool IsMenuOnScreen()
        {
            return Game1.onScreenMenus.Contains(apiConfigMenu) || Game1.onScreenMenus.Contains(modConfigMenu)
                   || Game.ActiveClickableMenu == apiConfigMenu || Game.ActiveClickableMenu == modConfigMenu;
        }

        private static void TitleMenuEvents_BeforeHoverAction(object sender, BeforeHoverEventArgs e)
        {
            if (IsMenuOnScreen())
            {
                e.Cancel = true;
            }
        }

        private static void TitleMenuEvents_BeforeReceiveLeftClick(object sender, BeforeReceiveLeftClickEventArgs e)
        {
            if (IsMenuOnScreen())
            {
                e.Cancel = true;
            }
        }

        private static void GameEvents_BeforeUpdateTick(object sender, BeforeGameUpdateEventArgs e)
        {
            var state = Mouse.GetState();

            if (removeOnNextFrame)
            {
                if (Game.ActiveClickableMenu is TitleMenu)
                {
                    Game1.onScreenMenus.Remove(apiConfigMenu);
                    Game1.onScreenMenus.Remove(modConfigMenu);
                }
                else
                {
                    Game.ActiveClickableMenu = null;
                }

                removeOnNextFrame = false;
            }

            if (!IsMenuOnScreen())
            {
                UpdateMenu(flyoutMenu, state, e.GameTime);
            }

            if (Game.ActiveClickableMenu is TitleMenu)
            {
                if (Game1.onScreenMenus.Contains(apiConfigMenu))
                {
                    UpdateMenu(apiConfigMenu, state, e.GameTime);
                }

                if (Game1.onScreenMenus.Contains(modConfigMenu))
                {
                    UpdateMenu(modConfigMenu, state, e.GameTime);
                }
            }
        }

        private static void UpdateMenu(IClickableMenu clickableMenu, MouseState state, GameTime gameTime)
        {
            if (clickableMenu == null)
            {
                return;
            }

            // We have to manually detect clicks on our flyout, since the Game only triggers the events for
            // the "activeClickableMenu".
            clickableMenu.performHoverAction(Game1.getMouseX(), Game1.getMouseY());

            if (Game.ActiveClickableMenu != null)
            {
                clickableMenu.update(gameTime);
            }

            if (state.LeftButton == ButtonState.Pressed && Game1.oldMouseState.LeftButton == ButtonState.Released)
            {
                clickableMenu.receiveLeftClick(Game1.getMouseX(), Game1.getMouseY(), true);
            }

            if (state.ScrollWheelValue != Game1.oldMouseState.ScrollWheelValue)
            {
                clickableMenu.receiveScrollWheelAction(state.ScrollWheelValue - Game1.oldMouseState.ScrollWheelValue);
            }
        }

        private static void GameEvents_AfterLoadedContent(object sender, EventArgs e)
        {
            var titleButtonsTexture = TextureRegistry.GetItem("FarmhandUI.modTitleMenu");
            flyoutMenu = new FrameworkMenu(new Rectangle(0, 30, 20, 60), false, true);

            var apiSettingsButton = new ClickableTextureComponent(
                new Rectangle(-4, -3, 8, 8),
                titleButtonsTexture?.Texture,
                null,
                new Rectangle(2, 25, 21, 21));
            apiSettingsButton.Handler += ApiSettingsButton_Handler;
            flyoutMenu.AddComponent(apiSettingsButton);

            var modSettingsButton = new ClickableTextureComponent(
                new Rectangle(-4, 7, 8, 8),
                titleButtonsTexture?.Texture,
                null,
                new Rectangle(25, 25, 21, 21));
            modSettingsButton.Handler += ModSettingsButton_Handler;
            flyoutMenu.AddComponent(modSettingsButton);

            apiConfigMenu = new FarmhandConfig();
            apiConfigMenu.Close += CloseMenu;

            modConfigMenu = new ModConfig();
            modConfigMenu.Close += CloseMenu;
        }

        private static void CloseMenu(object sender, EventArgs e)
        {
            removeOnNextFrame = true;
        }

        private static void ApiSettingsButton_Handler(
            IInteractiveMenuComponent component,
            IComponentContainer collection,
            FrameworkMenu frameworkMenu)
        {
            if (!Game1.onScreenMenus.Contains(apiConfigMenu))
            {
                if (Game.ActiveClickableMenu is TitleMenu)
                {
                    Game1.onScreenMenus.Add(apiConfigMenu);
                }
                else
                {
                    Game.ActiveClickableMenu = apiConfigMenu;
                }

                apiConfigMenu.OnOpen();
            }
        }

        private static void ModSettingsButton_Handler(
            IInteractiveMenuComponent component,
            IComponentContainer collection,
            FrameworkMenu frameworkMenu)
        {
            if (!Game1.onScreenMenus.Contains(modConfigMenu))
            {
                if (Game.ActiveClickableMenu is TitleMenu)
                {
                    Game1.onScreenMenus.Add(modConfigMenu);
                }
                else
                {
                    Game.ActiveClickableMenu = modConfigMenu;
                }

                modConfigMenu.OnOpen();
            }
        }

        private static void GraphicsEvents_PostRenderGuiEventNoCheck(object sender, DrawEventArgs e)
        {
            if (!IsMenuOnScreen())
            {
                var mouseX = Game1.getMouseX();

                if (mouseX < 20 * Game1.pixelZoom)
                {
                    ShowMenu(e);
                }
                else
                {
                    HideMenu(e);
                }
            }

            if (Game.ActiveClickableMenu is TitleMenu)
            {
                if (Game1.onScreenMenus.Contains(apiConfigMenu))
                {
                    apiConfigMenu.draw(e.SpriteBatch);
                }

                if (Game1.onScreenMenus.Contains(modConfigMenu))
                {
                    modConfigMenu.draw(e.SpriteBatch);
                }
            }
        }

        private static void HideMenu(DrawEventArgs e)
        {
            if (currentFlyoutState == FlyoutState.Closed)
            {
                return;
            }

            currentFlyoutState = FlyoutState.Closing;

            flyoutOffset += (float)e.GameTime.ElapsedGameTime.TotalSeconds;

            if (flyoutOffset >= 1.0f)
            {
                flyoutOffset = 1.0f;
                currentFlyoutState = FlyoutState.Closed;
            }

            flyoutMenu.Area = new Rectangle(
                (int)Ease(flyoutOffset, 0.0f, -20.0f, 1.0f) * Game1.pixelZoom,
                30 * Game1.pixelZoom,
                20 * Game1.pixelZoom,
                60 * Game1.pixelZoom);
            flyoutMenu.draw(e.SpriteBatch);
        }

        private static void ShowMenu(DrawEventArgs e)
        {
            if (currentFlyoutState == FlyoutState.Open)
            {
                flyoutMenu.draw(e.SpriteBatch);
                return;
            }

            currentFlyoutState = FlyoutState.Opening;

            flyoutOffset -= (float)e.GameTime.ElapsedGameTime.TotalSeconds;

            if (flyoutOffset <= 0.0f)
            {
                flyoutOffset = 0.0f;
                currentFlyoutState = FlyoutState.Open;
            }

            flyoutMenu.Area = new Rectangle(
                (int)-Ease(flyoutOffset, 0.0f, 20.0f, 1.0f) * Game1.pixelZoom,
                30 * Game1.pixelZoom,
                20 * Game1.pixelZoom,
                60 * Game1.pixelZoom);
            flyoutMenu.draw(e.SpriteBatch);
        }

        private static float Ease(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1)
            {
                return c / 2 * t * t * t * t * t + b;
            }

            t -= 2;
            return c / 2 * (t * t * t * t * t + 2) + b;
        }

        #region Nested type: FlyoutState

        private enum FlyoutState
        {
            Closed,

            Closing,

            Opening,

            Open
        }

        #endregion
    }
}