using Farmhand.UI.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Farmhand.UI.Generic
{
    public class ClickableAnimatedComponent : BaseInteractiveMenuComponent
    {
        protected TemporaryAnimatedSprite Sprite;
        protected bool ScaleOnHover;
        public event ClickHandler Handler;
        public ClickableAnimatedComponent(Rectangle area, TemporaryAnimatedSprite sprite, ClickHandler handler = null, bool scaleOnHover = true)
        {
            if (handler != null)
                Handler += handler;
            ScaleOnHover = scaleOnHover;
            Sprite = sprite;
            SetScaledArea(area);
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
            Handler?.Invoke(this,Parent,Parent.GetAttachedMenu());
        }
        public override void Update(GameTime t)
        {
            Sprite.update(t);
        }
        public override void Draw(SpriteBatch b, Point offset)
        {
            if (Visible)
                Sprite.draw(b, false, offset.X+Area.X, offset.Y+Area.Y);
        }
    }
}
