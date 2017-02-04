namespace ModTemplate.Controls
{
    using System;

    using Farmhand.UI.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    ///     A selectable text component.
    /// </summary>
    public class SelectableTextComponent : ClickableTextComponent
    {
        private bool isSelected;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SelectableTextComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of the component.
        /// </param>
        /// <param name="text">
        ///     The text to display.
        /// </param>
        /// <param name="handler">
        ///     The click event handler.
        /// </param>
        /// <param name="hoverEffect">
        ///     Whether hover effects should be used.
        /// </param>
        /// <param name="shadow">
        ///     Whether text shadow should be used.
        /// </param>
        /// <param name="scale">
        ///     The scale of the text.
        /// </param>
        /// <param name="color">
        ///     The color of the text.
        /// </param>
        /// <param name="font">
        ///     The font of the text.
        /// </param>
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

        /// <summary>
        ///     Gets or sets a value indicating whether the text is selected.
        /// </summary>
        /// <remarks>
        ///     This must be manually set by the control using this component. It does not automatically select itself.
        /// </remarks>
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                if (this.isSelected == value)
                {
                    return;
                }

                this.isSelected = value;
                if (this.isSelected)
                {
                    this.OnSelected();
                }
                else
                {
                    this.OnUnselected();
                }
            }
        }

        /// <summary>
        ///     Fires when the text is selected.
        /// </summary>
        public event EventHandler Selected = delegate { };

        /// <summary>
        ///     Fires when the text is unselected.
        /// </summary>
        public event EventHandler Unselected = delegate { };

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

            Utility.drawTextWithShadow(
                b,
                this.Text,
                this.Font,
                p,
                !this.IsSelected ? Color.DimGray : this.Color,
                this.Scale);
        }

        /// <summary>
        ///     Fires the Selected event.
        /// </summary>
        protected virtual void OnSelected()
        {
            this.Selected(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Fires the unselected event.
        /// </summary>
        protected virtual void OnUnselected()
        {
            this.Unselected(this, EventArgs.Empty);
        }
    }
}