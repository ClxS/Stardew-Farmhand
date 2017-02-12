namespace VignetteMod
{
    using System;

    using Farmhand.API;
    using Farmhand.Events;
    using Farmhand.Events.Arguments.GraphicsEvents;
    using Farmhand.Graphics.PostProcessing;

    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    internal class Mod : Farmhand.Mod
    {
        private Effect effect;

        private EffectParameter falloffParameter;

        private EffectParameter powerParameter;

        private float power;

        private float falloff;

        public override void Entry()
        {
            GameEvents.AfterLoadedContent += this.GameEvents_AfterLoadedContent;
            GraphicsEvents.PreRenderHudEventNoCheck += this.GraphicsEvents_PreRenderHudEventNoCheck;
            ControlEvents.KeyPressed += this.ControlEvents_KeyPressed;

            this.power = 15.0f;
            this.falloff = 0.25f;
        }

        private void ControlEvents_KeyPressed(object sender, Farmhand.Events.Arguments.ControlEvents.KeyPressedEventArgs e)
        {
            if (e.KeyPressed == Keys.NumPad9)
            {
                this.power += 1.0f;
                this.powerParameter.SetValue(this.power);
            }
            else if (e.KeyPressed == Keys.NumPad6)
            {
                this.power -= 1.0f;
                this.powerParameter.SetValue(this.power);
            }
            else if (e.KeyPressed == Keys.NumPad8)
            {
                this.falloff += 0.05f;
                this.falloffParameter.SetValue(this.falloff);
            }
            else if (e.KeyPressed == Keys.NumPad5)
            {
                this.falloff -= 0.05f;
                this.falloffParameter.SetValue(this.falloff);
            }
        }

        private void GameEvents_AfterLoadedContent(object sender, EventArgs e)
        {
            this.effect = Content.GetContentManagerForMod(this).Load<Effect>("Effects/Vignette");
            this.powerParameter = this.effect.Parameters["Power"];
            this.falloffParameter = this.effect.Parameters["Falloff"];
            this.powerParameter.SetValue(this.power);
            this.falloffParameter.SetValue(this.falloff);
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