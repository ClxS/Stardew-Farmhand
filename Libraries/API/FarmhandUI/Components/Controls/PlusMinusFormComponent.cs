namespace Farmhand.UI.Components.Controls
{
    using System;

    using Farmhand.UI.Base;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;
    using StardewValley.Menus;

    /// <summary>
    ///     The plus minus form component.
    /// </summary>
    public class PlusMinusFormComponent : BaseKeyboardFormComponent
    {
        /// <summary>
        ///     Texture location of the plus button
        /// </summary>
        protected static readonly Rectangle PlusButton = new Rectangle(185, 345, 6, 8);

        /// <summary>
        ///     Texture location of the minus button
        /// </summary>
        protected static readonly Rectangle MinusButton = new Rectangle(177, 345, 6, 8);

        /// <summary>
        ///     Texture location of the sprite background
        /// </summary>
        protected static readonly Rectangle Background = new Rectangle(227, 425, 9, 9);

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlusMinusFormComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of the component
        /// </param>
        /// <param name="minValue">
        ///     The min value.
        /// </param>
        /// <param name="maxValue">
        ///     The max value.
        /// </param>
        /// <param name="handler">
        ///     The value changed handler.
        /// </param>
        public PlusMinusFormComponent(Point position, int minValue, int maxValue, ValueChanged<int> handler = null)
        {
            var width = Math.Max(
                            this.GetStringWidth(minValue.ToString(), Game1.smallFont),
                            this.GetStringWidth(maxValue.ToString(), Game1.smallFont)) + 2;
            this.SetScaledArea(new Rectangle(position.X, position.Y, 16 + width, 8));
            this.MinusArea = new Rectangle(this.Area.X, this.Area.Y, Zoom7, this.Area.Height);
            this.PlusArea = new Rectangle(this.Area.X + this.Area.Width - Zoom7, this.Area.Y, Zoom7, this.Area.Height);
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Value = this.MinValue;
            this.OldValue = this.Value;
            this.SelectedValue = this.Value.ToString();
            if (handler != null)
            {
                this.Handler += handler;
            }
        }

        /// <summary>
        ///     Gets or sets the counter.
        /// </summary>
        protected int Counter { get; set; }

        /// <summary>
        ///     Gets or sets the limiter.
        /// </summary>
        protected int Limiter { get; set; } = 10;

        /// <summary>
        ///     Gets or sets the max value.
        /// </summary>
        protected int MaxValue { get; set; }

        /// <summary>
        ///     Gets or sets the minus area.
        /// </summary>
        protected Rectangle MinusArea { get; set; }

        /// <summary>
        ///     Gets or sets the min value.
        /// </summary>
        protected int MinValue { get; set; }

        /// <summary>
        ///     Gets or sets the old value.
        /// </summary>
        protected int OldValue { get; set; }

        /// <summary>
        ///     Gets or sets the option key.
        /// </summary>
        protected int OptionKey { get; set; }

        /// <summary>
        ///     Gets or sets the plus area.
        /// </summary>
        protected Rectangle PlusArea { get; set; }

        /// <summary>
        ///     Gets or sets the selected value.
        /// </summary>
        protected string SelectedValue { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        ///     The value changed handler.
        /// </summary>
        public event ValueChanged<int> Handler;

        /// <summary>
        ///     Resolves a click event, and determines whether to de/increment the value
        /// </summary>
        /// <param name="p">
        ///     The mouse click location
        /// </param>
        /// <param name="o">
        ///     The component origin
        /// </param>
        private void Resolve(Point p, Point o)
        {
            var plusAreaOffset = new Rectangle(
                this.PlusArea.X + o.X,
                this.PlusArea.Y + o.Y,
                this.PlusArea.Height,
                this.PlusArea.Width);
            if (plusAreaOffset.Contains(p) && this.Value < this.MaxValue)
            {
                this.Value++;
                Game1.playSound("drumkit6");
                this.SelectedValue = this.Value.ToString();
                return;
            }

            var minusAreaOffset = new Rectangle(
                this.MinusArea.X + o.X,
                this.MinusArea.Y + o.Y,
                this.MinusArea.Height,
                this.MinusArea.Width);
            if (minusAreaOffset.Contains(p) && this.Value > this.MinValue)
            {
                Game1.playSound("drumkit6");
                this.Value--;
                this.SelectedValue = this.Value.ToString();
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

            this.Counter = 0;
            this.Limiter = 10;
            var boxArea = new Rectangle(
                this.Area.X + o.X + Zoom7,
                this.Area.Y + o.Y,
                this.Area.Width - Zoom14,
                this.Area.Height);
            if (boxArea.Contains(p))
            {
                this.Selected = true;
                return;
            }

            if (this.Selected)
            {
                this.FocusLost();
            }

            this.Resolve(p, o);
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
            this.Counter++;
            if (this.Disabled || this.Counter % this.Limiter != 0)
            {
                return;
            }

            this.Counter = 0;
            this.Limiter = Math.Max(1, this.Limiter - 1);
            this.Resolve(p, o);
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
            this.Counter = 0;
            this.Limiter = 15;
            if (this.OldValue == this.Value)
            {
                return;
            }

            this.OldValue = this.Value;
            this.Handler?.Invoke(this, this.Parent, this.Parent.GetAttachedMenu(), this.Value);
        }

        /// <summary>
        ///     Called when focus is lost
        /// </summary>
        public override void FocusLost()
        {
            if (this.OldValue == this.Value)
            {
                return;
            }

            this.OldValue = this.Value;
            this.Handler?.Invoke(this, this.Parent, this.Parent.GetAttachedMenu(), this.Value);
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

            // Minus button on the left
            b.Draw(
                Game1.mouseCursors,
                new Vector2(o.X + this.Area.X, o.Y + this.Area.Y),
                MinusButton,
                Color.White * (this.Disabled || this.Value <= this.MinValue ? 0.33f : 1f),
                0.0f,
                Vector2.Zero,
                Game1.pixelZoom,
                SpriteEffects.None,
                0.4f);

            // Plus button on the right
            b.Draw(
                Game1.mouseCursors,
                new Vector2(o.X + this.Area.X + this.Area.Width - Zoom6, o.Y + this.Area.Y),
                PlusButton,
                Color.White * (this.Disabled || this.Value >= this.MaxValue ? 0.33f : 1f),
                0.0f,
                Vector2.Zero,
                Game1.pixelZoom,
                SpriteEffects.None,
                0.4f);

            // Box in the center
            IClickableMenu.drawTextureBox(
                b,
                Game1.mouseCursors,
                Background,
                o.X + this.Area.X + Zoom6,
                o.Y + this.Area.Y,
                this.Area.Width - Zoom12,
                this.Area.Height,
                Color.White * (this.Disabled ? 0.33f : 1),
                Game1.pixelZoom,
                false);
            if (!this.Selected)
            {
                // Text label in the center (Non-selected)
                Utility.drawTextWithShadow(
                    b,
                    this.Value.ToString(),
                    Game1.smallFont,
                    new Vector2(o.X + this.Area.X + Zoom8, o.Y + this.Area.Y + Game1.pixelZoom),
                    Game1.textColor * (this.Disabled ? 0.33f : 1f));
                return;
            }

            // Drawing code used when the textbox is selected
            var text = this.SelectedValue;
            Vector2 v;

            // Limit the draw length
            for (v = Game1.smallFont.MeasureString(text);
                 v.X > this.Area.Width - Zoom5;
                 v = Game1.smallFont.MeasureString(text))
            {
                text = text.Substring(1);
            }

            // Draw the caret (Text cursor)
            if (DateTime.Now.Millisecond % 1000 >= 500)
            {
                b.Draw(
                    Game1.staminaRect,
                    new Rectangle(
                        this.Area.X + o.X + Zoom05 + Zoom8 + (int)v.X,
                        this.Area.Y + o.Y + (int)(Game1.pixelZoom * 1.5),
                        Zoom05,
                        this.Area.Height - Zoom3),
                    Game1.textColor);
            }

            // Draw the actual text
            Utility.drawTextWithShadow(
                b,
                text,
                Game1.smallFont,
                new Vector2(o.X + this.Area.X + Zoom8, o.Y + this.Area.Y + Game1.pixelZoom),
                Game1.textColor);
        }

        /// <summary>
        ///     Called when a character is entered
        /// </summary>
        /// <param name="chr">
        ///     The character received
        /// </param>
        public override void TextReceived(char chr)
        {
            Game1.playSound("cowboy_monsterhit");
            this.TextReceived(chr.ToString());
        }

        /// <summary>
        ///     Called when a string is entered
        /// </summary>
        /// <param name="str">
        ///     The string received
        /// </param>
        public override void TextReceived(string str)
        {
            int t;
            if (this.Disabled || !int.TryParse(str, out t))
            {
                return;
            }

            this.Value = int.Parse(this.SelectedValue + str);
            this.Value = Math.Max(this.MinValue, Math.Min(this.MaxValue, this.Value));
            this.SelectedValue = this.Value.ToString();
        }

        /// <summary>
        ///     Called when a command is received
        /// </summary>
        /// <param name="cmd">
        ///     The command received
        /// </param>
        public override void CommandReceived(char cmd)
        {
            switch ((int)cmd)
            {
                case 8:
                    if (this.SelectedValue.Length <= 0)
                    {
                        return;
                    }

                    this.SelectedValue = this.SelectedValue.Substring(0, this.SelectedValue.Length - 1);
                    int t;
                    if (this.SelectedValue == string.Empty || !int.TryParse(this.SelectedValue, out t))
                    {
                        this.SelectedValue = "0";
                        this.Value = 0;
                        return;
                    }

                    Game1.playSound("tinyWhip");
                    this.Value = int.Parse(this.SelectedValue);
                    this.Value = Math.Max(this.MinValue, Math.Min(this.MaxValue, this.Value));
                    this.SelectedValue = this.Value.ToString();
                    return;
                case 13:
                    this.Parent.ResetFocus();
                    return;
            }
        }
    }
}