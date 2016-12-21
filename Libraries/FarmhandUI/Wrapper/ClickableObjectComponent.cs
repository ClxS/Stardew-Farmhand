namespace Farmhand.UI.Wrapper
{
    using Farmhand.UI.Generic;

    using Microsoft.Xna.Framework;

    using StardewValley;

    /// <summary>
    ///     A clickable object component.
    /// </summary>
    internal class ClickableObjectComponent : ClickableTextureComponent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ClickableObjectComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of the component.
        /// </param>
        /// <param name="index">
        ///     The tile sheet index for this sprite.
        /// </param>
        /// <param name="handler">
        ///     The click event handler.
        /// </param>
        /// <param name="scaleOnHover">
        ///     Whether scale on hover is enabled.
        /// </param>
        public ClickableObjectComponent(
            Point position,
            int index,
            ClickHandler handler = null,
            bool scaleOnHover = true)
            : base(
                new Rectangle(position.X, position.Y, 16, 16),
                Game1.objectSpriteSheet,
                handler,
                Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, index, 16, 16),
                scaleOnHover)
        {
        }
    }
}