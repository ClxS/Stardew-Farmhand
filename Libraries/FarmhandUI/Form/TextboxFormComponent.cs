using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardewValley;

namespace Farmhand.UI
{
    public class TextboxFormComponent : BaseKeyboardFormComponent
    {
        protected static Texture2D Box;
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }
        public event ValueChanged<string> Handler;
        public Func<TextboxFormComponent, FrameworkMenu, string, string> TabPressed;
        public Func<TextboxFormComponent, FrameworkMenu, string, string> EnterPressed;
        protected string _Value;
        protected Predicate<string> Validator = (e) => true;
        protected string OldValue;
        public TextboxFormComponent(Point position, ValueChanged<string> handler = null) : this(position, 75, null, handler)
        {

        }
        public TextboxFormComponent(Point position, Predicate<string> validator, ValueChanged<string> handler = null) : this(position, 75, validator, handler)
        {

        }
        public TextboxFormComponent(Point position, int width, ValueChanged<string> handler=null) : this(position, width, null, handler)
        {

        }
        public TextboxFormComponent(Point position, int width=75, Predicate<string> validator=null, ValueChanged<string> handler=null)
        {
            if(Box==null)
                Box=Game1.content.Load<Texture2D>("LooseSprites\\textBox");
            SetScaledArea(new Rectangle(position.X, position.Y, width, Box.Height/Game1.pixelZoom));
            if (validator != null)
                Validator = validator;
            if (handler != null)
                Handler += handler;
            Value = "";
            OldValue = Value;
        }
        public override void FocusLost()
        {
            if (Disabled || OldValue.Equals(Value))
                return;
            OldValue = Value;
            Handler?.Invoke(this, Parent, Parent.GetAttachedMenu(), Value);
        }
        public override void FocusGained()
        {
            if (Disabled)
                return;
            Selected = true;
        }
        protected int CaretSize = (int)Game1.smallFont.MeasureString("|").Y;
        public override void Draw(SpriteBatch b, Point o)
        {
            if (!Visible)
                return;
            bool flag = DateTime.Now.Millisecond % 1000 >= 500;
            string text = Value;
            b.Draw(Box, new Rectangle(Area.X + o.X, Area.Y + o.Y, zoom4, Area.Height), new Rectangle(Game1.pixelZoom, 0, zoom4, Area.Height), Color.White * (Disabled ? 0.33f : 1));
            b.Draw(Box, new Rectangle(Area.X+o.X + zoom4, Area.Y+o.Y, Area.Width - zoom8, Area.Height), new Rectangle(zoom4, 0, 4, Area.Height), Color.White * (Disabled ? 0.33f : 1));
            b.Draw(Box, new Rectangle(Area.X+o.X + Area.Width - zoom4, Area.Y+o.Y, zoom4, Area.Height), new Rectangle(Box.Bounds.Width - zoom4, 0, zoom4, Area.Height), Color.White * (Disabled ? 0.33f : 1));
            Vector2 v;
            for (v = Game1.smallFont.MeasureString(text); v.X > Area.Width - Game1.pixelZoom*5; v = Game1.smallFont.MeasureString(text))
                text = text.Substring(1);
            if (flag && Selected)
                b.Draw(Game1.staminaRect, new Rectangle(Area.X+o.X + zoom3 + zoom05 + (int)v.X, Area.Y+o.Y + 8, zoom05, CaretSize), Game1.textColor);
            Utility.drawTextWithShadow(b, text, Game1.smallFont, new Vector2(Area.X+o.X + zoom4, Area.Y+o.Y+zoom3), Game1.textColor * (Disabled ? 0.33f : 1));
        }
        public override void TextReceived(char chr)
        {
            if (Disabled || !Game1.smallFont.Characters.Contains(chr) || !Validator(chr.ToString()))
                return;
            Game1.playSound("cowboy_monsterhit");
            Value =Value +chr.ToString();
        }
        public override void TextReceived(string str)
        {
            foreach(char c in str)
                if (!Game1.smallFont.Characters.Contains(c))
                    return;
            if (Disabled || !Validator(str))
                return;
            Game1.playSound("coin");
            Value = Value + str;
        }
        public override void CommandReceived(char cmd)
        {
            if (Disabled)
                return;
            switch((int)cmd)
            {
                case 8:
                    if (Value.Length <= 0)
                        return;
                    Value = Value.Substring(0, Value.Length - 1);
                    Game1.playSound("tinyWhip");
                    return;
                case 9:
                    if (TabPressed != null)
                    {
                        Value = TabPressed(this, Parent.GetAttachedMenu(), Value);
                        return;
                    }
                    if (!(Parent is IComponentCollection))
                        return;
                    bool Next = false;
                    IInteractiveMenuComponent first=null;
                    foreach(IInteractiveMenuComponent imc in (Parent as IComponentCollection).InteractiveComponents)
                    {
                        if (first == null && imc is TextboxFormComponent)
                            first = imc;
                        if (imc == this)
                        {
                            Next = true;
                            continue;
                        }
                        if (Next && imc is TextboxFormComponent)
                        {
                            Parent.GiveFocus(imc);
                            return;
                        }
                    }
                    Parent.GiveFocus(first);
                    return;
                case 13:
                    if (EnterPressed != null)
                        Value = EnterPressed(this, Parent.GetAttachedMenu(), Value);
                    else
                        Parent.ResetFocus();
                    return;
            }
        }
    }
}
