namespace Farmhand.Graphics.PostProcessing
{
    using System;

    using Farmhand.Attributes;
    using Farmhand.Events;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Game = Farmhand.API.Game;

    /// <summary>
    ///     The post processing helper.
    /// </summary>
    public static class PostProcessing
    {
        /// <summary>
        ///     Gets the common effect target.
        /// </summary>
        /// <remarks>
        ///     The common effect target can be used my mods for their post-processing effects.
        ///     It is ensured to be the same size and format as the game's primary render target.
        ///     As it can be used by multiple mods, you have to make sure you are done using it within
        ///     a single render event function.
        /// </remarks>
        public static RenderTarget2D CommonEffectTarget { get; private set; }

        [Hook(HookType.Entry, "StardewValley.Game1", ".ctor")]
        internal static void AttachListeners()
        {
            GraphicsEvents.BeforeDraw += GraphicsEvents_BeforeDraw;
        }

        private static void GraphicsEvents_BeforeDraw(object sender, EventArgs e)
        {
            EnsureTargetSizeMatches(Game.Screen);
        }

        /// <summary>
        ///     Changes the render target to the specified target and draws a full screen quad
        /// </summary>
        /// <param name="spriteBatch">
        ///     The sprite batch.
        /// </param>
        /// <param name="texture">
        ///     The texture.
        /// </param>
        /// <param name="renderTarget">
        ///     The render target.
        /// </param>
        /// <param name="drawEffect">
        ///     The draw effect.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     If spriteBatch, texture, or renderTarget are null.
        /// </exception>
        public static void DrawFullscreenQuad(
            SpriteBatch spriteBatch,
            Texture2D texture,
            RenderTarget2D renderTarget,
            Effect drawEffect)
        {
            Game.GraphicsDevice.SetRenderTarget(renderTarget);

            if (spriteBatch == null)
            {
                throw new ArgumentNullException(nameof(spriteBatch));
            }

            if (texture == null)
            {
                throw new ArgumentNullException(nameof(texture));
            }

            if (renderTarget == null)
            {
                throw new ArgumentNullException(nameof(renderTarget));
            }

            if (drawEffect != null)
            {
                spriteBatch.Begin(0, BlendState.Opaque, null, null, null, drawEffect);
            }
            else
            {
                spriteBatch.Begin(0, BlendState.Opaque, null, null, null);
            }

            spriteBatch.Draw(texture, new Rectangle(0, 0, renderTarget.Width, renderTarget.Height), Color.White);
            spriteBatch.End();
        }

        private static void EnsureTargetSizeMatches(RenderTarget2D screen)
        {
            if (CommonEffectTarget == null || CommonEffectTarget.Width != screen.Width
                || CommonEffectTarget.Height != screen.Height)
            {
                CommonEffectTarget = new RenderTarget2D(
                    Game.GraphicsDevice,
                    screen.Width,
                    screen.Height,
                    false,
                    screen.Format,
                    screen.DepthStencilFormat);
            }
        }
    }
}