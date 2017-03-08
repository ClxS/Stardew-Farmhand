namespace Farmhand.API.Utilities
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Farmhand.Events;
    using Farmhand.Logging;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    ///     Utility to aid with texture loading and editing
    /// </summary>
    public static class TextureUtility
    {
        private static readonly BlendState BlendColorBlendState;

        private static readonly BlendState BlendAlphaBlendState;

        static TextureUtility()
        {
            BlendColorBlendState = new BlendState
                                       {
                                           ColorDestinationBlend = Blend.Zero,
                                           ColorWriteChannels =
                                               ColorWriteChannels.Red | ColorWriteChannels.Green
                                               | ColorWriteChannels.Blue,
                                           AlphaDestinationBlend = Blend.Zero,
                                           AlphaSourceBlend = Blend.SourceAlpha,
                                           ColorSourceBlend = Blend.SourceAlpha
                                       };

            BlendAlphaBlendState = new BlendState
                                       {
                                           ColorWriteChannels = ColorWriteChannels.Alpha,
                                           AlphaDestinationBlend = Blend.Zero,
                                           ColorDestinationBlend = Blend.Zero,
                                           AlphaSourceBlend = Blend.One,
                                           ColorSourceBlend = Blend.One
                                       };
        }

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

        /// <summary>
        ///     Loads a texture from a Stream and premultiplies it.
        /// </summary>
        /// <param name="stream">The stream to load the texture from.</param>
        /// <param name="preMultiplyAlpha">Whether the alpha should be premultiplied. Defaults to true.</param>
        /// <returns>The loaded texture.</returns>
        public static Texture2D FromStream(Stream stream, bool preMultiplyAlpha = true)
        {
            var texture = Texture2D.FromStream(Game1.graphics.GraphicsDevice, stream);
            if (!preMultiplyAlpha)
            {
                return texture;
            }

            texture = GraphicsEvents.IsMidDrawCall ? PremultiplyCpu(texture) : PremultiplyGpu(texture);
            return texture;
        }

        /// <summary>
        ///     Premultiplies the texture using a CPU method. This is considerably slower than
        ///     the GPU version, but is safe to use mid-draw call.
        /// </summary>
        /// <param name="texture">Texture to premultiply</param>
        /// <returns>A premultiplied texture.</returns>
        public static Texture2D PremultiplyCpu(Texture2D texture)
        {
            Log.Warning("Using PremultiplyCpu - Try to refactor your code to load the texture outside of the draw loop");

            var data = new Color[texture.Width * texture.Height];
            texture.GetData(data);
            Parallel.For(0, data.Length, i => { data[i] = Color.FromNonPremultiplied(data[i].ToVector4()); });

            texture.SetData(data);
            return texture;
        }

        /// <summary>
        ///     Loads a texture from a stream and premultiplies it using the GPU. This cannot safely be used mid-draw call
        /// </summary>
        /// <remarks>
        ///     Uses a slight variation of the method posted
        ///     here: https://gist.github.com/Layoric/6255384
        /// </remarks>
        /// <param name="texture">Texture to premultiply</param>
        /// <returns>
        ///     A premultiplied texture.
        /// </returns>
        public static Texture2D PremultiplyGpu(Texture2D texture)
        {
            // Setup a render target to hold our final texture which will have premulitplied alpha values
            using (var renderTarget = new RenderTarget2D(Game1.graphics.GraphicsDevice, texture.Width, texture.Height))
            {
                var spriteBatch = new SpriteBatch(Game1.graphics.GraphicsDevice);

                var viewportBackup = Game1.graphics.GraphicsDevice.Viewport;
                Game1.graphics.GraphicsDevice.SetRenderTarget(renderTarget);
                Game1.graphics.GraphicsDevice.Clear(Color.Black);

                // Multiply each color by the source alpha, and write in just the color values into the final texture
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendColorBlendState);
                spriteBatch.Draw(texture, texture.Bounds, Color.White);
                spriteBatch.End();

                // Now copy over the alpha values from the source texture to the final one, without multiplying them
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendAlphaBlendState);
                spriteBatch.Draw(texture, texture.Bounds, Color.White);
                spriteBatch.End();

                // Release the GPU back to drawing to the screen
                Game1.graphics.GraphicsDevice.SetRenderTarget(null);
                Game1.graphics.GraphicsDevice.Viewport = viewportBackup;

                // Store data from render target because the RenderTarget2D is volatile
                var data = new Color[texture.Width * texture.Height];
                renderTarget.GetData(data);

                // Unset texture from graphic device and set modified data back to it
                Game1.graphics.GraphicsDevice.Textures[0] = null;
                texture.SetData(data);
            }

            return texture;
        }
    }
}