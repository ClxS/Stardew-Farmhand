namespace Farmhand.UI.Components.Wrapper
{
    using Farmhand.UI.Components.Controls;

    using Microsoft.Xna.Framework;

    using StardewValley;

    internal class ObjectComponent : TextureComponent
    {
        public ObjectComponent(Point position, int index)
            : base(
                new Rectangle(position.X, position.Y, 16, 16),
                Game1.objectSpriteSheet,
                Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, index, 16, 16))
        {
        }
    }
}