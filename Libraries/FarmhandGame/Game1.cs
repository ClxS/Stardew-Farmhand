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
using xTile.Dimensions;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

// ReSharper disable once CheckNamespace
namespace Farmhand.Overrides
{
    /// <summary>
    /// Overrides Stardew's Game1, allowing for advanced callback events to be added
    /// </summary>
    public class Game1 : StardewValley.Game1
    {
        public readonly Dictionary<string, Action<GameTime>> VersionSpecificOverrides = new Dictionary<string, Action<GameTime>>();
        
        public bool ZoomLevelIsOne => options.zoomLevel.Equals(1.0f);

        protected override void Draw(GameTime gameTime)
        {
            if (Constants.OverrideGameDraw)
            {
                GraphicsEvents.InvokeBeforeDraw(this);
                if (VersionSpecificOverrides.ContainsKey(version))
                {
                    VersionSpecificOverrides[version](gameTime);
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
            try
            {
                if (!ZoomLevelIsOne)
                    GraphicsDevice.SetRenderTarget(screen);

                GraphicsDevice.Clear(bgColor);
                if (options.showMenuBackground && activeClickableMenu != null && activeClickableMenu.showWithoutTransparencyIfOptionIsSet())
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    activeClickableMenu.drawBackground(spriteBatch);
                    GraphicsEvents.InvokeOnPreRenderGuiEvent(this);
                    activeClickableMenu.draw(spriteBatch);
                    GraphicsEvents.InvokeOnPostRenderGuiEvent(this);
                    spriteBatch.End();
                    if (ZoomLevelIsOne) return;

                    GraphicsDevice.SetRenderTarget(null);
                    GraphicsDevice.Clear(bgColor);
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                    spriteBatch.Draw(screen, Vector2.Zero, screen.Bounds, Color.White, 0f, Vector2.Zero, options.zoomLevel, SpriteEffects.None, 1f);
                    spriteBatch.End();
                    return;
                }
                if (gameMode == 11)
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    spriteBatch.DrawString(smoothFont, "Stardew Valley has crashed...", new Vector2(16f, 16f), Color.HotPink);
                    spriteBatch.DrawString(smoothFont, "Please send the error report or a screenshot of this message to @ConcernedApe. (http://stardewvalley.net/contact/)", new Vector2(16f, 32f), new Color(0, 255, 0));
                    spriteBatch.DrawString(smoothFont, parseText(errorMessage, smoothFont, graphics.GraphicsDevice.Viewport.Width), new Vector2(16f, 48f), Color.White);
                    spriteBatch.End();
                    return;
                }
                if (currentMinigame != null)
                {
                    currentMinigame.draw(spriteBatch);
                    if (globalFade && !menuUp && (!nameSelectUp || messagePause))
                    {
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                        spriteBatch.Draw(fadeToBlackRect, graphics.GraphicsDevice.Viewport.Bounds, Color.Black * ((gameMode == 0) ? (1f - fadeToBlackAlpha) : fadeToBlackAlpha));
                        spriteBatch.End();
                    }
                    if (!ZoomLevelIsOne)
                    {
                        GraphicsDevice.SetRenderTarget(null);
                        GraphicsDevice.Clear(bgColor);
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                        spriteBatch.Draw(screen, Vector2.Zero, screen.Bounds, Color.White, 0f, Vector2.Zero, options.zoomLevel, SpriteEffects.None, 1f);
                        spriteBatch.End();
                    }
                    return;
                }
                if (showingEndOfNightStuff)
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    activeClickableMenu?.draw(spriteBatch);
                    spriteBatch.End();
                    if (ZoomLevelIsOne) return;

                    GraphicsDevice.SetRenderTarget(null);
                    GraphicsDevice.Clear(bgColor);
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                    spriteBatch.Draw(screen, Vector2.Zero, screen.Bounds, Color.White, 0f, Vector2.Zero, options.zoomLevel, SpriteEffects.None, 1f);
                    spriteBatch.End();
                    return;
                }
                if (gameMode == 6)
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    string text = "";
                    int num = 0;
                    while (num < gameTime.TotalGameTime.TotalMilliseconds % 999.0 / 333.0)
                    {
                        text += ".";
                        num++;
                    }
                    SpriteText.drawString(spriteBatch, "Loading" + text, 64, graphics.GraphicsDevice.Viewport.Height - 64, 999, -1, 999, 1f, 1f, false, 0, "Loading...");
                    spriteBatch.End();
                    if (!ZoomLevelIsOne)
                    {
                        GraphicsDevice.SetRenderTarget(null);
                        GraphicsDevice.Clear(bgColor);
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                        spriteBatch.Draw(screen, Vector2.Zero, screen.Bounds, Color.White, 0f, Vector2.Zero, options.zoomLevel, SpriteEffects.None, 1f);
                        spriteBatch.End();
                    }
                    return;
                }
                if (gameMode == 0)
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                else
                {
                    if (drawLighting)
                    {
                        GraphicsDevice.SetRenderTarget(lightmap);
                        GraphicsDevice.Clear(Color.White * 0f);
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
                        spriteBatch.Draw(staminaRect, lightmap.Bounds, currentLocation.name.Equals("UndergroundMine") ? mine.getLightingColor(gameTime) : ((!ambientLight.Equals(Color.White) && (!isRaining || !currentLocation.isOutdoors)) ? ambientLight : outdoorLight));
                        for (int i = 0; i < currentLightSources.Count; i++)
                        {
                            if (Utility.isOnScreen(currentLightSources.ElementAt(i).position, (int)(currentLightSources.ElementAt(i).radius * tileSize * 4f)))
                                spriteBatch.Draw(currentLightSources.ElementAt(i).lightTexture, GlobalToLocal(viewport, currentLightSources.ElementAt(i).position) / options.lightingQuality, currentLightSources.ElementAt(i).lightTexture.Bounds, currentLightSources.ElementAt(i).color, 0f, new Vector2(currentLightSources.ElementAt(i).lightTexture.Bounds.Center.X, currentLightSources.ElementAt(i).lightTexture.Bounds.Center.Y), currentLightSources.ElementAt(i).radius / options.lightingQuality, SpriteEffects.None, 0.9f);
                        }
                        spriteBatch.End();
                        GraphicsDevice.SetRenderTarget(ZoomLevelIsOne ? null : screen);
                    }
                    if (bloomDay)
                        bloom?.BeginDraw();
                    GraphicsDevice.Clear(bgColor);
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    GraphicsEvents.InvokeOnPreRenderEvent(this);
                    background?.draw(spriteBatch);
                    mapDisplayDevice.BeginScene(spriteBatch);
                    currentLocation.Map.GetLayer("Back").Draw(mapDisplayDevice, viewport, Location.Origin, false, pixelZoom);
                    currentLocation.drawWater(spriteBatch);
                    if (CurrentEvent == null)
                    {
                        using (List<NPC>.Enumerator enumerator = currentLocation.characters.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                NPC current = enumerator.Current;
                                if (current != null && !current.swimming && !current.hideShadow && !current.IsMonster && !currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(current.getTileLocation()))
                                    spriteBatch.Draw(shadowTexture, GlobalToLocal(viewport, current.position + new Vector2(current.sprite.spriteWidth * pixelZoom / 2f, current.GetBoundingBox().Height + (current.IsMonster ? 0 : (pixelZoom * 3)))), shadowTexture.Bounds, Color.White, 0f, new Vector2(shadowTexture.Bounds.Center.X, shadowTexture.Bounds.Center.Y), (pixelZoom + current.yJumpOffset / 40f) * current.scale, SpriteEffects.None, Math.Max(0f, current.getStandingY() / 10000f) - 1E-06f);
                            }
                            goto IL_B30;
                        }
                    }
                    foreach (NPC current2 in CurrentEvent.actors)
                    {
                        if (!current2.swimming && !current2.hideShadow && !currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(current2.getTileLocation()))
                            spriteBatch.Draw(shadowTexture, GlobalToLocal(viewport, current2.position + new Vector2(current2.sprite.spriteWidth * pixelZoom / 2f, current2.GetBoundingBox().Height + (current2.IsMonster ? 0 : (pixelZoom * 3)))), shadowTexture.Bounds, Color.White, 0f, new Vector2(shadowTexture.Bounds.Center.X, shadowTexture.Bounds.Center.Y), (pixelZoom + current2.yJumpOffset / 40f) * current2.scale, SpriteEffects.None, Math.Max(0f, current2.getStandingY() / 10000f) - 1E-06f);
                    }
                    IL_B30:
                    if (!player.swimming && !player.isRidingHorse() && !currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(player.getTileLocation()))
                        spriteBatch.Draw(shadowTexture, GlobalToLocal(player.position + new Vector2(32f, 24f)), shadowTexture.Bounds, Color.White, 0f, new Vector2(shadowTexture.Bounds.Center.X, shadowTexture.Bounds.Center.Y), 4f - (((player.running || player.usingTool) && player.FarmerSprite.indexInCurrentAnimation > 1) ? (Math.Abs(FarmerRenderer.featureYOffsetPerFrame[player.FarmerSprite.CurrentFrame]) * 0.5f) : 0f), SpriteEffects.None, 0f);
                    currentLocation.Map.GetLayer("Buildings").Draw(mapDisplayDevice, viewport, Location.Origin, false, pixelZoom);
                    mapDisplayDevice.EndScene();
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    if (CurrentEvent == null)
                    {
                        using (List<NPC>.Enumerator enumerator3 = currentLocation.characters.GetEnumerator())
                        {
                            while (enumerator3.MoveNext())
                            {
                                NPC current3 = enumerator3.Current;
                                if (current3 != null && !current3.swimming && !current3.hideShadow && currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(current3.getTileLocation()))
                                    spriteBatch.Draw(shadowTexture, GlobalToLocal(viewport, current3.position + new Vector2(current3.sprite.spriteWidth * pixelZoom / 2f, current3.GetBoundingBox().Height + (current3.IsMonster ? 0 : (pixelZoom * 3)))), shadowTexture.Bounds, Color.White, 0f, new Vector2(shadowTexture.Bounds.Center.X, shadowTexture.Bounds.Center.Y), (pixelZoom + current3.yJumpOffset / 40f) * current3.scale, SpriteEffects.None, Math.Max(0f, current3.getStandingY() / 10000f) - 1E-06f);
                            }
                            goto IL_F5F;
                        }
                    }
                    foreach (NPC current4 in CurrentEvent.actors)
                    {
                        if (!current4.swimming && !current4.hideShadow && currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(current4.getTileLocation()))
                            spriteBatch.Draw(shadowTexture, GlobalToLocal(viewport, current4.position + new Vector2(current4.sprite.spriteWidth * pixelZoom / 2f, current4.GetBoundingBox().Height + (current4.IsMonster ? 0 : (pixelZoom * 3)))), shadowTexture.Bounds, Color.White, 0f, new Vector2(shadowTexture.Bounds.Center.X, shadowTexture.Bounds.Center.Y), (pixelZoom + current4.yJumpOffset / 40f) * current4.scale, SpriteEffects.None, Math.Max(0f, current4.getStandingY() / 10000f) - 1E-06f);
                    }
                    IL_F5F:
                    if (!player.swimming && !player.isRidingHorse() && currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(player.getTileLocation()))
                        spriteBatch.Draw(shadowTexture, GlobalToLocal(player.position + new Vector2(32f, 24f)), shadowTexture.Bounds, Color.White, 0f, new Vector2(shadowTexture.Bounds.Center.X, shadowTexture.Bounds.Center.Y), 4f - (((player.running || player.usingTool) && player.FarmerSprite.indexInCurrentAnimation > 1) ? (Math.Abs(FarmerRenderer.featureYOffsetPerFrame[player.FarmerSprite.CurrentFrame]) * 0.5f) : 0f), SpriteEffects.None, Math.Max(0.0001f, player.getStandingY() / 10000f + 0.00011f) - 0.0001f);
                    if (displayFarmer)
                        player.draw(spriteBatch);
                    if ((eventUp || killScreen) && !killScreen)
                        currentLocation.currentEvent?.draw(spriteBatch);
                    if (player.currentUpgrade != null && player.currentUpgrade.daysLeftTillUpgradeDone <= 3 && currentLocation.Name.Equals("Farm"))
                        spriteBatch.Draw(player.currentUpgrade.workerTexture, GlobalToLocal(viewport, player.currentUpgrade.positionOfCarpenter), player.currentUpgrade.getSourceRectangle(), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, (player.currentUpgrade.positionOfCarpenter.Y + tileSize * 3 / 4) / 10000f);
                    currentLocation.draw(spriteBatch);
                    if (eventUp && currentLocation.currentEvent?.messageToScreen != null)
                        drawWithBorder(currentLocation.currentEvent.messageToScreen, Color.Black, Color.White, new Vector2(graphics.GraphicsDevice.Viewport.TitleSafeArea.Width / 2 - borderFont.MeasureString(currentLocation.currentEvent.messageToScreen).X / 2f, graphics.GraphicsDevice.Viewport.TitleSafeArea.Height - tileSize), 0f, 1f, 0.999f);
                    if (player.ActiveObject == null && (player.UsingTool || pickingTool) && player.CurrentTool != null && (!player.CurrentTool.Name.Equals("Seeds") || pickingTool))
                        drawTool(player);
                    if (currentLocation.Name.Equals("Farm"))
                        drawFarmBuildings();
                    if (tvStation >= 0)
                        spriteBatch.Draw(tvStationTexture, GlobalToLocal(viewport, new Vector2(6 * tileSize + tileSize / 4, 2 * tileSize + tileSize / 2)), new Rectangle(tvStation * 24, 0, 24, 15), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-08f);
                    if (panMode)
                    {
                        spriteBatch.Draw(fadeToBlackRect, new Rectangle((int)Math.Floor((getOldMouseX() + viewport.X) / (double)tileSize) * tileSize - viewport.X, (int)Math.Floor((getOldMouseY() + viewport.Y) / (double)tileSize) * tileSize - viewport.Y, tileSize, tileSize), Color.Lime * 0.75f);
                        foreach (Warp current5 in currentLocation.warps)
                            spriteBatch.Draw(fadeToBlackRect, new Rectangle(current5.X * tileSize - viewport.X, current5.Y * tileSize - viewport.Y, tileSize, tileSize), Color.Red * 0.75f);
                    }
                    mapDisplayDevice.BeginScene(spriteBatch);
                    currentLocation.Map.GetLayer("Front").Draw(mapDisplayDevice, viewport, Location.Origin, false, pixelZoom);
                    mapDisplayDevice.EndScene();
                    currentLocation.drawAboveFrontLayer(spriteBatch);
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    if (currentLocation.Name.Equals("Farm") && stats.SeedsSown >= 200u)
                    {
                        spriteBatch.Draw(debrisSpriteSheet, GlobalToLocal(viewport, new Vector2(3 * tileSize + tileSize / 4, tileSize + tileSize / 3)), getSourceRectForStandardTileSheet(debrisSpriteSheet, 16), Color.White);
                        spriteBatch.Draw(debrisSpriteSheet, GlobalToLocal(viewport, new Vector2(4 * tileSize + tileSize, 2 * tileSize + tileSize)), getSourceRectForStandardTileSheet(debrisSpriteSheet, 16), Color.White);
                        spriteBatch.Draw(debrisSpriteSheet, GlobalToLocal(viewport, new Vector2(5 * tileSize, 2 * tileSize)), getSourceRectForStandardTileSheet(debrisSpriteSheet, 16), Color.White);
                        spriteBatch.Draw(debrisSpriteSheet, GlobalToLocal(viewport, new Vector2(3 * tileSize + tileSize / 2, 3 * tileSize)), getSourceRectForStandardTileSheet(debrisSpriteSheet, 16), Color.White);
                        spriteBatch.Draw(debrisSpriteSheet, GlobalToLocal(viewport, new Vector2(5 * tileSize - tileSize / 4, tileSize)), getSourceRectForStandardTileSheet(debrisSpriteSheet, 16), Color.White);
                        spriteBatch.Draw(debrisSpriteSheet, GlobalToLocal(viewport, new Vector2(4 * tileSize, 3 * tileSize + tileSize / 6)), getSourceRectForStandardTileSheet(debrisSpriteSheet, 16), Color.White);
                        spriteBatch.Draw(debrisSpriteSheet, GlobalToLocal(viewport, new Vector2(4 * tileSize + tileSize / 5, 2 * tileSize + tileSize / 3)), getSourceRectForStandardTileSheet(debrisSpriteSheet, 16), Color.White);
                    }
                    if (displayFarmer && player.ActiveObject != null && player.ActiveObject.bigCraftable && checkBigCraftableBoundariesForFrontLayer() && currentLocation.Map.GetLayer("Front").PickTile(new Location(player.getStandingX(), player.getStandingY()), viewport.Size) == null)
                        drawPlayerHeldObject(player);
                    else if (displayFarmer && player.ActiveObject != null && ((currentLocation.Map.GetLayer("Front").PickTile(new Location((int)player.position.X, (int)player.position.Y - tileSize * 3 / 5), viewport.Size) != null && !currentLocation.Map.GetLayer("Front").PickTile(new Location((int)player.position.X, (int)player.position.Y - tileSize * 3 / 5), viewport.Size).TileIndexProperties.ContainsKey("FrontAlways")) || (currentLocation.Map.GetLayer("Front").PickTile(new Location(player.GetBoundingBox().Right, (int)player.position.Y - tileSize * 3 / 5), viewport.Size) != null && !currentLocation.Map.GetLayer("Front").PickTile(new Location(player.GetBoundingBox().Right, (int)player.position.Y - tileSize * 3 / 5), viewport.Size).TileIndexProperties.ContainsKey("FrontAlways"))))
                        drawPlayerHeldObject(player);
                    if ((player.UsingTool || pickingTool) && player.CurrentTool != null && (!player.CurrentTool.Name.Equals("Seeds") || pickingTool) && currentLocation.Map.GetLayer("Front").PickTile(new Location(player.getStandingX(), (int)player.position.Y - tileSize * 3 / 5), viewport.Size) != null && currentLocation.Map.GetLayer("Front").PickTile(new Location(player.getStandingX(), player.getStandingY()), viewport.Size) == null)
                        drawTool(player);
                    if (currentLocation.Map.GetLayer("AlwaysFront") != null)
                    {
                        mapDisplayDevice.BeginScene(spriteBatch);
                        currentLocation.Map.GetLayer("AlwaysFront").Draw(mapDisplayDevice, viewport, Location.Origin, false, pixelZoom);
                        mapDisplayDevice.EndScene();
                    }
                    if (toolHold > 400f && player.CurrentTool.UpgradeLevel >= 1 && player.canReleaseTool)
                    {
                        Color color = Color.White;
                        switch ((int)(toolHold / 600f) + 2)
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
                        spriteBatch.Draw(littleEffect, new Rectangle((int)player.getLocalPosition(viewport).X - 2, (int)player.getLocalPosition(viewport).Y - (player.CurrentTool.Name.Equals("Watering Can") ? 0 : tileSize) - 2, (int)(toolHold % 600f * 0.08f) + 4, tileSize / 8 + 4), Color.Black);
                        spriteBatch.Draw(littleEffect, new Rectangle((int)player.getLocalPosition(viewport).X, (int)player.getLocalPosition(viewport).Y - (player.CurrentTool.Name.Equals("Watering Can") ? 0 : tileSize), (int)(toolHold % 600f * 0.08f), tileSize / 8), color);
                    }
                    if (isDebrisWeather && currentLocation.IsOutdoors && !currentLocation.ignoreDebrisWeather && !currentLocation.Name.Equals("Desert") && viewport.X > -10)
                    {
                        foreach (WeatherDebris current6 in debrisWeather)
                            current6.draw(spriteBatch);
                    }
                    farmEvent?.draw(spriteBatch);
                    if (currentLocation.LightLevel > 0f && timeOfDay < 2000)
                        spriteBatch.Draw(fadeToBlackRect, graphics.GraphicsDevice.Viewport.Bounds, Color.Black * currentLocation.LightLevel);
                    if (screenGlow)
                        spriteBatch.Draw(fadeToBlackRect, graphics.GraphicsDevice.Viewport.Bounds, screenGlowColor * screenGlowAlpha);
                    currentLocation.drawAboveAlwaysFrontLayer(spriteBatch);
                    if (player.CurrentTool is FishingRod && ((player.CurrentTool as FishingRod).isTimingCast || (player.CurrentTool as FishingRod).castingChosenCountdown > 0f || (player.CurrentTool as FishingRod).fishCaught || (player.CurrentTool as FishingRod).showingTreasure))
                        player.CurrentTool.draw(spriteBatch);
                    if (isRaining && currentLocation.IsOutdoors && !currentLocation.Name.Equals("Desert") && !(currentLocation is Summit) && (!eventUp || currentLocation.isTileOnMap(new Vector2(viewport.X / tileSize, viewport.Y / tileSize))))
                    {
                        for (int j = 0; j < rainDrops.Length; j++)
                            spriteBatch.Draw(rainTexture, rainDrops[j].position, getSourceRectForStandardTileSheet(rainTexture, rainDrops[j].frame), Color.White);
                    }

