namespace Farmhand.UI.Components.Controls
{
    using Farmhand.UI.Base;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    ///     A texture component.
    /// </summary>
    public class TextureComponent : BaseMenuComponent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TextureComponent" /> class.
        /// </summary>
        /// <param name="area">
        ///     The bounding area of this component.
        /// </param>
        /// <param name="texture">
        ///     The texture to display.
        /// </param>
        /// <param name="crop">
        ///     The cropped area of this component.
        /// </param>
        public TextureComponent(Rectangle area, Texture2D texture, Rectangle? crop = null)
            : base(area, texture, crop)
        {
        }
    }
}