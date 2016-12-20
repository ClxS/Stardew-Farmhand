using Farmhand.UI.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmhand.UI.Generic
{
    public class TextureComponent : BaseMenuComponent
    {
        public TextureComponent(Rectangle area, Texture2D texture, Rectangle? crop = null) : base(area, texture, crop)
        {

        }
    }
}