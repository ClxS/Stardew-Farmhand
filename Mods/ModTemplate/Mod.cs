namespace ModTemplate
{
    using System;

    using Farmhand;
    using Farmhand.Events;
    using Farmhand.Events.Arguments.GameEvents;
    using Farmhand.Events.Arguments.GraphicsEvents;
    using Farmhand.Events.Arguments.Menus.TitleMenuEvents;
    using Farmhand.Events.Menus;
    using Farmhand.Registries;
    using Farmhand.UI;
    using Farmhand.UI.Interfaces;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    using ModTemplate.Pages;

    using StardewValley;
    using StardewValley.Menus;

    using ClickableTextureComponent = Farmhand.UI.Generic.ClickableTextureComponent;
    using FarmhandConfig = ModTemplate.Pages.FarmhandConfig;
    using TitleMenu = Farmhand.UI.TitleMenu;

    internal class Mod : Farmhand.Mod
    {
        private static FrameworkMenu menu;

        private static FarmhandConfig apiConfigMenu;

        private static ModConfig modConfigMenu;

        private static FlyoutState currentFlyoutState = FlyoutState.Closed;

        private static float flyoutOffset = 1.0f;

        private static bool removeOnNextFrame;

        public override void Entry()
        {
            // Add your entry logic here
            GraphicsEvents.PostRenderGuiEventNoCheck += GraphicsEvents_PostRenderGuiEventNoCheck;
            GameEvents.AfterLoadedContent += this.GameEvents_AfterLoadedContent;
            GameEvents.BeforeUpdateTick += GameEvents_BeforeUpdateTick;
            TitleMenuEvents.BeforeReceiveLeftClick += this.TitleMenuEvents_BeforeReceiveLeftClick;
            TitleMenuEvents.BeforeHoverAction += this.TitleMenuEvents_BeforeHoverAction;
        }

        private bool IsMenuOnScreen()
        {
            return Game1.onScreenMenus.Contains(apiConfigMenu)
                   || Game1.onScreenMenus.Contains(modConfigMenu);
        }

        private void TitleMenuEvents_BeforeHoverAction(object sender, BeforeHoverEventArgs e)
        {
            if (this.IsMenuOnScreen())
            {
                e.Cancel = true;
            }
        }

        private void TitleMenuEvents_BeforeReceiveLeftClick(
            object sender,
            BeforeReceiveLeftClickEventArgs e)
        {
            if (this.IsMenuOnScreen())
            {
                e.Cancel = true;
            }
        }

        private static void GameEvents_BeforeUpdateTick(object sender, BeforeGameUpdateEventArgs e)
        {
            var state = Mouse.GetState();

            if (removeOnNextFrame)
            {
                Game1.onScreenMenus.Remove(apiConfigMenu);
                Game1.onScreenMenus.Remove(modConfigMenu);
                removeOnNextFrame = false;
            }

            UpdateMenu(menu, state, e.GameTime);

            if (Game1.activeClickableMenu is TitleMenu)
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

        private static void UpdateMenu(IClickableMenu menu, MouseState state, GameTime gameTime)
        {
            if (menu == null)
            {
                return;
            }

            // We have to manually detect clicks on our flyout, since the Game only triggers the events for
            // the "activeClickableMenu".
            menu.performHoverAction(Game1.getMouseX(), Game1.getMouseY());

            if (Game1.activeClickableMenu != null)
            {
                menu.update(gameTime);
            }

            if (state.LeftButton == ButtonState.Pressed
                && Game1.oldMouseState.LeftButton == ButtonState.Released)
            {
                menu.receiveLeftClick(Game1.getMouseX(), Game1.getMouseY(), true);
            }
        }

        private void GameEvents_AfterLoadedContent(object sender, EventArgs e)
        {
            var gearTexture = TextureRegistry.GetItem("icon_gear", this.ModSettings);
            menu = new FrameworkMenu(new Rectangle(0, 30, 20, 60), false, true);

            var apiSettingsButton = new ClickableTextureComponent(
                new Rectangle(-5, -3, 10, 10),
                gearTexture?.Texture);
            apiSettingsButton.Handler += ApiSettingsButton_Handler;
            menu.AddComponent(apiSettingsButton);

            var modSettingsButton = new ClickableTextureComponent(
                new Rectangle(-5, 7, 10, 10),
                gearTexture?.Texture);
            modSettingsButton.Handler += ModSettingsButton_Handler;
            menu.AddComponent(modSettingsButton);

            apiConfigMenu = new FarmhandConfig();
            apiConfigMenu.Close += this.CloseMenu;

            modConfigMenu = new ModConfig();
            modConfigMenu.Close += this.CloseMenu;
        }

        private void CloseMenu(object sender, EventArgs e)
        {
            removeOnNextFrame = true;
        }

        private static void ApiSettingsButton_Handler(
            IInteractiveMenuComponent component,
            IComponentContainer collection,
            FrameworkMenu menu)
        {
            if (!Game1.onScreenMenus.Contains(apiConfigMenu))
            {
                Game1.onScreenMenus.Add(apiConfigMenu);
                apiConfigMenu.OnOpen();
            }
        }

        private static void ModSettingsButton_Handler(
            IInteractiveMenuComponent component,
            IComponentContainer collection,
            FrameworkMenu menu)
        {
            if (!Game1.onScreenMenus.Contains(modConfigMenu))
            {
                Game1.onScreenMenus.Add(modConfigMenu);
                modConfigMenu.OnOpen();
            }
        }

        private static void GraphicsEvents_PostRenderGuiEventNoCheck(object sender, DrawEventArgs e)
        {
            var mouseX = Game1.getMouseX();
            var mouseY = Game1.getMouseY();

            if (mouseX < 20 * Game1.pixelZoom)
            {
                ShowMenu(e);
            }
            else
            {
                HideMenu(e);
            }

            if (Game1.activeClickableMenu is TitleMenu)
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

            menu.Area = new Rectangle(
                (int)Ease(flyoutOffset, 0.0f, -20.0f, 1.0f) * Game1.pixelZoom,
                30 * Game1.pixelZoom,
                20 * Game1.pixelZoom,
                60 * Game1.pixelZoom);
            menu.draw(e.SpriteBatch);
        }

        private static void ShowMenu(DrawEventArgs e)
        {
            if (currentFlyoutState == FlyoutState.Open)
            {
                menu.draw(e.SpriteBatch);
                return;
            }

            currentFlyoutState = FlyoutState.Opening;

            flyoutOffset -= (float)e.GameTime.ElapsedGameTime.TotalSeconds;

            if (flyoutOffset <= 0.0f)
            {
                flyoutOffset = 0.0f;
                currentFlyoutState = FlyoutState.Open;
            }

            menu.Area = new Rectangle(
                (int)-Ease(flyoutOffset, 0.0f, 20.0f, 1.0f) * Game1.pixelZoom,
                30 * Game1.pixelZoom,
                20 * Game1.pixelZoom,
                60 * Game1.pixelZoom);
            menu.draw(e.SpriteBatch);
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

        #region ModSettings

        private readonly Settings settings = new Settings();

        public override ModSettings ConfigurationSettings => this.settings;

        #endregion
    }
}