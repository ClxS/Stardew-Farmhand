namespace Farmhand.UI.Wrapper
{
    using Farmhand.UI.Generic;

    using Microsoft.Xna.Framework;

    using StardewValley;

    /// <summary>
    ///     An object component.
    /// </summary>
    internal class ObjectComponent : TextureComponent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ObjectComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of the component.
        /// </param>
        /// <param name="index">
        ///     The tile sheet index of this sprite.
        /// </param>
        public ObjectComponent(Point position, int index)
            : base(
                new Rectangle(position.X, position.Y, 16, 16),
                Game1.objectSpriteSheet,
                Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, index, 16, 16))
        {
        }
    }
}