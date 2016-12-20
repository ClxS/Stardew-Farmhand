using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Farmhand.Attributes;
using Farmhand.Registries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace Farmhand.UI
{
    /// <summary>
    /// Override of Stardew's TitleMenu, providing methods to add custom titlemenu buttons
    /// </summary>
    [HookRedirectConstructorFromBase("StardewValley.Game1", "setGameMode")]
    public class TitleMenu : StardewValley.Menus.TitleMenu
    {
        public class CustomTitleOption
        {
            public string Key { get; set; }
            public Texture2D Texture { get; set; }
            public Rectangle TextureSourceRect { get; set; }
            public Action<TitleMenu, string> OnClick { get; set; }
        }

        public int ChuckleFishTimer
        {
            get { return this.chuckleFishTimer; }
            set { this.chuckleFishTimer = value; }
        }

        public int FadeFromWhiteTimer
        {
            get { return this.fadeFromWhiteTimer; }
            set { this.fadeFromWhiteTimer = value; }
        }

        public int LogoFadeTimer
        {
            get { return this.logoFadeTimer; }
            set { this.logoFadeTimer = value; }
        }

        public int LogoSurprisedTimer
        {
            get { return this.logoSurprisedTimer; }
            set { this.logoSurprisedTimer = value; }
        }

        public float LogoSwipeTimer
        {
            get { return this.logoSwipeTimer; }
            set { this.logoSwipeTimer = value; }
        }

        public int PauseBeforeViewportRiseTimer
        {
            get { return this.pauseBeforeViewportRiseTimer; }
            set { this.pauseBeforeViewportRiseTimer = value; }
        }

        public int QuitTimer
        {
            get { return this.quitTimer; }
            set { this.quitTimer = value; }
        }

        public int ShowButtonsTimer
        {
            get { return this.showButtonsTimer; }
            set { this.showButtonsTimer = value; }
        }

        public static List<CustomTitleOption> CustomOptions = new List<CustomTitleOption>();
        private ClickableTextureComponent _folderButton;
        private string HoverText { get; set; }

        public static void RegisterNewTitleButton(CustomTitleOption button)
        {
            CustomOptions.Add(button);
        }

        public static void RemoveCustomTileButton(string key)
        {
            var removedItems = CustomOptions.Where(n => n.Key == key);
            foreach (var item in removedItems)
            {
                CustomOptions.Remove(item);
            }
        }

        public int GetItemOffsetX(int index, int count)
        {
            var offset = 123 * (count-3);
            var itemSpacing = 249;
            var itemRoot = -354;

            return itemRoot - offset + index * itemSpacing;
        }

        [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", ".ctor")]
        [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", "update")]
        [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", "gameWindowSizeChanged")]
        public override void setUpIcons()
        {
            buttons.Clear();

            var index = 0;
            buttons.Add(new ClickableTextureComponent("New", new Rectangle(width / 2 + GetItemOffsetX(index++, 3 + CustomOptions.Count), height - 174 - 24, 222, 174), "", "New", titleButtonsTexture, new Rectangle(0, 187, 74, 58), 3f));
            buttons.Add(new ClickableTextureComponent("Load", new Rectangle(width / 2 + GetItemOffsetX(index++, 3 + CustomOptions.Count), height - 174 - 24, 222, 174), "", "Load", titleButtonsTexture, new Rectangle(74, 187, 74, 58), 3f));
            foreach (var customOption in CustomOptions) {
                buttons.Add(new ClickableTextureComponent(customOption.Key, new Rectangle(width / 2 + GetItemOffsetX(index++, 3 + CustomOptions.Count), height - 174 - 24, 222, 174), "", customOption.Key, customOption.Texture, customOption.TextureSourceRect, 3f));
            }
            buttons.Add(new ClickableTextureComponent("Exit", new Rectangle(width / 2 + GetItemOffsetX(index, 3 + CustomOptions.Count), height - 174 - 24, 222, 174), "", "Exit", titleButtonsTexture, new Rectangle(222, 187, 74, 58), 3f));
            
            var num = height < 800 ? 2 : 3;
            eRect = new Rectangle(width / 2 - 200 * num + 251 * num, -300 * num - (int)(viewportY / 3.0) * num + 26 * num, 42 * num, 68 * num);
            _folderButton = new ClickableTextureComponent("Folder", new Rectangle(57, height - 75 - 24, 72, 75), "", "Mods Folder", TextureRegistry.GetItem("FarmhandUI.modTitleMenu").Texture, new Rectangle(52, 458, 24, 25), 3f);
            backButton = new ClickableTextureComponent("Back", new Rectangle(width - 198 - 48, height - 81 - 24, 198, 81),"", "Back", titleButtonsTexture, new Rectangle(296, 252, 66, 27), 3f);
            aboutButton = new ClickableTextureComponent("About", new Rectangle(width - 66 - 48, height - 75 - 24, 66, 75), "", "About", titleButtonsTexture, new Rectangle(8, 458, 22, 25), 3f);
            skipButton = new ClickableComponent(new Rectangle(width / 2 - 261, height / 2 - 102, 249, 201), "Skip", "");
        }

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

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);

            if (_folderButton.containsPoint(x, y)) {
                Game1.playSound("select");
                Process.Start($"{Environment.CurrentDirectory}\\Mods");
            }
        }

        public override void performHoverAction(int x, int y)
        {
            HoverText = "";
            base.performHoverAction(x, y);

            _folderButton.tryHover(x, y, 0.25f);
            if (_folderButton.containsPoint(x, y)) {
                if (_folderButton.sourceRect.X == 52)
                    Game1.playSound("Cowboy_Footstep");
                
                _folderButton.sourceRect.X = 76;

                HoverText = _folderButton.hoverText;
            }
            else
                _folderButton.sourceRect.X = 52;
        }

        [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", "draw")]
        public override void draw(SpriteBatch b)
        {
            base.draw(b);

            if (subMenu == null && !isTransitioningButtons && titleInPosition && !transitioningCharacterCreationMenu)
                _folderButton.draw(b);

            if (!String.IsNullOrEmpty(HoverText))
                drawHoverText(b, HoverText, Game1.dialogueFont);

            if (QuitTimer > 0)
                b.Draw(Game1.staminaRect, new Rectangle(0, 0, width, height), Color.Black * (1f - QuitTimer / 500f));

            drawMouse(b);
        }

        public void SetSubmenu(IClickableMenu menu)
        {
            subMenu = menu;
        }

        public void StartMenuTransitioning()
        {
            buttonsDX = 1;
            isTransitioningButtons = true;
        }
        
        public override void update(GameTime time)
        {
            base.update(time);
            if (buttonsToShow == 3)
                buttonsToShow = 3 + CustomOptions.Count;
        }
    }
}
