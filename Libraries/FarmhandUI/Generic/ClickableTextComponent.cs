namespace Farmhand.UI.Generic
{
    using System;

    using Farmhand.UI.Base;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    ///     The clickable text component.
    /// </summary>
    public class ClickableTextComponent : BaseInteractiveMenuComponent
    {
        private Point position;

        private string text;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClickableTextComponent" /> class.
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
        public ClickableTextComponent(
            Point position,
            string text,
            ClickHandler handler = null,
            bool hoverEffect = true,
            bool shadow = true,
            float scale = 1,
            Color? color = null,
            SpriteFont font = null)
        {
            if (color == null)
            {
                color = Game1.textColor;
            }

            if (font == null)
            {
                font = Game1.smallFont;
            }

            if (handler != null)
            {
                this.Handler += handler;
            }

            this.position = position;
            this.HoverEffect = hoverEffect;
            this.Font = font;
            this.Color = (Color)color;
            this.Shadow = shadow;
            this.Scale = scale;
            this.Text = text;
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
        ///     Gets or sets a value indicating whether text is hovered.
        /// </summary>
        protected bool Hovered { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether hover effect is enabled.
        /// </summary>
        protected bool HoverEffect { get; set; }

        /// <summary>
        ///     Gets or sets the text scale.
        /// </summary>
        protected float Scale { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether shadow is enabled.
        /// </summary>
        protected bool Shadow { get; set; }

        /// <summary>
        ///     Gets or sets the text to display.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                this.text = value;
                var size = this.Font.MeasureString(this.Text) / Game1.pixelZoom * this.Scale;
                this.SetScaledArea(
                    new Rectangle(
                        this.position.X,
                        this.position.Y,
                        (int)Math.Ceiling(size.X),
                        (int)Math.Ceiling(size.Y)));
            }
        }

        /// <summary>
        ///     The click event handler.
        /// </summary>
        public event ClickHandler Handler;

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
        public override void LeftClick(Point p, Point o)
        {
            Game1.playSound("bigDeSelect");
            this.Handler?.Invoke(this, this.Parent, this.Parent.GetAttachedMenu());
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

            var p = new Vector2(this.Area.X + o.X, this.Area.Y + o.Y);
            if (this.Shadow)
            {
                Utility.drawTextWithShadow(
                    b,
                    this.Text,
                    this.Font,
                    p,
                    this.Color * (this.HoverEffect && !this.Hovered ? 0.8f : 1),
                    this.Scale);
            }
            else
            {
                b.DrawString(
                    this.Font,
                    this.Text,
                    p,
                    this.Color * (this.HoverEffect && !this.Hovered ? 0.8f : 1),
                    0,
                    Vector2.Zero,
                    Game1.pixelZoom * this.Scale,
                    SpriteEffects.None,
                    1);
            }
        }
    }
}