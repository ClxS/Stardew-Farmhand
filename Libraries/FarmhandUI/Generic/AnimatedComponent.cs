using Farmhand.UI.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Farmhand.UI.Generic
{
    public class AnimatedComponent : BaseMenuComponent
    {
        protected TemporaryAnimatedSprite Sprite;
        public AnimatedComponent(Point position, TemporaryAnimatedSprite sprite)
        {
            SetScaledArea(new Rectangle(position.X,position.Y,sprite.sourceRect.Width,sprite.sourceRect.Height));
            Sprite = sprite;
        }
        public override void Update(GameTime t)
        {
            Sprite.update(t);
        }
        public override void Draw(SpriteBatch b, Point o)
        {
            if (Visible)
                Sprite.draw(b, false, o.X+Area.X, o.Y+Area.Y);
        }
    }
}
