using System;
using Farmhand.UI.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace Farmhand.UI.Form
{
    public class PlusMinusFormComponent : BaseKeyboardFormComponent
    {
        protected static readonly Rectangle PlusButton = new Rectangle(185, 345, 6, 8);
        protected static readonly Rectangle MinusButton = new Rectangle(177, 345, 6, 8);
        protected static readonly Rectangle Background = new Rectangle(227, 425, 9, 9);

        public event ValueChanged<int> Handler;

        public int Value { get; set; }

        protected int MinValue;
        protected int MaxValue;
        protected Rectangle PlusArea;
        protected Rectangle MinusArea;
        protected int Counter;
        protected int Limiter = 10;
        protected int OptionKey;
        protected int OldValue;
        protected string SelectedValue;

        public PlusMinusFormComponent(Point position, int minValue, int maxValue, ValueChanged<int> handler=null)
        {
            var width = Math.Max(GetStringWidth(minValue.ToString(), Game1.smallFont), GetStringWidth(maxValue.ToString(), Game1.smallFont)) + 2;
            SetScaledArea(new Rectangle(position.X, position.Y, 16 + width, 8));
            MinusArea = new Rectangle(Area.X, Area.Y, Zoom7, Area.Height);
            PlusArea = new Rectangle(Area.X + Area.Width - Zoom7, Area.Y, Zoom7, Area.Height);
            MinValue = minValue;
            MaxValue = maxValue;
            Value = MinValue;
            OldValue = Value;
            SelectedValue = Value.ToString();
            if(handler!=null)
                Handler += handler;
        }
        private void Resolve(Point p, Point o)
        {
            var plusAreaOffset = new Rectangle(PlusArea.X + o.X, PlusArea.Y + o.Y, PlusArea.Height, PlusArea.Width);
            if (plusAreaOffset.Contains(p) && Value < MaxValue)
            {
                Value++;
                Game1.playSound("drumkit6");
                SelectedValue = Value.ToString();
                return;
            }
            var minusAreaOffset = new Rectangle(MinusArea.X + o.X, MinusArea.Y + o.Y, MinusArea.Height, MinusArea.Width);
            if (minusAreaOffset.Contains(p) && Value > MinValue)
            {
                Game1.playSound("drumkit6");
                Value--;
                SelectedValue = Value.ToString();
            }
        }
        public override void LeftClick(Point p, Point o)
        {
            if (Disabled)
                return;
            Counter = 0;
            Limiter = 10;
            Rectangle boxArea = new Rectangle(Area.X + o.X + Zoom7, Area.Y + o.Y, Area.Width - Zoom14, Area.Height);
            if (boxArea.Contains(p))
            {
                Selected = true;
                return;
            }
            if (Selected)
                FocusLost();
            Resolve(p, o);
        }
        public override void LeftHeld(Point p, Point o)
        {
            Counter++;
            if (Disabled || Counter % Limiter !=0)
                return;
            Counter = 0;
            Limiter = Math.Max(1, Limiter - 1);
            Resolve(p, o);
        }
        public override void LeftUp(Point p, Point o)
        {
            Counter = 0;
            Limiter = 15;
            if (OldValue == Value)
                return;
            OldValue = Value;
            Handler?.Invoke(this, Parent, Parent.GetAttachedMenu(), Value);
        }
        public override void FocusLost()
        {
            if (OldValue == Value)
                return;
            OldValue = Value;
            Handler?.Invoke(this, Parent, Parent.GetAttachedMenu(), Value);
        }
        public override void Draw(SpriteBatch b, Point o)
        {
            if (!Visible)
                return;
            // Minus button on the left
            b.Draw(Game1.mouseCursors, new Vector2(o.X + Area.X, o.Y + Area.Y), MinusButton, Color.White * (Disabled || Value <= MinValue ? 0.33f : 1f), 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.4f);
            // Plus button on the right
            b.Draw(Game1.mouseCursors, new Vector2(o.X + Area.X + Area.Width - Zoom6, o.Y + Area.Y), PlusButton, Color.White * (Disabled || Value >= MaxValue ? 0.33f : 1f), 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.4f);
            // Box in the center
            IClickableMenu.drawTextureBox(b, Game1.mouseCursors, Background, o.X + Area.X + Zoom6, o.Y + Area.Y, Area.Width - Zoom12, Area.Height, Color.White * (Disabled?0.33f:1), Game1.pixelZoom, false);
            if (!Selected)
            {
                // Text label in the center (Non-selected)
                Utility.drawTextWithShadow(b, Value.ToString(), Game1.smallFont, new Vector2(o.X + Area.X + Zoom8, o.Y + Area.Y + Game1.pixelZoom), Game1.textColor * (Disabled ? 0.33f : 1f));
                return;
            }
            // Drawing code used when the textbox is selected
            string text = SelectedValue;
            Vector2 v;
            // Limit the draw length
            for (v = Game1.smallFont.MeasureString(text); v.X > Area.Width - Zoom5; v = Game1.smallFont.MeasureString(text))
                text = text.Substring(1);
            // Draw the caret (Text cursor)
            if (DateTime.Now.Millisecond % 1000 >= 500)
                b.Draw(Game1.staminaRect, new Rectangle(Area.X + o.X + Zoom05 + Zoom8 + (int)v.X, Area.Y + o.Y + (int)(Game1.pixelZoom*1.5), Zoom05, Area.Height-Zoom3), Game1.textColor);
            // Draw the actual text
            Utility.drawTextWithShadow(b, text, Game1.smallFont, new Vector2(o.X + Area.X + Zoom8, o.Y + Area.Y + Game1.pixelZoom), Game1.textColor);
        }
        public override void TextReceived(char chr)
        {
            Game1.playSound("cowboy_monsterhit");
            TextReceived(chr.ToString());
        }
        public override void TextReceived(string str)
        {
            int t;
            if (Disabled || !int.TryParse(str, out t))
                return;
            Value = int.Parse(SelectedValue + str);
            Value = Math.Max(MinValue, Math.Min(MaxValue, Value));
            SelectedValue = Value.ToString();
        }
        public override void CommandReceived(char cmd)
        {
            switch ((int)cmd)
            {
                case 8:
                    if (SelectedValue.Length <= 0)
                        return;
                    SelectedValue = SelectedValue.Substring(0, SelectedValue.Length - 1);
                    int t;
                    if (SelectedValue == "" || !int.TryParse(SelectedValue, out t))
                    {
                        SelectedValue = "0";
                        Value = 0;
                        return;
                    }
                    Game1.playSound("tinyWhip");
                    Value = int.Parse(SelectedValue);
                    Value = Math.Max(MinValue, Math.Min(MaxValue, Value));
                    SelectedValue = Value.ToString();
                    return;
                case 13:
                    Parent.ResetFocus();
                    return;
            }
        }
    }
}