                    spriteBatch.End();

                    //base.Draw(gameTime);

                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    if (eventUp && currentLocation.currentEvent != null)
                    {
                        foreach (NPC current7 in currentLocation.currentEvent.actors)
                        {
                            if (current7.isEmoting)
                            {
                                Vector2 localPosition = current7.getLocalPosition(viewport);
                                localPosition.Y -= tileSize * 2 + pixelZoom * 3;
                                if (current7.age == 2)
                                    localPosition.Y += tileSize / 2;
                                else if (current7.gender == 1)
                                    localPosition.Y += tileSize / 6;
                                spriteBatch.Draw(emoteSpriteSheet, localPosition, new Rectangle(current7.CurrentEmoteIndex * (tileSize / 4) % emoteSpriteSheet.Width, current7.CurrentEmoteIndex * (tileSize / 4) / emoteSpriteSheet.Width * (tileSize / 4), tileSize / 4, tileSize / 4), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, current7.getStandingY() / 10000f);
                            }
                        }
                    }
                    spriteBatch.End();
                    if (drawLighting)
                    {
                        spriteBatch.Begin(SpriteSortMode.Deferred, new BlendState
                        {
                            ColorBlendFunction = BlendFunction.ReverseSubtract,
                            ColorDestinationBlend = Blend.One,
                            ColorSourceBlend = Blend.SourceColor
                        }, SamplerState.LinearClamp, null, null);
                        spriteBatch.Draw(lightmap, Vector2.Zero, lightmap.Bounds, Color.White, 0f, Vector2.Zero, options.lightingQuality, SpriteEffects.None, 1f);
                        if (isRaining && currentLocation.isOutdoors && !(currentLocation is Desert))
                        {
                            spriteBatch.Draw(staminaRect, graphics.GraphicsDevice.Viewport.Bounds, Color.OrangeRed * 0.45f);
                        }
                        spriteBatch.End();
                    }
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    if (drawGrid)
                    {
                        int num2 = -viewport.X % tileSize;
                        float num3 = -(float)viewport.Y % tileSize;
                        for (int k = num2; k < graphics.GraphicsDevice.Viewport.Width; k += tileSize)
                            spriteBatch.Draw(staminaRect, new Rectangle(k, (int)num3, 1, graphics.GraphicsDevice.Viewport.Height), Color.Red * 0.5f);
                        for (float num4 = num3; num4 < (float)graphics.GraphicsDevice.Viewport.Height; num4 += (float)tileSize)
                            spriteBatch.Draw(staminaRect, new Rectangle(num2, (int)num4, graphics.GraphicsDevice.Viewport.Width, 1), Color.Red * 0.5f);
                    }
                    if (currentBillboard != 0)
                        drawBillboard();

