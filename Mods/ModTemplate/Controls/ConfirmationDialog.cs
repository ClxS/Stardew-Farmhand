namespace ModTemplate.Controls
{
    // ReSharper disable StyleCop.SA1300
    using System;

    using Farmhand.UI;
    using Farmhand.UI.Containers;
    using Farmhand.UI.Form;
    using Farmhand.UI.Generic;
    using Farmhand.UI.Interfaces;

    using Microsoft.Xna.Framework;

    using StardewValley;

    internal sealed class ConfirmationDialog : FormCollectionComponent
    {
        private readonly FormCollectionComponent controlsCollection;

        private ColoredRectangleComponent background;

        public ConfirmationDialog(Rectangle rect)
            : base(rect)
        {
            var pos = Utility.getTopLeftPositionForCenteringOnScreen(100, 80);
            var controlArea = new Rectangle(
                (int)pos.X / Game1.pixelZoom - 50,
                (int)pos.Y / Game1.pixelZoom - 60,
                100,
                80);

            this.controlsCollection = new FormCollectionComponent(controlArea);
            this.AddControlBackgroundRect();

            this.controlsCollection.AddComponent(new LabelComponent(new Point(13, -6), "Save Changes"));

            this.controlsCollection.AddComponent(new TextComponent(new Point(10, 15), "Do you want to save"));
            this.controlsCollection.AddComponent(new TextComponent(new Point(10, 23), "changes to this mod"));
            this.controlsCollection.AddComponent(new TextComponent(new Point(10, 31), "configuration?"));

            var saveButton = new ButtonFormComponent(new Point(4, 55), 70, "Save Changes");
            saveButton.Handler += this.SaveButton_Handler;
            var cancelButton = new ButtonFormComponent(new Point(4, 43), 70, "Discard Changes");
            cancelButton.Handler += this.CancelButton_Handler;

            this.controlsCollection.AddComponent(saveButton);
            this.controlsCollection.AddComponent(cancelButton);
            this.AddComponent(this.controlsCollection);
        }

        public Action<bool> ConfirmHandler { get; set; }

        private void AddControlBackgroundRect()
        {
            var backgroundRect = this.ZoomEventRegion;
            this.background =
                new ColoredRectangleComponent(
                    new Rectangle(
                        backgroundRect.X - 40,
                        backgroundRect.Y - 40,
                        backgroundRect.Width + 40,
                        backgroundRect.Height + 40),
                    Color.Black * 0.75f) {
                                            Layer = -2 
                                         };
            this.AddComponent(this.background);

            var frameRect = this.controlsCollection.ZoomEventRegion;

            var frame =
                new FrameComponent(new Rectangle(frameRect.X - 10, frameRect.Y, frameRect.Width, frameRect.Height))
                {
                    Layer
                        =
                        -1
                };
            this.AddComponent(frame);
        }

        public void OnWindowResize(Rectangle oldBounds, Rectangle newBounds)
        {
            var xDiff = newBounds.Width - oldBounds.Width;
            var yDiff = newBounds.Height - oldBounds.Height;

            this.background.InflateRegion(xDiff, yDiff);
        }

        public void OnOpen()
        {
        }

        public event EventHandler Close = delegate { };

        private void CancelButton_Handler(
            IInteractiveMenuComponent component,
            IComponentContainer collection,
            FrameworkMenu menu)
        {
            this.ConfirmHandler?.Invoke(false);
            this.OnClose();
        }

        private void SaveButton_Handler(
            IInteractiveMenuComponent component,
            IComponentContainer collection,
            FrameworkMenu menu)
        {
            this.ConfirmHandler?.Invoke(true);
            this.OnClose();
        }

        private void OnClose()
        {
            // We can't use the built-in exit menu method, because it messes up in the TitleMenu
            this.Close(this, EventArgs.Empty);
        }
    }
}