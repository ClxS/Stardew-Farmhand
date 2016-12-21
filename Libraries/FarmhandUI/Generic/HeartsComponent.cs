namespace Farmhand.UI.Generic
{
    using System;

    using Farmhand.UI.Base;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    ///     A hearts component.
    /// </summary>
    public class HeartsComponent : BaseMenuComponent
    {
        /// <summary>
        ///     Texture location of full heart sprite.
        /// </summary>
        protected static readonly Rectangle HeartFull = new Rectangle(211, 428, 7, 6);

        /// <summary>
        ///     Texture location of empty heart sprite.
        /// </summary>
        protected static readonly Rectangle HeartEmpty = new Rectangle(218, 428, 7, 6);
        
        private int value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HeartsComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of the component.
        /// </param>
        /// <param name="value">
        ///     The default selected value.
        /// </param>
        /// <param name="maxValue">
        ///     The maximum value.
        /// </param>
        public HeartsComponent(Point position, int value, int maxValue)
        {
            if (maxValue % 2 != 0)
            {
                maxValue++;
            }

            this.SetScaledArea(new Rectangle(position.X, position.Y, 8 * (maxValue / 2), HeartEmpty.Height));
            this.MaxValue = maxValue;
            this.Value = value;
        }

        /// <summary>
        ///     Gets or sets the max value.
        /// </summary>
        protected int MaxValue { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public int Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = Math.Min(Math.Max(0, value), this.MaxValue);
            }
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
            if (!this.Visible)
            {
                return;
            }

            for (var c = 0; c < this.MaxValue / 2; c++)
            {
                b.Draw(
                    Game1.mouseCursors,
                    new Vector2(o.X + this.Area.X + Game1.pixelZoom + c * Zoom8, o.Y + this.Area.Y),
                    new Rectangle(HeartEmpty.X, HeartEmpty.Y, 7, 6),
                    Color.White,
                    0,
                    Vector2.Zero,
                    Game1.pixelZoom,
                    SpriteEffects.None,
                    1f);
            }

            for (var c = 0; c < this.Value; c++)
            {
                b.Draw(
                    Game1.mouseCursors,
                    new Vector2(o.X + this.Area.X + Game1.pixelZoom + c * Zoom4, o.Y + this.Area.Y),
                    new Rectangle(HeartFull.X + (c % 2 == 0 ? 0 : 4), HeartFull.Y, c % 2 == 0 ? 4 : 3, 6),
                    Color.White,
                    0,
                    Vector2.Zero,
                    Game1.pixelZoom,
                    SpriteEffects.None,
                    1f);
            }
        }
    }
}