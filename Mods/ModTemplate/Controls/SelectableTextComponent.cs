namespace ModTemplate.Controls
{
    using Farmhand.UI.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    internal class SelectableTextComponent : ClickableTextComponent
    {
        public SelectableTextComponent(
            Point position,
            string text,
            ClickHandler handler = null,
            bool hoverEffect = true,
            bool shadow = true,
            float scale = 1,
            Color? color = null,
            SpriteFont font = null)
            : base(position, text, handler, hoverEffect, shadow, scale, color, font)
        {
        }

        public bool IsSelected { get; set; }

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
            if (!this.IsSelected)
            {
                Utility.drawTextWithShadow(
                    b,
                    this.Text,
                    this.Font,
                    p,
                    Color.DimGray,
                    this.Scale);
            }
            else
            {
                Utility.drawTextWithShadow(
                    b,
                    this.Text,
                    this.Font,
                    p,
                    this.Color,
                    this.Scale);
            }
        }
    }
}