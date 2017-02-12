namespace Farmhand.UI.Components.Controls
{
    using System;

    using Farmhand.UI.Base;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    ///     A text component.
    /// </summary>
    public class TextComponent : BaseMenuComponent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TextComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of the component.
        /// </param>
        /// <param name="text">
        ///     The text.
        /// </param>
        /// <param name="shadow">
        ///     Whether text shadow is enabled.
        /// </param>
        /// <param name="scale">
        ///     The text scale.
        /// </param>
        /// <param name="color">
        ///     The text color.
        /// </param>
        /// <param name="font">
        ///     The text font.
        /// </param>
        public TextComponent(
            Point position,
            string text,
            bool shadow = true,
            float scale = 1,
            Color? color = null,
            SpriteFont font = null)
        {
            if (color == null)
            {
                this.Color = Game1.textColor;
            }
            else
            {
                this.Color = (Color)color;
            }

            this.Font = font ?? Game1.smallFont;
            this.Shadow = shadow;
            this.Scale = scale;
            this.Text = text;
            var size = this.Font.MeasureString(this.Text) / Game1.pixelZoom * this.Scale;
            this.SetScaledArea(
                new Rectangle(position.X, position.Y, (int)Math.Ceiling(size.X), (int)Math.Ceiling(size.Y)));
        }

        /// <summary>
        ///     Gets or sets the text color.
        /// </summary>
        protected Color Color { get; set; }

        /// <summary>
        ///     Gets or sets the text font.
        /// </summary>
        protected SpriteFont Font { get; set; }

        /// <summary>
        ///     Gets or sets the text scale.
        /// </summary>
        protected float Scale { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to use text shadow.
        /// </summary>
        protected bool Shadow { get; set; }

        /// <summary>
        ///     Gets or sets the text.
        /// </summary>
        protected string Text { get; set; }

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
            if (!this.Visible)
            {
                return;
            }

            var p = new Vector2(this.Area.X + o.X, this.Area.Y + o.Y);
            if (this.Shadow)
            {
                Utility.drawTextWithShadow(b, this.Text, this.Font, p, this.Color, this.Scale);
            }
            else
            {
                b.DrawString(
                    this.Font,
                    this.Text,
                    p,
                    this.Color,
                    0,
                    Vector2.Zero,
                    Game1.pixelZoom * this.Scale,
                    SpriteEffects.None,
                    1);
            }
        }
    }
}