                    GraphicsEvents.InvokeOnPreRenderHudEventNoCheck(this);
                    if ((displayHUD || eventUp) && currentBillboard == 0 && gameMode == 3 && !freezeControls && !panMode)
                    {
                        GraphicsEvents.InvokeOnPreRenderHudEvent(this);
                        drawHUD();
                        GraphicsEvents.InvokeOnPostRenderHudEvent(this);
                    }
                    else if (activeClickableMenu == null && farmEvent == null)
                        spriteBatch.Draw(mouseCursors, new Vector2(getOldMouseX(), getOldMouseY()), getSourceRectForStandardTileSheet(mouseCursors, 0, 16, 16), Color.White, 0f, Vector2.Zero, 4f + dialogueButtonScale / 150f, SpriteEffects.None, 1f);
                    GraphicsEvents.InvokeOnPostRenderHudEventNoCheck(this);

                    if (hudMessages.Any() && (!eventUp || isFestival()))
                    {
                        for (int l = hudMessages.Count - 1; l >= 0; l--)
                            hudMessages[l].draw(spriteBatch, l);
                    }
                }
                farmEvent?.draw(spriteBatch);
                if (dialogueUp && !nameSelectUp && !messagePause && !(activeClickableMenu is DialogueBox))
                    drawDialogueBox();

