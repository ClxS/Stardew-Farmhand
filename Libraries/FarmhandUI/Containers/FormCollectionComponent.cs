namespace Farmhand.UI.Containers
{
    using System;
    using System.Collections.Generic;

    using Farmhand.UI.Base;
    using Farmhand.UI.Interfaces;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    /// The form collection component.
    /// </summary>
    public class FormCollectionComponent : BaseFormComponent, IComponentCollection
    {
        /// <summary>
        /// Gets or sets the static components.
        /// </summary>
        public List<IMenuComponent> StaticComponents { get; set; } = new List<IMenuComponent>();

        /// <summary>
        /// Gets or sets the interactive components.
        /// </summary>
        public List<IInteractiveMenuComponent> InteractiveComponents { get; set; } = new List<IInteractiveMenuComponent>();

        /// <summary>
        /// Gets or sets if this component is disabled
        /// </summary>
        public override bool Disabled
        {
            get
            {
                return base.Disabled;
            }

            set
            {
                base.Disabled = value;
                foreach (var c in this.InteractiveComponents)
                {
                    var component = c as BaseFormComponent;
                    if (component != null)
                    {
                        component.Disabled = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the event region.
        /// </summary>
        public Rectangle EventRegion => this.Area;

        /// <summary>
        /// Gets the zoom event region.
        /// </summary>
        public Rectangle ZoomEventRegion => new Rectangle(this.Area.X / Game1.pixelZoom, this.Area.Y / Game1.pixelZoom, this.Area.Width / Game1.pixelZoom, this.Area.Height / Game1.pixelZoom);

        /// <summary>
        /// Gets or sets the hover element.
        /// </summary>
        protected IInteractiveMenuComponent HoverElement { get; set; }

        /// <summary>
        /// Gets or sets the focus element.
        /// </summary>
        protected IInteractiveMenuComponent FocusElement { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the mouse is being held on this component.
        /// </summary>
        protected bool Hold { get; set; }

        /// <summary>
        /// Gets or sets an ordered collection specifying the component draw order
        /// </summary>
        protected List<IMenuComponent> DrawOrder { get; set; }

        /// <summary>
        /// Gets or sets an ordered collection specifying the component event order
        /// </summary>
        protected List<IInteractiveMenuComponent> EventOrder { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormCollectionComponent"/> class.
        /// </summary>
        protected FormCollectionComponent()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormCollectionComponent"/> class.
        /// </summary>
        /// <param name="area">
        /// The bounding area of this component
        /// </param>
        /// <param name="components">
        /// The child components for this component
        /// </param>
        public FormCollectionComponent(Rectangle area, List<IMenuComponent> components = null)
        {
            if (components != null)
            {
                foreach (var c in components)
                {
                    this.AddComponent(c);
                }
            }

            this.SetScaledArea(area);
        }

        /// <summary>
        /// Gets the attached menu.
        /// </summary>
        /// <returns>
        /// The attached menu. <see cref="FrameworkMenu"/>.
        /// </returns>
        public FrameworkMenu GetAttachedMenu()
        {
            return this.Parent.GetAttachedMenu();
        }

        /// <summary>
        /// Resets the components focus
        /// </summary>
        public void ResetFocus()
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
        }

        /// <summary>
        /// Gives focus to the specified component
        /// </summary>
        /// <param name="component">
        /// The component to give focus to
        /// </param>
        public void GiveFocus(IInteractiveMenuComponent component)
        {
            if (!this.InteractiveComponents.Contains(component) || component == this.FocusElement)
            {
                return;
            }

            this.Parent.GiveFocus(this);
            this.ResetFocus();
            this.FocusElement = component;
            var element = this.FocusElement as IKeyboardComponent;
            if (element != null)
            {
                Game1.keyboardDispatcher.Subscriber = new KeyboardSubscriberProxy(element);
            }

            component.FocusGained();
        }

        /// <summary>
        /// Adds a component to this collection
        /// </summary>
        /// <param name="component">
        /// The component to add
        /// </param>
        public void AddComponent(IMenuComponent component)
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
        /// Removes a component from this collection
        /// </summary>
        /// <param name="component">
        /// The component to remove
        /// </param>
        public void RemoveComponent(IMenuComponent component)
        {
            var removed = false;
            this.RemoveComponents(
                a =>
                    {
                        if (a != component || removed)
                        {
                            return false;
                        }

                        removed = true;
                        a.Detach(this);
                        return true;
                    });
        }

        /// <summary>
        /// Remove components of type T
        /// </summary>
        /// <typeparam name="T">
        /// The type of components to remove
        /// </typeparam>
        public void RemoveComponents<T>() where T : IMenuComponent
        {
            this.RemoveComponents(a => a.GetType() == typeof(T));
        }

        /// <summary>
        /// Remove components matching a filter
        /// </summary>
        /// <param name="filter">
        /// The filter to use to remove
        /// </param>
        public void RemoveComponents(Predicate<IMenuComponent> filter)
        {
            this.InteractiveComponents.RemoveAll(
                a =>
                    {
                        if (filter(a))
                        {
                            a.Detach(this);
                            return true;
                        }

                        return false;
                    });
            this.StaticComponents.RemoveAll(
                a =>
                    {
                        if (filter(a))
                        {
                            a.Detach(this);
                            return true;
                        }

                        return false;
                    });
            this.UpdateDrawOrder();
        }

        /// <summary>
        /// Removes all components
        /// </summary>
        public void ClearComponents()
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
        /// Whether this components accepts this type of component
        /// </summary>
        /// <param name="component">
        /// The component you want to check
        /// </param>
        /// <returns>
        /// Whether the provided component is accepted by this component <see cref="bool"/>.
        /// </returns>
        public bool AcceptsComponent(IMenuComponent component)
        {
            return true;
        }
        
        /// <summary>
        /// Called when focus is lost
        /// </summary>
        public override void FocusLost()
        {
            this.ResetFocus();
        }

        /// <summary>
        /// Called when the left mouse button is released over this component
        /// </summary>
        /// <param name="p">
        /// The mouse position
        /// </param>
        /// <param name="o">
        /// The origin of this component
        /// </param>
        public override void LeftUp(Point p, Point o)
        {
            if (!this.Visible)
            {
                return;
            }

            if (this.HoverElement == null)
            {
                return;
            }

            var o2 = new Point(this.Area.X + o.X, this.Area.Y + o.Y);
            this.HoverElement.LeftUp(p, o2);
            this.Hold = false;
            if (this.HoverElement.InBounds(p, o2))
            {
                return;
            }

            this.HoverElement.HoverOut(p, o2);
            this.HoverElement = null;
        }

        /// <summary>
        /// Called when the left mouse button is held over this component
        /// </summary>
        /// <param name="p">
        /// The mouse position
        /// </param>
        /// <param name="o">
        /// The origin of this component
        /// </param>
        public override void LeftHeld(Point p, Point o)
        {
            if (!this.Visible)
            {
                return;
            }

            if (this.HoverElement == null)
            {
                return;
            }

            this.Hold = true;
            this.HoverElement.LeftHeld(p, new Point(this.Area.X + o.X, this.Area.Y + o.Y));
        }

        /// <summary>
        /// Called when the left mouse button is clicked over this component
        /// </summary>
        /// <param name="p">
        /// The mouse position
        /// </param>
        /// <param name="o">
        /// The origin of this component
        /// </param>
        public override void LeftClick(Point p, Point o)
        {
            if (!this.Visible)
            {
                return;
            }

            var o2 = new Point(this.Area.X + o.X, this.Area.Y + o.Y);
            foreach (var el in this.EventOrder)
            {
                if (el.InBounds(p, o2))
                {
                    this.GiveFocus(el);
                    el.LeftClick(p, o2);
                    return;
                }
            }

            this.ResetFocus();
        }

        /// <summary>
        /// Called when the right mouse button is clicked over this component
        /// </summary>
        /// <param name="p">
        /// The mouse position
        /// </param>
        /// <param name="o">
        /// The origin of this component
        /// </param>
        public override void RightClick(Point p, Point o)
        {
            if (!this.Visible)
            {
                return;
            }

            var o2 = new Point(this.Area.X + o.X, this.Area.Y + o.Y);
            foreach (var el in this.EventOrder)
            {
                if (el.InBounds(p, o2))
                {
                    this.GiveFocus(el);
                    this.FocusElement = el;
                    el.RightClick(p, o2);
                    return;
                }
            }

            this.ResetFocus();
        }

        /// <summary>
        /// Called when the mouse is hovered over this component
        /// </summary>
        /// <param name="p">
        /// The mouse position
        /// </param>
        /// <param name="o">
        /// The origin of this component
        /// </param>
        public override void HoverOver(Point p, Point o)
        {
            if (!this.Visible || this.Hold)
            {
                return;
            }

            var o2 = new Point(this.Area.X + o.X, this.Area.Y + o.Y);
            if (this.HoverElement != null && !this.HoverElement.InBounds(p, o2))
            {
                this.HoverElement.HoverOut(p, o2);
                this.HoverElement = null;
            }

            foreach (var el in this.EventOrder)
            {
                if (el.InBounds(p, o2))
                {
                    if (this.HoverElement == null)
                    {
                        this.HoverElement = el;
                        el.HoverIn(p, o2);
                    }

                    el.HoverOver(p, o2);
                    break;
                }
            }
        }

        /// <summary>
        /// Called when the scroll wheel is used over this component
        /// </summary>
        /// <param name="d">
        /// The delta value of the scroll
        /// </param>
        /// <param name="p">
        /// The mouse position
        /// </param>
        /// <param name="o">
        /// The origin of this component
        /// </param>
        public override void Scroll(int d, Point p, Point o)
        {
            if (!this.Visible)
            {
                return;
            }

            var o2 = new Point(this.Area.X + o.X, this.Area.Y + o.Y);
            foreach (var el in this.EventOrder)
            {
                el.Scroll(d, p, o2);
            }
        }

        /// <summary>
        /// The update loop for this component
        /// </summary>
        /// <param name="t">
        /// The elapsed time since the previous update
        /// </param>
        public override void Update(GameTime t)
        {
            if (!this.Visible)
            {
                return;
            }

            foreach (var el in this.DrawOrder)
            {
                el.Update(t);
            }
        }

        /// <summary>
        /// The draw loop for this component
        /// </summary>
        /// <param name="b">
        /// The sprite batch to use
        /// </param>
        /// <param name="o">
        /// The origin for this component
        /// </param>
        public override void Draw(SpriteBatch b, Point o)
        {
            if (!this.Visible)
            {
                return;
            }

            var o2 = new Point(this.Area.X + o.X, this.Area.Y + o.Y);
            foreach (var el in this.DrawOrder)
            {
                el.Draw(b, o2);
            }
        }

        /// <summary>
        /// Updates the component draw order.
        /// </summary>
        protected void UpdateDrawOrder()
        {
            var sorted = FrameworkMenu.GetOrderedLists(this.StaticComponents, this.InteractiveComponents);
            this.DrawOrder = sorted.Value;
            this.EventOrder = sorted.Key;
        }
    }
}
