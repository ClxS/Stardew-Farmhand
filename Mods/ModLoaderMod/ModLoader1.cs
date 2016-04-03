using System;
using Microsoft.Xna.Framework;
using ModLoaderMod.Menus;
using Revolution.Events.Arguments;
using Revolution.Events.Arguments.GameEvents;
using Revolution.Registries;
using StardewValley;

namespace ModLoaderMod
{
    class ModLoader1 : Revolution.Mod
    {
        public static ModLoader1 Instance { get; set; }

        public override void Entry()
        {
            Instance = this;
            Revolution.Events.GameEvents.OnAfterGameInitialised += OnAfterGameInitialise;

            Revolution.Events.GlobalRouteManager.Listen("StardewValley.Menus.TitleMenu", ".ctor", TitleMenuCreated);
        }

        private void TitleMenuCreated(EventArgsGlobalRouteManager eventArgsGlobalRouteManager)
        {
            //Revolution.Events.GlobalRouteManager.Remove("StardewValley.Menus.TitleMenu", ".ctor", TitleMenuCreated);
        }

        public void OnAfterGameInitialise(object sender, EventArgsOnGameInitialised e)
        {
            var test = ModRegistry.GetRegisteredItems();
            var texture = ModSettings.GetModTexture("icon_menuModsButton");
            //var texture2 = Texture2D.FromStream(Game1.graphics.GraphicsDevice, new FileStream("RevolutionContent\\customUI.png", FileMode.Open));
               
            Revolution.UI.TitleMenu.RegisterNewTitleButton(new Revolution.UI.TitleMenu.CustomTitleOption
            {
                Key = "Mods",
                Texture = texture,
                TextureSourceRect = new Rectangle(222, 187, 74, 58),
                OnClick = OnModMenuItemClicked
            });     
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
