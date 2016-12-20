using Farmhand.UI.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Farmhand.UI.Generic
{
    public class ClickableTextureComponent : BaseInteractiveMenuComponent
    {
        protected bool ScaleOnHover;
        public event ClickHandler Handler;
        public ClickableTextureComponent(Rectangle area, Texture2D texture, ClickHandler handler = null, Rectangle? crop = null, bool scaleOnHover = true) : base(area, texture, crop)
        {
            if (handler != null)
                Handler += handler;
            ScaleOnHover = scaleOnHover;
        }
        public override void HoverIn(Point p, Point o)
        {
            Game1.playSound("Cowboy_Footstep");
            if (!ScaleOnHover)
                return;
            InflateRegion(2, 2);
        }
        public override void HoverOut(Point p, Point o)
        {
            if (!ScaleOnHover)
                return;
            InflateRegion(-2, -2);
        }
        public override void LeftClick(Point p, Point o)
        {
            Game1.playSound("bigDeSelect");
            Handler?.Invoke(this, Parent, Parent.GetAttachedMenu());
        }
    }
}
