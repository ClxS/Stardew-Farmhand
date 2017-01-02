namespace Farmhand.API.Utilities
{
    using System;

    using Farmhand.Logging;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    ///     Utility to aid with texture editing
    /// </summary>
    public static class TextureUtility
    {
        /// <summary>
        ///     Adds a sprite to a spritesheet.
        /// </summary>
        /// <param name="spritesheet">
        ///     The spritesheet to edit.
        /// </param>
        /// <param name="sprite">
        ///     The sprite to insert.
        /// </param>
        /// <param name="spritesheetIndex">
        ///     The sprite's spritesheet index.
        /// </param>
        /// <param name="spriteWidth">
        ///     The sprite width.
        /// </param>
        /// <param name="spriteHeight">
        ///     The sprite height.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if either spritesheet or sprite are null.
        /// </exception>
        public static void AddSpriteToSpritesheet(
            ref Texture2D spritesheet,
            Texture2D sprite,
            int spritesheetIndex,
            int spriteWidth,
            int spriteHeight)
        {
            if (spritesheet == null)
            {
                throw new ArgumentNullException(nameof(spritesheet));
            }

            if (sprite == null)
            {
                throw new ArgumentNullException(nameof(sprite));
            }

            var rect = Game1.getSourceRectForStandardTileSheet(spritesheet, spritesheetIndex, spriteWidth, spriteHeight);
            spritesheet = PatchTexture(spritesheet, sprite, new Rectangle(0, 0, spriteWidth, spriteHeight), rect);
        }

        /// <summary>
        ///     Patches one texture into another texture.
        /// </summary>
        /// <param name="base">
        ///     The texture to edit.
        /// </param>
        /// <param name="input">
        ///     The texture to patch in.
        /// </param>
        /// <param name="source">
        ///     The source from the input texture.
        /// </param>
        /// <param name="destination">
        ///     The destination to patch in the target texture.
        /// </param>
        /// <param name="asNewTexture">
        ///     Whether a new texture should be created with the resulting sprite, or if it should edit the
        ///     target in-place
        /// </param>
        /// <returns>
        ///     The patched <see cref="Texture2D" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if @base, input, source, or destination are null.
        /// </exception>
        public static Texture2D PatchTexture(
            Texture2D @base,
            Texture2D input,
            Rectangle source,
            Rectangle destination,
            bool asNewTexture = false)
        {
            if (@base == null)
            {
                throw new ArgumentNullException(nameof(@base));
            }

            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (source.Width * source.Height != destination.Width * destination.Height)
            {
                Log.Exception(
                    "Error patching texture",
                    new Exception(
                        $"Texture source and destination must match when trying to patch a texture ({source.Width}x{source.Height}) "
                        + $"vs ({destination.Width}x{destination.Height})"));
            }

            var newData = new Color[source.Width * source.Height];
            input.GetData(0, source, newData, 0, source.Width * source.Height);

            if (asNewTexture || destination.Bottom > @base.Height || destination.Right > @base.Width)
            {
                var originalRect = new Rectangle(0, 0, @base.Width, @base.Height);
                var originalSize = @base.Width * @base.Height;
                var originalData = new Color[originalSize];
                @base.GetData(originalData);
                @base = new Texture2D(
                    Game1.graphics.GraphicsDevice,
                    Math.Max(destination.Right, @base.Width),
                    Math.Max(destination.Bottom, @base.Height));
                @base.SetData(0, originalRect, originalData, 0, originalSize);
            }

            @base.SetData(0, destination, newData, 0, destination.Width * destination.Height);

            return @base;
        }
    }
}