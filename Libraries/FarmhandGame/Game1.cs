using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Tools;
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
            if (options.zoomLevel != 1f)
            {
                GraphicsDevice.SetRenderTarget(screen);
            }
            framesThisSecond++;
            GraphicsDevice.Clear(bgColor);
            if ((options.showMenuBackground && (activeClickableMenu != null)) && activeClickableMenu.showWithoutTransparencyIfOptionIsSet())
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                activeClickableMenu.drawBackground(spriteBatch);
                GraphicsEvents.InvokeOnPreRenderGuiEvent(this);
                activeClickableMenu.draw(spriteBatch);
                GraphicsEvents.InvokeOnPostRenderGuiEvent(this);
                spriteBatch.End();
                if (options.zoomLevel != 1f)
                {
                    GraphicsDevice.SetRenderTarget(null);
                    GraphicsDevice.Clear(bgColor);
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                    spriteBatch.Draw(screen, Vector2.Zero, screen.Bounds, Color.White, 0f, Vector2.Zero, options.zoomLevel, SpriteEffects.None, 1f);
                    spriteBatch.End();
                }
            }
            else if (gameMode == 11)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                spriteBatch.DrawString(smoothFont, "Stardew Valley has crashed...", new Vector2(16f, 16f), Color.HotPink);
                spriteBatch.DrawString(smoothFont, "Please send the error report or a screenshot of this message to @ConcernedApe. (http://stardewvalley.net/contact/)", new Vector2(16f, 32f), new Color(0, 0xff, 0));
                spriteBatch.DrawString(smoothFont, parseText(errorMessage, smoothFont, graphics.GraphicsDevice.Viewport.Width), new Vector2(16f, 48f), Color.White);
                spriteBatch.End();
            }
            else if (currentMinigame != null)
            {
                currentMinigame.draw(spriteBatch);
                if ((globalFade && !menuUp) && (!nameSelectUp || messagePause))
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    spriteBatch.Draw(fadeToBlackRect, graphics.GraphicsDevice.Viewport.Bounds, Color.Black * ((gameMode == 0) ? (1f - fadeToBlackAlpha) : fadeToBlackAlpha));
                    spriteBatch.End();
                }
                if (options.zoomLevel != 1f)
                {
                    GraphicsDevice.SetRenderTarget(null);
                    GraphicsDevice.Clear(bgColor);
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                    spriteBatch.Draw(screen, Vector2.Zero, screen.Bounds, Color.White, 0f, Vector2.Zero, options.zoomLevel, SpriteEffects.None, 1f);
                    spriteBatch.End();
                }
            }
            else if (showingEndOfNightStuff)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                if (activeClickableMenu != null)
                {
                    activeClickableMenu.draw(spriteBatch);
                }
                spriteBatch.End();
                if (options.zoomLevel != 1f)
                {
                    GraphicsDevice.SetRenderTarget(null);
                    GraphicsDevice.Clear(bgColor);
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                    spriteBatch.Draw(screen, Vector2.Zero, screen.Bounds, Color.White, 0f, Vector2.Zero, options.zoomLevel, SpriteEffects.None, 1f);
                    spriteBatch.End();
                }
            }
            else if (gameMode == 6)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                string str = "";
                for (int i = 0; i < ((gameTime.TotalGameTime.TotalMilliseconds % 999.0) / 333.0); i++)
                {
                    str = str + ".";
                }
                SpriteText.drawString(spriteBatch, "Loading" + str, 0x40, graphics.GraphicsDevice.Viewport.Height - 0x40, 0x3e7, -1, 0x3e7, 1f, 1f, false, 0, "Loading...", -1);
                spriteBatch.End();
                if (options.zoomLevel != 1f)
                {
                    GraphicsDevice.SetRenderTarget(null);
                    GraphicsDevice.Clear(bgColor);
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                    spriteBatch.Draw(screen, Vector2.Zero, screen.Bounds, Color.White, 0f, Vector2.Zero, options.zoomLevel, SpriteEffects.None, 1f);
                    spriteBatch.End();
                }
            }
            else
            {
                if (gameMode == 0)
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                }
                else
                {
                    if (drawLighting)
                    {
                        GraphicsDevice.SetRenderTarget(lightmap);
                        GraphicsDevice.Clear(Color.White * 0f);
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
                        spriteBatch.Draw(staminaRect, lightmap.Bounds, currentLocation.name.Equals("UndergroundMine") ? mine.getLightingColor(gameTime) : ((!ambientLight.Equals(Color.White) && (!isRaining || !currentLocation.isOutdoors)) ? ambientLight : outdoorLight));
                        for (int j = 0; j < currentLightSources.Count; j++)
                        {
                            if (StardewValley.Utility.isOnScreen(currentLightSources.ElementAt(j).position, (int)((currentLightSources.ElementAt(j).radius * tileSize) * 4f)))
                            {
                                spriteBatch.Draw(currentLightSources.ElementAt(j).lightTexture, GlobalToLocal(viewport, currentLightSources.ElementAt(j).position) / ((float)(options.lightingQuality / 2)), currentLightSources.ElementAt(j).lightTexture.Bounds, currentLightSources.ElementAt(j).color, 0f, new Vector2((float)currentLightSources.ElementAt(j).lightTexture.Bounds.Center.X, (float)currentLightSources.ElementAt(j).lightTexture.Bounds.Center.Y), (float)(currentLightSources.ElementAt(j).radius / ((float)(options.lightingQuality / 2))), SpriteEffects.None, 0.9f);
                            }
                        }
                        spriteBatch.End();
                        GraphicsDevice.SetRenderTarget((options.zoomLevel == 1f) ? null : screen);
                    }
                    if (bloomDay && (bloom != null))
                    {
                        bloom.BeginDraw();
                    }
                    GraphicsDevice.Clear(bgColor);
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    GraphicsEvents.InvokeOnPreRenderEvent(this);
                    if (background != null)
                    {
                        background.draw(spriteBatch);
                    }
                    mapDisplayDevice.BeginScene(spriteBatch);
                    currentLocation.Map.GetLayer("Back").Draw(mapDisplayDevice, viewport, xTile.Dimensions.Location.Origin, false, pixelZoom);
                    currentLocation.drawWater(spriteBatch);
                    if (CurrentEvent == null)
                    {
                        foreach (NPC npc in currentLocation.characters)
                        {
                            if (((!npc.swimming && !npc.hideShadow) && (!npc.isInvisible && !npc.IsMonster)) && !currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(npc.getTileLocation()))
                            {
                                spriteBatch.Draw(shadowTexture, GlobalToLocal(viewport, npc.position + new Vector2(((float)(npc.sprite.spriteWidth * pixelZoom)) / 2f, (float)(npc.GetBoundingBox().Height + (npc.IsMonster ? 0 : (pixelZoom * 3))))), shadowTexture.Bounds, Color.White, 0f, new Vector2((float)shadowTexture.Bounds.Center.X, (float)shadowTexture.Bounds.Center.Y), (float)((pixelZoom + (((float)npc.yJumpOffset) / 40f)) * npc.scale), SpriteEffects.None, Math.Max((float)0f, (float)(((float)npc.getStandingY()) / 10000f)) - 1E-06f);
                            }
                        }
                    }
                    else
                    {
                        foreach (NPC npc2 in CurrentEvent.actors)
                        {
                            if ((!npc2.swimming && !npc2.hideShadow) && !currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(npc2.getTileLocation()))
                            {
                                spriteBatch.Draw(shadowTexture, GlobalToLocal(viewport, npc2.position + new Vector2(((float)(npc2.sprite.spriteWidth * pixelZoom)) / 2f, (float)(npc2.GetBoundingBox().Height + (npc2.IsMonster ? 0 : ((npc2.sprite.spriteHeight <= 0x10) ? -pixelZoom : (pixelZoom * 3)))))), shadowTexture.Bounds, Color.White, 0f, new Vector2((float)shadowTexture.Bounds.Center.X, (float)shadowTexture.Bounds.Center.Y), (float)((pixelZoom + (((float)npc2.yJumpOffset) / 40f)) * npc2.scale), SpriteEffects.None, Math.Max((float)0f, (float)(((float)npc2.getStandingY()) / 10000f)) - 1E-06f);
                            }
                        }
                    }
                    if ((displayFarmer && !player.swimming) && (!player.isRidingHorse() && !currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(player.getTileLocation())))
                    {
                        spriteBatch.Draw(shadowTexture, GlobalToLocal(player.position + new Vector2(32f, 24f)), shadowTexture.Bounds, Color.White, 0f, new Vector2((float)shadowTexture.Bounds.Center.X, (float)shadowTexture.Bounds.Center.Y), (float)(4f - (((player.running || player.usingTool) && (player.FarmerSprite.indexInCurrentAnimation > 1)) ? (Math.Abs(FarmerRenderer.featureYOffsetPerFrame[player.FarmerSprite.CurrentFrame]) * 0.5f) : 0f)), SpriteEffects.None, 0f);
                    }
                    currentLocation.Map.GetLayer("Buildings").Draw(mapDisplayDevice, viewport, xTile.Dimensions.Location.Origin, false, pixelZoom);
                    mapDisplayDevice.EndScene();
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    if (CurrentEvent == null)
                    {
                        foreach (NPC npc3 in currentLocation.characters)
                        {
                            if ((!npc3.swimming && !npc3.hideShadow) && currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(npc3.getTileLocation()))
                            {
                                spriteBatch.Draw(shadowTexture, GlobalToLocal(viewport, npc3.position + new Vector2(((float)(npc3.sprite.spriteWidth * pixelZoom)) / 2f, (float)(npc3.GetBoundingBox().Height + (npc3.IsMonster ? 0 : (pixelZoom * 3))))), new Rectangle?(shadowTexture.Bounds), Color.White, 0f, new Vector2((float)shadowTexture.Bounds.Center.X, (float)shadowTexture.Bounds.Center.Y), (float)((pixelZoom + (((float)npc3.yJumpOffset) / 40f)) * npc3.scale), SpriteEffects.None, Math.Max((float)0f, (float)(((float)npc3.getStandingY()) / 10000f)) - 1E-06f);
                            }
                        }
                    }
                    else
                    {
                        foreach (NPC npc4 in CurrentEvent.actors)
                        {
                            if ((!npc4.swimming && !npc4.hideShadow) && currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(npc4.getTileLocation()))
                            {
                                spriteBatch.Draw(shadowTexture, GlobalToLocal(viewport, npc4.position + new Vector2(((float)(npc4.sprite.spriteWidth * pixelZoom)) / 2f, (float)(npc4.GetBoundingBox().Height + (npc4.IsMonster ? 0 : (pixelZoom * 3))))), new Rectangle?(shadowTexture.Bounds), Color.White, 0f, new Vector2((float)shadowTexture.Bounds.Center.X, (float)shadowTexture.Bounds.Center.Y), (float)((pixelZoom + (((float)npc4.yJumpOffset) / 40f)) * npc4.scale), SpriteEffects.None, Math.Max((float)0f, (float)(((float)npc4.getStandingY()) / 10000f)) - 1E-06f);
                            }
                        }
                    }
                    if ((displayFarmer && !player.swimming) && (!player.isRidingHorse() && currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(player.getTileLocation())))
                    {
                        spriteBatch.Draw(shadowTexture, GlobalToLocal(player.position + new Vector2(32f, 24f)), new Rectangle?(shadowTexture.Bounds), Color.White, 0f, new Vector2((float)shadowTexture.Bounds.Center.X, (float)shadowTexture.Bounds.Center.Y), (float)(4f - (((player.running || player.usingTool) && (player.FarmerSprite.indexInCurrentAnimation > 1)) ? (Math.Abs(FarmerRenderer.featureYOffsetPerFrame[player.FarmerSprite.CurrentFrame]) * 0.5f) : 0f)), SpriteEffects.None, Math.Max((float)0.0001f, (float)((((float)player.getStandingY()) / 10000f) + 0.00011f)) - 0.0001f);
                    }
                    if (displayFarmer)
                    {
                        player.draw(spriteBatch);
                    }
                    if ((eventUp || killScreen) && (!killScreen && (currentLocation.currentEvent != null)))
                    {
                        currentLocation.currentEvent.draw(spriteBatch);
                    }
                    if (((player.currentUpgrade != null) && (player.currentUpgrade.daysLeftTillUpgradeDone <= 3)) && currentLocation.Name.Equals("Farm"))
                    {
                        spriteBatch.Draw(player.currentUpgrade.workerTexture, GlobalToLocal(viewport, player.currentUpgrade.positionOfCarpenter), new Rectangle?(player.currentUpgrade.getSourceRectangle()), Color.White, 0f, Vector2.Zero, (float)1f, SpriteEffects.None, (player.currentUpgrade.positionOfCarpenter.Y + ((tileSize * 3) / 4)) / 10000f);
                    }
                    currentLocation.draw(spriteBatch);
                    if ((eventUp && (currentLocation.currentEvent != null)) && (currentLocation.currentEvent.messageToScreen != null))
                    {
                        drawWithBorder(currentLocation.currentEvent.messageToScreen, Color.Black, Color.White, new Vector2((graphics.GraphicsDevice.Viewport.TitleSafeArea.Width / 2) - (borderFont.MeasureString(currentLocation.currentEvent.messageToScreen).X / 2f), (float)(graphics.GraphicsDevice.Viewport.TitleSafeArea.Height - tileSize)), 0f, 1f, 0.999f);
                    }
                    if (((player.ActiveObject == null) && (player.UsingTool || pickingTool)) && ((player.CurrentTool != null) && (!player.CurrentTool.Name.Equals("Seeds") || pickingTool)))
                    {
                        drawTool(player);
                    }
                    if (currentLocation.Name.Equals("Farm"))
                    {
                        drawFarmBuildings();
                    }
                    if (tvStation >= 0)
                    {
                        spriteBatch.Draw(tvStationTexture, GlobalToLocal(viewport, new Vector2((float)((6 * tileSize) + (tileSize / 4)), (float)((2 * tileSize) + (tileSize / 2)))), new Rectangle(tvStation * 0x18, 0, 0x18, 15), Color.White, 0f, Vector2.Zero, (float)4f, SpriteEffects.None, 1E-08f);
                    }
                    if (panMode)
                    {
                        spriteBatch.Draw(fadeToBlackRect, new Rectangle((((int)Math.Floor((double)(((double)(getOldMouseX() + viewport.X)) / ((double)tileSize)))) * tileSize) - viewport.X, (((int)Math.Floor((double)(((double)(getOldMouseY() + viewport.Y)) / ((double)tileSize)))) * tileSize) - viewport.Y, tileSize, tileSize), (Color)(Color.Lime * 0.75f));
                        foreach (Warp warp in currentLocation.warps)
                        {
                            spriteBatch.Draw(fadeToBlackRect, new Rectangle((warp.X * tileSize) - viewport.X, (warp.Y * tileSize) - viewport.Y, tileSize, tileSize), (Color)(Color.Red * 0.75f));
                        }
                    }
                    mapDisplayDevice.BeginScene(spriteBatch);
                    currentLocation.Map.GetLayer("Front").Draw(mapDisplayDevice, viewport, xTile.Dimensions.Location.Origin, false, pixelZoom);
                    mapDisplayDevice.EndScene();
                    currentLocation.drawAboveFrontLayer(spriteBatch);
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    if (currentLocation.Name.Equals("Farm") && (stats.SeedsSown >= 200))
                    {
                        spriteBatch.Draw(debrisSpriteSheet, GlobalToLocal(viewport, new Vector2((float)((3 * tileSize) + (tileSize / 4)), (float)(tileSize + (tileSize / 3)))), new Rectangle?(getSourceRectForStandardTileSheet(debrisSpriteSheet, 0x10, -1, -1)), Color.White);
                        spriteBatch.Draw(debrisSpriteSheet, GlobalToLocal(viewport, new Vector2((float)((4 * tileSize) + tileSize), (float)((2 * tileSize) + tileSize))), new Rectangle?(getSourceRectForStandardTileSheet(debrisSpriteSheet, 0x10, -1, -1)), Color.White);
                        spriteBatch.Draw(debrisSpriteSheet, GlobalToLocal(viewport, new Vector2((float)(5 * tileSize), (float)(2 * tileSize))), new Rectangle?(getSourceRectForStandardTileSheet(debrisSpriteSheet, 0x10, -1, -1)), Color.White);
                        spriteBatch.Draw(debrisSpriteSheet, GlobalToLocal(viewport, new Vector2((float)((3 * tileSize) + (tileSize / 2)), (float)(3 * tileSize))), new Rectangle?(getSourceRectForStandardTileSheet(debrisSpriteSheet, 0x10, -1, -1)), Color.White);
                        spriteBatch.Draw(debrisSpriteSheet, GlobalToLocal(viewport, new Vector2((float)((5 * tileSize) - (tileSize / 4)), (float)tileSize)), new Rectangle?(getSourceRectForStandardTileSheet(debrisSpriteSheet, 0x10, -1, -1)), Color.White);
                        spriteBatch.Draw(debrisSpriteSheet, GlobalToLocal(viewport, new Vector2((float)(4 * tileSize), (float)((3 * tileSize) + (tileSize / 6)))), new Rectangle?(getSourceRectForStandardTileSheet(debrisSpriteSheet, 0x10, -1, -1)), Color.White);
                        spriteBatch.Draw(debrisSpriteSheet, GlobalToLocal(viewport, new Vector2((float)((4 * tileSize) + (tileSize / 5)), (float)((2 * tileSize) + (tileSize / 3)))), new Rectangle?(getSourceRectForStandardTileSheet(debrisSpriteSheet, 0x10, -1, -1)), Color.White);
                    }
                    if (((displayFarmer && (player.ActiveObject != null)) && (player.ActiveObject.bigCraftable && this.checkBigCraftableBoundariesForFrontLayer())) && (currentLocation.Map.GetLayer("Front").PickTile(new xTile.Dimensions.Location(player.getStandingX(), player.getStandingY()), viewport.Size) == null))
                    {
                        drawPlayerHeldObject(player);
                    }
                    else if ((displayFarmer && (player.ActiveObject != null)) && (((currentLocation.Map.GetLayer("Front").PickTile(new xTile.Dimensions.Location((int)player.position.X, ((int)player.position.Y) - ((tileSize * 3) / 5)), viewport.Size) != null) && !currentLocation.Map.GetLayer("Front").PickTile(new xTile.Dimensions.Location((int)player.position.X, ((int)player.position.Y) - ((tileSize * 3) / 5)), viewport.Size).TileIndexProperties.ContainsKey("FrontAlways")) || ((currentLocation.Map.GetLayer("Front").PickTile(new xTile.Dimensions.Location(player.GetBoundingBox().Right, ((int)player.position.Y) - ((tileSize * 3) / 5)), viewport.Size) != null) && !currentLocation.Map.GetLayer("Front").PickTile(new xTile.Dimensions.Location(player.GetBoundingBox().Right, ((int)player.position.Y) - ((tileSize * 3) / 5)), viewport.Size).TileIndexProperties.ContainsKey("FrontAlways"))))
                    {
                        drawPlayerHeldObject(player);
                    }
                    if (((player.UsingTool || pickingTool) && (player.CurrentTool != null)) && ((!player.CurrentTool.Name.Equals("Seeds") || pickingTool) && ((currentLocation.Map.GetLayer("Front").PickTile(new xTile.Dimensions.Location(player.getStandingX(), ((int)player.position.Y) - ((tileSize * 3) / 5)), viewport.Size) != null) && (currentLocation.Map.GetLayer("Front").PickTile(new xTile.Dimensions.Location(player.getStandingX(), player.getStandingY()), viewport.Size) == null))))
                    {
                        drawTool(player);
                    }
                    if (currentLocation.Map.GetLayer("AlwaysFront") != null)
                    {
                        mapDisplayDevice.BeginScene(spriteBatch);
                        currentLocation.Map.GetLayer("AlwaysFront").Draw(mapDisplayDevice, viewport, xTile.Dimensions.Location.Origin, false, pixelZoom);
                        mapDisplayDevice.EndScene();
                    }
                    if (((toolHold > 400f) && (player.CurrentTool.UpgradeLevel >= 1)) && player.canReleaseTool)
                    {
                        Color white = Color.White;
                        switch ((((int)(toolHold / 600f)) + 2))
                        {
                            case 1:
                                white = Tool.copperColor;
                                break;

                            case 2:
                                white = Tool.steelColor;
                                break;

                            case 3:
                                white = Tool.goldColor;
                                break;

                            case 4:
                                white = Tool.iridiumColor;
                                break;
                        }
                        spriteBatch.Draw(littleEffect, new Rectangle(((int)player.getLocalPosition(viewport).X) - 2, (((int)player.getLocalPosition(viewport).Y) - (player.CurrentTool.Name.Equals("Watering Can") ? 0 : tileSize)) - 2, ((int)((toolHold % 600f) * 0.08f)) + 4, (tileSize / 8) + 4), Color.Black);
                        spriteBatch.Draw(littleEffect, new Rectangle((int)player.getLocalPosition(viewport).X, ((int)player.getLocalPosition(viewport).Y) - (player.CurrentTool.Name.Equals("Watering Can") ? 0 : tileSize), (int)((toolHold % 600f) * 0.08f), tileSize / 8), white);
                    }
                    if (((isDebrisWeather && currentLocation.IsOutdoors) && (!currentLocation.ignoreDebrisWeather && !currentLocation.Name.Equals("Desert"))) && (viewport.X > -10))
                    {
                        using (List<WeatherDebris>.Enumerator enumerator3 = debrisWeather.GetEnumerator())
                        {
                            while (enumerator3.MoveNext())
                            {
                                enumerator3.Current.draw(spriteBatch);
                            }
                        }
                    }
                    if (farmEvent != null)
                    {
                        farmEvent.draw(spriteBatch);
                    }
                    if ((currentLocation.LightLevel > 0f) && (timeOfDay < 0x7d0))
                    {
                        spriteBatch.Draw(fadeToBlackRect, graphics.GraphicsDevice.Viewport.Bounds, (Color)(Color.Black * currentLocation.LightLevel));
                    }
                    if (screenGlow)
                    {
                        spriteBatch.Draw(fadeToBlackRect, graphics.GraphicsDevice.Viewport.Bounds, (Color)(screenGlowColor * screenGlowAlpha));
                    }
                    currentLocation.drawAboveAlwaysFrontLayer(spriteBatch);
                    if (((player.CurrentTool != null) && (player.CurrentTool is FishingRod)) && (((player.CurrentTool as FishingRod).isTimingCast || ((player.CurrentTool as FishingRod).castingChosenCountdown > 0f)) || ((player.CurrentTool as FishingRod).fishCaught || (player.CurrentTool as FishingRod).showingTreasure)))
                    {
                        player.CurrentTool.draw(spriteBatch);
                    }
                    if (((isRaining && currentLocation.IsOutdoors) && (!currentLocation.Name.Equals("Desert") && !(currentLocation is Summit))) && (!eventUp || currentLocation.isTileOnMap(new Vector2((float)(viewport.X / tileSize), (float)(viewport.Y / tileSize)))))
                    {
                        for (int k = 0; k < rainDrops.Length; k++)
                        {
                            spriteBatch.Draw(rainTexture, rainDrops[k].position, new Rectangle?(getSourceRectForStandardTileSheet(rainTexture, rainDrops[k].frame, -1, -1)), Color.White);
                        }
                    }
                    spriteBatch.End();
                    base.Draw(gameTime);
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    if (eventUp && (currentLocation.currentEvent != null))
                    {
                        foreach (NPC npc5 in currentLocation.currentEvent.actors)
                        {
                            if (npc5.isEmoting)
                            {
                                Vector2 position = npc5.getLocalPosition(viewport);
                                position.Y -= (tileSize * 2) + (pixelZoom * 3);
                                if (npc5.age == 2)
                                {
                                    position.Y += tileSize / 2;
                                }
                                else if (npc5.gender == 1)
                                {
                                    position.Y += tileSize / 6;
                                }
                                spriteBatch.Draw(emoteSpriteSheet, position, new Rectangle((npc5.CurrentEmoteIndex * (tileSize / 4)) % emoteSpriteSheet.Width, ((npc5.CurrentEmoteIndex * (tileSize / 4)) / emoteSpriteSheet.Width) * (tileSize / 4), tileSize / 4, tileSize / 4), Color.White, 0f, Vector2.Zero, (float)4f, SpriteEffects.None, ((float)npc5.getStandingY()) / 10000f);
                            }
                        }
                    }
                    spriteBatch.End();
                    if (drawLighting)
                    {
                        BlendState blendState = new BlendState
                        {
                            ColorBlendFunction = BlendFunction.ReverseSubtract,
                            ColorDestinationBlend = Blend.One,
                            ColorSourceBlend = Blend.SourceColor
                        };
                        spriteBatch.Begin(SpriteSortMode.Deferred, blendState, SamplerState.LinearClamp, null, null);
                        spriteBatch.Draw(lightmap, Vector2.Zero, new Rectangle?(lightmap.Bounds), Color.White, 0f, Vector2.Zero, (float)(options.lightingQuality / 2), SpriteEffects.None, 1f);
                        if ((isRaining && currentLocation.isOutdoors) && !(currentLocation is Desert))
                        {
                            spriteBatch.Draw(staminaRect, graphics.GraphicsDevice.Viewport.Bounds, (Color)(Color.OrangeRed * 0.45f));
                        }
                        spriteBatch.End();
                    }
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    if (drawGrid)
                    {
                        int x = -viewport.X % tileSize;
                        float num6 = -viewport.Y % tileSize;
                        for (int m = x; m < graphics.GraphicsDevice.Viewport.Width; m += tileSize)
                        {
                            spriteBatch.Draw(staminaRect, new Rectangle(m, (int)num6, 1, graphics.GraphicsDevice.Viewport.Height), (Color)(Color.Red * 0.5f));
                        }
                        for (float n = num6; n < graphics.GraphicsDevice.Viewport.Height; n += tileSize)
                        {
                            spriteBatch.Draw(staminaRect, new Rectangle(x, (int)n, graphics.GraphicsDevice.Viewport.Width, 1), (Color)(Color.Red * 0.5f));
                        }
                    }
                    if (currentBillboard != 0)
                    {
                        drawBillboard();
                    }
                    GraphicsEvents.InvokeOnPreRenderHudEventNoCheck(this);
                    if (((displayHUD || eventUp) && ((currentBillboard == 0) && (gameMode == 3))) && (!freezeControls && !panMode))
                    {
                        GraphicsEvents.InvokeOnPreRenderHudEvent(this);
                        drawHUD();
                        GraphicsEvents.InvokeOnPostRenderHudEvent(this);
                    }
                    else if ((activeClickableMenu == null) && (farmEvent == null))
                    {
                        spriteBatch.Draw(mouseCursors, new Vector2((float)getOldMouseX(), (float)getOldMouseY()), new Rectangle?(getSourceRectForStandardTileSheet(mouseCursors, 0, 0x10, 0x10)), Color.White, 0f, Vector2.Zero, (float)(4f + (dialogueButtonScale / 150f)), SpriteEffects.None, 1f);
                    }
                    GraphicsEvents.InvokeOnPostRenderHudEventNoCheck(this);
                    if ((hudMessages.Count > 0) && (!eventUp || isFestival()))
                    {
                        for (int num9 = hudMessages.Count - 1; num9 >= 0; num9--)
                        {
                            hudMessages[num9].draw(spriteBatch, num9);
                        }
                    }
                }
                if (farmEvent != null)
                {
                    farmEvent.draw(spriteBatch);
                }
                if (((dialogueUp && !nameSelectUp) && !messagePause) && ((activeClickableMenu == null) || !(activeClickableMenu is DialogueBox)))
                {
                    drawDialogueBox();
                }
                if (progressBar)
                {
                    spriteBatch.Draw(fadeToBlackRect, new Rectangle((graphics.GraphicsDevice.Viewport.TitleSafeArea.Width - dialogueWidth) / 2, graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - (tileSize * 2), dialogueWidth, tileSize / 2), Color.LightGray);
                    spriteBatch.Draw(staminaRect, new Rectangle((graphics.GraphicsDevice.Viewport.TitleSafeArea.Width - dialogueWidth) / 2, graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - (tileSize * 2), (int)((pauseAccumulator / pauseTime) * dialogueWidth), tileSize / 2), Color.DimGray);
                }
                if (eventUp && (currentLocation.currentEvent != null))
                {
                    currentLocation.currentEvent.drawAfterMap(spriteBatch);
                }
                if ((isRaining && currentLocation.isOutdoors) && !(currentLocation is Desert))
                {
                    spriteBatch.Draw(staminaRect, graphics.GraphicsDevice.Viewport.Bounds, (Color)(Color.Blue * 0.2f));
                }
                if (((fadeToBlack || globalFade) && !menuUp) && (!nameSelectUp || messagePause))
                {
                    spriteBatch.Draw(fadeToBlackRect, graphics.GraphicsDevice.Viewport.Bounds, (Color)(Color.Black * ((gameMode == 0) ? (1f - fadeToBlackAlpha) : fadeToBlackAlpha)));
                }
                else if (flashAlpha > 0f)
                {
                    if (options.screenFlash)
                    {
                        spriteBatch.Draw(fadeToBlackRect, graphics.GraphicsDevice.Viewport.Bounds, (Color)(Color.White * Math.Min(1f, flashAlpha)));
                    }
                    flashAlpha -= 0.1f;
                }
                if ((messagePause || globalFade) && dialogueUp)
                {
                    drawDialogueBox();
                }
                using (List<TemporaryAnimatedSprite>.Enumerator enumerator4 = screenOverlayTempSprites.GetEnumerator())
                {
                    while (enumerator4.MoveNext())
                    {
                        enumerator4.Current.draw(spriteBatch, true, 0, 0);
                    }
                }
                if (debugMode)
                {
                    spriteBatch.DrawString(smallFont, string.Concat(panMode ? (((getOldMouseX() + viewport.X) / tileSize) + "," + ((getOldMouseY() + viewport.Y) / tileSize)) : string.Concat(new object[] { "player: ", player.getStandingX() / tileSize, ", ", player.getStandingY() / tileSize }), " backIndex:", currentLocation.getTileIndexAt(player.getTileX(), player.getTileY(), "Back"), Environment.NewLine, "debugOutput: ", debugOutput), new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y), Color.Red, 0f, Vector2.Zero, (float)1f, SpriteEffects.None, 0.9999999f);
                }
                if (showKeyHelp)
                {
                    spriteBatch.DrawString(smallFont, keyHelpString, new Vector2((float)tileSize, ((viewport.Height - tileSize) - (dialogueUp ? ((tileSize * 3) + (isQuestion ? (questionChoices.Count * tileSize) : 0)) : 0)) - smallFont.MeasureString(keyHelpString).Y), Color.LightGray, 0f, Vector2.Zero, (float)1f, SpriteEffects.None, 0.9999999f);
                }
                GraphicsEvents.InvokeOnPreRenderGuiEventNoCheck(this);
                if (activeClickableMenu != null)
                {
                    GraphicsEvents.InvokeOnPreRenderGuiEvent(this);
                    activeClickableMenu.draw(spriteBatch);
                    GraphicsEvents.InvokeOnPostRenderGuiEvent(this);
                }
                else if (farmEvent != null)
                {
                    farmEvent.drawAboveEverything(spriteBatch);
                }
                GraphicsEvents.InvokeOnPostRenderGuiEventNoCheck(this);

                GraphicsEvents.InvokeOnPostRenderEvent(this);
                spriteBatch.End();
                GraphicsEvents.InvokeDrawInRenderTargetTick(this);
                if (options.zoomLevel != 1f)
                {
                    GraphicsDevice.SetRenderTarget(null);
                    GraphicsDevice.Clear(bgColor);
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                    spriteBatch.Draw(screen, Vector2.Zero, new Rectangle?(screen.Bounds), Color.White, 0f, Vector2.Zero, options.zoomLevel, SpriteEffects.None, 1f);
                    spriteBatch.End();
                }                
            }
        }
    }
}
