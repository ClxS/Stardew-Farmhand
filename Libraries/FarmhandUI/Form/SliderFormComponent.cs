namespace Farmhand.UI.Form
{
    // ReSharper disable StaticMemberInGenericType
    using System;
    using System.Collections.Generic;

    using Farmhand.UI.Base;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;
    using StardewValley.Menus;

    /// <summary>
    ///     A generic slider component. This receives a list of discrete values
    ///     which each index (where max index == number of values) represents
    /// </summary>
    /// <typeparam name="T">
    ///     The type of value represented by the slider
    /// </typeparam>
    public class SliderFormComponent<T> : BaseFormComponent
    {
        /// <summary>
        ///     Texture location of the slider background
        /// </summary>
        protected static readonly Rectangle Background = new Rectangle(403, 383, 6, 6);

        /// <summary>
        ///     Texture location of the slider button
        /// </summary>
        protected static readonly Rectangle Button = new Rectangle(420, 441, 10, 6);

        /// <summary>
        ///     The value.
        /// </summary>
        private T value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SliderFormComponent{T}" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of this component
        /// </param>
        /// <param name="values">
        ///     The values of the slider
        /// </param>
        /// <param name="handler">
        ///     The value changed handler
        /// </param>
        public SliderFormComponent(Point position, List<T> values, ValueChanged<T> handler = null)
            : this(position, 100, values, handler)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SliderFormComponent{T}" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of this component
        /// </param>
        /// <param name="width">
        ///     The width of this component
        /// </param>
        /// <param name="values">
        ///     The values of the slider
        /// </param>
        /// <param name="handler">
        ///     The value changed handler
        /// </param>
        public SliderFormComponent(Point position, int width, List<T> values, ValueChanged<T> handler = null)
        {
            this.Offset = (int)Math.Round((width - 10) * Game1.pixelZoom / (values.Count - 1D));
            this.SetScaledArea(new Rectangle(position.X, position.Y, width, 6));
            this.Values = values;
            this.Value = values[0];
            this.Index = 0;
            this.OldIndex = 0;
            if (handler != null)
            {
                this.Handler += handler;
            }
        }

        /// <summary>
        ///     Gets or sets the value changed handler.
        /// </summary>
        protected ValueChanged<T> Handler { get; set; }

        /// <summary>
        ///     Gets or sets the index.
        /// </summary>
        protected int Index { get; set; }

        /// <summary>
        ///     Gets or sets the offset.
        /// </summary>
        protected int Offset { get; set; }

        /// <summary>
        ///     Gets or sets the old index.
        /// </summary>
        protected int OldIndex { get; set; }

        /// <summary>
        ///     Gets or sets the option key.
        /// </summary>
        protected int OptionKey { get; set; }

        /// <summary>
        ///     Gets or sets the values.
        /// </summary>
        protected List<T> Values { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public T Value
        {
            get
            {
                return this.value;
            }

            set
            {
                var i = this.Values.IndexOf(value);
                if (i == -1)
                {
                    return;
                }

                this.Index = i;
                this.value = value;
            }
        }

        /// <summary>
        ///     Called when the left mouse button is held over this component
        /// </summary>
        /// <param name="p">
        ///     The mouse position
        /// </param>
        /// <param name="o">
        ///     The origin of this component
        /// </param>
        public override void LeftHeld(Point p, Point o)
        {
            if (this.Disabled)
            {
                return;
            }

            this.Index =
                Math.Max(
                    Math.Min((int)Math.Floor((double)(p.X - (o.X + this.Area.X)) / this.Offset), this.Values.Count - 1),
                    0);
            this.Value = this.Values[this.Index];
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
            if (this.OldIndex == this.Index)
            {
                return;
            }

            this.OldIndex = this.Index;
            this.Handler?.Invoke(this, this.Parent, this.Parent.GetAttachedMenu(), this.Value);
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
            this.LeftHeld(p, o);
        }

        /// <summary>
        ///     The draw loop for this component
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

            IClickableMenu.drawTextureBox(
                b,
                Game1.mouseCursors,
                Background,
                o.X + this.Area.X,
                o.Y + this.Area.Y,
                this.Area.Width,
                this.Area.Height,
                Color.White * (this.Disabled ? 0.33f : 1),
                Game1.pixelZoom,
                false);
            b.Draw(
                Game1.mouseCursors,
                new Vector2(
                    o.X + this.Area.X
                    + (this.Index == this.Values.Count - 1 ? this.Area.Width - Zoom10 : this.Index * this.Offset),
                    o.Y + this.Area.Y),
                Button,
                Color.White * (this.Disabled ? 0.33f : 1),
                0.0f,
                Vector2.Zero,
                Game1.pixelZoom,
                SpriteEffects.None,
                0.9f);
        }
    }
}