                if (progressBar)
                {
                    spriteBatch.Draw(fadeToBlackRect, new Rectangle((graphics.GraphicsDevice.Viewport.TitleSafeArea.Width - dialogueWidth) / 2, graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - tileSize * 2, dialogueWidth, tileSize / 2), Color.LightGray);
                    spriteBatch.Draw(staminaRect, new Rectangle((graphics.GraphicsDevice.Viewport.TitleSafeArea.Width - dialogueWidth) / 2, graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - tileSize * 2, (int)(pauseAccumulator / pauseTime * dialogueWidth), tileSize / 2), Color.DimGray);
                }
                if (eventUp)
                    currentLocation.currentEvent?.drawAfterMap(spriteBatch);
                if (isRaining && currentLocation.isOutdoors && !(currentLocation is Desert))
                    spriteBatch.Draw(staminaRect, graphics.GraphicsDevice.Viewport.Bounds, Color.Blue * 0.2f);
                if ((fadeToBlack || globalFade) && !menuUp && (!nameSelectUp || messagePause))
                    spriteBatch.Draw(fadeToBlackRect, graphics.GraphicsDevice.Viewport.Bounds, Color.Black * ((gameMode == 0) ? (1f - fadeToBlackAlpha) : fadeToBlackAlpha));
                else if (flashAlpha > 0f)
                {
                    if (options.screenFlash)
                        spriteBatch.Draw(fadeToBlackRect, graphics.GraphicsDevice.Viewport.Bounds, Color.White * Math.Min(1f, flashAlpha));
                    flashAlpha -= 0.1f;
                }

