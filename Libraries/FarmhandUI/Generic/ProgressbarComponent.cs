using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardewValley;
using StardewValley.Menus;

namespace Farmhand.UI
{
    public class ProgressbarComponent : BaseMenuComponent
    {
        protected readonly static Rectangle Background = new Rectangle(403, 383, 6, 6);
        protected readonly static Rectangle Filler = new Rectangle(306,320,16,16);
        public int Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = Math.Max(0, Math.Min(MaxValue, value));
            }
        }
        protected int _Value;
        protected int MaxValue;
        protected int OffsetValue;
        public ProgressbarComponent(Point position, int value, int maxValue)
        {
            MaxValue = maxValue;
            Value = value;
            OffsetValue = Value * Game1.pixelZoom;
            SetScaledArea(new Rectangle(position.X, position.Y, MaxValue + 2, 6));
        }
        protected int GetDiff()
        {
            int v = _Value * Game1.pixelZoom;
            if (OffsetValue == v)
                return 0;
            if (OffsetValue > v)
                return -((int)Math.Floor((OffsetValue - v) / 10D + 1));
            return (int)Math.Floor((v - OffsetValue) / 10D + 1);
        }
        public override void Draw(SpriteBatch b, Point o)
        {
            if (DateTime.Now.Millisecond % 5 == 0)
                OffsetValue += GetDiff();
            IClickableMenu.drawTextureBox(b, Game1.mouseCursors, Background, Area.X + o.X, Area.Y + o.Y, Area.Width, Area.Height, Color.White, Game1.pixelZoom, false);
            b.Draw(Game1.mouseCursors, new Rectangle(Area.X + o.X + Game1.pixelZoom, Area.Y + o.Y + Game1.pixelZoom, OffsetValue, zoom4), Filler, Color.White);
        }
    }
}
