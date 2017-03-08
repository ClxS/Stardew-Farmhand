namespace Farmhand.UI.Components.Controls
{
    using Farmhand.UI.Base;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    /// Component to draw a colored rectangle.
    /// </summary>
    public class ColoredRectangleComponent : BaseMenuComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColoredRectangleComponent"/> class.
        /// </summary>
        /// <param name="area">
        /// The rectangle area.
        /// </param>
        /// <param name="color">
        /// The color to display.
        /// </param>
        public ColoredRectangleComponent(Rectangle area, Color color)
        {
            this.SetScaledArea(area);
            this.Color = color;
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Draws the component.
        /// </summary>
        /// <param name="b">
        /// The sprite batch to use.
        /// </param>
        /// <param name="o">
        /// The origin offset.
        /// </param>
        public override void Draw(SpriteBatch b, Point o)
        {
            b.Draw(
                Game1.staminaRect,
                new Rectangle(this.Area.X + o.X, this.Area.Y + o.Y, this.Area.Width, this.Area.Height),
                this.Color);
        }
    }
}