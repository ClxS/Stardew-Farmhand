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

    using StardewValley;
    using StardewValley.Menus;

    using ClickableTextureComponent = Farmhand.UI.Generic.ClickableTextureComponent;
    using FarmhandConfig = ModTemplate.Pages.FarmhandConfig;
    using TitleMenu = Farmhand.UI.TitleMenu;

    internal class Mod : Farmhand.Mod
    {
        private static FrameworkMenu Menu;

        private static FarmhandConfig ApiConfigMenu;

        private static FlyoutState CurrentFlyoutState = FlyoutState.Closed;

        private static float FlyoutOffset = 1.0f;

        public override void Entry()
        {
            // Add your entry logic here
            GraphicsEvents.PostRenderGuiEventNoCheck += GraphicsEvents_PostRenderGuiEventNoCheck;
            GameEvents.AfterLoadedContent += this.GameEvents_AfterLoadedContent;
            GameEvents.BeforeUpdateTick += GameEvents_BeforeUpdateTick;
            TitleMenuEvents.BeforeReceiveLeftClick += TitleMenuEvents_BeforeReceiveLeftClick;
        }

        private void TitleMenuEvents_BeforeReceiveLeftClick(object sender, BeforeReceiveLeftClick e)
        {
            if (Game1.onScreenMenus.Contains(ApiConfigMenu))
            {
                e.Cancel = true;
            }
        }

        private static void GameEvents_BeforeUpdateTick(object sender, BeforeGameUpdateEventArgs e)
        {
            var state = Mouse.GetState();

            UpdateMenu(Menu, state, e.GameTime);
            
            if (Game1.activeClickableMenu is TitleMenu)
            {
                if (Game1.onScreenMenus.Contains(ApiConfigMenu))
                {
                    UpdateMenu(ApiConfigMenu, state, e.GameTime);
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
            Menu = new FrameworkMenu(new Rectangle(0, 30, 20, 60), false, true);
            var settingsButton = new ClickableTextureComponent(
                new Rectangle(-5, -3, 10, 10),
                gearTexture?.Texture);
            settingsButton.Handler += SettingsButton_Handler;
            Menu.AddComponent(settingsButton);

            ApiConfigMenu = new FarmhandConfig();
        }
        
        private static void SettingsButton_Handler(
            IInteractiveMenuComponent component,
            IComponentContainer collection,
            FrameworkMenu menu)
        {
            if (!Game1.onScreenMenus.Contains(ApiConfigMenu))
            {
                Game1.onScreenMenus.Add(ApiConfigMenu);
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
                if (Game1.onScreenMenus.Contains(ApiConfigMenu))
                {
                    ApiConfigMenu.draw(e.SpriteBatch);
                }
            }
        }

        private static void HideMenu(DrawEventArgs e)
        {
            if (CurrentFlyoutState == FlyoutState.Closed)
            {
                return;
            }

            CurrentFlyoutState = FlyoutState.Closing;

            FlyoutOffset += (float)e.GameTime.ElapsedGameTime.TotalSeconds;

            if (FlyoutOffset >= 1.0f)
            {
                FlyoutOffset = 1.0f;
                CurrentFlyoutState = FlyoutState.Closed;
            }

            Menu.Area = new Rectangle(
                (int)Ease(FlyoutOffset, 0.0f, -20.0f, 1.0f) * Game1.pixelZoom,
                30 * Game1.pixelZoom,
                20 * Game1.pixelZoom,
                60 * Game1.pixelZoom);
            Menu.draw(e.SpriteBatch);
        }

        private static void ShowMenu(DrawEventArgs e)
        {
            if (CurrentFlyoutState == FlyoutState.Open)
            {
                Menu.draw(e.SpriteBatch);
                return;
            }

            CurrentFlyoutState = FlyoutState.Opening;

            FlyoutOffset -= (float)e.GameTime.ElapsedGameTime.TotalSeconds;

            if (FlyoutOffset <= 0.0f)
            {
                FlyoutOffset = 0.0f;
                CurrentFlyoutState = FlyoutState.Open;
            }

            Menu.Area = new Rectangle(
                (int)-Ease(FlyoutOffset, 0.0f, 20.0f, 1.0f) * Game1.pixelZoom,
                30 * Game1.pixelZoom,
                20 * Game1.pixelZoom,
                60 * Game1.pixelZoom);
            Menu.draw(e.SpriteBatch);
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