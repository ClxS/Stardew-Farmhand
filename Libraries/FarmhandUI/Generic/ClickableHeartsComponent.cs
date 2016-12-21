namespace Farmhand.UI.Generic
{
    using System;

    using Farmhand.UI.Base;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    ///     A clickable hearts component.
    /// </summary>
    public class ClickableHeartsComponent : BaseInteractiveMenuComponent
    {
        /// <summary>
        ///     Texture location for full heart sprite
        /// </summary>
        protected static readonly Rectangle HeartFull = new Rectangle(211, 428, 7, 6);

        /// <summary>
        ///     Texture location for empty heart sprite
        /// </summary>
        protected static readonly Rectangle HeartEmpty = new Rectangle(218, 428, 7, 6);

        /// <summary>
        ///     The number of selected hearts
        /// </summary>
        private int value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClickableHeartsComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of this component
        /// </param>
        /// <param name="value">
        ///     The default number of hearts selected
        /// </param>
        /// <param name="maxValue">
        ///     The max value.
        /// </param>
        /// <param name="handler">
        ///     The value changed handler.
        /// </param>
        public ClickableHeartsComponent(Point position, int value, int maxValue, ValueChanged<int> handler = null)
        {
            if (maxValue % 2 != 0)
            {
                maxValue++;
            }

            this.SetScaledArea(new Rectangle(position.X, position.Y, 8 * (maxValue / 2), HeartEmpty.Height));
            this.MaxValue = maxValue;
            this.Value = value;
            this.OldValue = this.Value;
            if (handler != null)
            {
                this.Handler += handler;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether hovered.
        /// </summary>
        protected bool Hovered { get; set; }

        /// <summary>
        ///     Gets or sets the max value.
        /// </summary>
        protected int MaxValue { get; set; }

        /// <summary>
        ///     Gets or sets the old value.
        /// </summary>
        protected int OldValue { get; set; }

        /// <summary>
        ///     Gets or sets the number of selected hearts.
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
        ///     The value changed handler.
        /// </summary>
        public event ValueChanged<int> Handler;

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
            this.Hovered = true;
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
            this.Hovered = false;
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
        public override void LeftUp(Point p, Point o)
        {
            this.Value = (int)Math.Round((p.X - (this.Area.X + o.X)) / 4D / Game1.pixelZoom);
            if (this.OldValue == this.Value)
            {
                return;
            }

            this.OldValue = this.Value;
            this.Handler?.Invoke(this, this.Parent, this.Parent.GetAttachedMenu(), this.Value);
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
                    Color.White * (this.Hovered ? 0.5f : 1),
                    0,
                    Vector2.Zero,
                    Game1.pixelZoom,
                    SpriteEffects.None,
                    1f);
            }

            if (!this.Hovered)
            {
                return;
            }

            var currentValue = Math.Min(
                this.MaxValue,
                (int)Math.Round((Game1.getMouseX() - (this.Area.X + o.X)) / 4D / Game1.pixelZoom));
            for (var c = 0; c < currentValue; c++)
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