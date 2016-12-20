using System;
using System.Collections.Generic;
using Farmhand.UI.Base;
using Farmhand.UI.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Farmhand.UI.Containers
{
    public class GenericCollectionComponent : BaseInteractiveMenuComponent, IComponentCollection
    {
        protected List<IMenuComponent> DrawOrder;
        protected List<IInteractiveMenuComponent> EventOrder;
        
        protected IInteractiveMenuComponent HoverElement;
        protected IInteractiveMenuComponent FocusElement;
        protected bool Hold;
        
        public List<IMenuComponent> StaticComponents { get; set; } = new List<IMenuComponent>();
        public List<IInteractiveMenuComponent> InteractiveComponents { get; set; } = new List<IInteractiveMenuComponent>();

        public Rectangle EventRegion => Area;

        public Rectangle ZoomEventRegion => new Rectangle(Area.X / Game1.pixelZoom, Area.Y / Game1.pixelZoom, Area.Width / Game1.pixelZoom, Area.Height / Game1.pixelZoom);

        protected GenericCollectionComponent()
        {

        }
        public GenericCollectionComponent(Rectangle area, List<IMenuComponent> components=null)
        {
            if (components != null)
                foreach (IMenuComponent c in components)
                    AddComponent(c);
            SetScaledArea(area);
        }
        // IComponentCollection
        protected virtual void UpdateDrawOrder()
        {
            KeyValuePair<List<IInteractiveMenuComponent>, List<IMenuComponent>> sorted = FrameworkMenu.GetOrderedLists(StaticComponents, InteractiveComponents);
            DrawOrder = sorted.Value;
            EventOrder = sorted.Key;
        }
        public FrameworkMenu GetAttachedMenu()
        {
            return Parent.GetAttachedMenu();
        }
        public void ResetFocus()
        {
            if (FocusElement == null)
                return;
            FocusElement.FocusLost();
            if (FocusElement is IKeyboardComponent)
            {
                Game1.keyboardDispatcher.Subscriber.Selected = false;
                Game1.keyboardDispatcher.Subscriber = null;
            }
            FocusElement = null;
        }
        public void GiveFocus(IInteractiveMenuComponent component)
        {
            if (!InteractiveComponents.Contains(component) || component == FocusElement)
                return;
            Parent.GiveFocus(this);
            ResetFocus();
            FocusElement = component;
            var element = FocusElement as IKeyboardComponent;
            if (element != null)
                Game1.keyboardDispatcher.Subscriber = new KeyboardSubscriberProxy(element);
            component.FocusGained();
        }
        public void AddComponent(IMenuComponent component)
        {
            var interactiveMenuComponent = component as IInteractiveMenuComponent;
            if (interactiveMenuComponent != null)
                InteractiveComponents.Add(interactiveMenuComponent);
            else
                StaticComponents.Add(component);
            component.Attach(this);
            UpdateDrawOrder();
        }
        public void RemoveComponent(IMenuComponent component)
        {
            bool removed = false;
            RemoveComponents(a => { bool b = a == component && !removed; if (b) { removed = true; a.Detach(this); } return b; });
        }
        public void RemoveComponents<T>() where T : IMenuComponent
        {
            RemoveComponents(a => a.GetType() == typeof(T));
        }
        public void RemoveComponents(Predicate<IMenuComponent> filter)
        {
            InteractiveComponents.RemoveAll(a => { bool b = filter(a);if (b)a.Detach(this); return b; });
            StaticComponents.RemoveAll(a => { bool b = filter(a); if (b) a.Detach(this); return b; });
            UpdateDrawOrder();
        }
        public void ClearComponents()
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
        public bool AcceptsComponent(IMenuComponent component)
        {
            return true;
        }
        
        // IInteractiveMenuComponent
        public override void FocusLost()
        {
            ResetFocus();
        }
        public override void LeftUp(Point p, Point o)
        {
            if (!Visible)
                return;
            if (HoverElement == null)
                return;
            Point o2 = new Point(Area.X + o.X, Area.Y+o.Y);
            HoverElement.LeftUp(p, o2);
            Hold = false;
            if (HoverElement.InBounds(p, o2))
                return;
            HoverElement.HoverOut(p, o2);
            HoverElement = null;
        }
        public override void LeftHeld(Point p, Point o)
        {
            if (!Visible)
                return;
            if (HoverElement == null)
                return;
            Hold = true;
            HoverElement.LeftHeld(p, new Point(Area.X + o.X, Area.Y + o.Y));
        }
        public override void LeftClick(Point p, Point o)
        {
            if (!Visible)
                return;
            Point o2 = new Point(Area.X + o.X, Area.Y + o.Y);
            foreach (IInteractiveMenuComponent el in EventOrder)
            {
                if (el.InBounds(p, o2))
                {
                    GiveFocus(el);
                    el.LeftClick(p, o2);
                    return;
                }
            }
            ResetFocus();
        }
        public override void RightClick(Point p, Point o)
        {
            if (!Visible)
                return;
            Point o2 = new Point(Area.X + o.X, Area.Y + o.Y);
            foreach (IInteractiveMenuComponent el in EventOrder)
            {
                if (el.InBounds(p, o2))
                {
                    GiveFocus(el);
                    FocusElement = el;
                    el.RightClick(p, o2);
                    return;
                }
            }
            ResetFocus();
        }
        public override void HoverOver(Point p, Point o)
        {
            if (!Visible || Hold)
                return;
            Point o2 = new Point(Area.X + o.X, Area.Y + o.Y);
            if (HoverElement != null && !HoverElement.InBounds(p, o2))
            {
                HoverElement.HoverOut(p, o2);
                HoverElement = null;
            }
            foreach (IInteractiveMenuComponent el in EventOrder)
            {
                if (el.InBounds(p, o2))
                {
                    if (HoverElement == null)
                    {
                        HoverElement = el;
                        el.HoverIn(p, o2);
                    }
                    el.HoverOver(p, o2);
                    break;
                }
            }
        }
        public override void Scroll(int d, Point p, Point o)
        {
            if (!Visible)
                return;
            Point o2 = new Point(Area.X + o.X, Area.Y + o.Y);
            foreach (IInteractiveMenuComponent el in EventOrder)
                el.Scroll(d, p, o2);
        }
        public override void Update(GameTime t)
        {
            if (!Visible)
                return;
            foreach(IMenuComponent el in DrawOrder)
                el.Update(t);
        }
        public override void Draw(SpriteBatch b, Point o)
        {
            if (!Visible)
                return;
            Point o2 = new Point(Area.X + o.X, Area.Y + o.Y);
            foreach(IMenuComponent el in DrawOrder)
                el.Draw(b, o2);
        }
    }
}
