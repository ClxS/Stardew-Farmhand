namespace ModTemplate.Pages
{
    // ReSharper disable StyleCop.SA1300
    using System;
    using System.Linq;

    using Farmhand.Registries;
    using Farmhand.Registries.Containers;
    using Farmhand.UI;
    using Farmhand.UI.Containers;
    using Farmhand.UI.Generic;
    using Farmhand.UI.Interfaces;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using ModTemplate.Controls;

    using StardewValley;

    internal sealed class ModConfig : FrameworkMenu
    {
        private readonly FrameworkMenu controlContainer;

        private readonly ScrollableCollectionComponent ModList;

        public ModConfig()
            : base(FrameDimensions, false, false)
        {
            // TODO: Find a better way of assuming the default resolution.
            this.controlContainer =
                new FrameworkMenu(
                    new Point(
                        (int)(1280 * 0.90 / Game1.pixelZoom),
                        (int)(720 * 0.90 / Game1.pixelZoom)),
                    false);
            var tab =
                new FormCollectionComponent(
                    new Rectangle(0, 0, this.ZoomEventRegion.Width, this.ZoomEventRegion.Height));

            tab.AddComponent(new TextComponent(new Point(10, 0), "Available Mods"));
            tab.AddComponent(new TextComponent(new Point(550 / Game1.pixelZoom, 0), "Mod Settings"));

            this.ModList =
                new ScrollableCollectionComponent(
                    new Rectangle(
                        0,
                        10,
                        300 / Game1.pixelZoom,
                        this.controlContainer.ZoomEventRegion.Height - 10));

            var mods =
                ModRegistry.GetRegisteredItems()
                    .Where(m => m is ModManifest)
                    .Cast<ModManifest>()
                    //.Where(m => m.Instance?.ConfigurationSettings != null)
                    .ToArray();
            for (var i = 0; i < mods.Length; ++i)
            {
                var text = new SelectableTextComponent(new Point(0, 10 * i), mods[i].Name);
                text.Handler += this.ModSelected_Handler;
                this.ModList.AddComponent(text);
            }

            tab.AddComponent(this.ModList);

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

        private void ModSelected_Handler(
            IInteractiveMenuComponent component,
            IComponentContainer collection,
            FrameworkMenu menu)
        {
            var textComponent = component as SelectableTextComponent;
            if (textComponent != null)
            {
                var otherItems =
                    this.ModList.InteractiveComponents.Where(c => c is SelectableTextComponent)
                        .Cast<SelectableTextComponent>();
                foreach (var item in otherItems)
                {
                    item.IsSelected = false;
                }

                textComponent.IsSelected = true;
            }
        }

        public void OnOpen()
        {
        }

        public event EventHandler Close = delegate { };

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