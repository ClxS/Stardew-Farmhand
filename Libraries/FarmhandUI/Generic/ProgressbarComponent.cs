namespace Farmhand.UI.Generic
{
    using System;

    using Farmhand.UI.Base;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;
    using StardewValley.Menus;

    /// <summary>
    ///     A progress bar component.
    /// </summary>
    public class ProgressbarComponent : BaseMenuComponent
    {
        /// <summary>
        ///     Texture location of background sprite
        /// </summary>
        protected static readonly Rectangle Background = new Rectangle(403, 383, 6, 6);

        /// <summary>
        ///     Texture location of filler sprite
        /// </summary>
        protected static readonly Rectangle Filler = new Rectangle(306, 320, 16, 16);

        private int value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProgressbarComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of the component.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="maxValue">
        ///     The max value.
        /// </param>
        public ProgressbarComponent(Point position, int value, int maxValue)
        {
            this.MaxValue = maxValue;
            this.Value = value;
            this.OffsetValue = this.Value * Game1.pixelZoom;
            this.SetScaledArea(new Rectangle(position.X, position.Y, this.MaxValue + 2, 6));
        }

        /// <summary>
        ///     Gets or sets the max value.
        /// </summary>
        protected int MaxValue { get; set; }

        /// <summary>
        ///     Gets or sets the offset value.
        /// </summary>
        protected int OffsetValue { get; set; }

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
                this.value = Math.Max(0, Math.Min(this.MaxValue, value));
            }
        }

        /// <summary>
        ///     Gets the difference between the value and the offset value
        /// </summary>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        protected int GetDiff()
        {
            var v = this.value * Game1.pixelZoom;
            if (this.OffsetValue == v)
            {
                return 0;
            }

            if (this.OffsetValue > v)
            {
                return -(int)Math.Floor((this.OffsetValue - v) / 10D + 1);
            }

            return (int)Math.Floor((v - this.OffsetValue) / 10D + 1);
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
            if (DateTime.Now.Millisecond % 5 == 0)
            {
                this.OffsetValue += this.GetDiff();
            }

            IClickableMenu.drawTextureBox(
                b,
                Game1.mouseCursors,
                Background,
                this.Area.X + o.X,
                this.Area.Y + o.Y,
                this.Area.Width,
                this.Area.Height,
                Color.White,
                Game1.pixelZoom,
                false);
            b.Draw(
                Game1.mouseCursors,
                new Rectangle(
                    this.Area.X + o.X + Game1.pixelZoom,
                    this.Area.Y + o.Y + Game1.pixelZoom,
                    this.OffsetValue,
                    Zoom4),
                Filler,
                Color.White);
        }
    }
}