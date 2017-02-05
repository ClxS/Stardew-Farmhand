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
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    internal sealed class ConfirmationDialog : FormCollectionComponent
    {
        private readonly FormCollectionComponent controlsCollection;

        public ConfirmationDialog(Rectangle rect)
            : base(rect)
        {
            FrameworkMenu controlContainer = new FrameworkMenu(new Point(100, 80), false);
            this.controlsCollection = new FormCollectionComponent(controlContainer.ZoomEventRegion);

            this.controlsCollection.AddComponent(new TextComponent(new Point(0, 0), "Save Changes"));

            this.controlsCollection.AddComponent(new TextComponent(new Point(0, 10), "Do you want to save"));
            this.controlsCollection.AddComponent(new TextComponent(new Point(0, 18), "changes to this mod"));
            this.controlsCollection.AddComponent(new TextComponent(new Point(0, 26), "configuration?"));

            var saveButton = new ButtonFormComponent(new Point(4, 50), 70, "Save Changes");
            saveButton.Handler += this.SaveButton_Handler;
            var cancelButton = new ButtonFormComponent(new Point(4, 38), 70, "Discard Changes");
            cancelButton.Handler += this.CancelButton_Handler;

            this.controlsCollection.AddComponent(saveButton);
            this.controlsCollection.AddComponent(cancelButton);
            this.AddComponent(this.controlsCollection);
        }

        public Action<bool> ConfirmHandler { get; set; }

        private static Rectangle FrameDimensions
        {
            get
            {
                var bounds = Game1.game1.Window.ClientBounds;
                return new Rectangle(0, 0, bounds.Width / Game1.pixelZoom, bounds.Height / Game1.pixelZoom);
            }
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

        public override void Draw(SpriteBatch b, Point o)
        {
            if (!this.Visible)
            {
                return;
            }

            b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);

            FrameworkMenu.DrawMenuRect(
                b,
                this.controlsCollection.EventRegion.X - 10 * Game1.pixelZoom,
                this.controlsCollection.EventRegion.Y - 10 * Game1.pixelZoom,
                this.controlsCollection.EventRegion.Width + 20 * Game1.pixelZoom,
                this.controlsCollection.EventRegion.Height + 20 * Game1.pixelZoom);

            this.controlsCollection.Draw(b, Point.Zero);
        }

        public override void LeftClick(Point p, Point o)
        {
            this.controlsCollection.LeftClick(p, Point.Zero);
        }

        public override void HoverOver(Point p, Point o)
        {
            this.controlsCollection.HoverOver(p, Point.Zero);
        }
    }
}