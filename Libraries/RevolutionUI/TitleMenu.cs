using Microsoft.Xna.Framework;
using Revolution.Attributes;
using StardewValley.Menus;
using System;
using Revolution.Registries;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Revolution
{
    namespace UI
    {     
        [HookRedirectConstructorFromBase("StardewValley.Game1", "setGameMode")]
        public class TitleMenu : StardewValley.Menus.TitleMenu
        {
            public class CustomTitleOption
            {
                public string Key { get; set; }
                public Texture2D Texture { get; set; }
                public Rectangle TextureSourceRect { get; set; }
                public Action<Revolution.UI.TitleMenu, string> OnClick { get; set; }
            }

            public static List<CustomTitleOption> CustomOptions = new List<CustomTitleOption>();

            public TitleMenu()
            {
            }

            public static void RegisterNewTitleButton(CustomTitleOption button)
            {
                CustomOptions.Add(button);
            }

            public static void RemoveCustomTileButton(string key)
            {
                var removedItems = CustomOptions.Where(n => n.Key == key);
                foreach(var item in removedItems)
                {
                    CustomOptions.Remove(item);
                }
            }

            public int getItemOffsetX(int index, int count)
            {
                int offset = 123 * (count - 3);
                int itemSpacing = offset * 2;
                int itemRoot = -381;

                return itemRoot - offset + index * itemSpacing;
            }

            [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", ".ctor")]
            [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", "update")]
            [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", "gameWindowSizeChanged")]
            public override void setUpIcons()
            {
                this.buttons.Clear();

                int index = 0;
                this.buttons.Add(new ClickableTextureComponent(new Rectangle(this.width / 2 + getItemOffsetX(index++, 4), this.height - 174 - 24, 222, 174), "New", "", this.titleButtonsTexture, new Rectangle(0, 187, 74, 58), 3f, false, false));
                this.buttons.Add(new ClickableTextureComponent(new Rectangle(this.width / 2 + getItemOffsetX(index++, 4), this.height - 174 - 24, 222, 174), "Load", "", this.titleButtonsTexture, new Rectangle(74, 187, 74, 58), 3f, false, false));
                foreach(var customOption in CustomOptions)
                {
                    this.buttons.Add(new ClickableTextureComponent(new Rectangle(this.width / 2 + getItemOffsetX(index++, 4), this.height - 174 - 24, 222, 174), "Mods", "", customOption.Texture, customOption.TextureSourceRect, 3f, false, false));
                }
                this.buttons.Add(new ClickableTextureComponent(new Rectangle(this.width / 2 + getItemOffsetX(index++, 4), this.height - 174 - 24, 222, 174), "Exit", "", this.titleButtonsTexture, new Rectangle(222, 187, 74, 58), 3f, false, false));
                
                int num = this.height < 800 ? 2 : 3;
                this.eRect = new Rectangle(this.width / 2 - 200 * num + 251 * num, -300 * num - (int)((double)this.viewportY / 3.0) * num + 26 * num, 42 * num, 68 * num);
                this.backButton = new ClickableTextureComponent(new Rectangle(this.width - 198 - 48, this.height - 81 - 24, 198, 81), "Back", "", this.titleButtonsTexture, new Rectangle(296, 252, 66, 27), 3f, false, false);
                this.aboutButton = new ClickableTextureComponent(new Rectangle(this.width - 66 - 48, this.height - 75 - 24, 66, 75), "About", "", this.titleButtonsTexture, new Rectangle(8, 458, 22, 25), 3f, false, false);
                this.skipButton = new ClickableComponent(new Rectangle(this.width / 2 - 261, this.height / 2 - 102, 249, 201), "Skip");
            }

            [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", "receiveLeftClick")]
            public override void performButtonAction(string which)
            {
                base.performButtonAction(which);

                var customPressed = CustomOptions.Where(n => n.Key == which);
                foreach(var customAction in customPressed)
                {
                    if(customAction.OnClick != null)
                    {
                        customAction.OnClick(this, which);
                    }
                }                
            }

            public void SetSubmenu(IClickableMenu menu)
            {
                this.subMenu = menu;
            }

            public void StartMenuTransitioning()
            {
                this.buttonsDX = 1;
                this.isTransitioningButtons = true;
            }

            public override void update(GameTime time)
            {
                base.update(time);
                if (this.buttonsToShow == 3)
                    this.buttonsToShow = 3 + CustomOptions.Count;                               
            }
        }
    }
}
