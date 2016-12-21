namespace Farmhand.UI.Generic
{
    using Farmhand.UI.Base;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    ///     A clickable texture component.
    /// </summary>
    public class ClickableTextureComponent : BaseInteractiveMenuComponent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ClickableTextureComponent" /> class.
        /// </summary>
        /// <param name="area">
        ///     The bounding area of this component.
        /// </param>
        /// <param name="texture">
        ///     The texture to use.
        /// </param>
        /// <param name="handler">
        ///     The click event handler.
        /// </param>
        /// <param name="crop">
        ///     The cropped area.
        /// </param>
        /// <param name="scaleOnHover">
        ///     Whether scale on hover is enabled.
        /// </param>
        public ClickableTextureComponent(
            Rectangle area,
            Texture2D texture,
            ClickHandler handler = null,
            Rectangle? crop = null,
            bool scaleOnHover = true)
            : base(area, texture, crop)
        {
            if (handler != null)
            {
                this.Handler += handler;
            }

            this.ScaleOnHover = scaleOnHover;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether to scale on hover.
        /// </summary>
        protected bool ScaleOnHover { get; set; }

        /// <summary>
        ///     The click event handler.
        /// </summary>
        public event ClickHandler Handler;

        /// <summary>
        ///     Called when the mouse enters this component
        /// </summary>
        /// <param name="p">
        ///     The mouse position
        /// </param>
        /// <param name="o">
        ///     The origin of the component
        /// </param>
        public override void HoverIn(Point p, Point o)
        {
            Game1.playSound("Cowboy_Footstep");
            if (!this.ScaleOnHover)
            {
                return;
            }

            this.InflateRegion(2, 2);
        }

        /// <summary>
        ///     Called when the mouse leaves this component
        /// </summary>
        /// <param name="p">
        ///     The mouse position
        /// </param>
        /// <param name="o">
        ///     The origin of the component
        /// </param>
        public override void HoverOut(Point p, Point o)
        {
            if (!this.ScaleOnHover)
            {
                return;
            }

            this.InflateRegion(-2, -2);
        }

        /// <summary>
        ///     Called when the left mouse button is released over this component
        /// </summary>
        /// <param name="p">
        ///     The mouse position
        /// </param>
        /// <param name="o">
        ///     The origin of this component
        /// </param>
        public override void LeftClick(Point p, Point o)
        {
            Game1.playSound("bigDeSelect");
            this.Handler?.Invoke(this, this.Parent, this.Parent.GetAttachedMenu());
        }
    }
}