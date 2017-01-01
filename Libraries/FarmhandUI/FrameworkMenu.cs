namespace Farmhand.UI
{
    // ReSharper disable StyleCop.SA1300
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Farmhand.UI.Interfaces;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using StardewValley;
    using StardewValley.Menus;

    /// <summary>
    ///     The framework menu.
    /// </summary>
    public class FrameworkMenu : IClickableMenu, IComponentCollection
    {
        /// <summary>
        ///     Texture location of the top-left menu background sprite.
        /// </summary>
        protected static readonly Rectangle Tl = new Rectangle(0, 0, 64, 64);

        /// <summary>
        ///     Texture location of the top-center menu background sprite.
        /// </summary>
        protected static readonly Rectangle Tc = new Rectangle(128, 0, 64, 64);

        /// <summary>
        ///     Texture location of the top-right menu background sprite.
        /// </summary>
        protected static readonly Rectangle Tr = new Rectangle(192, 0, 64, 64);

        /// <summary>
        ///     Texture location of the middle-left menu background sprite.
        /// </summary>
        protected static readonly Rectangle Ml = new Rectangle(0, 128, 64, 64);

        /// <summary>
        ///     Texture location of the middle-right menu background sprite.
        /// </summary>
        protected static readonly Rectangle Mr = new Rectangle(192, 128, 64, 64);

        /// <summary>
        ///     Texture location of the bottom-right menu background sprite.
        /// </summary>
        protected static readonly Rectangle Br = new Rectangle(192, 192, 64, 64);

        /// <summary>
        ///     Texture location of the bottom-left menu background sprite.
        /// </summary>
        protected static readonly Rectangle Bl = new Rectangle(0, 192, 64, 64);

        /// <summary>
        ///     Texture location of the bottom-center menu background sprite.
        /// </summary>
        protected static readonly Rectangle Bc = new Rectangle(128, 192, 64, 64);

        /// <summary>
        ///     Texture location of the menu background sprite.
        /// </summary>
        protected static readonly Rectangle Bg = new Rectangle(64, 128, 64, 64);

        /// <summary>
        ///     Zoom level 2 (pixelZoom * 2).
        /// </summary>
        protected static readonly int Zoom2 = Game1.pixelZoom * 2;

        /// <summary>
        ///     Zoom level 2 (pixelZoom * 2).
        /// </summary>
        protected static readonly int Zoom3 = Game1.pixelZoom * 3;

        /// <summary>
        ///     Zoom level 4 (pixelZoom * 4).
        /// </summary>
        protected static readonly int Zoom4 = Game1.pixelZoom * 4;

        /// <summary>
        ///     Zoom level 6 (pixelZoom * 6).
        /// </summary>
        protected static readonly int Zoom6 = Game1.pixelZoom * 6;

        /// <summary>
        ///     Zoom level 10 (pixelZoom * 10).
        /// </summary>
        protected static readonly int Zoom10 = Game1.pixelZoom * 10;

        /// <summary>
        ///     Zoom level 20 (pixelZoom * 20).
        /// </summary>
        protected static readonly int Zoom20 = Game1.pixelZoom * 20;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FrameworkMenu" /> class.
        /// </summary>
        /// <param name="area">
        ///     The bounding area of this component.
        /// </param>
        /// <param name="showCloseButton">
        ///     Whether the show close button should be displayed.
        /// </param>
        /// <param name="drawChrome">
        ///     Whether draw chrome is enabled.
        /// </param>
        public FrameworkMenu(Rectangle area, bool showCloseButton = true, bool drawChrome = true)
        {
            this.DrawChrome = drawChrome;
            this.Area = new Rectangle(
                area.X * Game1.pixelZoom,
                area.Y * Game1.pixelZoom,
                area.Width * Game1.pixelZoom,
                area.Height * Game1.pixelZoom);
            this.initialize(this.Area.X, this.Area.Y, this.Area.Width, this.Area.Height, showCloseButton);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FrameworkMenu" /> class.
        /// </summary>
        /// <param name="size">
        ///     The size of the menu.
        /// </param>
        /// <param name="showCloseButton">
        ///     Whether the show close button should be displayed.
        /// </param>
        /// <param name="drawChrome">
        ///     Whether draw chrome is enabled.
        /// </param>
        public FrameworkMenu(Point size, bool showCloseButton = true, bool drawChrome = true)
        {
            this.DrawChrome = drawChrome;
            this.Centered = true;
            var pos = Utility.getTopLeftPositionForCenteringOnScreen(size.X * Game1.pixelZoom, size.Y * Game1.pixelZoom);
            this.Area = new Rectangle((int)pos.X, (int)pos.Y, size.X * Game1.pixelZoom, size.Y * Game1.pixelZoom);
            this.initialize(this.Area.X, this.Area.Y, this.Area.Width, this.Area.Height, showCloseButton);
        }

        /// <summary>
        ///     Gets or sets the bounding area of this component.
        /// </summary>
        public Rectangle Area { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether centered.
        /// </summary>
        protected bool Centered { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to draw chrome.
        /// </summary>
        protected bool DrawChrome { get; set; }

        /// <summary>
        ///     Gets or sets an ordered collection that determines the draw order.
        /// </summary>
        protected List<IMenuComponent> DrawOrder { get; set; } = new List<IMenuComponent>();

        /// <summary>
        ///     Gets or sets an ordered collection that determines the event order.
        /// </summary>
        protected List<IInteractiveMenuComponent> EventOrder { get; set; } = new List<IInteractiveMenuComponent>();

        /// <summary>
        ///     Gets or sets the floating component.
        /// </summary>
        protected IInteractiveMenuComponent FloatingComponent { get; set; }

        /// <summary>
        ///     Gets or sets the focus element.
        /// </summary>
        protected IInteractiveMenuComponent FocusElement { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether mouse is held on this component.
        /// </summary>
        protected bool Hold { get; set; }

        /// <summary>
        ///     Gets or sets the hover in element.
        /// </summary>
        protected IInteractiveMenuComponent HoverInElement { get; set; }

        #region IComponentCollection Members

        /// <summary>
        ///     Gets or sets the static components.
        /// </summary>
        public List<IMenuComponent> StaticComponents { get; set; } = new List<IMenuComponent>();

        /// <summary>
        ///     Gets or sets the interactive components.
        /// </summary>
        public List<IInteractiveMenuComponent> InteractiveComponents { get; set; } =
            new List<IInteractiveMenuComponent>();

        /// <summary>
        ///     Gets the area in which events (such as mouse click/hover events) are captured.
        /// </summary>
        public virtual Rectangle EventRegion
            =>
                new Rectangle(
                    this.Area.X + Zoom10,
                    this.Area.Y + Zoom10,
                    this.Area.Width - Zoom20,
                    this.Area.Height - Zoom20);

        /// <summary>
        ///     Gets the zoomed event region.
        /// </summary>
        public virtual Rectangle ZoomEventRegion
            =>
                new Rectangle(
                    (this.Area.X + Zoom10) / Game1.pixelZoom,
                    (this.Area.Y + Zoom10) / Game1.pixelZoom,
                    (this.Area.Width - Zoom20) / Game1.pixelZoom,
                    (this.Area.Height - Zoom20) / Game1.pixelZoom);

        /// <summary>
        ///     Gets the menu attached to this container.
        /// </summary>
        /// <returns>
        ///     The attached menu <see cref="FrameworkMenu" />.
        /// </returns>
        public virtual FrameworkMenu GetAttachedMenu()
        {
            return this;
        }

        /// <summary>
        ///     Resets the current focus for this container.
        /// </summary>
        public virtual void ResetFocus()
        {
            if (this.FocusElement == null)
            {
                return;
            }

            this.FocusElement.FocusLost();
            if (this.FocusElement is IKeyboardComponent)
            {
                Game1.keyboardDispatcher.Subscriber.Selected = false;
                Game1.keyboardDispatcher.Subscriber = null;
            }

            this.FocusElement = null;
            if (this.FloatingComponent != null)
            {
                this.FloatingComponent.Detach(this);
                this.FloatingComponent = null;
            }
        }

        /// <summary>
        ///     Gives focus to a specified component.
        /// </summary>
        /// <param name="component">
        ///     The component to give focus.
        /// </param>
        public virtual void GiveFocus(IInteractiveMenuComponent component)
        {
            if (component == this.FocusElement)
            {
                return;
            }

            this.ResetFocus();
            this.FocusElement = component;
            var focusElement = this.FocusElement as IKeyboardComponent;
            if (focusElement != null)
            {
                Game1.keyboardDispatcher.Subscriber = new KeyboardSubscriberProxy(focusElement);
            }

            if (!this.InteractiveComponents.Contains(component))
            {
                this.FloatingComponent = component;
                component.Attach(this);
            }

            component.FocusGained();
        }

        /// <summary>
        ///     Adds a component to this collection
        /// </summary>
        /// <param name="component">
        ///     The component to add.
        /// </param>
        public virtual void AddComponent(IMenuComponent component)
        {
            var interactiveMenuComponent = component as IInteractiveMenuComponent;
            if (interactiveMenuComponent != null)
            {
                this.InteractiveComponents.Add(interactiveMenuComponent);
            }
            else
            {
                this.StaticComponents.Add(component);
            }

            component.Attach(this);
            this.UpdateDrawOrder();
        }

        /// <summary>
        ///     Removes a component from this collection
        /// </summary>
        /// <param name="component">
        ///     The component to remove.
        /// </param>
        public virtual void RemoveComponent(IMenuComponent component)
        {
            var removed = false;
            this.RemoveComponents(
                a =>
                    {
                        var b = a == component && !removed;
                        if (b)
                        {
                            removed = true;
                            a.Detach(this);
                        }

                        return b;
                    });
        }

        /// <summary>
        ///     Removes all components of a specific type from this collection.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of components to remove.
        /// </typeparam>
        public virtual void RemoveComponents<T>() where T : IMenuComponent
        {
            this.RemoveComponents(a => a.GetType() == typeof(T));
        }

        /// <summary>
        ///     Removes all components matching a filter from this collection.
        /// </summary>
        /// <param name="filter">
        ///     The filter to use.
        /// </param>
        public virtual void RemoveComponents(Predicate<IMenuComponent> filter)
        {
            this.InteractiveComponents.RemoveAll(
                a =>
                    {
                        var b = filter(a);
                        if (b)
                        {
                            a.Detach(this);
                        }

                        return b;
                    });
            this.StaticComponents.RemoveAll(
                a =>
                    {
                        var b = filter(a);
                        if (b)
                        {
                            a.Detach(this);
                        }

                        return b;
                    });
            this.UpdateDrawOrder();
        }

        /// <summary>
        ///     Removes all components from this collection
        /// </summary>
        public virtual void ClearComponents()
        {
            foreach (var component in this.InteractiveComponents)
            {
                component.Detach(this);
            }

            foreach (var component in this.StaticComponents)
            {
                component.Detach(this);
            }

            this.InteractiveComponents.Clear();
            this.StaticComponents.Clear();
            this.UpdateDrawOrder();
        }

        /// <summary>
        ///     Gets whether this collection can accept the provided component.
        /// </summary>
        /// <param name="component">
        ///     The component to check.
        /// </param>
        /// <returns>
        ///     Whether this collection accepts the component.
        /// </returns>
        public virtual bool AcceptsComponent(IMenuComponent component)
        {
            return true;
        }

        #endregion

        /// <summary>
        ///     Draws the menu background
        /// </summary>
        /// <param name="b">
        ///     The sprite batch to use.
        /// </param>
        /// <param name="x">
        ///     The x position of the frame.
        /// </param>
        /// <param name="y">
        ///     The y position of the frame.
        /// </param>
        /// <param name="width">
        ///     The width of the frame.
        /// </param>
        /// <param name="height">
        ///     The height of the frame.
        /// </param>
        public static void DrawMenuRect(SpriteBatch b, int x, int y, int width, int height)
        {
            var o = new Rectangle(x + Zoom2, y + Zoom2, width - Zoom4, height - Zoom4);
            b.Draw(Game1.menuTexture, new Rectangle(o.X, o.Y, o.Width, o.Height), Bg, Color.White);
            o = new Rectangle(x - Zoom3, y - Zoom3, width + Zoom6, height + Zoom6);
            b.Draw(Game1.menuTexture, new Rectangle(o.X, o.Y, 64, 64), Tl, Color.White);
            b.Draw(Game1.menuTexture, new Rectangle(o.X + o.Width - 64, o.Y, 64, 64), Tr, Color.White);
            b.Draw(Game1.menuTexture, new Rectangle(o.X + 64, o.Y, o.Width - 128, 64), Tc, Color.White);
            b.Draw(Game1.menuTexture, new Rectangle(o.X, o.Y + o.Height - 64, 64, 64), Bl, Color.White);
            b.Draw(Game1.menuTexture, new Rectangle(o.X + o.Width - 64, o.Y + o.Height - 64, 64, 64), Br, Color.White);
            b.Draw(Game1.menuTexture, new Rectangle(o.X + 64, o.Y + o.Height - 64, o.Width - 128, 64), Bc, Color.White);
            b.Draw(Game1.menuTexture, new Rectangle(o.X, o.Y + 64, 64, o.Height - 128), Ml, Color.White);
            b.Draw(Game1.menuTexture, new Rectangle(o.X + o.Width - 64, o.Y + 64, 64, o.Height - 128), Mr, Color.White);
        }

        /// <summary>
        ///     Gets the list of components ordered by draw order
        /// </summary>
        /// <param name="statics">
        ///     Static components.
        /// </param>
        /// <param name="interactives">
        ///     Interactive components.
        /// </param>
        /// <returns>
        ///     An KeyValuePair of ordered lists of the components.
        /// </returns>
        public static KeyValuePair<List<IInteractiveMenuComponent>, List<IMenuComponent>> GetOrderedLists(
            List<IMenuComponent> statics,
            List<IInteractiveMenuComponent> interactives)
        {
            var drawOrder = new List<IMenuComponent>(statics);
            drawOrder.AddRange(interactives);
            drawOrder =
                drawOrder.OrderByDescending(x => x.Layer)
                    .ThenByDescending(x => x.GetPosition().Y)
                    .ThenByDescending(x => x.GetPosition().X)
                    .ToList();
            var eventOrder =
                interactives.OrderBy(x => x.Layer)
                    .ThenBy(x => x.GetPosition().Y)
                    .ThenBy(x => x.GetPosition().X)
                    .ToList();
            return new KeyValuePair<List<IInteractiveMenuComponent>, List<IMenuComponent>>(eventOrder, drawOrder);
        }

        /// <summary>
        ///     Updates the draw and event order collections.
        /// </summary>
        protected virtual void UpdateDrawOrder()
        {
            var sorted = GetOrderedLists(this.StaticComponents, this.InteractiveComponents);
            this.DrawOrder = sorted.Value;
            this.EventOrder = sorted.Key;
        }

        /// <summary>
        ///     Called when game window size changed. (Game override)
        /// </summary>
        /// <param name="oldBounds">
        ///     The old bounds.
        /// </param>
        /// <param name="newBounds">
        ///     The new bounds.
        /// </param>
        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            if (!this.Centered)
            {
                return;
            }

            var pos = Utility.getTopLeftPositionForCenteringOnScreen(this.Area.Width, this.Area.Height);
            this.Area = new Rectangle((int)pos.X, (int)pos.Y, this.Area.Width, this.Area.Height);
        }

        /// <summary>
        ///     Called when left click is released. (Game override)
        /// </summary>
        /// <param name="x">
        ///     The x position.
        /// </param>
        /// <param name="y">
        ///     The y position.
        /// </param>
        public override void releaseLeftClick(int x, int y)
        {
            if (this.HoverInElement == null)
            {
                return;
            }

            var p = new Point(x, y);
            var o = new Point(this.Area.X + Zoom10, this.Area.Y + Zoom10);
            this.HoverInElement.LeftUp(p, o);
            this.Hold = false;
            if (this.HoverInElement.InBounds(p, o))
            {
                return;
            }

            this.HoverInElement.HoverOut(p, o);
            this.HoverInElement = null;
        }

        /// <summary>
        ///     Called when left click is held. (Game override)
        /// </summary>
        /// <param name="x">
        ///     The x position.
        /// </param>
        /// <param name="y">
        ///     The y position.
        /// </param>
        public override void leftClickHeld(int x, int y)
        {
            if (this.HoverInElement == null)
            {
                return;
            }

            this.Hold = true;
            this.HoverInElement.LeftHeld(new Point(x, y), new Point(this.Area.X + Zoom10, this.Area.Y + Zoom10));
        }

        /// <summary>
        ///     Called when left click is received. (Game override)
        /// </summary>
        /// <param name="x">
        ///     The x position.
        /// </param>
        /// <param name="y">
        ///     The y position.
        /// </param>
        /// <param name="playSound">
        ///     Whether the default right click sound should be played
        /// </param>
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);
            var p = new Point(x, y);
            var o = new Point(this.Area.X + Zoom10, this.Area.Y + Zoom10);
            if (this.FloatingComponent != null && this.FloatingComponent.InBounds(p, o))
            {
                this.FloatingComponent.LeftClick(p, o);
                return;
            }

            foreach (var el in this.EventOrder)
            {
                if (el.InBounds(p, o))
                {
                    this.GiveFocus(el);
                    el.LeftClick(p, o);
                    return;
                }
            }

            this.ResetFocus();
        }

        /// <summary>
        ///     Exits this menu
        /// </summary>
        public void ExitMenu()
        {
            this.exitThisMenu();
        }

        /// <summary>
        ///     Called when right click is received. (Game override)
        /// </summary>
        /// <param name="x">
        ///     The x position.
        /// </param>
        /// <param name="y">
        ///     The y position.
        /// </param>
        /// <param name="playSound">
        ///     Whether the default right click sound should be played
        /// </param>
        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            var p = new Point(x, y);
            var o = new Point(this.Area.X + Zoom10, this.Area.Y + Zoom10);
            if (this.FloatingComponent != null && this.FloatingComponent.InBounds(p, o))
            {
                this.FloatingComponent.RightClick(p, o);
                return;
            }

            foreach (var el in this.EventOrder)
            {
                if (el.InBounds(p, o))
                {
                    this.GiveFocus(el);
                    this.FocusElement = el;
                    el.RightClick(p, o);
                    return;
                }
            }

            this.ResetFocus();
        }

        /// <summary>
        ///     Called when hovered. (Game override)
        /// </summary>
        /// <param name="x">
        ///     The x position.
        /// </param>
        /// <param name="y">
        ///     The y position.
        /// </param>
        public override void performHoverAction(int x, int y)
        {
            base.performHoverAction(x, y);
            if (!this.Area.Contains(x, y) || this.Hold)
            {
                return;
            }

            var p = new Point(x, y);
            var o = new Point(this.Area.X + Zoom10, this.Area.Y + Zoom10);
            if (this.HoverInElement != null && !this.HoverInElement.InBounds(p, o))
            {
                this.HoverInElement.HoverOut(p, o);
                this.HoverInElement = null;
            }

            if (this.FloatingComponent != null && this.FloatingComponent.InBounds(p, o))
            {
                this.FloatingComponent.HoverOver(p, o);
                return;
            }

            foreach (var el in this.EventOrder)
            {
                if (el.InBounds(p, o))
                {
                    if (this.HoverInElement == null)
                    {
                        this.HoverInElement = el;
                        el.HoverIn(p, o);
                    }

                    el.HoverOver(p, o);
                    break;
                }
            }
        }

        /// <summary>
        ///     Called when a scroll event is received. (Game override)
        /// </summary>
        /// <param name="direction">
        ///     The direction of the scroll
        /// </param>
        public override void receiveScrollWheelAction(int direction)
        {
            base.receiveScrollWheelAction(direction);
            var p = Game1.getMousePosition();
            var o = new Point(this.Area.X + Zoom10, this.Area.Y + Zoom10);
            this.FloatingComponent?.Scroll(direction, p, o);
            foreach (var el in this.EventOrder)
            {
                el.Scroll(direction, p, o);
            }
        }

        /// <summary>
        ///     Called when a key press event is received. (Game override)
        /// </summary>
        /// <param name="key">
        ///     The pressed key
        /// </param>
        public override void receiveKeyPress(Keys key)
        {
            if (Game1.keyboardDispatcher.Subscriber == null)
            {
                base.receiveKeyPress(key);
            }
        }

        /// <summary>
        ///     Called on game update. (Game override)
        /// </summary>
        /// <param name="time">
        ///     The elapsed time since the previous frame
        /// </param>
        public override void update(GameTime time)
        {
            base.update(time);
            this.FloatingComponent?.Update(time);
            foreach (var el in this.DrawOrder)
            {
                el.Update(time);
            }
        }

        /// <summary>
        ///     Called on draw. (Game override)
        /// </summary>
        /// <param name="b">
        ///     The sprite batch to use for this draw.
        /// </param>
        public override void draw(SpriteBatch b)
        {
            if (this.DrawChrome)
            {
                DrawMenuRect(b, this.Area.X, this.Area.Y, this.Area.Width, this.Area.Height);
            }

            var o = new Point(this.Area.X + Zoom10, this.Area.Y + Zoom10);
            foreach (var el in this.DrawOrder)
            {
                el.Draw(b, o);
            }

            this.FloatingComponent?.Draw(b, o);
            base.draw(b);
            this.drawMouse(b);
        }
    }
}