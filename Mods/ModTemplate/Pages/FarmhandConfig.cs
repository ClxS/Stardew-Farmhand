namespace ModTemplate.Pages
{
    // ReSharper disable StyleCop.SA1300
    using System;
    using System.Collections.Generic;

    using Farmhand.UI;
    using Farmhand.UI.Containers;
    using Farmhand.UI.Form;
    using Farmhand.UI.Generic;
    using Farmhand.UI.Interfaces;

    using Microsoft.Xna.Framework;

    using ModTemplate.Controls;

    using StardewValley;

    using Program = Farmhand.Program;

    internal sealed class FarmhandConfig : FrameworkMenu
    {
        private ColoredRectangleComponent background;

        private CheckboxFormComponent cachePortsBox;

        private CheckboxFormComponent debugModeBox;

        private List<TextComponent> warningTextComponents;

        public FarmhandConfig()
            : base(FrameDimensions, false, false)
        {
            this.Centered = true;
            var pos = Utility.getTopLeftPositionForCenteringOnScreen(100, 120);
            var controlArea = new Rectangle(
                (int)pos.X / Game1.pixelZoom - 50,
                (int)pos.Y / Game1.pixelZoom - 60,
                100,
                120);

            var tab = new FormCollectionComponent(controlArea);

            this.AddControlBackgroundRect(tab);
            this.AddControlTextLabels(tab);
            this.AddControlOptions(tab);
            this.AddControlButtons(tab);

            this.AddComponent(tab);
        }

        private static Rectangle FrameDimensions
        {
            get
            {
                var bounds = Game1.game1.Window.ClientBounds;
                return new Rectangle(0, 0, bounds.Width / Game1.pixelZoom, bounds.Height / Game1.pixelZoom);
            }
        }

        private void AddControlOptions(IComponentCollection tab)
        {
            this.debugModeBox = new CheckboxFormComponent(new Point(0, 24), "Debug Mode");
            this.cachePortsBox = new CheckboxFormComponent(new Point(0, 36), "Cache Ports");
            this.debugModeBox.Handler += this.FieldChanged_Handler;
            this.cachePortsBox.Handler += this.FieldChanged_Handler;

            tab.AddComponent(this.debugModeBox);
            tab.AddComponent(this.cachePortsBox);
        }

        private void AddControlButtons(IComponentCollection tab)
        {
            var saveButton = new ButtonFormComponent(new Point(4, 92), 70, "Save");
            saveButton.Handler += this.SaveButton_Handler;
            var cancelButton = new ButtonFormComponent(new Point(4, 80), 70, "Cancel");
            cancelButton.Handler += this.CancelButton_Handler;

            tab.AddComponent(saveButton);
            tab.AddComponent(cancelButton);
        }

        private void AddControlTextLabels(IComponentCollection tab)
        {
            this.warningTextComponents = new List<TextComponent>
                                         {
                                             new TextComponent(
                                                 new Point(0, 52),
                                                 "You might need to restart"),
                                             new TextComponent(
                                                 new Point(0, 60),
                                                 "the game for some changes"),
                                             new TextComponent(
                                                 new Point(0, 68),
                                                 "to take affect")
                                         };
            foreach (var component in this.warningTextComponents)
            {
                tab.AddComponent(component);
            }

            tab.AddComponent(new LabelComponent(new Point(7, -6), "Farmhand Settings"));
        }

        private void AddControlBackgroundRect(IComponentCollection tab)
        {
            var backgroundRect = this.ZoomEventRegion;
            this.background =
                new ColoredRectangleComponent(
                    new Rectangle(
                        backgroundRect.X - 20,
                        backgroundRect.Y - 20,
                        backgroundRect.Width + 20,
                        backgroundRect.Height + 20),
                    Color.Black * 0.75f) {
                                            Layer = -2 
                                         };
            this.AddComponent(this.background);

            var frameRect = tab.ZoomEventRegion;

            var frame =
                new FrameComponent(
                    new Rectangle(frameRect.X-10, frameRect.Y, frameRect.Width, frameRect.Height)) {
                                                        Layer = -1 
                                                     };
            this.AddComponent(frame);
        }

        private void FieldChanged_Handler(
            IInteractiveMenuComponent c,
            IComponentContainer collection,
            FrameworkMenu menu,
            bool value)
        {
            foreach (var component in this.warningTextComponents)
            {
                component.Visible = true;
            }
        }

        public void OnOpen()
        {
            this.debugModeBox.Value = Program.Config.DebugMode;
            this.cachePortsBox.Value = Program.Config.CachePorts;
            foreach (var component in this.warningTextComponents)
            {
                component.Visible = false;
            }
        }

        public event EventHandler Close = delegate { };

        private void CancelButton_Handler(
            IInteractiveMenuComponent component,
            IComponentContainer collection,
            FrameworkMenu menu)
        {
            this.OnClose();
        }

        private void SaveButton_Handler(
            IInteractiveMenuComponent component,
            IComponentContainer collection,
            FrameworkMenu menu)
        {
            Program.Config.CachePorts = this.cachePortsBox.Value;
            Program.Config.DebugMode = this.debugModeBox.Value;
            Program.SaveConfig();
            this.OnClose();
        }

        private void OnClose()
        {
            // We can't use the built-in exit menu method, because it messes up in the TitleMenu
            this.Close(this, EventArgs.Empty);
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);

            var xDiff = newBounds.Width - oldBounds.Width;
            var yDiff = newBounds.Height - oldBounds.Height;

            this.background.InflateRegion(xDiff, yDiff);
        }
    }
}