using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Farmhand.Attributes;
using StardewValley.Menus;

namespace Farmhand.UI
{
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

        public static List<CustomTitleOption> CustomOptions = new List<CustomTitleOption>();

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
            buttons.Clear();

            var index = 0;
            buttons.Add(new ClickableTextureComponent(new Rectangle(width / 2 + GetItemOffsetX(index++, 4), height - 174 - 24, 222, 174), "New", "", titleButtonsTexture, new Rectangle(0, 187, 74, 58), 3f, false));
            buttons.Add(new ClickableTextureComponent(new Rectangle(width / 2 + GetItemOffsetX(index++, 4), height - 174 - 24, 222, 174), "Load", "", titleButtonsTexture, new Rectangle(74, 187, 74, 58), 3f, false));
            foreach (var customOption in CustomOptions)
            {
                buttons.Add(new ClickableTextureComponent(new Rectangle(width / 2 + GetItemOffsetX(index++, 4), height - 174 - 24, 222, 174), "Mods", "", customOption.Texture, customOption.TextureSourceRect, 3f, false));
            }
            buttons.Add(new ClickableTextureComponent(new Rectangle(width / 2 + GetItemOffsetX(index, 4), height - 174 - 24, 222, 174), "Exit", "", titleButtonsTexture, new Rectangle(222, 187, 74, 58), 3f, false));

            var num = height < 800 ? 2 : 3;
            eRect = new Rectangle(width / 2 - 200 * num + 251 * num, -300 * num - (int)(viewportY / 3.0) * num + 26 * num, 42 * num, 68 * num);
            backButton = new ClickableTextureComponent(new Rectangle(width - 198 - 48, height - 81 - 24, 198, 81), "Back", "", titleButtonsTexture, new Rectangle(296, 252, 66, 27), 3f, false);
            aboutButton = new ClickableTextureComponent(new Rectangle(width - 66 - 48, height - 75 - 24, 66, 75), "About", "", titleButtonsTexture, new Rectangle(8, 458, 22, 25), 3f, false);
            skipButton = new ClickableComponent(new Rectangle(width / 2 - 261, height / 2 - 102, 249, 201), "Skip");
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
