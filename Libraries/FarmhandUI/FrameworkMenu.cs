using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

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

        protected List<IMenuComponent> _StaticComponents = new List<IMenuComponent>();
        protected List<IInteractiveMenuComponent> _InteractiveComponents = new List<IInteractiveMenuComponent>();

        protected IInteractiveMenuComponent HoverInElement=null;
        protected IInteractiveMenuComponent FocusElement=null;
        protected IInteractiveMenuComponent FloatingComponent = null;

        protected bool Hold;

        protected bool DrawChrome;
        protected bool Centered = false;

        public Rectangle Area;

        public List<IMenuComponent> StaticComponents { get { return new List<IMenuComponent>(_StaticComponents); } }
        public List<IInteractiveMenuComponent> InteractiveComponents { get { return new List<IInteractiveMenuComponent>(_InteractiveComponents); } }
        protected readonly static Rectangle tl = new Rectangle(0, 0, 64, 64);
        protected readonly static Rectangle tc = new Rectangle(128, 0, 64, 64);
        protected readonly static Rectangle tr = new Rectangle(192, 0, 64, 64);
        protected readonly static Rectangle ml = new Rectangle(0, 128, 64, 64);
        protected readonly static Rectangle mr = new Rectangle(192, 128, 64, 64);
        protected readonly static Rectangle br = new Rectangle(192, 192, 64, 64);
        protected readonly static Rectangle bl = new Rectangle(0, 192, 64, 64);
        protected readonly static Rectangle bc = new Rectangle(128, 192, 64, 64);
        protected readonly static Rectangle bg = new Rectangle(64, 128, 64, 64);
        protected readonly static int zoom2 = Game1.pixelZoom * 2;
        protected readonly static int zoom3 = Game1.pixelZoom * 3;
        protected readonly static int zoom4 = Game1.pixelZoom * 4;
        protected readonly static int zoom6 = Game1.pixelZoom * 6;
        protected readonly static int zoom10 = Game1.pixelZoom * 10;
        protected readonly static int zoom20 = Game1.pixelZoom * 20;
        public static void DrawMenuRect(SpriteBatch b, int x, int y, int width, int height)
        {
            Rectangle o = new Rectangle(x + zoom2, y + zoom2, width - zoom4, height - zoom4);
            b.Draw(Game1.menuTexture, new Rectangle(o.X, o.Y, o.Width, o.Height), bg, Color.White);
            o = new Rectangle(x - zoom3, y - zoom3, width + zoom6, height + zoom6);
            b.Draw(Game1.menuTexture, new Rectangle(o.X, o.Y, 64, 64), tl, Color.White);
            b.Draw(Game1.menuTexture, new Rectangle(o.X + o.Width - 64, o.Y, 64, 64), tr, Color.White);
            b.Draw(Game1.menuTexture, new Rectangle(o.X + 64, o.Y, o.Width - 128, 64), tc, Color.White);
            b.Draw(Game1.menuTexture, new Rectangle(o.X, o.Y + o.Height - 64, 64, 64), bl, Color.White);
            b.Draw(Game1.menuTexture, new Rectangle(o.X + o.Width - 64, o.Y + o.Height - 64, 64, 64), br, Color.White);
            b.Draw(Game1.menuTexture, new Rectangle(o.X + 64, o.Y + o.Height - 64, o.Width - 128, 64), bc, Color.White);
            b.Draw(Game1.menuTexture, new Rectangle(o.X, o.Y + 64, 64, o.Height - 128), ml, Color.White);
            b.Draw(Game1.menuTexture, new Rectangle(o.X + o.Width - 64, o.Y + 64, 64, o.Height - 128), mr, Color.White);
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
            Vector2 pos = Utility.getTopLeftPositionForCenteringOnScreen(size.X * Game1.pixelZoom, size.Y * Game1.pixelZoom, 0, 0);
            Area = new Rectangle((int)pos.X, (int)pos.Y, size.X * Game1.pixelZoom, size.Y * Game1.pixelZoom);
            initialize(Area.X, Area.Y, Area.Width, Area.Height, showCloseButton);
        }
        protected virtual void UpdateDrawOrder()
        {
            KeyValuePair<List<IInteractiveMenuComponent>, List<IMenuComponent>> sorted = GetOrderedLists(_StaticComponents, _InteractiveComponents);
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
            if(FocusElement is IKeyboardComponent)
                Game1.keyboardDispatcher.Subscriber = new KeyboardSubscriberProxy((IKeyboardComponent)FocusElement);
            if (!_InteractiveComponents.Contains(component))
            {
                FloatingComponent = component;
                component.Attach(this);
            }
            component.FocusGained();
        }
        public virtual void AddComponent(IMenuComponent component)
        {
            if (component is IInteractiveMenuComponent)
                _InteractiveComponents.Add(component as IInteractiveMenuComponent);
            else
                _StaticComponents.Add(component);
            component.Attach(this);
            UpdateDrawOrder();
        }
        public virtual void RemoveComponent(IMenuComponent component)
        {
            bool Removed = false;
            RemoveComponents(a => { bool b = a == component && !Removed; if (b) { Removed = true; a.Detach(this); } return b; });
        }
        public virtual void RemoveComponents<T>() where T : IMenuComponent
        {
            RemoveComponents(a => a.GetType() == typeof(T));
        }
        public virtual void RemoveComponents(Predicate<IMenuComponent> filter)
        {
            _InteractiveComponents.RemoveAll(a => { bool b = filter(a); if (b) a.Detach(this); return b; });
            _StaticComponents.RemoveAll(a => { bool b = filter(a); if (b) a.Detach(this); return b; });
            UpdateDrawOrder();
        }
        public virtual void ClearComponents()
        {
            _InteractiveComponents.TrueForAll(a => { a.Detach(this); return true; });
            _StaticComponents.TrueForAll(a => { a.Detach(this); return true; });
            _InteractiveComponents.Clear();
            _StaticComponents.Clear();
            UpdateDrawOrder();
        }
        public virtual bool AcceptsComponent(IMenuComponent component)
        {
            return true;
        }
        public virtual Rectangle EventRegion
        {
            get { return new Rectangle(Area.X + zoom10, Area.Y + zoom10, Area.Width - zoom20, Area.Height - zoom20); }
        }
        public virtual Rectangle ZoomEventRegion
        {
            get { return new Rectangle((Area.X + zoom10)/Game1.pixelZoom,(Area.Y + zoom10)/Game1.pixelZoom,(Area.Width - zoom20)/Game1.pixelZoom,(Area.Height - zoom20)/Game1.pixelZoom); }
        }
        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            if (!Centered)
                return;
            Vector2 pos = Utility.getTopLeftPositionForCenteringOnScreen(Area.Width, Area.Height, 0, 0);
            Area = new Rectangle((int)pos.X, (int)pos.Y, Area.Width, Area.Height);
        }
        public override void releaseLeftClick(int x, int y)
        {
            if (HoverInElement == null)
                return;
            Point p = new Point(x, y);
            Point o = new Point(Area.X + zoom10, Area.Y + zoom10);
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
            HoverInElement.LeftHeld(new Point(x, y), new Point(Area.X + zoom10, Area.Y + zoom10));
        }
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);
            Point p = new Point(x, y);
            Point o = new Point(Area.X + zoom10, Area.Y + zoom10);
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
            exitThisMenu(true);
        }
        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            Point p = new Point(x, y);
            Point o = new Point(Area.X + zoom10, Area.Y + zoom10);
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
            Point o = new Point(Area.X + zoom10, Area.Y + zoom10);
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
            Point o = new Point(Area.X + zoom10, Area.Y + zoom10);
            if (FloatingComponent!=null)
                FloatingComponent.Scroll(direction, p, o);
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
            if (FloatingComponent != null)
                FloatingComponent.Update(time);
            foreach (IMenuComponent el in DrawOrder)
                el.Update(time);
        }
        public override void draw(SpriteBatch b)
        {
            if (DrawChrome)
                DrawMenuRect(b, Area.X, Area.Y, Area.Width, Area.Height);
            Point o = new Point(Area.X + zoom10, Area.Y + zoom10);
            foreach (IMenuComponent el in DrawOrder)
                el.Draw(b, o);
            if (FloatingComponent != null)
                FloatingComponent.Draw(b, o);
            base.draw(b);
            drawMouse(b);
        }
    }
}
