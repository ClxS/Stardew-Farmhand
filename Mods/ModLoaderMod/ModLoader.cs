using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModLoaderMod.Menus;
using Revolution.Events.Arguments;
using Revolution.Registries;
using StardewValley;
using StardewValley.Menus;
using System;
using System.IO;

namespace ModLoaderMod
{
    class ModLoader : Revolution.Mod
    {
        public static ModLoader Instance { get; set; }

        public override void Entry()
        {
            Instance = this;
            Revolution.Events.GameEvents.OnAfterGameInitialised += OnAfterGameInitialise;            
        }

        public void OnAfterGameInitialise(object sender, EventArgsOnGameInitialised e)
        {
            try
            {
                var texture = Texture2D.FromStream(Game1.graphics.GraphicsDevice, new FileStream("RevolutionContent\\customUI.png", FileMode.Open));
                TextureRegistry.RegisterItem("icon_menuModsButton", texture);

                Revolution.UI.TitleMenu.RegisterNewTitleButton(new Revolution.UI.TitleMenu.CustomTitleOption()
                {
                    Key = "Mods",
                    Texture = texture,
                    TextureSourceRect = new Rectangle(222, 187, 74, 58),
                    OnClick = new Action<Revolution.UI.TitleMenu, string>(OnModMenuItemClicked)
                });
            }
            catch (System.Exception ex)
            {
            	
            }            
        }

        public void OnModMenuItemClicked(Revolution.UI.TitleMenu menu, string choice)
        {
            if (choice == "Mods")
            {
                menu.StartMenuTransitioning();
                Game1.playSound("select");
                Game1.changeMusicTrack("CloudCountry");
                menu.SetSubmenu(new ModMenu());
            }
        }
    }
}
