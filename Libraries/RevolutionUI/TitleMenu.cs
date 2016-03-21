using Microsoft.Xna.Framework;
using Revolution.Attributes;
using StardewValley.Menus;
using System;
using Revolution.Registries;
using StardewValley;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

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

                var texture = Texture2D.FromStream(Game1.graphics.GraphicsDevice, new FileStream("RevolutionContent\\customUI.png", FileMode.Open));

                TextureRegistry.RegisterItem("icon_menuModsButton", texture);
            }

            [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", ".ctor")]
            [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", "update")]
            [HookMakeBaseVirtualCall("StardewValley.Menus.TitleMenu", "gameWindowSizeChanged")]
            public override void setUpIcons()
            {
                Console.WriteLine("Custom Icon Setup");
                this.buttons.Clear();

                int offset = 125;
                this.buttons.Add(new ClickableTextureComponent(new Rectangle(this.width / 2 - 381 - offset, this.height - 174 - 24, 222, 174), "New", "", this.titleButtonsTexture, new Rectangle(0, 187, 74, 58), 3f, false, false));
                this.buttons.Add(new ClickableTextureComponent(new Rectangle(this.width / 2 - 135 - offset, this.height - 174 - 24, 222, 174), "Load", "", this.titleButtonsTexture, new Rectangle(74, 187, 74, 58), 3f, false, false));
                this.buttons.Add(new ClickableTextureComponent(new Rectangle(this.width / 2 + 111 - offset, this.height - 174 - 24, 222, 174), "Mods", "", TextureRegistry.GetItem("icon_menuModsButton"), new Rectangle(222, 187, 74, 58), 3f, false, false));
                this.buttons.Add(new ClickableTextureComponent(new Rectangle(this.width / 2 + 357 - offset, this.height - 174 - 24, 222, 174), "Exit", "", this.titleButtonsTexture, new Rectangle(222, 187, 74, 58), 3f, false, false));
                
                int num = this.height < 800 ? 2 : 3;
                this.eRect = new Rectangle(this.width / 2 - 200 * num + 251 * num, -300 * num - (int)((double)this.viewportY / 3.0) * num + 26 * num, 42 * num, 68 * num);
                this.backButton = new ClickableTextureComponent(new Rectangle(this.width - 198 - 48, this.height - 81 - 24, 198, 81), "Back", "", this.titleButtonsTexture, new Rectangle(296, 252, 66, 27), 3f, false, false);
                this.aboutButton = new ClickableTextureComponent(new Rectangle(this.width - 66 - 48, this.height - 75 - 24, 66, 75), "About", "", this.titleButtonsTexture, new Rectangle(8, 458, 22, 25), 3f, false, false);
                this.skipButton = new ClickableComponent(new Rectangle(this.width / 2 - 261, this.height / 2 - 102, 249, 201), "Skip");
            }

            public override void update(GameTime time)
            {
                base.update(time);
                if (this.buttonsToShow == 3)
                    this.buttonsToShow = 4;
            }
        }
    }
}
