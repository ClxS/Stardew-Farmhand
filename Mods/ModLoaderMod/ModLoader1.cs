using System;
using Microsoft.Xna.Framework;
using ModLoaderMod.Menus;
using Farmhand.Events.Arguments;
using Farmhand.Events.Arguments.GameEvents;
using Farmhand.Registries;
using StardewValley;

namespace ModLoaderMod
{
    class ModLoader1 : Farmhand.Mod
    {
        public static ModLoader1 Instance { get; set; }

        public override void Entry()
        {
            Instance = this;
            Farmhand.Events.GameEvents.OnAfterGameInitialised += OnAfterGameInitialise;
            Farmhand.Events.GlobalRouteManager.Listen("StardewValley.Menus.TitleMenu", ".ctor", TitleMenuCreated);
        }


        private void TitleMenuCreated(EventArgsGlobalRouteManager eventArgsGlobalRouteManager)
        {
            //Farmhand.Events.GlobalRouteManager.Remove("StardewValley.Menus.TitleMenu", ".ctor", TitleMenuCreated);
        }

        public void OnAfterGameInitialise(object sender, EventArgsOnGameInitialised e)
        {
            var test = ModRegistry.GetRegisteredItems();
            var texture = ModSettings.GetTexture("icon_menuModsButton");
            
            Farmhand.Overrides.UI.TitleMenu.RegisterNewTitleButton(new Farmhand.Overrides.UI.TitleMenu.CustomTitleOption
            {
                Key = "Mods",
                Texture = texture,
                TextureSourceRect = new Rectangle(222, 187, 74, 58),
                OnClick = OnModMenuItemClicked
            });
        }

        public void OnModMenuItemClicked(Farmhand.Overrides.UI.TitleMenu menu, string choice)
        {
            if (choice != "Mods") return;

            menu.StartMenuTransitioning();
            Game1.playSound("select");
            Game1.changeMusicTrack("CloudCountry");
            menu.SetSubmenu(new ModMenu());
        }
    }
}
