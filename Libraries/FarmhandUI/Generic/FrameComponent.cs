namespace Farmhand.UI.Generic
{
    using Farmhand.UI.Base;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;
    using StardewValley.Menus;

    /// <summary>
    ///     An empty frame component.
    /// </summary>
    public class FrameComponent : BaseMenuComponent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FrameComponent" /> class.
        /// </summary>
        /// <param name="area">
        ///     The bounding area of this component.
        /// </param>
        /// <param name="texture">
        ///     The frame texture.
        /// </param>
        /// <param name="crop">
        ///     The cropped area of this component.
        /// </param>
        public FrameComponent(Rectangle area, Texture2D texture, Rectangle? crop = null)
            : base(area, texture, crop)
        {
        }

        /// <summary>
        ///     The draw handler for this component
        /// </summary>
        /// <param name="b">
        ///     The sprite batch to use
        /// </param>
        /// <param name="o">
        ///     The origin for this component
        /// </param>
        public override void Draw(SpriteBatch b, Point o)
        {
            if (this.Visible)
            {
                IClickableMenu.drawTextureBox(
                    b,
                    this.Texture,
                    this.Crop,
                    this.Area.X + o.X,
                    this.Area.Y + o.Y,
                    this.Area.Width,
                    this.Area.Height,
                    Color.White,
                    Game1.pixelZoom,
                    false);
            }
        }
    }
}