                if ((messagePause || globalFade) && dialogueUp)
                    drawDialogueBox();

                foreach (var current8 in screenOverlayTempSprites)
                {
                    current8.draw(spriteBatch, true);
                }

                if (debugMode)
                {
                    spriteBatch.DrawString(smallFont, string.Concat(new object[]
                    {
                            panMode ? ((getOldMouseX() + viewport.X) / tileSize + "," + (getOldMouseY() + viewport.Y) / tileSize) : string.Concat("aplayer: ", player.getStandingX() / tileSize, ", ", player.getStandingY() / tileSize),
                            Environment.NewLine,
                            "debugOutput: ",
                            debugOutput
                    }), new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999999f);
                }
                /*if (inputMode)
                {
                    spriteBatch.DrawString(smallFont, "Input: " + debugInput, new Vector2(tileSize, tileSize * 3), Color.Purple);
                }*/
                if (showKeyHelp)
                    spriteBatch.DrawString(smallFont, keyHelpString, new Vector2(tileSize, viewport.Height - tileSize - (dialogueUp ? (tileSize * 3 + (isQuestion ? (questionChoices.Count * tileSize) : 0)) : 0) - smallFont.MeasureString(keyHelpString).Y), Color.LightGray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999999f);

                GraphicsEvents.InvokeOnPreRenderGuiEventNoCheck(this);
                if (activeClickableMenu != null)
                {
                    GraphicsEvents.InvokeOnPreRenderGuiEvent(this);
                    activeClickableMenu.draw(spriteBatch);
                    GraphicsEvents.InvokeOnPostRenderGuiEvent(this);
                }
                else
                {
                    farmEvent?.drawAboveEverything(spriteBatch);
                }
                GraphicsEvents.InvokeOnPostRenderGuiEventNoCheck(this);

                GraphicsEvents.InvokeOnPostRenderEvent(this);
                spriteBatch.End();

                GraphicsEvents.InvokeDrawInRenderTargetTick(this);

                if (!ZoomLevelIsOne)
                {
                    GraphicsDevice.SetRenderTarget(null);
                    GraphicsDevice.Clear(bgColor);
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                    spriteBatch.Draw(screen, Vector2.Zero, screen.Bounds, Color.White, 0f, Vector2.Zero, options.zoomLevel, SpriteEffects.None, 1f);
                    spriteBatch.End();
                }

                GraphicsEvents.InvokeAfterDraw(this);
            }
            catch (Exception ex)
            {
                Logging.Log.Exception($"An error occured in the overridden draw loop", ex);
            }
        }
    }
}
