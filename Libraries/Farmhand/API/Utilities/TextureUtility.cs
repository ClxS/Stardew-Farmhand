using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Farmhand.API.Utilities
{
    public static class TextureUtility
    {
        public static void AddSpriteToSpritesheet(ref Texture2D spritesheet, Texture2D sprite, int spritesheetIndex, int spriteWidth, int spriteHeight)
        {
            var rect = Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, spritesheetIndex, spriteWidth,
                spriteHeight);

            var originalSize = spritesheet.Width * spritesheet.Height;
            Rectangle originalRect = new Rectangle(0, 0, spritesheet.Width, spritesheet.Height);
            var originalData = new Color[originalSize];
            var newData = new Color[spriteWidth * spriteHeight];
            spritesheet.GetData<Color>(originalData);
            sprite.GetData<Color>(newData);
            if (rect.Bottom > spritesheet.Height || rect.Right > spritesheet.Width)
            {
                spritesheet = new Texture2D(Game1.graphics.GraphicsDevice, Math.Max(rect.Right, spritesheet.Width), Math.Max(rect.Bottom, spritesheet.Height));
                spritesheet.SetData<Color>(0, originalRect, originalData, 0, originalSize);
            }
            spritesheet.SetData<Color>(0, rect, newData, 0, spriteWidth * spriteHeight);
        }
    }
}
