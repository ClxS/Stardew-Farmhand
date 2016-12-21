namespace Farmhand.UI.Generic
{
    using Farmhand.UI.Base;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    /// Animated component.
    /// </summary>
    public class AnimatedComponent : BaseMenuComponent
    {
        /// <summary>
        /// Gets or sets the sprite to display.
        /// </summary>
        protected TemporaryAnimatedSprite Sprite { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedComponent"/> class.
        /// </summary>
        /// <param name="position">
        /// The position of this component
        /// </param>
        /// <param name="sprite">
        /// The animated sprite.
        /// </param>
        public AnimatedComponent(Point position, TemporaryAnimatedSprite sprite)
        {
            this.SetScaledArea(new Rectangle(position.X, position.Y, sprite.sourceRect.Width, sprite.sourceRect.Height));
            this.Sprite = sprite;
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
        /// <param name="o">
        ///     The origin for this component
        /// </param>
        public override void Draw(SpriteBatch b, Point o)
        {
            if (this.Visible)
            {
                this.Sprite.draw(b, false, o.X + this.Area.X, o.Y + this.Area.Y);
            }
        }
    }
}