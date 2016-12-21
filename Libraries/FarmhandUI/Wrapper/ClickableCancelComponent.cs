namespace Farmhand.UI.Wrapper
{
    using Farmhand.UI.Generic;

    using Microsoft.Xna.Framework;

    using StardewValley;

    internal class ClickableCancelComponent : ClickableTextureComponent
    {
        public ClickableCancelComponent(Point position, ClickHandler handler = null)
            : base(
                new Rectangle(position.X, position.Y, 16, 16),
                Game1.mouseCursors,
                handler,
                Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47))
        {
        }
    }
}