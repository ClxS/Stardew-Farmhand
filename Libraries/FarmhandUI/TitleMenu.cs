namespace Farmhand.UI
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Farmhand.Attributes;
    using Farmhand.Registries;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;
    using StardewValley.Menus;

    /// <summary>
    ///     Override of the game's TitleMenu, providing methods to add custom title menu buttons
    /// </summary>
    [HookRedirectConstructorFromBase("StardewValley.Game1", "setGameMode")]
    public class TitleMenu : StardewValley.Menus.TitleMenu
    {
        private ClickableTextureComponent folderButton;

        /// <summary>
        ///     Gets a list of custom options to display on the title screen.
        /// </summary>
        public static List<CustomTitleOption> CustomOptions { get; } = new List<CustomTitleOption>();

        /// <summary>
        ///     Gets or sets the chuckle fish timer.
        /// </summary>
        public int ChuckleFishTimer
        {
            get
            {
                return this.chuckleFishTimer;
            }

            set
            {
                this.chuckleFishTimer = value;
            }
        }

        /// <summary>
        ///     Gets or sets the fade from white timer.
        /// </summary>
        public int FadeFromWhiteTimer
        {
            get
            {
                return this.fadeFromWhiteTimer;
            }

            set
            {
                this.fadeFromWhiteTimer = value;
            }
        }

        /// <summary>
        ///     Gets or sets the logo fade timer.
        /// </summary>
        public int LogoFadeTimer
        {
            get
            {
                return this.logoFadeTimer;
            }

            set
            {
                this.logoFadeTimer = value;
            }
        }

        /// <summary>
        ///     Gets or sets the logo surprised timer.
        /// </summary>
        public int LogoSurprisedTimer
        {
            get
            {
                return this.logoSurprisedTimer;
            }

            set
            {
                this.logoSurprisedTimer = value;
            }
        }

        /// <summary>
        ///     Gets or sets the logo swipe timer.
        /// </summary>
        public float LogoSwipeTimer
        {
            get
            {
                return this.logoSwipeTimer;
            }

            set
            {
                this.logoSwipeTimer = value;
            }
        }

        /// <summary>
        ///     Gets or sets the pause before viewport rise timer.
        /// </summary>
        public int PauseBeforeViewportRiseTimer
        {
            get
            {
                return this.pauseBeforeViewportRiseTimer;
            }

            set
            {
                this.pauseBeforeViewportRiseTimer = value;
            }
        }

        /// <summary>
        ///     Gets or sets the quit timer.
        /// </summary>
        public int QuitTimer
        {
            get
            {
                return this.quitTimer;
            }

            set
            {
                this.quitTimer = value;
            }
        }

        /// <summary>
        ///     Gets or sets the show buttons timer.
        /// </summary>
        public int ShowButtonsTimer
        {
            get
            {
                return this.showButtonsTimer;
            }

            set
            {
                this.showButtonsTimer = value;
            }
        }

        private string HoverText { get; set; }

        /// <summary>
        ///     Registers a new title menu to display on screen.
        /// </summary>
        /// <param name="button">
        ///     The button to add.
        /// </param>
        public static void RegisterNewTitleButton(CustomTitleOption button)
        {
            CustomOptions.Add(button);
        }

        /// <summary>
        ///     Removes a title menu button.
        /// </summary>
        /// <param name="key">
        ///     The key of the button to remove.
        /// </param>
        public static void RemoveCustomTileButton(string key)
        {
            var removedItems = CustomOptions.Where(n => n.Key == key);
            foreach (var item in removedItems)
            {
                CustomOptions.Remove(item);
            }
        }

        private int GetItemOffsetX(int index, int count)
        {
            var offset = 123 * (count - 3);
            var itemSpacing = 249;
            var itemRoot = -354;

            return itemRoot - offset + index * itemSpacing;
        }

        /// <summary>
        ///     Game method override.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", ".ctor")]
        [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", "update")]
        [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", "gameWindowSizeChanged")]
        public override void setUpIcons()
        {
            this.buttons.Clear();

            var index = 0;
            this.buttons.Add(
                new ClickableTextureComponent(
                    "New",
                    new Rectangle(
                        this.width / 2 + this.GetItemOffsetX(index++, 3 + CustomOptions.Count),
                        this.height - 174 - 24,
                        222,
                        174),
                    string.Empty,
                    "New",
                    this.titleButtonsTexture,
                    new Rectangle(0, 187, 74, 58),
                    3f));
            this.buttons.Add(
                new ClickableTextureComponent(
                    "Load",
                    new Rectangle(
                        this.width / 2 + this.GetItemOffsetX(index++, 3 + CustomOptions.Count),
                        this.height - 174 - 24,
                        222,
                        174),
                    string.Empty,
                    "Load",
                    this.titleButtonsTexture,
                    new Rectangle(74, 187, 74, 58),
                    3f));
            foreach (var customOption in CustomOptions)
            {
                this.buttons.Add(
                    new ClickableTextureComponent(
                        customOption.Key,
                        new Rectangle(
                            this.width / 2 + this.GetItemOffsetX(index++, 3 + CustomOptions.Count),
                            this.height - 174 - 24,
                            222,
                            174),
                        string.Empty,
                        customOption.Key,
                        customOption.Texture,
                        customOption.TextureSourceRect,
                        3f));
            }

            this.buttons.Add(
                new ClickableTextureComponent(
                    "Exit",
                    new Rectangle(
                        this.width / 2 + this.GetItemOffsetX(index, 3 + CustomOptions.Count),
                        this.height - 174 - 24,
                        222,
                        174),
                    string.Empty,
                    "Exit",
                    this.titleButtonsTexture,
                    new Rectangle(222, 187, 74, 58),
                    3f));

            var num = this.height < 800 ? 2 : 3;
            this.eRect = new Rectangle(
                this.width / 2 - 200 * num + 251 * num,
                -300 * num - (int)(this.viewportY / 3.0) * num + 26 * num,
                42 * num,
                68 * num);
            this.folderButton = new ClickableTextureComponent(
                "Folder",
                new Rectangle(57, this.height - 75 - 24, 72, 75),
                string.Empty,
                "Mods Folder",
                TextureRegistry.GetItem("FarmhandUI.modTitleMenu").Texture,
                new Rectangle(0, 0, 24, 25),
                3f);
            this.backButton = new ClickableTextureComponent(
                "Back",
                new Rectangle(this.width - 198 - 48, this.height - 81 - 24, 198, 81),
                string.Empty,
                "Back",
                this.titleButtonsTexture,
                new Rectangle(296, 252, 66, 27),
                3f);
            this.aboutButton = new ClickableTextureComponent(
                "About",
                new Rectangle(this.width - 66 - 48, this.height - 75 - 24, 66, 75),
                string.Empty,
                "About",
                this.titleButtonsTexture,
                new Rectangle(8, 458, 22, 25),
                3f);
            this.skipButton =
                new ClickableComponent(
                    new Rectangle(this.width / 2 - 261, this.height / 2 - 102, 249, 201),
                    "Skip",
                    string.Empty);
        }

        /// <summary>
        ///     Game method override.
        /// </summary>
        /// <param name="which">
        ///     Which button action to perform.
        /// </param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", "receiveLeftClick")]
        public override void performButtonAction(string which)
        {
            base.performButtonAction(which);

            var customPressed = CustomOptions.Where(n => n.Key == which);
            foreach (var customAction in customPressed)
            {
                customAction.OnClick?.Invoke(this, which);
            }
        }

        /// <summary>
        /// Skips initial logo screens
        /// </summary>
        public void SkipToTitleButtons()
        {
            this.skipToTitleButtons();
            this.ChuckleFishTimer = 0;
        }

        /// <summary>
        ///     Game method override.
        /// </summary>
        /// <param name="x">
        ///     The x positon of the click.
        /// </param>
        /// <param name="y">
        ///     The y positon of the click.
        /// </param>
        /// <param name="playSound">
        ///     Whether the default click sound should be played.
        /// </param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);

            if (this.folderButton.containsPoint(x, y))
            {
                Game1.playSound("select");
                Process.Start($"{Environment.CurrentDirectory}\\Mods");
            }
        }

        /// <summary>
        ///     Game method override.
        /// </summary>
        /// <param name="x">
        ///     The x position of the click.
        /// </param>
        /// <param name="y">
        ///     The y position of the click.
        /// </param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        public override void performHoverAction(int x, int y)
        {
            this.HoverText = string.Empty;
            base.performHoverAction(x, y);

            this.folderButton.tryHover(x, y, 0.25f);
            if (this.folderButton.containsPoint(x, y))
            {
                if (this.folderButton.sourceRect.X == 0)
                {
                    Game1.playSound("Cowboy_Footstep");
                }

                this.folderButton.sourceRect.X = 24;

                this.HoverText = this.folderButton.hoverText;
            }
            else
            {
                this.folderButton.sourceRect.X = 0;
            }
        }

        /// <summary>
        ///     Game method override.
        /// </summary>
        /// <param name="b">
        ///     The sprite batch to use.
        /// </param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", "draw")]
        public override void draw(SpriteBatch b)
        {
            base.draw(b);

            if (this.subMenu == null && !this.isTransitioningButtons && this.titleInPosition
                && !this.transitioningCharacterCreationMenu)
            {
                this.folderButton.draw(b);
            }

            if (!string.IsNullOrEmpty(this.HoverText))
            {
                drawHoverText(b, this.HoverText, Game1.dialogueFont);
            }

            if (this.QuitTimer > 0)
            {
                b.Draw(
                    Game1.staminaRect,
                    new Rectangle(0, 0, this.width, this.height),
                    Color.Black * (1f - this.QuitTimer / 500f));
            }

            this.drawMouse(b);
        }

        /// <summary>
        ///     Sets the title screen's sub menu.
        /// </summary>
        /// <param name="menu">
        ///     The menu.
        /// </param>
        public void SetSubmenu(IClickableMenu menu)
        {
            this.subMenu = menu;
        }

        /// <summary>
        ///     Starts transitioning to the next menu.
        /// </summary>
        public void StartMenuTransitioning()
        {
            this.buttonsDX = 1;
            this.isTransitioningButtons = true;
        }

        /// <summary>
        ///     Game method override.
        /// </summary>
        /// <param name="time">
        ///     Elapsed time since previous update.
        /// </param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        public override void update(GameTime time)
        {
            base.update(time);
            if (this.buttonsToShow == 3)
            {
                this.buttonsToShow = 3 + CustomOptions.Count;
            }
        }

        #region Nested type: CustomTitleOption

        /// <summary>
        ///     A custom title option.
        /// </summary>
        public class CustomTitleOption
        {
            /// <summary>
            ///     Gets or sets the unique key for this option.
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            ///     Gets or sets the texture to use for this button.
            /// </summary>
            public Texture2D Texture { get; set; }

            /// <summary>
            ///     Gets or sets the texture source location.
            /// </summary>
            public Rectangle TextureSourceRect { get; set; }

            /// <summary>
            ///     Gets or sets the on click action.
            /// </summary>
            public Action<TitleMenu, string> OnClick { get; set; }
        }

        #endregion
    }
}