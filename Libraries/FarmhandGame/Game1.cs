using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Tools;
using xTile.Dimensions;
using Farmhand.Events;

// ReSharper disable once CheckNamespace
namespace Farmhand.Overrides
{
    /// <summary>
    /// Overrides Stardew's Game1, allowing for advanced callback events to be added
    /// </summary>
    public class Game1 : Farmhand.Overrides.GameOverrideBase
    {
        private readonly Dictionary<string, Action<GameTime>> _versionSpecificOverrides = new Dictionary<string, Action<GameTime>>();

        public Game1()
        {
        }

        protected override void Draw(GameTime gameTime)
        {
            if (Farmhand.Constants.OverrideGameDraw)
            {
                GraphicsEvents.InvokeBeforeDraw(this);
                if (_versionSpecificOverrides.ContainsKey(Game1.version))
                {
                    _versionSpecificOverrides[Game1.version](gameTime);
                }
                else
                {
                    DefaultDraw(gameTime);
                }
                GraphicsEvents.InvokeAfterDraw(this);
            }
            else
            {
                base.Draw(gameTime);
            }
        }

        private void DefaultDraw(GameTime gameTime)
        {
            if (Math.Abs((double)Game1.options.zoomLevel - 1.0) > 0.001f)
                this.GraphicsDevice.SetRenderTarget(this.screen);
            ++Game1.framesThisSecond;
            this.GraphicsDevice.Clear(this.bgColor);
            if (Game1.options.showMenuBackground && Game1.activeClickableMenu != null && Game1.activeClickableMenu.showWithoutTransparencyIfOptionIsSet())
            {
                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
                Game1.activeClickableMenu.drawBackground(Game1.spriteBatch);
                GraphicsEvents.InvokeOnPreRenderGuiEvent(this);
                Game1.activeClickableMenu.draw(Game1.spriteBatch);
                GraphicsEvents.InvokeOnPostRenderGuiEvent(this);
                Game1.spriteBatch.End();
                if (Math.Abs((double)Game1.options.zoomLevel - 1.0) < 0.001f)
                    return;
                this.GraphicsDevice.SetRenderTarget((RenderTarget2D)null);
                this.GraphicsDevice.Clear(this.bgColor);
                Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                Game1.spriteBatch.Draw((Texture2D)this.screen, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(this.screen.Bounds), Color.White, 0.0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
                Game1.spriteBatch.End();
            }
            else if ((int)Game1.gameMode == 11)
            {
                Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
                Game1.spriteBatch.DrawString(Game1.smoothFont, "Stardew Valley has crashed...", new Vector2(16f, 16f), Color.HotPink);
                Game1.spriteBatch.DrawString(Game1.smoothFont, "Please send the error report or a screenshot of this message to @ConcernedApe. (http://stardewvalley.net/contact/)", new Vector2(16f, 32f), new Color(0, (int)byte.MaxValue, 0));
                Game1.spriteBatch.DrawString(Game1.smoothFont, Game1.parseText(Game1.errorMessage, Game1.smoothFont, Game1.graphics.GraphicsDevice.Viewport.Width), new Vector2(16f, 48f), Color.White);
                Game1.spriteBatch.End();
            }
            else if (Game1.currentMinigame != null)
            {
                Game1.currentMinigame.draw(Game1.spriteBatch);
                if (Game1.globalFade && !Game1.menuUp && (!Game1.nameSelectUp || Game1.messagePause))
                {
                    Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
                    Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * ((int)Game1.gameMode == 0 ? 1f - Game1.fadeToBlackAlpha : Game1.fadeToBlackAlpha));
                    Game1.spriteBatch.End();
                }
                if (Math.Abs((double)Game1.options.zoomLevel - 1.0) < 0.001f)
                    return;
                this.GraphicsDevice.SetRenderTarget((RenderTarget2D)null);
                this.GraphicsDevice.Clear(this.bgColor);
                Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                Game1.spriteBatch.Draw((Texture2D)this.screen, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(this.screen.Bounds), Color.White, 0.0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
                Game1.spriteBatch.End();
            }
            else if (Game1.showingEndOfNightStuff)
            {
                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
                if (Game1.activeClickableMenu != null)
                    Game1.activeClickableMenu.draw(Game1.spriteBatch);
                Game1.spriteBatch.End();
                if (Math.Abs((double)Game1.options.zoomLevel - 1.0) < 0.001f)
                    return;
                this.GraphicsDevice.SetRenderTarget((RenderTarget2D)null);
                this.GraphicsDevice.Clear(this.bgColor);
                Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                Game1.spriteBatch.Draw((Texture2D)this.screen, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(this.screen.Bounds), Color.White, 0.0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
                Game1.spriteBatch.End();
            }
            else if ((int)Game1.gameMode == 6)
            {
                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
                string str = "";
                for (int index = 0; (double)index < gameTime.TotalGameTime.TotalMilliseconds % 999.0 / 333.0; ++index)
                    str += ".";
                SpriteText.drawString(Game1.spriteBatch, "Loading" + str, 64, Game1.graphics.GraphicsDevice.Viewport.Height - 64, 999, -1, 999, 1f, 1f, false, 0, "Loading...", -1);
                Game1.spriteBatch.End();
                if (Math.Abs((double)Game1.options.zoomLevel - 1.0) < 0.001f)
                    return;
                this.GraphicsDevice.SetRenderTarget((RenderTarget2D)null);
                this.GraphicsDevice.Clear(this.bgColor);
                Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                Game1.spriteBatch.Draw((Texture2D)this.screen, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(this.screen.Bounds), Color.White, 0.0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
                Game1.spriteBatch.End();
            }
            else
            {
                if ((int)Game1.gameMode == 0)
                {
                    Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
                }
                else
                {
                    if (Game1.drawLighting)
                    {
                        this.GraphicsDevice.SetRenderTarget(Game1.lightmap);
                        this.GraphicsDevice.Clear(Color.White * 0.0f);
                        Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
                        Game1.spriteBatch.Draw(Game1.staminaRect, Game1.lightmap.Bounds, Game1.currentLocation.name.Equals("UndergroundMine") ? Game1.mine.getLightingColor(gameTime) : (Game1.ambientLight.Equals(Color.White) || Game1.isRaining && Game1.currentLocation.isOutdoors ? Game1.outdoorLight : Game1.ambientLight));
                        for (var index = 0; index < Game1.currentLightSources.Count; ++index)
                        {
                            if (Utility.isOnScreen(((IEnumerable<LightSource>)Game1.currentLightSources).ElementAt<LightSource>(index).position, (int)((double)((IEnumerable<LightSource>)Game1.currentLightSources).ElementAt<LightSource>(index).radius * (double)Game1.tileSize * 4.0)))
                                Game1.spriteBatch.Draw(((IEnumerable<LightSource>)Game1.currentLightSources).ElementAt<LightSource>(index).lightTexture, Game1.GlobalToLocal(Game1.viewport, ((IEnumerable<LightSource>)Game1.currentLightSources).ElementAt<LightSource>(index).position) / (float)Game1.options.lightingQuality, new Microsoft.Xna.Framework.Rectangle?(Enumerable.ElementAt<LightSource>((IEnumerable<LightSource>)Game1.currentLightSources, index).lightTexture.Bounds), Enumerable.ElementAt<LightSource>((IEnumerable<LightSource>)Game1.currentLightSources, index).color, 0.0f, new Vector2((float)Enumerable.ElementAt<LightSource>((IEnumerable<LightSource>)Game1.currentLightSources, index).lightTexture.Bounds.Center.X, (float)Enumerable.ElementAt<LightSource>((IEnumerable<LightSource>)Game1.currentLightSources, index).lightTexture.Bounds.Center.Y), Enumerable.ElementAt<LightSource>((IEnumerable<LightSource>)Game1.currentLightSources, index).radius / (float)Game1.options.lightingQuality, SpriteEffects.None, 0.9f);
                        }
                        Game1.spriteBatch.End();
                        this.GraphicsDevice.SetRenderTarget(Math.Abs((double)Game1.options.zoomLevel - 1.0) < 0.001f ? (RenderTarget2D)null : this.screen);
                    }
                    if (Game1.bloomDay && Game1.bloom != null)
                        Game1.bloom.BeginDraw();
                    this.GraphicsDevice.Clear(this.bgColor);
                    Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
                    GraphicsEvents.InvokeOnPreRenderEvent(this);
                    if (Game1.background != null)
                        Game1.background.draw(Game1.spriteBatch);
                    Game1.mapDisplayDevice.BeginScene(Game1.spriteBatch);
                    Game1.currentLocation.Map.GetLayer("Back").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
                    Game1.currentLocation.drawWater(Game1.spriteBatch);
                    if (Game1.CurrentEvent == null)
                    {
                        foreach (var npc in Game1.currentLocation.characters)
                        {
                            if (!npc.swimming && !npc.hideShadow && (!npc.IsMonster && !Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(npc.getTileLocation())))
                                Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, npc.position + new Vector2((float)(npc.sprite.spriteWidth * Game1.pixelZoom) / 2f, (float)(npc.GetBoundingBox().Height + (npc.IsMonster ? 0 : Game1.pixelZoom * 3)))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0.0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), ((float)Game1.pixelZoom + (float)npc.yJumpOffset / 40f) * npc.scale, SpriteEffects.None, Math.Max(0.0f, (float)npc.getStandingY() / 10000f) - 1E-06f);
                        }
                    }
                    else
                    {
                        foreach (NPC npc in Game1.CurrentEvent.actors)
                        {
                            if (!npc.swimming && !npc.hideShadow && !Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(npc.getTileLocation()))
                                Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, npc.position + new Vector2((float)(npc.sprite.spriteWidth * Game1.pixelZoom) / 2f, (float)(npc.GetBoundingBox().Height + (npc.IsMonster ? 0 : Game1.pixelZoom * 3)))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0.0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), ((float)Game1.pixelZoom + (float)npc.yJumpOffset / 40f) * npc.scale, SpriteEffects.None, Math.Max(0.0f, (float)npc.getStandingY() / 10000f) - 1E-06f);
                        }
                    }
                    if (!Game1.player.swimming && !Game1.player.isRidingHorse() && !Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(Game1.player.getTileLocation()))
                        Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.player.position + new Vector2(32f, 24f)), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0.0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), (float)(4.0 - (!Game1.player.running && !Game1.player.usingTool || Game1.player.FarmerSprite.indexInCurrentAnimation <= 1 ? 0.0 : (double)Math.Abs(FarmerRenderer.featureYOffsetPerFrame[Game1.player.FarmerSprite.CurrentFrame]) * 0.5)), SpriteEffects.None, 0.0f);
                    Game1.currentLocation.Map.GetLayer("Buildings").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
                    Game1.mapDisplayDevice.EndScene();
                    Game1.spriteBatch.End();
                    Game1.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
                    if (Game1.CurrentEvent == null)
                    {
                        foreach (NPC npc in Game1.currentLocation.characters)
                        {
                            if (!npc.swimming && !npc.hideShadow && Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(npc.getTileLocation()))
                                Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, npc.position + new Vector2((float)(npc.sprite.spriteWidth * Game1.pixelZoom) / 2f, (float)(npc.GetBoundingBox().Height + (npc.IsMonster ? 0 : Game1.pixelZoom * 3)))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0.0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), ((float)Game1.pixelZoom + (float)npc.yJumpOffset / 40f) * npc.scale, SpriteEffects.None, Math.Max(0.0f, (float)npc.getStandingY() / 10000f) - 1E-06f);
                        }
                    }
                    else
                    {
                        foreach (NPC npc in Game1.CurrentEvent.actors)
                        {
                            if (!npc.swimming && !npc.hideShadow && Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(npc.getTileLocation()))
                                Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, npc.position + new Vector2((float)(npc.sprite.spriteWidth * Game1.pixelZoom) / 2f, (float)(npc.GetBoundingBox().Height + (npc.IsMonster ? 0 : Game1.pixelZoom * 3)))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0.0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), ((float)Game1.pixelZoom + (float)npc.yJumpOffset / 40f) * npc.scale, SpriteEffects.None, Math.Max(0.0f, (float)npc.getStandingY() / 10000f) - 1E-06f);
                        }
                    }
                    if (!Game1.player.swimming && !Game1.player.isRidingHorse() && Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(Game1.player.getTileLocation()))
                        Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.player.position + new Vector2(32f, 24f)), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0.0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), (float)(4.0 - (!Game1.player.running && !Game1.player.usingTool || Game1.player.FarmerSprite.indexInCurrentAnimation <= 1 ? 0.0 : (double)Math.Abs(FarmerRenderer.featureYOffsetPerFrame[Game1.player.FarmerSprite.CurrentFrame]) * 0.5)), SpriteEffects.None, Math.Max(0.0001f, (float)((double)Game1.player.getStandingY() / 10000.0 + 0.000110000000859145)) - 0.0001f);
                    if (Game1.displayFarmer)
                        Game1.player.draw(Game1.spriteBatch);
                    if ((Game1.eventUp || Game1.killScreen) && (!Game1.killScreen && Game1.currentLocation.currentEvent != null))
                        Game1.currentLocation.currentEvent.draw(Game1.spriteBatch);
                    if (Game1.player.currentUpgrade != null && Game1.player.currentUpgrade.daysLeftTillUpgradeDone <= 3 && Game1.currentLocation.Name.Equals("Farm"))
                        Game1.spriteBatch.Draw(Game1.player.currentUpgrade.workerTexture, Game1.GlobalToLocal(Game1.viewport, Game1.player.currentUpgrade.positionOfCarpenter), new Microsoft.Xna.Framework.Rectangle?(Game1.player.currentUpgrade.getSourceRectangle()), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, (float)(((double)Game1.player.currentUpgrade.positionOfCarpenter.Y + (double)(Game1.tileSize * 3 / 4)) / 10000.0));
                    Game1.currentLocation.draw(Game1.spriteBatch);
                    if (Game1.eventUp && Game1.currentLocation.currentEvent != null && Game1.currentLocation.currentEvent.messageToScreen != null)
                        Game1.drawWithBorder(Game1.currentLocation.currentEvent.messageToScreen, Color.Black, Color.White, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width / 2) - Game1.borderFont.MeasureString(Game1.currentLocation.currentEvent.messageToScreen).X / 2f, (float)(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Height - Game1.tileSize)), 0.0f, 1f, 0.999f);
                    if (Game1.player.ActiveObject == null && (Game1.player.UsingTool || Game1.pickingTool) && (Game1.player.CurrentTool != null && (!Game1.player.CurrentTool.Name.Equals("Seeds") || Game1.pickingTool)))
                        Game1.drawTool(Game1.player);
                    if (Game1.currentLocation.Name.Equals("Farm"))
                        this.drawFarmBuildings();
                    if (Game1.tvStation >= 0)
                        Game1.spriteBatch.Draw(Game1.tvStationTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(6 * Game1.tileSize + Game1.tileSize / 4), (float)(2 * Game1.tileSize + Game1.tileSize / 2))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(Game1.tvStation * 24, 0, 24, 15)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-08f);
                    if (Game1.panMode)
                    {
                        Game1.spriteBatch.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle((int)Math.Floor((double)(Game1.getOldMouseX() + Game1.viewport.X) / (double)Game1.tileSize) * Game1.tileSize - Game1.viewport.X, (int)Math.Floor((double)(Game1.getOldMouseY() + Game1.viewport.Y) / (double)Game1.tileSize) * Game1.tileSize - Game1.viewport.Y, Game1.tileSize, Game1.tileSize), Color.Lime * 0.75f);
                        foreach (Warp warp in Game1.currentLocation.warps)
                            Game1.spriteBatch.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(warp.X * Game1.tileSize - Game1.viewport.X, warp.Y * Game1.tileSize - Game1.viewport.Y, Game1.tileSize, Game1.tileSize), Color.Red * 0.75f);
                    }
                    Game1.mapDisplayDevice.BeginScene(Game1.spriteBatch);
                    Game1.currentLocation.Map.GetLayer("Front").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
                    Game1.mapDisplayDevice.EndScene();
                    Game1.currentLocation.drawAboveFrontLayer(Game1.spriteBatch);
                    Game1.spriteBatch.End();
                    Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
                    if (Game1.currentLocation.Name.Equals("Farm") && Game1.stats.SeedsSown >= 200U)
                    {
                        Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(3 * Game1.tileSize + Game1.tileSize / 4), (float)(Game1.tileSize + Game1.tileSize / 3))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
                        Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(4 * Game1.tileSize + Game1.tileSize), (float)(2 * Game1.tileSize + Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
                        Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(5 * Game1.tileSize), (float)(2 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
                        Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(3 * Game1.tileSize + Game1.tileSize / 2), (float)(3 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
                        Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(5 * Game1.tileSize - Game1.tileSize / 4), (float)Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
                        Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(4 * Game1.tileSize), (float)(3 * Game1.tileSize + Game1.tileSize / 6))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
                        Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(4 * Game1.tileSize + Game1.tileSize / 5), (float)(2 * Game1.tileSize + Game1.tileSize / 3))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
                    }
                    if (Game1.displayFarmer && Game1.player.ActiveObject != null && (Game1.player.ActiveObject.bigCraftable && this.checkBigCraftableBoundariesForFrontLayer()) && Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX(), Game1.player.getStandingY()), Game1.viewport.Size) == null)
                        Game1.drawPlayerHeldObject(Game1.player);
                    else if (Game1.displayFarmer && Game1.player.ActiveObject != null && (Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location((int)Game1.player.position.X, (int)Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size) != null && !Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location((int)Game1.player.position.X, (int)Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size).TileIndexProperties.ContainsKey("FrontAlways") || Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.GetBoundingBox().Right, (int)Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size) != null && !Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.GetBoundingBox().Right, (int)Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size).TileIndexProperties.ContainsKey("FrontAlways")))
                        Game1.drawPlayerHeldObject(Game1.player);
                    if ((Game1.player.UsingTool || Game1.pickingTool) && Game1.player.CurrentTool != null && ((!Game1.player.CurrentTool.Name.Equals("Seeds") || Game1.pickingTool) && (Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX(), (int)Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size) != null && Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX(), Game1.player.getStandingY()), Game1.viewport.Size) == null)))
                        Game1.drawTool(Game1.player);
                    if (Game1.currentLocation.Map.GetLayer("AlwaysFront") != null)
                    {
                        Game1.mapDisplayDevice.BeginScene(Game1.spriteBatch);
                        Game1.currentLocation.Map.GetLayer("AlwaysFront").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
                        Game1.mapDisplayDevice.EndScene();
                    }
                    if ((double)Game1.toolHold > 400.0 && Game1.player.CurrentTool.UpgradeLevel >= 1 && Game1.player.canReleaseTool)
                    {
                        Color color = Color.White;
                        switch ((int)((double)Game1.toolHold / 600.0) + 2)
                        {
                            case 1:
                                color = Tool.copperColor;
                                break;
                            case 2:
                                color = Tool.steelColor;
                                break;
                            case 3:
                                color = Tool.goldColor;
                                break;
                            case 4:
                                color = Tool.iridiumColor;
                                break;
                        }
                        Game1.spriteBatch.Draw(Game1.littleEffect, new Microsoft.Xna.Framework.Rectangle((int)Game1.player.getLocalPosition(Game1.viewport).X - 2, (int)Game1.player.getLocalPosition(Game1.viewport).Y - (Game1.player.CurrentTool.Name.Equals("Watering Can") ? 0 : Game1.tileSize) - 2, (int)((double)Game1.toolHold % 600.0 * 0.0799999982118607) + 4, Game1.tileSize / 8 + 4), Color.Black);
                        Game1.spriteBatch.Draw(Game1.littleEffect, new Microsoft.Xna.Framework.Rectangle((int)Game1.player.getLocalPosition(Game1.viewport).X, (int)Game1.player.getLocalPosition(Game1.viewport).Y - (Game1.player.CurrentTool.Name.Equals("Watering Can") ? 0 : Game1.tileSize), (int)((double)Game1.toolHold % 600.0 * 0.0799999982118607), Game1.tileSize / 8), color);
                    }
                    if (Game1.isDebrisWeather && Game1.currentLocation.IsOutdoors && (!Game1.currentLocation.ignoreDebrisWeather && !Game1.currentLocation.Name.Equals("Desert")) && Game1.viewport.X > -10)
                    {
                        foreach (WeatherDebris weatherDebris in Game1.debrisWeather)
                            weatherDebris.draw(Game1.spriteBatch);
                    }
                    if (Game1.farmEvent != null)
                        Game1.farmEvent.draw(Game1.spriteBatch);
                    if ((double)Game1.currentLocation.LightLevel > 0.0 && Game1.timeOfDay < 2000)
                        Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * Game1.currentLocation.LightLevel);
                    if (Game1.screenGlow)
                        Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Game1.screenGlowColor * Game1.screenGlowAlpha);
                    Game1.currentLocation.drawAboveAlwaysFrontLayer(Game1.spriteBatch);
                    if (Game1.player.CurrentTool != null && Game1.player.CurrentTool is FishingRod && ((Game1.player.CurrentTool as FishingRod).isTimingCast || (double)(Game1.player.CurrentTool as FishingRod).castingChosenCountdown > 0.0 || ((Game1.player.CurrentTool as FishingRod).fishCaught || (Game1.player.CurrentTool as FishingRod).showingTreasure)))
                        Game1.player.CurrentTool.draw(Game1.spriteBatch);
                    if (Game1.isRaining && Game1.currentLocation.IsOutdoors && (!Game1.currentLocation.Name.Equals("Desert") && !(Game1.currentLocation is Summit)) && (!Game1.eventUp || Game1.currentLocation.isTileOnMap(new Vector2((float)(Game1.viewport.X / Game1.tileSize), (float)(Game1.viewport.Y / Game1.tileSize)))))
                    {
                        for (int index = 0; index < Game1.rainDrops.Length; ++index)
                            Game1.spriteBatch.Draw(Game1.rainTexture, Game1.rainDrops[index].position, new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.rainTexture, Game1.rainDrops[index].frame, -1, -1)), Color.White);
                    }
                    Game1.spriteBatch.End();
                    base.Draw(gameTime);
                    Game1.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
                    if (Game1.eventUp && Game1.currentLocation.currentEvent != null)
                    {
                        foreach (NPC npc in Game1.currentLocation.currentEvent.actors)
                        {
                            if (npc.isEmoting)
                            {
                                Vector2 localPosition = npc.getLocalPosition(Game1.viewport);
                                localPosition.Y -= (float)(Game1.tileSize * 2 + Game1.pixelZoom * 3);
                                if (npc.age == 2)
                                    localPosition.Y += (float)(Game1.tileSize / 2);
                                else if (npc.gender == 1)
                                    localPosition.Y += (float)(Game1.tileSize / 6);
                                Game1.spriteBatch.Draw(Game1.emoteSpriteSheet, localPosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(npc.CurrentEmoteIndex * (Game1.tileSize / 4) % Game1.emoteSpriteSheet.Width, npc.CurrentEmoteIndex * (Game1.tileSize / 4) / Game1.emoteSpriteSheet.Width * (Game1.tileSize / 4), Game1.tileSize / 4, Game1.tileSize / 4)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float)npc.getStandingY() / 10000f);
                            }
                        }
                    }
                    Game1.spriteBatch.End();
                    if (Game1.drawLighting)
                    {
                        Game1.spriteBatch.Begin(SpriteSortMode.Deferred, new BlendState()
                        {
                            ColorBlendFunction = BlendFunction.ReverseSubtract,
                            ColorDestinationBlend = Blend.One,
                            ColorSourceBlend = Blend.SourceColor
                        }, SamplerState.LinearClamp, (DepthStencilState)null, (RasterizerState)null);
                        Game1.spriteBatch.Draw((Texture2D)Game1.lightmap, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(Game1.lightmap.Bounds), Color.White, 0.0f, Vector2.Zero, (float)Game1.options.lightingQuality, SpriteEffects.None, 1f);
                        if (Game1.isRaining && Game1.currentLocation.isOutdoors && !(Game1.currentLocation is Desert))
                            Game1.spriteBatch.Draw(Game1.staminaRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.OrangeRed * 0.45f);
                        Game1.spriteBatch.End();
                    }
                    Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
                    if (Game1.drawGrid)
                    {
                        int x1 = -Game1.viewport.X % Game1.tileSize;
                        float num1 = (float)(-Game1.viewport.Y % Game1.tileSize);
                        int x2 = x1;
                        while (x2 < Game1.graphics.GraphicsDevice.Viewport.Width)
                        {
                            Game1.spriteBatch.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle(x2, (int)num1, 1, Game1.graphics.GraphicsDevice.Viewport.Height), Color.Red * 0.5f);
                            x2 += Game1.tileSize;
                        }
                        float num2 = num1;
                        while ((double)num2 < (double)Game1.graphics.GraphicsDevice.Viewport.Height)
                        {
                            Game1.spriteBatch.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle(x1, (int)num2, Game1.graphics.GraphicsDevice.Viewport.Width, 1), Color.Red * 0.5f);
                            num2 += (float)Game1.tileSize;
                        }
                    }
                    if (Game1.currentBillboard != 0)
                        this.drawBillboard();

                    GraphicsEvents.InvokeOnPreRenderHudEventNoCheck(this);
                    if ((Game1.displayHUD || Game1.eventUp) &&
                        (Game1.currentBillboard == 0 && (int) Game1.gameMode == 3) &&
                        (!Game1.freezeControls && !Game1.panMode))
                    {
                        GraphicsEvents.InvokeOnPreRenderHudEvent(this);
                        this.drawHUD();
                        GraphicsEvents.InvokeOnPostRenderHudEvent(this);
                    }
                    else if (Game1.activeClickableMenu == null && Game1.farmEvent == null)
                        Game1.spriteBatch.Draw(Game1.mouseCursors,
                            new Vector2((float) Game1.getOldMouseX(), (float) Game1.getOldMouseY()),
                            new Microsoft.Xna.Framework.Rectangle?(
                                Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White,
                            0.0f, Vector2.Zero, (float) (4.0 + (double) Game1.dialogueButtonScale/150.0),
                            SpriteEffects.None, 1f);

                    GraphicsEvents.InvokeOnPostRenderHudEventNoCheck(this);

                    if (Enumerable.Count<HUDMessage>((IEnumerable<HUDMessage>)Game1.hudMessages) > 0 && (!Game1.eventUp || Game1.isFestival()))
                    {
                        for (int i = Enumerable.Count<HUDMessage>((IEnumerable<HUDMessage>)Game1.hudMessages) - 1; i >= 0; --i)
                            Game1.hudMessages[i].draw(Game1.spriteBatch, i);
                    }
                }
                if (Game1.farmEvent != null)
                    Game1.farmEvent.draw(Game1.spriteBatch);
                if (Game1.dialogueUp && !Game1.nameSelectUp && !Game1.messagePause && (Game1.activeClickableMenu == null || !(Game1.activeClickableMenu is DialogueBox)))
                    this.drawDialogueBox();
                if (Game1.progressBar)
                {
                    Game1.spriteBatch.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle((Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width - Game1.dialogueWidth) / 2, Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - Game1.tileSize * 2, Game1.dialogueWidth, Game1.tileSize / 2), Color.LightGray);
                    Game1.spriteBatch.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle((Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width - Game1.dialogueWidth) / 2, Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - Game1.tileSize * 2, (int)((double)Game1.pauseAccumulator / (double)Game1.pauseTime * (double)Game1.dialogueWidth), Game1.tileSize / 2), Color.DimGray);
                }
                if (Game1.eventUp && Game1.currentLocation.currentEvent != null)
                    Game1.currentLocation.currentEvent.drawAfterMap(Game1.spriteBatch);
                if (Game1.isRaining && Game1.currentLocation.isOutdoors && !(Game1.currentLocation is Desert))
                    Game1.spriteBatch.Draw(Game1.staminaRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Blue * 0.2f);
                if ((Game1.fadeToBlack || Game1.globalFade) && !Game1.menuUp && (!Game1.nameSelectUp || Game1.messagePause))
                    Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * ((int)Game1.gameMode == 0 ? 1f - Game1.fadeToBlackAlpha : Game1.fadeToBlackAlpha));
                else if ((double)Game1.flashAlpha > 0.0)
                {
                    if (Game1.options.screenFlash)
                        Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.White * Math.Min(1f, Game1.flashAlpha));
                    Game1.flashAlpha -= 0.1f;
                }
                if ((Game1.messagePause || Game1.globalFade) && Game1.dialogueUp)
                    this.drawDialogueBox();
                foreach (TemporaryAnimatedSprite temporaryAnimatedSprite in Game1.screenOverlayTempSprites)
                    temporaryAnimatedSprite.draw(Game1.spriteBatch, true, 0, 0);
                if (Game1.debugMode)
                {
                    SpriteBatch spriteBatch = Game1.spriteBatch;
                    SpriteFont spriteFont = Game1.smallFont;
                    object[] objArray1 = new object[6];
                    object[] objArray2 = objArray1;
                    int index = 0;
                    string str;
                    if (!Game1.panMode)
                        str = string.Concat(new object[4]
                        {
              (object) "player: ",
              (object) (Game1.player.getStandingX() / Game1.tileSize),
              (object) ", ",
              (object) (Game1.player.getStandingY() / Game1.tileSize)
                        });
                    else
                        str = (string)(object)((Game1.getOldMouseX() + Game1.viewport.X) / Game1.tileSize) + (object)"," + (string)(object)((Game1.getOldMouseY() + Game1.viewport.Y) / Game1.tileSize);
                    objArray2[index] = (object)str;
                    objArray1[1] = (object)" fps:";
                    objArray1[2] = (object)Game1.currentfps;
                    objArray1[3] = (object)Environment.NewLine;
                    objArray1[4] = (object)"debugOutput: ";
                    objArray1[5] = (object)Game1.debugOutput;
                    var text = string.Concat(objArray1);
                    var position = new Vector2((float)this.GraphicsDevice.Viewport.TitleSafeArea.X, (float)this.GraphicsDevice.Viewport.TitleSafeArea.Y);
                    var red = Color.Red;
                    var num1 = 0.0;
                    var zero = Vector2.Zero;
                    var num2 = 1.0;
                    var num3 = 0;
                    var num4 = 0.99999988079071;
                    spriteBatch.DrawString(spriteFont, text, position, red, (float)num1, zero, (float)num2, (SpriteEffects)num3, (float)num4);
                }
                if (Game1.inputMode)
                    Game1.spriteBatch.DrawString(Game1.smallFont, "Input: " + Game1.debugInput, new Vector2((float)Game1.tileSize, (float)(Game1.tileSize * 3)), Color.Purple);
                if (Game1.showKeyHelp)
                    Game1.spriteBatch.DrawString(Game1.smallFont, Game1.keyHelpString, new Vector2((float)Game1.tileSize, (float)(Game1.viewport.Height - Game1.tileSize - (Game1.dialogueUp ? Game1.tileSize * 3 + (Game1.isQuestion ? Game1.questionChoices.Count * Game1.tileSize : 0) : 0)) - Game1.smallFont.MeasureString(Game1.keyHelpString).Y), Color.LightGray, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999999f);
                GraphicsEvents.InvokeOnPreRenderGuiEventNoCheck(this);
                if (Game1.activeClickableMenu != null)
                {
                    GraphicsEvents.InvokeOnPreRenderGuiEvent(this);
                    Game1.activeClickableMenu.draw(Game1.spriteBatch);
                    GraphicsEvents.InvokeOnPostRenderGuiEvent(this);
                }
                else if (Game1.farmEvent != null)
                    Game1.farmEvent.drawAboveEverything(Game1.spriteBatch);

                GraphicsEvents.InvokeOnPostRenderGuiEventNoCheck(this);
                GraphicsEvents.InvokeOnPostRenderEvent(this);
                Game1.spriteBatch.End();
                GraphicsEvents.InvokeDrawInRenderTargetTick(this);
                if (Math.Abs((double)Game1.options.zoomLevel - 1.0) < 0.001f)
                    return;
                this.GraphicsDevice.SetRenderTarget((RenderTarget2D)null);
                this.GraphicsDevice.Clear(this.bgColor);
                Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                Game1.spriteBatch.Draw((Texture2D)this.screen, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(this.screen.Bounds), Color.White, 0.0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
                Game1.spriteBatch.End();
            }
        }
    }
}
