using Farmhand.UI.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Farmhand.UI.Generic
{
    public class LabelComponent : BaseMenuComponent
    {
        protected static readonly Rectangle Left = new Rectangle(256, 267, 6, 16);
        protected static readonly Rectangle Right = new Rectangle(263, 267, 6, 16);
        protected static readonly Rectangle Center = new Rectangle(262, 267, 1, 16);
        protected string Label;
        public LabelComponent(Point position, string label)
        {
            SetScaledArea(new Rectangle(position.X, position.Y, GetStringWidth(label, Game1.smallFont) + 12, 16));
            Label = label;
        }
        public override void Draw(SpriteBatch b, Point o)
        {
            // Left
            b.Draw(Game1.mouseCursors, new Rectangle(o.X + Area.X, o.Y + Area.Y, Zoom6, Area.Height), Left, Color.White);
            // Right
            b.Draw(Game1.mouseCursors, new Rectangle(o.X + Area.X + Area.Width - Zoom6, o.Y + Area.Y, Zoom6, Area.Height), Right, Color.White);
            // Center
            b.Draw(Game1.mouseCursors, new Rectangle(o.X + Area.X + Zoom6, o.Y + Area.Y, Area.Width - Zoom12, Area.Height), Center, Color.White);
            // Label
            Utility.drawTextWithShadow(b, Label, Game1.smallFont, new Vector2(o.X + Area.X + Zoom6, o.Y + Area.Y + Zoom5), Game1.textColor);
        }
    }
}
