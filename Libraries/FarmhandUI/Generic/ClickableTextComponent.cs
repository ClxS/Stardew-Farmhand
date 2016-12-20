using System;
using Farmhand.UI.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Farmhand.UI.Generic
{
    public class ClickableTextComponent : BaseInteractiveMenuComponent
    {
        protected string Text;
        protected float Scale;
        protected Color Color;
        protected SpriteFont Font;
        protected bool Shadow;
        protected bool HoverEffect;
        protected bool Hovered;
        public event ClickHandler Handler;
        public ClickableTextComponent(Point position, string text, ClickHandler handler = null, bool hoverEffect = true, bool shadow = true, float scale = 1, Color? color = null, SpriteFont font = null)
        {
            if (color == null)
                color = Game1.textColor;
            if (font == null)
                font = Game1.smallFont;
            if (handler != null)
                Handler += handler;
            HoverEffect = hoverEffect;
            Font = font;
            Color = (Color)color;
            Shadow = shadow;
            Scale = scale;
            Text = text;
            Vector2 size = Font.MeasureString(Text) / Game1.pixelZoom * Scale;
            SetScaledArea(new Rectangle(position.X, position.Y, (int)Math.Ceiling(size.X), (int)Math.Ceiling(size.Y)));
        }
        public override void HoverIn(Point p, Point o)
        {
            Game1.playSound("Cowboy_Footstep");
            Hovered = true;
        }
        public override void HoverOut(Point p, Point o)
        {
            Hovered = false;
        }
        public override void LeftClick(Point p, Point o)
        {
            Game1.playSound("bigDeSelect");
            Handler?.Invoke(this, Parent, Parent.GetAttachedMenu());
        }
        public override void Draw(SpriteBatch b, Point o)
        {
            if (!Visible)
                return;
            Vector2 p = new Vector2(Area.X + o.X, Area.Y + o.Y);
            if (Shadow)
                Utility.drawTextWithShadow(b, Text, Font, p, Color * (HoverEffect && !Hovered ? 0.8f : 1), Scale);
            else
                b.DrawString(Font, Text, p, Color * (HoverEffect && !Hovered ? 0.8f : 1), 0, Vector2.Zero, Game1.pixelZoom * Scale, SpriteEffects.None, 1);
        }
    }
}