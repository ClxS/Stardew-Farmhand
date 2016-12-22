namespace MenuTestMod
{
    using System;
    using System.Collections.Generic;

    using Farmhand;
    using Farmhand.Events;
    using Farmhand.UI;
    using Farmhand.UI.Containers;
    using Farmhand.UI.Form;
    using Farmhand.UI.Generic;
    using Farmhand.UI.Interfaces;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    using StardewValley;

    internal class FrameworkMenuTestMod : Mod
    {
        internal static ProgressbarComponent Prog { get; set; } = new ProgressbarComponent(new Point(0, 20), 0, 40);

        internal static FormCollectionComponent Page1B { get; set; }

        internal static FrameworkMenu Menu { get; set; }

        internal static FrameworkMenu Example { get; set; }

        internal static GenericCollectionComponent ExampleList { get; set; }

        internal static TablistComponent ExamplePopup { get; set; }

        internal static ClickableTextureComponent ExamplePopupClose { get; set; }

        public override void Entry()
        {
            GameEvents.OnAfterUpdateTick += GameEvents_UpdateTick;
            ControlEvents.OnKeyReleased += ControlEvents_OnKeyReleased;
        }

        private static void ControlEvents_OnKeyReleased(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.F5)
            {
                MainInterface(null, null);
            }
            else if (e.KeyPressed == Keys.F6)
            {
                ExampleInterface(null, null);
            }
        }

        public void OnModMenuItemClicked(TitleMenu menu, string choice)
        {
            if (choice != "MenuTest")
            {
                return;
            }

            menu.StartMenuTransitioning();
            Game1.playSound("select");
            menu.SetSubmenu(Example);
        }

        internal static void MainInterface(object s, EventArgs e)
        {
            Farmhand.API.Game.ActiveClickableMenu = Menu;
        }

        internal static void ExampleInterface(object s, EventArgs e)
        {
            Farmhand.API.Game.ActiveClickableMenu = Example;
        }

        internal static void GameEvents_UpdateTick(object s, EventArgs e)
        {
            if (!Farmhand.API.Game.HasLoadedGame || Farmhand.API.Game.CurrentEvent != null)
            {
                return;
            }

            GameEvents.OnAfterUpdateTick -= GameEvents_UpdateTick;
            GameEvents.OnHalfSecondTick += GameEvents_HalfSecondTick;
            Menu = new FrameworkMenu(new Point(256, 168));
            TablistComponent tablist = new TablistComponent(new Rectangle(0, 0, Menu.ZoomEventRegion.Width, Menu.ZoomEventRegion.Height));
            Rectangle size = tablist.ZoomEventRegion;
            GenericCollectionComponent page1A =
                new GenericCollectionComponent(new Rectangle(0, 0, size.Width, size.Height));
            Page1B = new FormCollectionComponent(new Rectangle(0, 0, size.Width / 2, size.Height));
            GenericCollectionComponent page2 =
                new GenericCollectionComponent(new Rectangle(0, 0, size.Width, size.Height));
            GenericCollectionComponent page3 = new ScrollableCollectionComponent(new Rectangle(0, 0, size.Width, size.Height));

            Menu.AddComponent(tablist);

            tablist.AddTab(2, page1A);
            tablist.AddTab(4, page2);
            tablist.AddTab(6, page3);

            page1A.AddComponent(Page1B);
            page1A.AddComponent(
                new ButtonFormComponent(
                    new Point(size.Width / 2, 0),
                    "Toggle Enabled",
                    (t, c, m) =>
                        {
                            Page1B.Disabled = !Page1B.Disabled;
                            Console.WriteLine("Form state changed: " + (Page1B.Disabled ? "Disabled" : "Enabled"));
                        }));

            Page1B.AddComponent(new LabelComponent(new Point(0, -38), "Form Components"));
            Page1B.AddComponent(new CheckboxFormComponent(new Point(0, 2), "Example Checkbox", CheckboxChanged));
            Page1B.AddComponent(new PlusMinusFormComponent(new Point(0, 12), 0, 100, PlusMinusChanged));
            Page1B.AddComponent(new SliderFormComponent(new Point(0, 22), 3, IntSliderChanged));
            Page1B.AddComponent(new SliderFormComponent(new Point(0, 32), 12, IntSliderChanged));
            Page1B.AddComponent(new SliderFormComponent(new Point(0, 42), 20, IntSliderChanged));
            Page1B.AddComponent(
                new DropdownFormComponent(
                    new Point(0, 51),
                    new List<string>() { "First", "Second", "Third", "Fourth" },
                    DropdownChanged));
            Page1B.AddComponent(
                new DropdownFormComponent(
                    new Point(0, 65),
                    new List<string>() { "#1", "#2", "#3", "#4", "#5", "#6", "#7", "#8", "#9", "#10", "#11", "#12" },
                    DropdownChanged));
            Page1B.AddComponent(new ButtonFormComponent(new Point(0, 78), "Test Button", ButtonClicked));
            Page1B.AddComponent(new TextboxFormComponent(new Point(0, 90), TextboxChanged));
            Page1B.AddComponent(new TextboxFormComponent(new Point(0, 105), TextboxChanged));

            page2.AddComponent(new LabelComponent(new Point(0, -38), "Generic Components"));
            page2.AddComponent(new HeartsComponent(new Point(0, 0), 3, 10));
            page2.AddComponent(new ClickableHeartsComponent(new Point(0, 10), 8, 10, HeartsChanged));
            page2.AddComponent(Prog);
            page2.AddComponent(new TextureComponent(new Rectangle(0, 30, 16, 16), Game1.objectSpriteSheet, new Rectangle(0, 0, 16, 16)));
            page2.AddComponent(new ClickableTextureComponent(new Rectangle(20, 30, 16, 16), Game1.objectSpriteSheet, ButtonClicked, new Rectangle(0, 0, 16, 16)));
            page2.AddComponent(
                new ClickableTextureComponent(
                    new Rectangle(40, 30, 16, 16),
                    Game1.objectSpriteSheet,
                    ButtonClicked,
                    new Rectangle(0, 0, 16, 16),
                    false));
            page2.AddComponent(new TextComponent(new Point(0, 50), "Static text component"));
            page2.AddComponent(new ClickableTextComponent(new Point(0, 60), "Clickable text component", ButtonClicked));
            page2.AddComponent(new ClickableTextComponent(new Point(0, 70), "Clickable text component", ButtonClicked, false));

            for (var c = 0; c < 41; c++)
            {
                page3.AddComponent(new HeartsComponent(new Point(0, 10 * c), c, 40));
            }

            Example = new FrameworkMenu(new Point(256, 128), false);

            ExampleList = new GenericCollectionComponent(new Rectangle(0, 0, Example.ZoomEventRegion.Width, Example.ZoomEventRegion.Height));
            Example.AddComponent(ExampleList);

            Rectangle r = new Rectangle(0, 0, Example.ZoomEventRegion.Width, 32);
            ExampleList.AddComponent(new FrameComponent(r, Game1.mouseCursors, new Rectangle(384, 396, 15, 15)));
            ExampleList.AddComponent(new ClickableTextureComponent(r, Game1.mouseCursors, ExampleClicked, new Rectangle(383, 395, 1, 1), false));

            ExamplePopup =
                new TablistComponent(
                    new Rectangle(0, -10, Example.ZoomEventRegion.Width, Example.ZoomEventRegion.Height + 10));
            Example.AddComponent(ExamplePopup);
            ExamplePopup.Visible = false;

            ExamplePopupClose = new ClickableTextureComponent(new Rectangle(ExamplePopup.ZoomEventRegion.Width, -11, 12, 12), Game1.mouseCursors, Example2Clicked, new Rectangle(337, 494, 12, 12));
            Example.AddComponent(ExamplePopupClose);
            ExamplePopupClose.Visible = false;

            GenericCollectionComponent tab1 = new GenericCollectionComponent(ExamplePopup.ZoomEventRegion);
            GenericCollectionComponent tab2 = new GenericCollectionComponent(ExamplePopup.ZoomEventRegion);
            ExamplePopup.AddTab(102, tab1);
            ExamplePopup.AddTab(112, tab2);

            tab1.AddComponent(new TextComponent(new Point(0, 0), "Example Mod, by Example User", true, 1.5f, Color.Black));
            tab1.AddComponent(new TextComponent(new Point(0, 20), "Example Description for the Example Mod by Example User"));

            tab2.AddComponent(new TextComponent(new Point(0, 0), "Normally the mod-specific config elements would show here"));
        }

        private static int skipped;

        internal static void ExampleClicked(IMenuComponent component, IComponentContainer container, FrameworkMenu menu)
        {
            ExampleList.Visible = false;
            ExamplePopup.Visible = true;
            ExamplePopupClose.Visible = true;
        }

        internal static void Example2Clicked(
            IMenuComponent component,
            IComponentContainer container,
            FrameworkMenu menu)
        {
            ExampleList.Visible = true;
            ExamplePopup.Visible = false;
            ExamplePopupClose.Visible = false;
        }

        internal static void GameEvents_HalfSecondTick(object s, EventArgs e)
        {
            if (Prog.Value == 40)
            {
                if (skipped > 5)
                {
                    Prog.Value = 0;
                }

                skipped++;
            }
            else if (skipped > 20)
            {
                skipped = 0;
            }
            else
            {
                Prog.Value += 4;
            }

            if (skipped > 0)
            {
                skipped++;
            }
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
