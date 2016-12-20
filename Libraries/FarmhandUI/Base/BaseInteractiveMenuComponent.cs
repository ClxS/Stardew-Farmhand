namespace Farmhand.UI.Base
{
    using Farmhand.UI.Interfaces;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The base component for interactive menus
    /// </summary>
    public abstract class BaseInteractiveMenuComponent : BaseMenuComponent, IInteractiveMenuComponent
    {
        /// <summary>
        /// A delegate used for notifying when a value has changed
        /// </summary>
        /// <param name="component">
        /// The component in which the value changed
        /// </param>
        /// <param name="collection">
        /// The collection in which the value changed
        /// </param>
        /// <param name="menu">
        /// The menu in which the value changed
        /// </param>
        /// <param name="value">
        /// The new value
        /// </param>
        /// <typeparam name="T">
        /// The type of the parameter that changed
        /// </typeparam>
        public delegate void ValueChanged<in T>(IInteractiveMenuComponent component, IComponentContainer collection, FrameworkMenu menu, T value);

        /// <summary>
        /// A delegate used for notifying on click events
        /// </summary>
        /// <param name="component">
        /// The component in which the click occurred
        /// </param>
        /// <param name="collection">
        /// The collection in which the click occurred
        /// </param>
        /// <param name="menu">
        /// The menu in which the click occurred
        /// </param>
        public delegate void ClickHandler(IInteractiveMenuComponent component, IComponentContainer collection, FrameworkMenu menu);

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseInteractiveMenuComponent"/> class.
        /// </summary>
        protected BaseInteractiveMenuComponent()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseInteractiveMenuComponent"/> class.
        /// </summary>
        /// <param name="area">
        /// The bounds of this menu
        /// </param>
        /// <param name="texture">
        /// The backing texture for this menu
        /// </param>
        /// <param name="crop">
        /// The crop area of this menu, defaults to null
        /// </param>
        protected BaseInteractiveMenuComponent(Rectangle area, Texture2D texture, Rectangle? crop = null)
            : base(area, texture, crop)
        {
        }

        /// <summary>
        /// Checks whether a certain point is within the bounds of this component.
        /// If the component is not visible, this will always return false.
        /// </summary>
        /// <param name="p">
        /// The point in which to check
        /// </param>
        /// <param name="o">
        /// The origin of the component
        /// </param>
        /// <returns>
        /// The whether the point was within the bounds of the component
        /// </returns>
        public virtual bool InBounds(Point p, Point o)
        {
            return this.Visible
                   && new Rectangle(this.Area.X + o.X, this.Area.Y + o.Y, this.Area.Width, this.Area.Height).Contains(p);
        }

        /// <summary>
        /// Called when a right click is performed within the bounds of this component
        /// </summary>
        /// <param name="p">
        /// The point the click occurred
        /// </param>
        /// <param name="o">
        /// The origin of the component
        /// </param>
        public virtual void RightClick(Point p, Point o)
        {
        }

        /// <summary>
        /// Called when a left click is performed within the bounds of this component
        /// </summary>
        /// <param name="p">
        /// The point the click occurred
        /// </param>
        /// <param name="o">
        /// The origin of the component
        /// </param>
        public virtual void LeftClick(Point p, Point o)
        {
        }

        /// <summary>
        /// Called when a left click is held within the bounds of this component
        /// </summary>
        /// <param name="p">
        /// The point the click occurred
        /// </param>
        /// <param name="o">
        /// The origin of the component
        /// </param>
        public virtual void LeftHeld(Point p, Point o)
        {
        }

        /// <summary>
        /// Called when a left up is performed within the bounds of this component
        /// </summary>
        /// <param name="p">
        /// The point the click occurred
        /// </param>
        /// <param name="o">
        /// The origin of the component
        /// </param>
        public virtual void LeftUp(Point p, Point o)
        {
        }

        /// <summary>
        /// Called when the mouse enters this component
        /// </summary>
        /// <param name="p">
        /// The mouse location
        /// </param>
        /// <param name="o">
        /// The origin of the component
        /// </param>
        public virtual void HoverIn(Point p, Point o)
        {
        }

        /// <summary>
        /// Called when the mouse leaves this component
        /// </summary>
        /// <param name="p">
        /// The mouse location
        /// </param>
        /// <param name="o">
        /// The origin of the component
        /// </param>
        public virtual void HoverOut(Point p, Point o)
        {
        }

        /// <summary>
        /// Called when the mouse hovers over this component
        /// </summary>
        /// <param name="p">
        /// The mouse location
        /// </param>
        /// <param name="o">
        /// The origin of the component
        /// </param>
        public virtual void HoverOver(Point p, Point o)
        {
        }

        /// <summary>
        /// Called when the focus is lost on this component
        /// </summary>
        public virtual void FocusLost()
        {
        }

        /// <summary>
        /// Called when the focus is gained by this component
        /// </summary>
        public virtual void FocusGained()
        {
        }

        /// <summary>
        /// Called when the scroll wheel is used on this component
        /// </summary>
        /// <param name="d">
        /// The delta value of the scroll
        /// </param>
        /// <param name="p">
        /// The mouse position at the time of the scroll
        /// </param>
        /// <param name="o">
        /// The origin of this component
        /// </param>
        public virtual void Scroll(int d, Point p, Point o)
        {
        }
    }
}
