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
    public class Game1 : Farmhand.Overrides.GameOverrideBase
    {
        private readonly Dictionary<string, Action<GameTime>> _versionSpecificOverrides = new Dictionary<string, Action<GameTime>>();
        
        public bool ZoomLevelIsOne => Game1.options.zoomLevel.Equals(1.0f);

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
            try
            {
                if (!this.ZoomLevelIsOne)
                    this.GraphicsDevice.SetRenderTarget(screen);

                this.GraphicsDevice.Clear(bgColor);
                if (Game1.options.showMenuBackground && Game1.activeClickableMenu != null && Game1.activeClickableMenu.showWithoutTransparencyIfOptionIsSet())
                {
                    Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    Game1.activeClickableMenu.drawBackground(Game1.spriteBatch);
                    GraphicsEvents.InvokeOnPreRenderGuiEvent(this);
                    Game1.activeClickableMenu.draw(Game1.spriteBatch);
                    GraphicsEvents.InvokeOnPostRenderGuiEvent(this);
                    Game1.spriteBatch.End();
                    if (!this.ZoomLevelIsOne)
                    {
                        this.GraphicsDevice.SetRenderTarget(null);
                        this.GraphicsDevice.Clear(bgColor);
                        Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                        Game1.spriteBatch.Draw(screen, Vector2.Zero, screen.Bounds, Color.White, 0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
                        Game1.spriteBatch.End();
                    }
                    return;
                }
                if (Game1.gameMode == 11)
                {
                    Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    Game1.spriteBatch.DrawString(Game1.smoothFont, "Stardew Valley has crashed...", new Vector2(16f, 16f), Color.HotPink);
                    Game1.spriteBatch.DrawString(Game1.smoothFont, "Please send the error report or a screenshot of this message to @ConcernedApe. (http://stardewvalley.net/contact/)", new Vector2(16f, 32f), new Color(0, 255, 0));
                    Game1.spriteBatch.DrawString(Game1.smoothFont, Game1.parseText(Game1.errorMessage, Game1.smoothFont, Game1.graphics.GraphicsDevice.Viewport.Width), new Vector2(16f, 48f), Color.White);
                    Game1.spriteBatch.End();
                    return;
                }
                if (Game1.currentMinigame != null)
                {
                    Game1.currentMinigame.draw(Game1.spriteBatch);
                    if (Game1.globalFade && !Game1.menuUp && (!Game1.nameSelectUp || Game1.messagePause))
                    {
                        Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                        Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * ((Game1.gameMode == 0) ? (1f - Game1.fadeToBlackAlpha) : Game1.fadeToBlackAlpha));
                        Game1.spriteBatch.End();
                    }
                    if (!this.ZoomLevelIsOne)
                    {
                        this.GraphicsDevice.SetRenderTarget(null);
                        this.GraphicsDevice.Clear(bgColor);
                        Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                        Game1.spriteBatch.Draw(screen, Vector2.Zero, screen.Bounds, Color.White, 0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
                        Game1.spriteBatch.End();
                    }
                    return;
                }
                if (Game1.showingEndOfNightStuff)
                {
                    Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    Game1.activeClickableMenu?.draw(Game1.spriteBatch);
                    Game1.spriteBatch.End();
                    if (!this.ZoomLevelIsOne)
                    {
                        this.GraphicsDevice.SetRenderTarget(null);
                        this.GraphicsDevice.Clear(bgColor);
                        Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                        Game1.spriteBatch.Draw(screen, Vector2.Zero, screen.Bounds, Color.White, 0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
                        Game1.spriteBatch.End();
                    }
                    return;
                }
                if (Game1.gameMode == 6)
                {
                    Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    string text = "";
                    int num = 0;
                    while (num < gameTime.TotalGameTime.TotalMilliseconds % 999.0 / 333.0)
                    {
                        text += ".";
                        num++;
                    }
                    SpriteText.drawString(Game1.spriteBatch, "Loading" + text, 64, Game1.graphics.GraphicsDevice.Viewport.Height - 64, 999, -1, 999, 1f, 1f, false, 0, "Loading...");
                    Game1.spriteBatch.End();
                    if (!this.ZoomLevelIsOne)
                    {
                        this.GraphicsDevice.SetRenderTarget(null);
                        this.GraphicsDevice.Clear(bgColor);
                        Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                        Game1.spriteBatch.Draw(screen, Vector2.Zero, screen.Bounds, Color.White, 0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
                        Game1.spriteBatch.End();
                    }
                    return;
                }
                if (Game1.gameMode == 0)
                    Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                else
                {
                    if (Game1.drawLighting)
                    {
                        this.GraphicsDevice.SetRenderTarget(Game1.lightmap);
                        this.GraphicsDevice.Clear(Color.White * 0f);
                        Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
                        Game1.spriteBatch.Draw(Game1.staminaRect, Game1.lightmap.Bounds, Game1.currentLocation.name.Equals("UndergroundMine") ? Game1.mine.getLightingColor(gameTime) : ((!Game1.ambientLight.Equals(Color.White) && (!Game1.isRaining || !Game1.currentLocation.isOutdoors)) ? Game1.ambientLight : Game1.outdoorLight));
                        for (int i = 0; i < Game1.currentLightSources.Count; i++)
                        {
                            if (Utility.isOnScreen(Game1.currentLightSources.ElementAt(i).position, (int)(Game1.currentLightSources.ElementAt(i).radius * Game1.tileSize * 4f)))
                                Game1.spriteBatch.Draw(Game1.currentLightSources.ElementAt(i).lightTexture, Game1.GlobalToLocal(Game1.viewport, Game1.currentLightSources.ElementAt(i).position) / Game1.options.lightingQuality, Game1.currentLightSources.ElementAt(i).lightTexture.Bounds, Game1.currentLightSources.ElementAt(i).color, 0f, new Vector2(Game1.currentLightSources.ElementAt(i).lightTexture.Bounds.Center.X, Game1.currentLightSources.ElementAt(i).lightTexture.Bounds.Center.Y), Game1.currentLightSources.ElementAt(i).radius / Game1.options.lightingQuality, SpriteEffects.None, 0.9f);
                        }
                        Game1.spriteBatch.End();
                        this.GraphicsDevice.SetRenderTarget(this.ZoomLevelIsOne ? null : screen);
                    }
                    if (Game1.bloomDay)
                        Game1.bloom?.BeginDraw();
                    this.GraphicsDevice.Clear(bgColor);
                    Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    GraphicsEvents.InvokeOnPreRenderEvent(this);
                    Game1.background?.draw(Game1.spriteBatch);
                    Game1.mapDisplayDevice.BeginScene(Game1.spriteBatch);
                    Game1.currentLocation.Map.GetLayer("Back").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
                    Game1.currentLocation.drawWater(Game1.spriteBatch);
                    if (Game1.CurrentEvent == null)
                    {
                        using (List<NPC>.Enumerator enumerator = Game1.currentLocation.characters.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                NPC current = enumerator.Current;
                                if (current != null && !current.swimming && !current.hideShadow && !current.IsMonster && !Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(current.getTileLocation()))
                                    Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, current.position + new Vector2(current.sprite.spriteWidth * Game1.pixelZoom / 2f, current.GetBoundingBox().Height + (current.IsMonster ? 0 : (Game1.pixelZoom * 3)))), Game1.shadowTexture.Bounds, Color.White, 0f, new Vector2(Game1.shadowTexture.Bounds.Center.X, Game1.shadowTexture.Bounds.Center.Y), (Game1.pixelZoom + current.yJumpOffset / 40f) * current.scale, SpriteEffects.None, Math.Max(0f, current.getStandingY() / 10000f) - 1E-06f);
                            }
                            goto IL_B30;
                        }
                    }
                    foreach (NPC current2 in Game1.CurrentEvent.actors)
                    {
                        if (!current2.swimming && !current2.hideShadow && !Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(current2.getTileLocation()))
                            Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, current2.position + new Vector2(current2.sprite.spriteWidth * Game1.pixelZoom / 2f, current2.GetBoundingBox().Height + (current2.IsMonster ? 0 : (Game1.pixelZoom * 3)))), Game1.shadowTexture.Bounds, Color.White, 0f, new Vector2(Game1.shadowTexture.Bounds.Center.X, Game1.shadowTexture.Bounds.Center.Y), (Game1.pixelZoom + current2.yJumpOffset / 40f) * current2.scale, SpriteEffects.None, Math.Max(0f, current2.getStandingY() / 10000f) - 1E-06f);
                    }
                    IL_B30:
                    if (!Game1.player.swimming && !Game1.player.isRidingHorse() && !Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(Game1.player.getTileLocation()))
                        Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.player.position + new Vector2(32f, 24f)), Game1.shadowTexture.Bounds, Color.White, 0f, new Vector2(Game1.shadowTexture.Bounds.Center.X, Game1.shadowTexture.Bounds.Center.Y), 4f - (((Game1.player.running || Game1.player.usingTool) && Game1.player.FarmerSprite.indexInCurrentAnimation > 1) ? (Math.Abs(FarmerRenderer.featureYOffsetPerFrame[Game1.player.FarmerSprite.CurrentFrame]) * 0.5f) : 0f), SpriteEffects.None, 0f);
                    Game1.currentLocation.Map.GetLayer("Buildings").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
                    Game1.mapDisplayDevice.EndScene();
                    Game1.spriteBatch.End();
                    Game1.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    if (Game1.CurrentEvent == null)
                    {
                        using (List<NPC>.Enumerator enumerator3 = Game1.currentLocation.characters.GetEnumerator())
                        {
                            while (enumerator3.MoveNext())
                            {
                                NPC current3 = enumerator3.Current;
                                if (current3 != null && !current3.swimming && !current3.hideShadow && Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(current3.getTileLocation()))
                                    Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, current3.position + new Vector2(current3.sprite.spriteWidth * Game1.pixelZoom / 2f, current3.GetBoundingBox().Height + (current3.IsMonster ? 0 : (Game1.pixelZoom * 3)))), Game1.shadowTexture.Bounds, Color.White, 0f, new Vector2(Game1.shadowTexture.Bounds.Center.X, Game1.shadowTexture.Bounds.Center.Y), (Game1.pixelZoom + current3.yJumpOffset / 40f) * current3.scale, SpriteEffects.None, Math.Max(0f, current3.getStandingY() / 10000f) - 1E-06f);
                            }
                            goto IL_F5F;
                        }
                    }
                    foreach (NPC current4 in Game1.CurrentEvent.actors)
                    {
                        if (!current4.swimming && !current4.hideShadow && Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(current4.getTileLocation()))
                            Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, current4.position + new Vector2(current4.sprite.spriteWidth * Game1.pixelZoom / 2f, current4.GetBoundingBox().Height + (current4.IsMonster ? 0 : (Game1.pixelZoom * 3)))), Game1.shadowTexture.Bounds, Color.White, 0f, new Vector2(Game1.shadowTexture.Bounds.Center.X, Game1.shadowTexture.Bounds.Center.Y), (Game1.pixelZoom + current4.yJumpOffset / 40f) * current4.scale, SpriteEffects.None, Math.Max(0f, current4.getStandingY() / 10000f) - 1E-06f);
                    }
                    IL_F5F:
                    if (!Game1.player.swimming && !Game1.player.isRidingHorse() && Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(Game1.player.getTileLocation()))
                        Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.player.position + new Vector2(32f, 24f)), Game1.shadowTexture.Bounds, Color.White, 0f, new Vector2(Game1.shadowTexture.Bounds.Center.X, Game1.shadowTexture.Bounds.Center.Y), 4f - (((Game1.player.running || Game1.player.usingTool) && Game1.player.FarmerSprite.indexInCurrentAnimation > 1) ? (Math.Abs(FarmerRenderer.featureYOffsetPerFrame[Game1.player.FarmerSprite.CurrentFrame]) * 0.5f) : 0f), SpriteEffects.None, Math.Max(0.0001f, Game1.player.getStandingY() / 10000f + 0.00011f) - 0.0001f);
                    if (Game1.displayFarmer)
                        Game1.player.draw(Game1.spriteBatch);
                    if ((Game1.eventUp || Game1.killScreen) && !Game1.killScreen)
                        Game1.currentLocation.currentEvent?.draw(Game1.spriteBatch);
                    if (Game1.player.currentUpgrade != null && Game1.player.currentUpgrade.daysLeftTillUpgradeDone <= 3 && Game1.currentLocation.Name.Equals("Farm"))
                        Game1.spriteBatch.Draw(Game1.player.currentUpgrade.workerTexture, Game1.GlobalToLocal(Game1.viewport, Game1.player.currentUpgrade.positionOfCarpenter), Game1.player.currentUpgrade.getSourceRectangle(), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, (Game1.player.currentUpgrade.positionOfCarpenter.Y + Game1.tileSize * 3 / 4) / 10000f);
                    Game1.currentLocation.draw(Game1.spriteBatch);
                    if (Game1.eventUp && Game1.currentLocation.currentEvent?.messageToScreen != null)
                        Game1.drawWithBorder(Game1.currentLocation.currentEvent.messageToScreen, Color.Black, Color.White, new Vector2(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width / 2 - Game1.borderFont.MeasureString(Game1.currentLocation.currentEvent.messageToScreen).X / 2f, Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Height - Game1.tileSize), 0f, 1f, 0.999f);
                    if (Game1.player.ActiveObject == null && (Game1.player.UsingTool || Game1.pickingTool) && Game1.player.CurrentTool != null && (!Game1.player.CurrentTool.Name.Equals("Seeds") || Game1.pickingTool))
                        Game1.drawTool(Game1.player);
                    if (Game1.currentLocation.Name.Equals("Farm"))
                        drawFarmBuildings();
                    if (Game1.tvStation >= 0)
                        Game1.spriteBatch.Draw(Game1.tvStationTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(6 * Game1.tileSize + Game1.tileSize / 4, 2 * Game1.tileSize + Game1.tileSize / 2)), new Rectangle(Game1.tvStation * 24, 0, 24, 15), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-08f);
                    if (Game1.panMode)
                    {
                        Game1.spriteBatch.Draw(Game1.fadeToBlackRect, new Rectangle((int)Math.Floor((Game1.getOldMouseX() + Game1.viewport.X) / (double)Game1.tileSize) * Game1.tileSize - Game1.viewport.X, (int)Math.Floor((Game1.getOldMouseY() + Game1.viewport.Y) / (double)Game1.tileSize) * Game1.tileSize - Game1.viewport.Y, Game1.tileSize, Game1.tileSize), Color.Lime * 0.75f);
                        foreach (Warp current5 in Game1.currentLocation.warps)
                            Game1.spriteBatch.Draw(Game1.fadeToBlackRect, new Rectangle(current5.X * Game1.tileSize - Game1.viewport.X, current5.Y * Game1.tileSize - Game1.viewport.Y, Game1.tileSize, Game1.tileSize), Color.Red * 0.75f);
                    }
                    Game1.mapDisplayDevice.BeginScene(Game1.spriteBatch);
                    Game1.currentLocation.Map.GetLayer("Front").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
                    Game1.mapDisplayDevice.EndScene();
                    Game1.currentLocation.drawAboveFrontLayer(Game1.spriteBatch);
                    Game1.spriteBatch.End();
                    Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    if (Game1.currentLocation.Name.Equals("Farm") && Game1.stats.SeedsSown >= 200u)
                    {
                        Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(3 * Game1.tileSize + Game1.tileSize / 4, Game1.tileSize + Game1.tileSize / 3)), Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16), Color.White);
                        Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(4 * Game1.tileSize + Game1.tileSize, 2 * Game1.tileSize + Game1.tileSize)), Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16), Color.White);
                        Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(5 * Game1.tileSize, 2 * Game1.tileSize)), Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16), Color.White);
                        Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(3 * Game1.tileSize + Game1.tileSize / 2, 3 * Game1.tileSize)), Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16), Color.White);
                        Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(5 * Game1.tileSize - Game1.tileSize / 4, Game1.tileSize)), Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16), Color.White);
                        Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(4 * Game1.tileSize, 3 * Game1.tileSize + Game1.tileSize / 6)), Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16), Color.White);
                        Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(4 * Game1.tileSize + Game1.tileSize / 5, 2 * Game1.tileSize + Game1.tileSize / 3)), Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16), Color.White);
                    }
                    if (Game1.displayFarmer && Game1.player.ActiveObject != null && Game1.player.ActiveObject.bigCraftable && this.checkBigCraftableBoundariesForFrontLayer() && Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX(), Game1.player.getStandingY()), Game1.viewport.Size) == null)
                        Game1.drawPlayerHeldObject(Game1.player);
                    else if (Game1.displayFarmer && Game1.player.ActiveObject != null && ((Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location((int)Game1.player.position.X, (int)Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size) != null && !Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location((int)Game1.player.position.X, (int)Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size).TileIndexProperties.ContainsKey("FrontAlways")) || (Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.GetBoundingBox().Right, (int)Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size) != null && !Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.GetBoundingBox().Right, (int)Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size).TileIndexProperties.ContainsKey("FrontAlways"))))
                        Game1.drawPlayerHeldObject(Game1.player);
                    if ((Game1.player.UsingTool || Game1.pickingTool) && Game1.player.CurrentTool != null && (!Game1.player.CurrentTool.Name.Equals("Seeds") || Game1.pickingTool) && Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX(), (int)Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size) != null && Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX(), Game1.player.getStandingY()), Game1.viewport.Size) == null)
                        Game1.drawTool(Game1.player);
                    if (Game1.currentLocation.Map.GetLayer("AlwaysFront") != null)
                    {
                        Game1.mapDisplayDevice.BeginScene(Game1.spriteBatch);
                        Game1.currentLocation.Map.GetLayer("AlwaysFront").Draw(Game1.mapDisplayDevice, Game1.viewport, xTile.Dimensions.Location.Origin, false, Game1.pixelZoom);
                        Game1.mapDisplayDevice.EndScene();
                    }
                    if (Game1.toolHold > 400f && Game1.player.CurrentTool.UpgradeLevel >= 1 && Game1.player.canReleaseTool)
                    {
                        Color color = Color.White;
                        switch ((int)(Game1.toolHold / 600f) + 2)
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
                        Game1.spriteBatch.Draw(Game1.littleEffect, new Rectangle((int)Game1.player.getLocalPosition(Game1.viewport).X - 2, (int)Game1.player.getLocalPosition(Game1.viewport).Y - (Game1.player.CurrentTool.Name.Equals("Watering Can") ? 0 : Game1.tileSize) - 2, (int)(Game1.toolHold % 600f * 0.08f) + 4, Game1.tileSize / 8 + 4), Color.Black);
                        Game1.spriteBatch.Draw(Game1.littleEffect, new Rectangle((int)Game1.player.getLocalPosition(Game1.viewport).X, (int)Game1.player.getLocalPosition(Game1.viewport).Y - (Game1.player.CurrentTool.Name.Equals("Watering Can") ? 0 : Game1.tileSize), (int)(Game1.toolHold % 600f * 0.08f), Game1.tileSize / 8), color);
                    }
                    if (Game1.isDebrisWeather && Game1.currentLocation.IsOutdoors && !Game1.currentLocation.ignoreDebrisWeather && !Game1.currentLocation.Name.Equals("Desert") && Game1.viewport.X > -10)
                    {
                        foreach (WeatherDebris current6 in Game1.debrisWeather)
                            current6.draw(Game1.spriteBatch);
                    }
                    Game1.farmEvent?.draw(Game1.spriteBatch);
                    if (Game1.currentLocation.LightLevel > 0f && Game1.timeOfDay < 2000)
                        Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * Game1.currentLocation.LightLevel);
                    if (Game1.screenGlow)
                        Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Game1.screenGlowColor * Game1.screenGlowAlpha);
                    Game1.currentLocation.drawAboveAlwaysFrontLayer(Game1.spriteBatch);
                    if (Game1.player.CurrentTool is FishingRod && ((Game1.player.CurrentTool as FishingRod).isTimingCast || (Game1.player.CurrentTool as FishingRod).castingChosenCountdown > 0f || (Game1.player.CurrentTool as FishingRod).fishCaught || (Game1.player.CurrentTool as FishingRod).showingTreasure))
                        Game1.player.CurrentTool.draw(Game1.spriteBatch);
                    if (Game1.isRaining && Game1.currentLocation.IsOutdoors && !Game1.currentLocation.Name.Equals("Desert") && !(Game1.currentLocation is Summit) && (!Game1.eventUp || Game1.currentLocation.isTileOnMap(new Vector2(Game1.viewport.X / Game1.tileSize, Game1.viewport.Y / Game1.tileSize))))
                    {
                        for (int j = 0; j < Game1.rainDrops.Length; j++)
                            Game1.spriteBatch.Draw(Game1.rainTexture, Game1.rainDrops[j].position, Game1.getSourceRectForStandardTileSheet(Game1.rainTexture, Game1.rainDrops[j].frame), Color.White);
                    }

                    Game1.spriteBatch.End();

                    //base.Draw(gameTime);

                    Game1.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    if (Game1.eventUp && Game1.currentLocation.currentEvent != null)
                    {
                        foreach (NPC current7 in Game1.currentLocation.currentEvent.actors)
                        {
                            if (current7.isEmoting)
                            {
                                Vector2 localPosition = current7.getLocalPosition(Game1.viewport);
                                localPosition.Y -= Game1.tileSize * 2 + Game1.pixelZoom * 3;
                                if (current7.age == 2)
                                    localPosition.Y += Game1.tileSize / 2;
                                else if (current7.gender == 1)
                                    localPosition.Y += Game1.tileSize / 6;
                                Game1.spriteBatch.Draw(Game1.emoteSpriteSheet, localPosition, new Rectangle(current7.CurrentEmoteIndex * (Game1.tileSize / 4) % Game1.emoteSpriteSheet.Width, current7.CurrentEmoteIndex * (Game1.tileSize / 4) / Game1.emoteSpriteSheet.Width * (Game1.tileSize / 4), Game1.tileSize / 4, Game1.tileSize / 4), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, current7.getStandingY() / 10000f);
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
                        this.drawBillboard();

                    GraphicsEvents.InvokeOnPreRenderHudEventNoCheck(this);
                    if ((displayHUD || eventUp) && currentBillboard == 0 && gameMode == 3 && !freezeControls && !panMode)
                    {
                        GraphicsEvents.InvokeOnPreRenderHudEvent(this);
                        drawHUD();
                        GraphicsEvents.InvokeOnPostRenderHudEvent(this);
                    }
                    else if (Game1.activeClickableMenu == null && Game1.farmEvent == null)
                        Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2(Game1.getOldMouseX(), Game1.getOldMouseY()), Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16), Color.White, 0f, Vector2.Zero, 4f + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
                    GraphicsEvents.InvokeOnPostRenderHudEventNoCheck(this);

                    if (Game1.hudMessages.Any() && (!Game1.eventUp || Game1.isFestival()))
                    {
                        for (int l = Game1.hudMessages.Count - 1; l >= 0; l--)
                            Game1.hudMessages[l].draw(Game1.spriteBatch, l);
                    }
                }
                Game1.farmEvent?.draw(Game1.spriteBatch);
                if (Game1.dialogueUp && !Game1.nameSelectUp && !Game1.messagePause && !(Game1.activeClickableMenu is DialogueBox))
                    drawDialogueBox();

                if (Game1.progressBar)
                {
                    Game1.spriteBatch.Draw(Game1.fadeToBlackRect, new Rectangle((Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width - Game1.dialogueWidth) / 2, Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - Game1.tileSize * 2, Game1.dialogueWidth, Game1.tileSize / 2), Color.LightGray);
                    Game1.spriteBatch.Draw(Game1.staminaRect, new Rectangle((Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width - Game1.dialogueWidth) / 2, Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - Game1.tileSize * 2, (int)(Game1.pauseAccumulator / Game1.pauseTime * Game1.dialogueWidth), Game1.tileSize / 2), Color.DimGray);
                }
                if (Game1.eventUp)
                    Game1.currentLocation.currentEvent?.drawAfterMap(Game1.spriteBatch);
                if (Game1.isRaining && Game1.currentLocation.isOutdoors && !(Game1.currentLocation is Desert))
                    Game1.spriteBatch.Draw(Game1.staminaRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Blue * 0.2f);
                if ((Game1.fadeToBlack || Game1.globalFade) && !Game1.menuUp && (!Game1.nameSelectUp || Game1.messagePause))
                    Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * ((Game1.gameMode == 0) ? (1f - Game1.fadeToBlackAlpha) : Game1.fadeToBlackAlpha));
                else if (Game1.flashAlpha > 0f)
                {
                    if (Game1.options.screenFlash)
                        Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.White * Math.Min(1f, Game1.flashAlpha));
                    Game1.flashAlpha -= 0.1f;
                }

                if ((Game1.messagePause || Game1.globalFade) && Game1.dialogueUp)
                    drawDialogueBox();

                foreach (var current8 in Game1.screenOverlayTempSprites)
                {
                    current8.draw(Game1.spriteBatch, true);
                }

                if (Game1.debugMode)
                {
                    Game1.spriteBatch.DrawString(Game1.smallFont, string.Concat(new object[]
                    {
                            Game1.panMode ? ((Game1.getOldMouseX() + Game1.viewport.X) / Game1.tileSize + "," + (Game1.getOldMouseY() + Game1.viewport.Y) / Game1.tileSize) : string.Concat("aplayer: ", Game1.player.getStandingX() / Game1.tileSize, ", ", Game1.player.getStandingY() / Game1.tileSize),
                            Environment.NewLine,
                            "debugOutput: ",
                            Game1.debugOutput
                    }), new Vector2(this.GraphicsDevice.Viewport.TitleSafeArea.X, this.GraphicsDevice.Viewport.TitleSafeArea.Y), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999999f);
                }
                /*if (inputMode)
                {
                    spriteBatch.DrawString(smallFont, "Input: " + debugInput, new Vector2(tileSize, tileSize * 3), Color.Purple);
                }*/
                if (Game1.showKeyHelp)
                    Game1.spriteBatch.DrawString(Game1.smallFont, Game1.keyHelpString, new Vector2(Game1.tileSize, Game1.viewport.Height - Game1.tileSize - (Game1.dialogueUp ? (Game1.tileSize * 3 + (Game1.isQuestion ? (Game1.questionChoices.Count * Game1.tileSize) : 0)) : 0) - Game1.smallFont.MeasureString(Game1.keyHelpString).Y), Color.LightGray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999999f);

                GraphicsEvents.InvokeOnPreRenderGuiEventNoCheck(this);
                if (Game1.activeClickableMenu != null)
                {
                    GraphicsEvents.InvokeOnPreRenderGuiEvent(this);
                    Game1.activeClickableMenu.draw(Game1.spriteBatch);
                    GraphicsEvents.InvokeOnPostRenderGuiEvent(this);
                }
                else
                {
                    Game1.farmEvent?.drawAboveEverything(Game1.spriteBatch);
                }
                GraphicsEvents.InvokeOnPostRenderGuiEventNoCheck(this);

                GraphicsEvents.InvokeOnPostRenderEvent(this);
                Game1.spriteBatch.End();

                GraphicsEvents.InvokeDrawInRenderTargetTick(this);

                if (!this.ZoomLevelIsOne)
                {
                    this.GraphicsDevice.SetRenderTarget(null);
                    this.GraphicsDevice.Clear(bgColor);
                    Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                    Game1.spriteBatch.Draw(screen, Vector2.Zero, screen.Bounds, Color.White, 0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
                    Game1.spriteBatch.End();
                }

                GraphicsEvents.InvokeAfterDraw(this);
            }
            catch (Exception ex)
            {
                Farmhand.Logging.Log.Exception($"An error occured in the overridden draw loop", ex);
            }
        }
    }
}
