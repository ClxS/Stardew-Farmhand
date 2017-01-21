namespace VignetteMod
{
    using System;

    using Farmhand.API;
    using Farmhand.Events;
    using Farmhand.Events.Arguments.GraphicsEvents;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Game = Farmhand.API.Game;

    internal class VignetteMod : Farmhand.Mod
    {
        private Effect effect;

        private RenderTarget2D effectTarget;

        public override void Entry()
        {
            GameEvents.AfterLoadedContent += this.GameEvents_AfterLoadedContent;
            GraphicsEvents.PreRenderHudEventNoCheck += this.GraphicsEvents_PreRenderHudEventNoCheck;
        }

        private void GameEvents_AfterLoadedContent(object sender, EventArgs e)
        {
            this.effect = Content.GetContentManagerForMod(this).Load<Effect>("Effects/Vignette.Xna");
        }

        private void GraphicsEvents_PreRenderHudEventNoCheck(object sender, DrawEventArgs e)
        {
            // End the already running one
            e.SpriteBatch.End();

            this.EnsureTargetSizeMatches(e.Screen);

            this.DrawFullscreenQuad(e.SpriteBatch, e.Screen, this.effectTarget, this.effect);
            this.DrawFullscreenQuad(e.SpriteBatch, this.effectTarget, e.Screen, this.effect);

            // Restore the previous sprite settings
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
        }

        private void EnsureTargetSizeMatches(RenderTarget2D screen)
        {
            if (this.effectTarget == null || this.effectTarget.Width != screen.Width
                || this.effectTarget.Height != screen.Height)
            {
                this.effectTarget = new RenderTarget2D(
                    Game.GraphicsDevice,
                    screen.Width,
                    screen.Height,
                    false,
                    screen.Format,
                    screen.DepthStencilFormat);
            }
        }
        
        private void DrawFullscreenQuad(
            SpriteBatch spriteBatch,
            Texture2D texture,
            RenderTarget2D renderTarget,
            Effect drawEffect)
        {
            Game.GraphicsDevice.SetRenderTarget(renderTarget);

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
    }
}