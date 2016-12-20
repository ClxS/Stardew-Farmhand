using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardewValley;
using StardewValley.Menus;

namespace Farmhand.UI
{
    public class FrameComponent : BaseMenuComponent
    {
        public FrameComponent(Rectangle area, Texture2D texture, Rectangle? crop=null) : base(area,texture,crop)
        {
            
        }
        public override void Draw(SpriteBatch b, Point o)
        {
            if(Visible)
                IClickableMenu.drawTextureBox(b, Texture, Crop, Area.X + o.X, Area.Y + o.Y, Area.Width, Area.Height, Color.White, Game1.pixelZoom, false);
        }
    }
}
