namespace Farmhand.UI.Components.Controls
{
    using Farmhand.UI.Base;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    ///     A clickable animated component
    /// </summary>
    public class ClickableAnimatedComponent : BaseInteractiveMenuComponent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ClickableAnimatedComponent" /> class.
        /// </summary>
        /// <param name="area">
        ///     The bounding area of this component
        /// </param>
        /// <param name="sprite">
        ///     The sprite to display
        /// </param>
        /// <param name="handler">
        ///     The click event handler.
        /// </param>
        /// <param name="scaleOnHover">
        ///     Whether scale on hover is enabled.
        /// </param>
        public ClickableAnimatedComponent(
            Rectangle area,
            TemporaryAnimatedSprite sprite,
            ClickHandler handler = null,
            bool scaleOnHover = true)
        {
            if (handler != null)
            {
                this.Handler += handler;
            }

            this.ScaleOnHover = scaleOnHover;
            this.Sprite = sprite;
            this.SetScaledArea(area);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether to scale on hover.
        /// </summary>
        protected bool ScaleOnHover { get; set; }

        /// <summary>
        ///     Gets or sets the sprite to display.
        /// </summary>
        protected TemporaryAnimatedSprite Sprite { get; set; }

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
        ///     Called when the left mouse button is clicked over this component
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

        /// <summary>
        ///     The update handler for this component
        /// </summary>
        /// <param name="t">
        ///     The elapsed time since the previous update
        /// </param>
        public override void Update(GameTime t)
        {
            this.Sprite.update(t);
        }

        /// <summary>
        ///     The draw handler for this component
        /// </summary>
        /// <param name="b">
        ///     The sprite batch to use
        /// </param>
        /// <param name="offset">
        ///     The origin for this component
        /// </param>
        public override void Draw(SpriteBatch b, Point offset)
        {
            if (this.Visible)
            {
                this.Sprite.draw(b, false, offset.X + this.Area.X, offset.Y + this.Area.Y);
            }
        }
    }
}