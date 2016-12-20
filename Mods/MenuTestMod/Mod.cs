using System;
using System.Collections.Generic;
using Farmhand;
using Farmhand.Events;
using Farmhand.Events.Arguments.GameEvents;
using Farmhand.Registries;
using Farmhand.UI;
using Microsoft.Xna.Framework;
using StardewValley;

namespace MenuTestMod
{
    public class FrameworkMenuTestMod : Mod
    {
        internal static ProgressbarComponent prog=new ProgressbarComponent(new Point(0, 20), 0, 40);
        internal static FormCollectionComponent page1b;
        internal static FrameworkMenu menu;
        internal static FrameworkMenu example;
        internal static GenericCollectionComponent exampleList;
        internal static TablistComponent examplePopup;
        internal static ClickableTextureComponent examplePopupClose;

        public override void Entry()
        {
            GameEvents.OnAfterUpdateTick += GameEvents_UpdateTick;
            GameEvents.OnAfterGameInitialised += OnAfterGameInitialise;
            //GameEvents.OnAfterLoadedContent += GameEvents_OnAfterLoadedContent;
            //Command.RegisterCommand("fmt_main", "Show the main Framework Menu Test interface").CommandFired += MainInterface;
            //Command.RegisterCommand("fmt_example", "Show the mod menu example Framework Menu Test interface").CommandFired += ExampleInterface;
        }

        public void OnAfterGameInitialise(object sender, EventArgsOnGameInitialised e)
        {
            var test = ModRegistry.GetRegisteredItems();
            var texture = ModSettings.GetTexture("icon_menuModsButton");

            Farmhand.UI.TitleMenu.RegisterNewTitleButton(new Farmhand.UI.TitleMenu.CustomTitleOption
            {
                Key = "MenuTest",
                Texture = texture,
                TextureSourceRect = new Rectangle(222, 237, 74, 58),
                OnClick = OnModMenuItemClicked
            });
        }

        public void OnModMenuItemClicked(Farmhand.UI.TitleMenu menu, string choice)
        {
            if (choice != "MenuTest") return;

            menu.StartMenuTransitioning();
            Game1.playSound("select");
            menu.SetSubmenu(FrameworkMenuTestMod.example);
        }

        internal static void MainInterface(object s, EventArgs e)
        {
            Game1.activeClickableMenu = menu;
        }
        internal static void ExampleInterface(object s, EventArgs e)
        {
            Game1.activeClickableMenu = example;
        }

