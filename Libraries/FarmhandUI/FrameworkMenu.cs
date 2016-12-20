using System;
using System.Linq;
using System.Collections.Generic;
using Farmhand.UI.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using StardewValley;
using StardewValley.Menus;

namespace Farmhand.UI
{
    public class FrameworkMenu : IClickableMenu, IComponentCollection
    {
        protected List<IMenuComponent> DrawOrder=new List<IMenuComponent>();
        protected List<IInteractiveMenuComponent> EventOrder=new List<IInteractiveMenuComponent>();
        
        protected IInteractiveMenuComponent HoverInElement;
        protected IInteractiveMenuComponent FocusElement;
        protected IInteractiveMenuComponent FloatingComponent;

        protected bool Hold;

        protected bool DrawChrome;
        protected bool Centered;

        public Rectangle Area;

        public List<IMenuComponent> StaticComponents { get; set; } = new List<IMenuComponent>();
        public List<IInteractiveMenuComponent> InteractiveComponents { get; set; } = new List<IInteractiveMenuComponent>();



        protected static readonly Rectangle Tl = new Rectangle(0, 0, 64, 64);
        protected static readonly Rectangle Tc = new Rectangle(128, 0, 64, 64);
        protected static readonly Rectangle Tr = new Rectangle(192, 0, 64, 64);
        protected static readonly Rectangle Ml = new Rectangle(0, 128, 64, 64);
        protected static readonly Rectangle Mr = new Rectangle(192, 128, 64, 64);
        protected static readonly Rectangle Br = new Rectangle(192, 192, 64, 64);
        protected static readonly Rectangle Bl = new Rectangle(0, 192, 64, 64);
        protected static readonly Rectangle Bc = new Rectangle(128, 192, 64, 64);
        protected static readonly Rectangle Bg = new Rectangle(64, 128, 64, 64);
        protected static readonly int Zoom2 = Game1.pixelZoom * 2;
        protected static readonly int Zoom3 = Game1.pixelZoom * 3;
        protected static readonly int Zoom4 = Game1.pixelZoom * 4;
        protected static readonly int Zoom6 = Game1.pixelZoom * 6;
        protected static readonly int Zoom10 = Game1.pixelZoom * 10;
        protected static readonly int Zoom20 = Game1.pixelZoom * 20;

        public virtual Rectangle EventRegion => new Rectangle(Area.X + Zoom10, Area.Y + Zoom10, Area.Width - Zoom20, Area.Height - Zoom20);

        public virtual Rectangle ZoomEventRegion => new Rectangle((Area.X + Zoom10) / Game1.pixelZoom, (Area.Y + Zoom10) / Game1.pixelZoom, (Area.Width - Zoom20) / Game1.pixelZoom, (Area.Height - Zoom20) / Game1.pixelZoom);


