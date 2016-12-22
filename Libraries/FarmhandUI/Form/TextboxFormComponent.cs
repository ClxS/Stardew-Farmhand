namespace Farmhand.UI.Form
{
    using System;

    using Farmhand.UI.Base;
    using Farmhand.UI.Interfaces;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    ///     The textbox form component.
    /// </summary>
    public class TextboxFormComponent : BaseKeyboardFormComponent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TextboxFormComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of this component
        /// </param>
        /// <param name="handler">
        ///     The value changed handler
        /// </param>
        public TextboxFormComponent(Point position, ValueChanged<string> handler = null)
            : this(position, 75, null, handler)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextboxFormComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of this component
        /// </param>
        /// <param name="validator">
        ///     Is used to validate input
        /// </param>
        /// <param name="handler">
        ///     The value changed handler.
        /// </param>
        public TextboxFormComponent(Point position, Predicate<string> validator, ValueChanged<string> handler = null)
            : this(position, 75, validator, handler)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextboxFormComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of this component.
        /// </param>
        /// <param name="width">
        ///     The width of this component.
        /// </param>
        /// <param name="handler">
        ///     The value changed handler.
        /// </param>
        public TextboxFormComponent(Point position, int width, ValueChanged<string> handler = null)
            : this(position, width, null, handler)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextboxFormComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of this component.
        /// </param>
        /// <param name="width">
        ///     The width of this component.
        /// </param>
        /// <param name="validator">
        ///     Is used to validate input.
        /// </param>
        /// <param name="handler">
        ///     The value changed handler.
        /// </param>
        public TextboxFormComponent(
            Point position,
            int width = 75,
            Predicate<string> validator = null,
            ValueChanged<string> handler = null)
        {
            if (Box == null)
            {
                Box = Game1.content.Load<Texture2D>("LooseSprites\\textBox");
            }

            this.SetScaledArea(new Rectangle(position.X, position.Y, width, Box.Height / Game1.pixelZoom));
            if (validator != null)
            {
                this.Validator = validator;
            }

            if (handler != null)
            {
                this.Handler += handler;
            }

            this.Value = string.Empty;
            this.OldValue = this.Value;
        }

        /// <summary>
        ///     Gets or sets the texture for textbox
        /// </summary>
        protected static Texture2D Box { get; set; }

        /// <summary>
        ///     Gets the caret size.
        /// </summary>
        protected int CaretSize { get; } = (int)Game1.smallFont.MeasureString("|").Y;

        /// <summary>
        ///     Gets or sets the enter pressed callback.
        /// </summary>
        public Func<TextboxFormComponent, FrameworkMenu, string, string> EnterPressed { get; set; }

        /// <summary>
        ///     Gets or sets the tab pressed callback.
        /// </summary>
        public Func<TextboxFormComponent, FrameworkMenu, string, string> TabPressed { get; set; }

        /// <summary>
        ///     Gets or sets the validator.
        /// </summary>
        protected Predicate<string> Validator { get; set; } = e => true;

        /// <summary>
        ///     Gets or sets the old value.
        /// </summary>
        protected string OldValue { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        ///     The value changed handler.
        /// </summary>
        public event ValueChanged<string> Handler;

        /// <summary>
        ///     Called when focus is lost
        /// </summary>
        public override void FocusLost()
        {
            if (this.Disabled || this.OldValue.Equals(this.Value))
            {
                return;
            }

            this.OldValue = this.Value;
            this.Handler?.Invoke(this, this.Parent, this.Parent.GetAttachedMenu(), this.Value);
        }

        /// <summary>
        ///     Called when focus is gained
        /// </summary>
        public override void FocusGained()
        {
            if (this.Disabled)
            {
                return;
            }

            this.Selected = true;
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

            var flag = DateTime.Now.Millisecond % 1000 >= 500;
            var text = this.Value;
            b.Draw(
                Box,
                new Rectangle(this.Area.X + o.X, this.Area.Y + o.Y, Zoom4, this.Area.Height),
                new Rectangle(Game1.pixelZoom, 0, Zoom4, this.Area.Height),
                Color.White * (this.Disabled ? 0.33f : 1));
            b.Draw(
                Box,
                new Rectangle(this.Area.X + o.X + Zoom4, this.Area.Y + o.Y, this.Area.Width - Zoom8, this.Area.Height),
                new Rectangle(Zoom4, 0, 4, this.Area.Height),
                Color.White * (this.Disabled ? 0.33f : 1));
            b.Draw(
                Box,
                new Rectangle(this.Area.X + o.X + this.Area.Width - Zoom4, this.Area.Y + o.Y, Zoom4, this.Area.Height),
                new Rectangle(Box.Bounds.Width - Zoom4, 0, Zoom4, this.Area.Height),
                Color.White * (this.Disabled ? 0.33f : 1));
            Vector2 v;
            for (v = Game1.smallFont.MeasureString(text);
                 v.X > this.Area.Width - Game1.pixelZoom * 5;
                 v = Game1.smallFont.MeasureString(text))
            {
                text = text.Substring(1);
            }

            if (flag && this.Selected)
            {
                b.Draw(
                    Game1.staminaRect,
                    new Rectangle(
                        this.Area.X + o.X + Zoom3 + Zoom05 + (int)v.X,
                        this.Area.Y + o.Y + 8,
                        Zoom05,
                        this.CaretSize),
                    Game1.textColor);
            }

            Utility.drawTextWithShadow(
                b,
                text,
                Game1.smallFont,
                new Vector2(this.Area.X + o.X + Zoom4, this.Area.Y + o.Y + Zoom3),
                Game1.textColor * (this.Disabled ? 0.33f : 1));
        }

        /// <summary>
        ///     Called when a character is entered
        /// </summary>
        /// <param name="chr">
        ///     The character received
        /// </param>
        public override void TextReceived(char chr)
        {
            if (this.Disabled || !Game1.smallFont.Characters.Contains(chr) || !this.Validator(chr.ToString()))
            {
                return;
            }

            Game1.playSound("cowboy_monsterhit");
            this.Value = this.Value + chr;
        }

        /// <summary>
        ///     Called when a string is entered
        /// </summary>
        /// <param name="str">
        ///     The string received
        /// </param>
        public override void TextReceived(string str)
        {
            foreach (var c in str)
            {
                if (!Game1.smallFont.Characters.Contains(c))
                {
                    return;
                }
            }

            if (this.Disabled || !this.Validator(str))
            {
                return;
            }

            Game1.playSound("coin");
            this.Value = this.Value + str;
        }

        /// <summary>
        ///     Called when a command is received
        /// </summary>
        /// <param name="cmd">
        ///     The command received
        /// </param>
        public override void CommandReceived(char cmd)
        {
            if (this.Disabled)
            {
                return;
            }

            switch ((int)cmd)
            {
                case 8:
                    if (this.Value.Length <= 0)
                    {
                        return;
                    }

                    this.Value = this.Value.Substring(0, this.Value.Length - 1);
                    Game1.playSound("tinyWhip");
                    return;
                case 9:
                    if (this.TabPressed != null)
                    {
                        this.Value = this.TabPressed(this, this.Parent.GetAttachedMenu(), this.Value);
                        return;
                    }

                    if (!(this.Parent is IComponentCollection))
                    {
                        return;
                    }

                    var next = false;
                    IInteractiveMenuComponent first = null;
                    foreach (var imc in ((IComponentCollection)this.Parent).InteractiveComponents)
                    {
                        if (first == null && imc is TextboxFormComponent)
                        {
                            first = imc;
                        }

                        if (imc == this)
                        {
                            next = true;
                            continue;
                        }

                        if (next && imc is TextboxFormComponent)
                        {
                            this.Parent.GiveFocus(imc);
                            return;
                        }
                    }

                    this.Parent.GiveFocus(first);
                    return;
                case 13:
                    if (this.EnterPressed != null)
                    {
                        this.Value = this.EnterPressed(this, this.Parent.GetAttachedMenu(), this.Value);
                    }
                    else
                    {
                        this.Parent.ResetFocus();
                    }

                    return;
            }
        }
    }
}