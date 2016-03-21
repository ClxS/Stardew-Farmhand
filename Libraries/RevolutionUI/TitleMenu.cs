using Microsoft.Xna.Framework;
using Revolution.Attributes;
using StardewValley.Menus;
using System;

namespace Revolution
{
    namespace UI
    {
        [HookRedirectConstructorFromBase("StardewValley.Game1", "setGameMode")]
        public class TitleMenu : StardewValley.Menus.TitleMenu
        {
            public TitleMenu()
            {
                Console.WriteLine("Using Overloaded Title Menu!");
            }

            [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", ".ctor")]
            [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", "update")]
            [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", "gameWindowSizeChanged")]
            public override void setUpIcons()
            {
                Console.WriteLine("Custom Icon Setup");
                this.buttons.Clear();
                this.buttons.Add(new ClickableTextureComponent(new Rectangle(this.width / 2 - 333 - 48, this.height - 174 - 24, 222, 174), "New", "", this.titleButtonsTexture, new Rectangle(0, 187, 74, 58), 3f, false, false));
                this.buttons.Add(new ClickableTextureComponent(new Rectangle(this.width / 2 - 111 - 24, this.height - 174 - 24, 222, 174), "Load", "", this.titleButtonsTexture, new Rectangle(74, 187, 74, 58), 3f, false, false));
                this.buttons.Add(new ClickableTextureComponent(new Rectangle(this.width / 2 + 111, this.height - 174 - 24, 222, 174), "Exit", "", this.titleButtonsTexture, new Rectangle(222, 187, 74, 58), 3f, false, false));

                int num = this.height < 800 ? 2 : 3;
                this.eRect = new Rectangle(this.width / 2 - 200 * num + 251 * num, -300 * num - (int)((double)this.viewportY / 3.0) * num + 26 * num, 42 * num, 68 * num);
                this.backButton = new ClickableTextureComponent(new Rectangle(this.width - 198 - 48, this.height - 81 - 24, 198, 81), "Back", "", this.titleButtonsTexture, new Rectangle(296, 252, 66, 27), 3f, false, false);
                this.aboutButton = new ClickableTextureComponent(new Rectangle(this.width - 66 - 48, this.height - 75 - 24, 66, 75), "About", "", this.titleButtonsTexture, new Rectangle(8, 458, 22, 25), 3f, false, false);
                this.skipButton = new ClickableComponent(new Rectangle(this.width / 2 - 261, this.height / 2 - 102, 249, 201), "Skip");
            }
        }
    }
}
