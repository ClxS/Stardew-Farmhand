namespace Farmhand.UI.Components.Controls
{
    using Farmhand.UI.Base;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    ///     A label component.
    /// </summary>
    public class LabelComponent : BaseMenuComponent
    {
        /// <summary>
        ///     Texture location of left side of label.
        /// </summary>
        protected static readonly Rectangle Left = new Rectangle(256, 267, 6, 16);

        /// <summary>
        ///     Texture location of right side of label.
        /// </summary>
        protected static readonly Rectangle Right = new Rectangle(263, 267, 6, 16);

        /// <summary>
        ///     Texture location of center of label.
        /// </summary>
        protected static readonly Rectangle Center = new Rectangle(262, 267, 1, 16);

        /// <summary>
        ///     Initializes a new instance of the <see cref="LabelComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of this component.
        /// </param>
        /// <param name="label">
        ///     The label text.
        /// </param>
        public LabelComponent(Point position, string label)
        {
            this.SetScaledArea(
                new Rectangle(position.X, position.Y, this.GetStringWidth(label, Game1.smallFont) + 12, 16));
            this.Label = label;
        }

        /// <summary>
        ///     Gets or sets the label text.
        /// </summary>
        protected string Label { get; set; }

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
            // Left
            b.Draw(
                Game1.mouseCursors,
                new Rectangle(o.X + this.Area.X, o.Y + this.Area.Y, Zoom6, this.Area.Height),
                Left,
                Color.White);

            // Right
            b.Draw(
                Game1.mouseCursors,
                new Rectangle(o.X + this.Area.X + this.Area.Width - Zoom6, o.Y + this.Area.Y, Zoom6, this.Area.Height),
                Right,
                Color.White);

            // Center
            b.Draw(
                Game1.mouseCursors,
                new Rectangle(o.X + this.Area.X + Zoom6, o.Y + this.Area.Y, this.Area.Width - Zoom12, this.Area.Height),
                Center,
                Color.White);

            // Label
            Utility.drawTextWithShadow(
                b,
                this.Label,
                Game1.smallFont,
                new Vector2(o.X + this.Area.X + Zoom6, o.Y + this.Area.Y + Zoom5),
                Game1.textColor);
        }
    }
}