namespace ModTemplate.Pages
{
    // ReSharper disable StyleCop.SA1300
    using System;

    using Farmhand;
    using Farmhand.UI;
    using Farmhand.UI.Containers;
    using Farmhand.UI.Form;
    using Farmhand.UI.Generic;
    using Farmhand.UI.Interfaces;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    internal sealed class FarmhandConfig : FrameworkMenu
    {
        private readonly CheckboxFormComponent cachePortsBox;

        private readonly FrameworkMenu controlContainer;

        private readonly CheckboxFormComponent debugModeBox;

        public FarmhandConfig()
            : base(FrameDimensions, false, false)
        {
            this.controlContainer = new FrameworkMenu(new Point(100, 120), false);
            var tab =
                new FormCollectionComponent(
                    new Rectangle(0, 0, this.ZoomEventRegion.Width, this.ZoomEventRegion.Height));

            tab.AddComponent(new TextComponent(new Point(10, 0), "Farmhand Settings"));
            this.debugModeBox = new CheckboxFormComponent(new Point(0, 12), "Debug Mode");
            this.cachePortsBox = new CheckboxFormComponent(new Point(0, 24), "Cache Ports");
            
            tab.AddComponent(this.debugModeBox);
            tab.AddComponent(this.cachePortsBox);

            var saveButton = new ButtonFormComponent(new Point(4, 92), 70, "Save");
            saveButton.Handler += this.SaveButton_Handler;
            var cancelButton = new ButtonFormComponent(new Point(4, 80), 70, "Cancel");
            cancelButton.Handler += this.CancelButton_Handler;
            tab.AddComponent(saveButton);
            tab.AddComponent(cancelButton);

            this.controlContainer.AddComponent(tab);
        }

        private static Rectangle FrameDimensions
        {
            get
            {
                var bounds = Game1.game1.Window.ClientBounds;
                return new Rectangle(
                    0,
                    0,
                    bounds.Width / Game1.pixelZoom,
                    bounds.Height / Game1.pixelZoom);
            }
        }

        public void OnOpen()
        {
            this.debugModeBox.Value = Farmhand.Program.Config.DebugMode;
            this.cachePortsBox.Value = Farmhand.Program.Config.CachePorts;
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
            Farmhand.Program.Config.CachePorts = this.cachePortsBox.Value;
            Farmhand.Program.Config.DebugMode = this.debugModeBox.Value;
            Farmhand.Program.SaveConfig();
            this.OnClose();
        }

        public override void draw(SpriteBatch b)
        {
            b.Draw(
                Game1.fadeToBlackRect,
                Game1.graphics.GraphicsDevice.Viewport.Bounds,
                Color.Black * 0.75f);

            DrawMenuRect(
                b,
                this.controlContainer.Area.X,
                this.controlContainer.Area.Y,
                this.controlContainer.Area.Width,
                this.controlContainer.Area.Height);

            var o = new Point(this.Area.X + Zoom10, this.Area.Y + Zoom10);
            foreach (var el in this.DrawOrder)
            {
                el.Draw(b, o);
            }

            this.controlContainer.draw(b);
            base.draw(b);
            this.drawMouse(b);
        }

        public override void performHoverAction(int x, int y)
        {
            base.performHoverAction(x, y);
            this.controlContainer.performHoverAction(x, y);
        }

        public override void update(GameTime time)
        {
            base.update(time);
            this.controlContainer.update(time);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);
            this.controlContainer.receiveLeftClick(x, y, playSound);
        }

        public override void receiveScrollWheelAction(int direction)
        {
            base.receiveScrollWheelAction(direction);
            this.controlContainer.receiveScrollWheelAction(direction);
        }

        private void OnClose()
        {
            // We can't use the built-in exit menu method, because it messes up in the TitleMenu
            this.Close(this, EventArgs.Empty);
        }
    }
}