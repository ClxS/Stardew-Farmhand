namespace VignetteMod
{
    using System;

    using Farmhand.API;
    using Farmhand.Events;
    using Farmhand.Events.Arguments.GraphicsEvents;
    using Farmhand.Graphics.PostProcessing;

    using Microsoft.Xna.Framework.Graphics;

    internal class Mod : Farmhand.Mod
    {
        private Effect effect;

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

            PostProcessing.DrawFullscreenQuad(e.SpriteBatch, e.Screen, PostProcessing.CommonEffectTarget, this.effect);
            PostProcessing.DrawFullscreenQuad(e.SpriteBatch, PostProcessing.CommonEffectTarget, e.Screen, this.effect);

            // Restore the previous sprite settings
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
        }
    }
}