        internal static void GameEvents_UpdateTick(object s, EventArgs e)
        {
            if (!Game1.hasLoadedGame || Game1.CurrentEvent != null)
                return;
            GameEvents.OnAfterUpdateTick -= GameEvents_UpdateTick;
            GameEvents.OnHalfSecondTick += GameEvents_HalfSecondTick;
            menu = new FrameworkMenu(new Point(256, 168)); ;
            TablistComponent tablist = new TablistComponent(new Rectangle(0, 0, menu.ZoomEventRegion.Width, menu.ZoomEventRegion.Height));
            Rectangle size = tablist.ZoomEventRegion;
            GenericCollectionComponent page1a = new GenericCollectionComponent(new Rectangle(0, 0, size.Width, size.Height));
            page1b = new FormCollectionComponent(new Rectangle(0, 0, size.Width/2, size.Height));
            GenericCollectionComponent page2 = new GenericCollectionComponent(new Rectangle(0, 0, size.Width, size.Height));
            GenericCollectionComponent page3 = new ScrollableCollectionComponent(new Rectangle(0, 0, size.Width, size.Height));

            menu.AddComponent(tablist);

            tablist.AddTab(2, page1a);
            tablist.AddTab(4, page2);
            tablist.AddTab(6, page3);

            page1a.AddComponent(page1b);
            page1a.AddComponent(new ButtonFormComponent(new Point(size.Width/2, 0), "Toggle Enabled", (t, c, m) =>
            {
                page1b.Disabled = !page1b.Disabled;
                Console.WriteLine("Form state changed: " + (page1b.Disabled?"Disabled":"Enabled"));
            }));

            page1b.AddComponent(new LabelComponent(new Point(0, -38), "Form Components"));
            page1b.AddComponent(new CheckboxFormComponent(new Point(0, 2), "Example Checkbox", CheckboxChanged));
            page1b.AddComponent(new PlusMinusFormComponent(new Point(0, 12), 0, 100, PlusMinusChanged));
            page1b.AddComponent(new SliderFormComponent(new Point(0, 22), 3, IntSliderChanged));
            page1b.AddComponent(new SliderFormComponent(new Point(0, 32), 12, IntSliderChanged));
            page1b.AddComponent(new SliderFormComponent(new Point(0, 42), 20, IntSliderChanged));
            page1b.AddComponent(new DropdownFormComponent(new Point(0, 51), new List<string>() {"First","Second","Third","Fourth"}, DropdownChanged));
            page1b.AddComponent(new DropdownFormComponent(new Point(0, 65), new List<string>() { "#1", "#2", "#3", "#4", "#5", "#6", "#7", "#8", "#9", "#10","#11","#12" }, DropdownChanged));
            page1b.AddComponent(new ButtonFormComponent(new Point(0, 78), "Test Button", ButtonClicked));
            page1b.AddComponent(new TextboxFormComponent(new Point(0, 90), TextboxChanged));
            page1b.AddComponent(new TextboxFormComponent(new Point(0, 105), TextboxChanged));

            page2.AddComponent(new LabelComponent(new Point(0, -38), "Generic Components"));
            page2.AddComponent(new HeartsComponent(new Point(0, 0), 3, 10));
            page2.AddComponent(new ClickableHeartsComponent(new Point(0, 10), 8, 10, HeartsChanged));
            page2.AddComponent(prog);
            page2.AddComponent(new TextureComponent(new Rectangle(0, 30, 16, 16), Game1.objectSpriteSheet, new Rectangle(0, 0, 16, 16)));
            page2.AddComponent(new ClickableTextureComponent(new Rectangle(20, 30, 16, 16), Game1.objectSpriteSheet, ButtonClicked, new Rectangle(0, 0, 16, 16)));
            page2.AddComponent(new ClickableTextureComponent(new Rectangle(40, 30, 16, 16), Game1.objectSpriteSheet, ButtonClicked, new Rectangle(0, 0, 16, 16),false));
            page2.AddComponent(new TextComponent(new Point(0, 50), "Static text component"));
            page2.AddComponent(new ClickableTextComponent(new Point(0, 60), "Clickable text component", ButtonClicked));
            page2.AddComponent(new ClickableTextComponent(new Point(0, 70), "Clickable text component", ButtonClicked, false));

            for (var c=0;c<41;c++)
                page3.AddComponent(new HeartsComponent(new Point(0, 10*c), c, 40));
            MainInterface(null, null);

            example = new FrameworkMenu(new Point(256, 128), false);

            exampleList = new GenericCollectionComponent(new Rectangle(0, 0, example.ZoomEventRegion.Width, example.ZoomEventRegion.Height));
            example.AddComponent(exampleList);

            Rectangle r = new Rectangle(0, 0, example.ZoomEventRegion.Width, 32);
            exampleList.AddComponent(new FrameComponent(r, Game1.mouseCursors, new Rectangle(384, 396, 15, 15)));
            exampleList.AddComponent(new ClickableTextureComponent(r, Game1.mouseCursors, ExampleClicked, new Rectangle(383, 395, 1, 1), false));

            examplePopup = new TablistComponent(new Rectangle(0,-10,example.ZoomEventRegion.Width,example.ZoomEventRegion.Height+10));
            example.AddComponent(examplePopup);
            examplePopup.Visible = false;

            examplePopupClose = new ClickableTextureComponent(new Rectangle(examplePopup.ZoomEventRegion.Width, -11, 12, 12), Game1.mouseCursors, Example2Clicked, new Rectangle(337, 494, 12, 12));
            example.AddComponent(examplePopupClose);
            examplePopupClose.Visible = false;

            GenericCollectionComponent tab1 = new GenericCollectionComponent(examplePopup.ZoomEventRegion);
            GenericCollectionComponent tab2 = new GenericCollectionComponent(examplePopup.ZoomEventRegion);
            examplePopup.AddTab(102, tab1);
            examplePopup.AddTab(112, tab2);

            tab1.AddComponent(new TextComponent(new Point(0, 0), "Example Mod, by Example User", true, 1.5f, Color.Black));
            tab1.AddComponent(new TextComponent(new Point(0, 20), "Example Description for the Example Mod by Example User"));

            tab2.AddComponent(new TextComponent(new Point(0, 0), "Normally the mod-specific config elements would show here"));
        }
        private static int Skipped = 0;
        internal static void ExampleClicked(IMenuComponent component, IComponentContainer container, FrameworkMenu menu)
        {
            exampleList.Visible = false;
            examplePopup.Visible = true;
            examplePopupClose.Visible = true;
        }
        internal static void Example2Clicked(IMenuComponent component, IComponentContainer container, FrameworkMenu menu)
        {
            exampleList.Visible = true;
            examplePopup.Visible = false;
            examplePopupClose.Visible = false;
        }
        internal static void GameEvents_HalfSecondTick(object s, EventArgs e)
        {
            if (prog.Value == 40)
            {
                if (Skipped>5)
                    prog.Value = 0;
                Skipped++;
            }
            else if(Skipped>20)
            {
                Skipped = 0;
            }
            else
                prog.Value += 4;
            if (Skipped > 0)
                Skipped++;
        }
        internal static void CheckboxChanged(IInteractiveMenuComponent component, IComponentContainer collection, FrameworkMenu menu, bool value)
        {
            Console.WriteLine("CheckBoxChanged: " + (value ? "true" : "false"));
        }
        internal static void PlusMinusChanged(IInteractiveMenuComponent component, IComponentContainer collection, FrameworkMenu menu, int value)
        {
            Console.WriteLine("PlusMinusChanged: " + value.ToString());
        }
        internal static void IntSliderChanged(IInteractiveMenuComponent component, IComponentContainer collection, FrameworkMenu menu, int value)
        {
            Console.WriteLine("IntSliderChanged: " + value.ToString());
        }
        internal static void HeartsChanged(IInteractiveMenuComponent component, IComponentContainer collection, FrameworkMenu menu, int value)
        {
            Console.WriteLine("HeartsChanged: " + value.ToString());
        }
        internal static void DropdownChanged(IInteractiveMenuComponent component, IComponentContainer collection, FrameworkMenu menu, string value)
        {
            Console.WriteLine("DropdownChanged: " + value);
        }
        internal static void TextboxChanged(IInteractiveMenuComponent component, IComponentContainer collection, FrameworkMenu menu, string value)
        {
            Console.WriteLine("TextboxChanged: " + value);
        }
        internal static void ButtonClicked(IInteractiveMenuComponent component, IComponentContainer collection, FrameworkMenu menu)
        {
            Console.WriteLine("ButtonClicked");
        }

       
    }
}
