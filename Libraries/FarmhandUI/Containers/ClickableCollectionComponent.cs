namespace Farmhand.UI.Containers
{
    using System.Collections.Generic;

    using Farmhand.UI.Interfaces;

    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The clickable collection component.
    /// </summary>
    public class ClickableCollectionComponent : GenericCollectionComponent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ClickableCollectionComponent" /> class.
        /// </summary>
        /// <param name="area">
        ///     The bounding area of this component
        /// </param>
        /// <param name="handler">
        ///     The click event handler
        /// </param>
        /// <param name="components">
        ///     The components to attach to this component
        /// </param>
        public ClickableCollectionComponent(
            Rectangle area,
            ClickHandler handler = null,
            List<IMenuComponent> components = null)
            : base(area, components)
        {
            if (handler != null)
            {
                this.Handler += handler;
            }
        }

        /// <summary>
        ///     The handler for the click event
        /// </summary>
        public event ClickHandler Handler;

        /// <summary>
        ///     Called when the left button is clicked on this component
        /// </summary>
        /// <param name="p">
        ///     The mouse position of the click
        /// </param>
        /// <param name="o">
        ///     The origin of this component
        /// </param>
        public override void LeftClick(Point p, Point o)
        {
            this.Handler?.Invoke(this, this.Parent, this.Parent.GetAttachedMenu());
        }
    }
}