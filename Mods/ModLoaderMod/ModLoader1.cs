using Microsoft.Xna.Framework;
using ModLoaderMod.Menus;
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
            Farmhand.Events.GameEvents.AfterGameInitialised += OnAfterGameInitialise;         
        }
        
        public void OnAfterGameInitialise(object sender, EventArgsOnGameInitialised e)
        {
            var test = ModRegistry.GetRegisteredItems();
            var texture = ModSettings.GetTexture("icon_menuModsButton");
            
            Farmhand.UI.TitleMenu.RegisterNewTitleButton(new Farmhand.UI.TitleMenu.CustomTitleOption
            {
                Key = "Mods",
                Texture = texture,
                TextureSourceRect = new Rectangle(222, 187, 74, 58),
                OnClick = OnModMenuItemClicked
            });
        }

        public void OnModMenuItemClicked(Farmhand.UI.TitleMenu menu, string choice)
        {
            if (choice != "Mods") return;

            menu.StartMenuTransitioning();
            Game1.playSound("select");
            menu.SetSubmenu(new ModMenu());
        }
    }
}
