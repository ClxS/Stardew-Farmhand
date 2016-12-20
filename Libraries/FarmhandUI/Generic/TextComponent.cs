using System;
using Farmhand.UI.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Farmhand.UI.Generic
{
    public class TextComponent : BaseMenuComponent
    {
        protected string Text;
        protected float Scale;
        protected Color Color;
        protected SpriteFont Font;
        protected bool Shadow;
        public TextComponent(Point position, string text, bool shadow=true, float scale=1, Color? color=null, SpriteFont font=null)
        {
            if (color == null)
                Color = Game1.textColor;
            else
                Color = (Color)color;
            Font = font ?? Game1.smallFont;
            Shadow = shadow;
            Scale = scale;
            Text = text;
            var size = Font.MeasureString(Text) / Game1.pixelZoom * Scale;
            SetScaledArea(new Rectangle(position.X, position.Y,(int)Math.Ceiling(size.X),(int)Math.Ceiling(size.Y)));
        }
        public override void Draw(SpriteBatch b, Point o)
        {
            if (!Visible)
                return;
            var p = new Vector2(Area.X + o.X, Area.Y + o.Y);
            if (Shadow)
                Utility.drawTextWithShadow(b, Text, Font, p, Color, Scale);
            else
                b.DrawString(Font, Text, p, Color, 0, Vector2.Zero, Game1.pixelZoom * Scale, SpriteEffects.None, 1);
        }
    }
}
