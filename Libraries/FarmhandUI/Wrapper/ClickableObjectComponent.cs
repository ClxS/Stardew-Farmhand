using Farmhand.UI.Generic;
using Microsoft.Xna.Framework;
using StardewValley;

namespace Farmhand.UI.Wrapper
{
    class ClickableObjectComponent : ClickableTextureComponent
    {
        public ClickableObjectComponent(Point position, int index, ClickHandler handler = null, bool scaleOnHover = true) : base(new Rectangle(position.X, position.Y, 16, 16), Game1.objectSpriteSheet, handler, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, index, 16, 16), scaleOnHover)
        {

        }
    }
}
