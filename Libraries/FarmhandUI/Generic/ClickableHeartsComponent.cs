using System;
using Farmhand.UI.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Farmhand.UI.Generic
{
    public class ClickableHeartsComponent : BaseInteractiveMenuComponent
    {
        protected static readonly Rectangle HeartFull = new Rectangle(211, 428, 7, 6);
        protected static readonly Rectangle HeartEmpty = new Rectangle(218, 428, 7, 6);
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = Math.Min(Math.Max(0, value), MaxValue);
            }
        }
        public event ValueChanged<int> Handler;
        private int _value;
        protected int OldValue;
        protected int MaxValue;
        protected bool Hovered;
        public ClickableHeartsComponent(Point position, int value, int maxValue, ValueChanged<int> handler=null)
        {
            if (maxValue % 2 != 0)
                maxValue++;
            SetScaledArea(new Rectangle(position.X, position.Y, 8 * (maxValue / 2), HeartEmpty.Height));
            MaxValue = maxValue;
            Value = value;
            OldValue = Value;
            if (handler != null)
                Handler += handler;
        }
        public override void HoverIn(Point p, Point o)
        {
            Hovered = true;
        }
        public override void HoverOut(Point p, Point o)
        {
            Hovered = false;
        }
        public override void LeftUp(Point p, Point o)
        {
            Value = (int)Math.Round((p.X - (Area.X + o.X)) / 4D / Game1.pixelZoom);
            if (OldValue == Value)
                return;
            OldValue = Value;
            Handler?.Invoke(this, Parent, Parent.GetAttachedMenu(), Value);
        }
        public override void Draw(SpriteBatch b, Point o)
        {
            if (!Visible)
                return;
            for (int c = 0; c < MaxValue / 2; c++)
                b.Draw(Game1.mouseCursors, new Vector2(o.X + Area.X + Game1.pixelZoom + c * Zoom8, o.Y + Area.Y), new Rectangle(HeartEmpty.X, HeartEmpty.Y, 7, 6), Color.White, 0, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 1f);
            for (int c = 0; c < Value; c++)
                b.Draw(Game1.mouseCursors, new Vector2(o.X + Area.X + Game1.pixelZoom + c * Zoom4, o.Y + Area.Y), new Rectangle(HeartFull.X + (c % 2 == 0 ? 0 : 4), HeartFull.Y, (c % 2 == 0 ? 4 : 3), 6), Color.White * (Hovered?0.5f:1), 0, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 1f);
            if (!Hovered)
                return;
            int value = Math.Min(MaxValue, (int)Math.Round((Game1.getMouseX() - (Area.X + o.X)) / 4D / Game1.pixelZoom));
            for (int c = 0; c < value; c++)
                b.Draw(Game1.mouseCursors, new Vector2(o.X + Area.X + Game1.pixelZoom + c * Zoom4, o.Y + Area.Y), new Rectangle(HeartFull.X + (c % 2 == 0 ? 0 : 4), HeartFull.Y, (c % 2 == 0 ? 4 : 3), 6), Color.White, 0, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 1f);
        }
    }
}
