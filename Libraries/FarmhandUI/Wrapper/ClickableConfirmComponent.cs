namespace Farmhand.UI.Wrapper
{
    using Farmhand.UI.Generic;

    using Microsoft.Xna.Framework;

    using StardewValley;

    /// <summary>
    ///     A clickable confirm component.
    /// </summary>
    internal class ClickableConfirmComponent : ClickableTextureComponent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ClickableConfirmComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of the component.
        /// </param>
        /// <param name="handler">
        ///     The click event handler.
        /// </param>
        public ClickableConfirmComponent(Point position, ClickHandler handler = null)
            : base(
                new Rectangle(position.X, position.Y, 16, 16),
                Game1.mouseCursors,
                handler,
                Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46))
        {
        }
    }
}