namespace Farmhand.UI.Form
{
    using System;

    using Farmhand.UI.Base;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    ///     Component for Buttons
    /// </summary>
    public class ButtonFormComponent : BaseFormComponent
    {
        /// <summary>
        ///     Texture location for a normal button
        /// </summary>
        protected static readonly Rectangle ButtonNormal = new Rectangle(256, 256, 10, 10);

        /// <summary>
        ///     Texture location for hovered button
        /// </summary>
        protected static readonly Rectangle ButtonHover = new Rectangle(267, 256, 10, 10);

        /// <summary>
        ///     Initializes a new instance of the <see cref="ButtonFormComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The button's position.
        /// </param>
        /// <param name="label">
        ///     The button's label.
        /// </param>
        /// <param name="handler">
        ///     The button's click handler
        /// </param>
        public ButtonFormComponent(Point position, string label, ClickHandler handler = null)
            : this(position, 50, label, handler)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ButtonFormComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The button's position.
        /// </param>
        /// <param name="width">
        ///     The button's width.
        /// </param>
        /// <param name="label">
        ///     The button's label.
        /// </param>
        /// <param name="handler">
        ///     The button's click handler.
        /// </param>
        public ButtonFormComponent(Point position, int width, string label, ClickHandler handler = null)
        {
            var labelWidth = this.GetStringWidth(label, Game1.smallFont);
            width = Math.Max(width, labelWidth + 4);
            this.LabelOffset = (int)Math.Round((width - labelWidth) / 2D);
            this.SetScaledArea(new Rectangle(position.X, position.Y, width, 10));
            this.Label = label;
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
        ///     Gets or sets the label.
        /// </summary>
        protected string Label { get; set; }

        /// <summary>
        ///     Gets or sets the label offset.
        /// </summary>
        protected int LabelOffset { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether pressed.
        /// </summary>
        protected bool Pressed { get; set; }

        /// <summary>
        ///     The click event handler.
        /// </summary>
        public event ClickHandler Handler;

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
            if (!this.Disabled)
            {
                this.Pressed = true;
            }
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
            if (!this.Disabled)
            {
                this.Pressed = false;
            }
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
            if (this.Disabled)
            {
                return;
            }

            Game1.playSound("bigDeSelect");
            this.Handler?.Invoke(this, this.Parent, this.Parent.GetAttachedMenu());
        }

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
            if (this.Disabled)
            {
                return;
            }

            Game1.playSound("Cowboy_Footstep");
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
            if (this.Disabled)
            {
                return;
            }

            this.Hovered = false;
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

            if (this.Pressed)
            {
                o.Y += Game1.pixelZoom / 2;
            }

            var r = this.Hovered && !this.Pressed ? ButtonHover : ButtonNormal;

            // Begin
            b.Draw(
                Game1.mouseCursors,
                new Rectangle(this.Area.X + o.X, this.Area.Y + o.Y, Zoom2, this.Area.Height),
                new Rectangle(r.X, r.Y, 2, r.Height),
                Color.White * (this.Disabled ? 0.33f : 1),
                0,
                Vector2.Zero,
                SpriteEffects.None,
                1f);

            // End
            b.Draw(
                Game1.mouseCursors,
                new Rectangle(this.Area.X + o.X + this.Area.Width - Zoom2, this.Area.Y + o.Y, Zoom2, this.Area.Height),
                new Rectangle(r.X + r.Width - 2, r.Y, 2, r.Height),
                Color.White * (this.Disabled ? 0.33f : 1),
                0,
                Vector2.Zero,
                SpriteEffects.None,
                1f);

            // Center
            b.Draw(
                Game1.mouseCursors,
                new Rectangle(this.Area.X + o.X + Zoom2, this.Area.Y + o.Y, this.Area.Width - Zoom4, this.Area.Height),
                new Rectangle(r.X + 2, r.Y, r.Width - 4, r.Height),
                Color.White * (this.Disabled ? 0.33f : 1),
                0,
                Vector2.Zero,
                SpriteEffects.None,
                1f);

            // Text
            Utility.drawTextWithShadow(
                b,
                this.Label,
                Game1.smallFont,
                new Vector2(o.X + this.Area.X + this.LabelOffset * Game1.pixelZoom, o.Y + this.Area.Y + Zoom2),
                Game1.textColor * (this.Disabled ? 0.33f : 1));
        }
    }
}