        public static void DrawMenuRect(SpriteBatch b, int x, int y, int width, int height)
        {
            Rectangle o = new Rectangle(x + Zoom2, y + Zoom2, width - Zoom4, height - Zoom4);
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
        public static KeyValuePair<List<IInteractiveMenuComponent>,List<IMenuComponent>> GetOrderedLists(List<IMenuComponent> statics, List<IInteractiveMenuComponent> interactives)
        {
            List<IMenuComponent> drawOrder = new List<IMenuComponent>(statics);
            drawOrder.AddRange(interactives);
            drawOrder = drawOrder.OrderByDescending(x => x.Layer).ThenByDescending(x => x.GetPosition().Y).ThenByDescending(x => x.GetPosition().X).ToList();
            List<IInteractiveMenuComponent>  eventOrder = interactives.OrderBy(x => x.Layer).ThenBy(x => x.GetPosition().Y).ThenBy(x => x.GetPosition().X).ToList();
            return new KeyValuePair<List<IInteractiveMenuComponent>, List<IMenuComponent>>(eventOrder, drawOrder);
        }
        public FrameworkMenu(Rectangle area, bool showCloseButton = true, bool drawChrome = true)
        {
            DrawChrome = drawChrome;
            Area = new Rectangle(area.X * Game1.pixelZoom, area.Y * Game1.pixelZoom, area.Width * Game1.pixelZoom, area.Height * Game1.pixelZoom);
            initialize(Area.X, Area.Y, Area.Width, Area.Height, showCloseButton);
        }
        public FrameworkMenu(Point size, bool showCloseButton = true, bool drawChrome = true)
        {
            DrawChrome = drawChrome;
            Centered = true;
            Vector2 pos = Utility.getTopLeftPositionForCenteringOnScreen(size.X * Game1.pixelZoom, size.Y * Game1.pixelZoom);
            Area = new Rectangle((int)pos.X, (int)pos.Y, size.X * Game1.pixelZoom, size.Y * Game1.pixelZoom);
            initialize(Area.X, Area.Y, Area.Width, Area.Height, showCloseButton);
        }
        protected virtual void UpdateDrawOrder()
        {
            KeyValuePair<List<IInteractiveMenuComponent>, List<IMenuComponent>> sorted = GetOrderedLists(StaticComponents, InteractiveComponents);
            DrawOrder = sorted.Value;
            EventOrder = sorted.Key;
        }
        public virtual FrameworkMenu GetAttachedMenu()
        {
            return this;
        }
        public virtual void ResetFocus()
        {
            if (FocusElement == null)
                return;
            FocusElement.FocusLost();
            if(FocusElement is IKeyboardComponent)
            {
                Game1.keyboardDispatcher.Subscriber.Selected = false;
                Game1.keyboardDispatcher.Subscriber = null;
            }
            FocusElement = null;
            if (FloatingComponent != null)
            {
                FloatingComponent.Detach(this);
                FloatingComponent = null;
            }
        }
        public virtual void GiveFocus(IInteractiveMenuComponent component)
        {
            if (component == FocusElement)
                return;
            ResetFocus();
            FocusElement = component;
            var focusElement = FocusElement as IKeyboardComponent;
            if(focusElement != null)
                Game1.keyboardDispatcher.Subscriber = new KeyboardSubscriberProxy(focusElement);
            if (!InteractiveComponents.Contains(component))
            {
                FloatingComponent = component;
                component.Attach(this);
            }
            component.FocusGained();
        }
        public virtual void AddComponent(IMenuComponent component)
        {
            var interactiveMenuComponent = component as IInteractiveMenuComponent;
            if (interactiveMenuComponent != null)
                InteractiveComponents.Add(interactiveMenuComponent);
            else
                StaticComponents.Add(component);
            component.Attach(this);
            UpdateDrawOrder();
        }
        public virtual void RemoveComponent(IMenuComponent component)
        {
            bool removed = false;
            RemoveComponents(a => { bool b = a == component && !removed; if (b) { removed = true; a.Detach(this); } return b; });
        }
        public virtual void RemoveComponents<T>() where T : IMenuComponent
        {
            RemoveComponents(a => a.GetType() == typeof(T));
        }
        public virtual void RemoveComponents(Predicate<IMenuComponent> filter)
        {
            InteractiveComponents.RemoveAll(a => { bool b = filter(a); if (b) a.Detach(this); return b; });
            StaticComponents.RemoveAll(a => { bool b = filter(a); if (b) a.Detach(this); return b; });
            UpdateDrawOrder();
        }
        public virtual void ClearComponents()
        {
            foreach (var component in InteractiveComponents)
            {
                component.Detach(this);
            }
            foreach (var component in StaticComponents)
            {
                component.Detach(this);
            }
            InteractiveComponents.Clear();
            StaticComponents.Clear();
            UpdateDrawOrder();
        }
        public virtual bool AcceptsComponent(IMenuComponent component)
        {
            return true;
        }
        
        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            if (!Centered)
                return;
            Vector2 pos = Utility.getTopLeftPositionForCenteringOnScreen(Area.Width, Area.Height);
            Area = new Rectangle((int)pos.X, (int)pos.Y, Area.Width, Area.Height);
        }
        public override void releaseLeftClick(int x, int y)
        {
            if (HoverInElement == null)
                return;
            Point p = new Point(x, y);
            Point o = new Point(Area.X + Zoom10, Area.Y + Zoom10);
            HoverInElement.LeftUp(p, o);
            Hold = false;
            if (HoverInElement.InBounds(p, o))
                return;
            HoverInElement.HoverOut(p, o);
            HoverInElement = null;
        }
        public override void leftClickHeld(int x, int y)
        {
            if (HoverInElement == null)
                return;
            Hold = true;
            HoverInElement.LeftHeld(new Point(x, y), new Point(Area.X + Zoom10, Area.Y + Zoom10));
        }
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);
            Point p = new Point(x, y);
            Point o = new Point(Area.X + Zoom10, Area.Y + Zoom10);
            if(FloatingComponent != null && FloatingComponent.InBounds(p,o))
            {
                FloatingComponent.LeftClick(p, o);
                return;
            }
            foreach (IInteractiveMenuComponent el in EventOrder)
            {
                if (el.InBounds(p, o))
                {
                    GiveFocus(el);
                    el.LeftClick(p, o);
                    return;
                }
            }
            ResetFocus();
        }
        public void ExitMenu()
        {
            exitThisMenu();
        }
        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            Point p = new Point(x, y);
            Point o = new Point(Area.X + Zoom10, Area.Y + Zoom10);
            if (FloatingComponent != null && FloatingComponent.InBounds(p, o))
            {
                FloatingComponent.RightClick(p, o);
                return;
            }
            foreach (IInteractiveMenuComponent el in EventOrder)
            {
                if (el.InBounds(p, o))
                {
                    GiveFocus(el);
                    FocusElement = el;
                    el.RightClick(p, o);
                    return;
                }
            }
            ResetFocus();
        }
        public override void performHoverAction(int x, int y)
        {
            base.performHoverAction(x, y);
            if (!Area.Contains(x, y) || Hold)
                return;
            Point p = new Point(x, y);
            Point o = new Point(Area.X + Zoom10, Area.Y + Zoom10);
            if (HoverInElement != null && !HoverInElement.InBounds(p, o))
            {
                HoverInElement.HoverOut(p, o);
                HoverInElement = null;
            }
            if (FloatingComponent!=null && FloatingComponent.InBounds(p, o))
            {
                FloatingComponent.HoverOver(p, o);
                return;
            }
            foreach (IInteractiveMenuComponent el in EventOrder)
            {
                if (el.InBounds(p, o))
                {
                    if (HoverInElement == null)
                    {
                        HoverInElement = el;
                        el.HoverIn(p, o);
                    }
                    el.HoverOver(p, o);
                    break;
                }
            }
        }
        public override void receiveScrollWheelAction(int direction)
        {
            base.receiveScrollWheelAction(direction);
            Point p = Game1.getMousePosition();
            Point o = new Point(Area.X + Zoom10, Area.Y + Zoom10);
            FloatingComponent?.Scroll(direction, p, o);
            foreach (IInteractiveMenuComponent el in EventOrder)
                el.Scroll(direction, p, o);
        }
        public override void receiveKeyPress(Keys key)
        {
            if (Game1.keyboardDispatcher.Subscriber == null)
                base.receiveKeyPress(key);
        }
        public override void update(GameTime time)
        {
            base.update(time);
            FloatingComponent?.Update(time);
            foreach (IMenuComponent el in DrawOrder)
                el.Update(time);
        }
        public override void draw(SpriteBatch b)
        {
            if (DrawChrome)
                DrawMenuRect(b, Area.X, Area.Y, Area.Width, Area.Height);
            Point o = new Point(Area.X + Zoom10, Area.Y + Zoom10);
            foreach (IMenuComponent el in DrawOrder)
                el.Draw(b, o);
            FloatingComponent?.Draw(b, o);
            base.draw(b);
            drawMouse(b);
        }
    }
}
