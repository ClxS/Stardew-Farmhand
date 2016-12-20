using System;
using Farmhand.UI.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Farmhand.UI.Form
{
    public class ButtonFormComponent : BaseFormComponent
    {
        protected static readonly Rectangle ButtonNormal = new Rectangle(256, 256, 10, 10);
        protected static readonly Rectangle ButtonHover = new Rectangle(267, 256, 10, 10);
        public event ClickHandler Handler;
        protected string Label;
        protected int LabelOffset;
        protected bool Hovered;
        protected bool Pressed;
        public ButtonFormComponent(Point position, string label, ClickHandler handler=null) : this(position,50,label,handler)
        {

        }
        public ButtonFormComponent(Point position, int width, string label, ClickHandler handler =null)
        {
            int labelWidth = GetStringWidth(label, Game1.smallFont);
            width = Math.Max(width,labelWidth+4);
            LabelOffset = (int)Math.Round((width - labelWidth) / 2D);
            SetScaledArea(new Rectangle(position.X, position.Y, width, 10));
            Label = label;
            if (handler!=null)
                Handler += handler;
        }
        public override void LeftHeld(Point p, Point o)
        {
            if(!Disabled)
                Pressed = true;
        }
        public override void LeftUp(Point p, Point o)
        {
            if(!Disabled)
                Pressed = false;
        }
        public override void LeftClick(Point p, Point o)
        {
            if (Disabled)
                return;
            Game1.playSound("bigDeSelect");
            Handler?.Invoke(this, Parent, Parent.GetAttachedMenu());
        }
        public override void HoverIn(Point p, Point o)
        {
            if (Disabled)
                return;
            Game1.playSound("Cowboy_Footstep");
            Hovered = true;
        }
        public override void HoverOut(Point p, Point o)
        {
            if (Disabled)
                return;
            Hovered = false;
        }
        public override void Draw(SpriteBatch b, Point o)
        {
            if (!Visible)
                return;
            if (Pressed)
                o.Y += Game1.pixelZoom / 2;
            Rectangle r = Hovered && !Pressed ? ButtonHover : ButtonNormal;
            // Begin
            b.Draw(Game1.mouseCursors, new Rectangle(Area.X + o.X, Area.Y + o.Y, Zoom2, Area.Height), new Rectangle(r.X, r.Y, 2, r.Height), Color.White * (Disabled ? 0.33f : 1), 0, Vector2.Zero, SpriteEffects.None, 1f);
            // End
            b.Draw(Game1.mouseCursors, new Rectangle(Area.X + o.X + Area.Width-Zoom2, Area.Y + o.Y, Zoom2, Area.Height), new Rectangle(r.X+r.Width-2, r.Y, 2, r.Height), Color.White * (Disabled ? 0.33f : 1), 0, Vector2.Zero, SpriteEffects.None, 1f);
            // Center
            b.Draw(Game1.mouseCursors, new Rectangle(Area.X + o.X + Zoom2, Area.Y + o.Y, Area.Width - Zoom4, Area.Height), new Rectangle(r.X+2, r.Y, r.Width - 4, r.Height), Color.White *(Disabled ? 0.33f : 1), 0, Vector2.Zero, SpriteEffects.None, 1f);
            // Text
            Utility.drawTextWithShadow(b, Label, Game1.smallFont, new Vector2(o.X + Area.X + LabelOffset*Game1.pixelZoom, o.Y + Area.Y + Zoom2), Game1.textColor * (Disabled?0.33f:1));
        }
    }
}
