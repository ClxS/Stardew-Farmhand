using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using System.Linq;
using Farmhand;
using Farmhand.Registries;
using Farmhand.Registries.Containers;
using Farmhand.Logging;
using Farmhand.UI.Components;
using StardewValley.BellsAndWhistles;

namespace ModLoaderMod.Menus
{
    public class ModMenu : IClickableMenu
    {
        private readonly List<ClickableComponent> _labels = new List<ClickableComponent>();
        private readonly List<DisableableOptionCheckbox> _modToggles = new List<DisableableOptionCheckbox>();
        private readonly List<ClickableComponent> _optionSlots = new List<ClickableComponent>();
        private readonly Dictionary<DisableableOptionCheckbox, IModManifest> _modOptions = new Dictionary<DisableableOptionCheckbox, IModManifest>();
        private int _currentItemIndex;
        
        private ClickableTextureComponent upArrow;
        private ClickableTextureComponent downArrow;
        private ClickableTextureComponent scrollBar;
        private Rectangle scrollBarRunner;
        private bool scrolling;
        private string hoverText;

        public ModMenu()
          : base(Game1.viewport.Width / 2 - (1050 + borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + borderWidth * 2) / 2, 800 + borderWidth * 2, 600 + borderWidth * 2)
        {
            SetUpPositions();
            Game1.player.faceDirection(2);
            Game1.player.FarmerSprite.StopAnimation();
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);
            SetUpPositions();
        }

        private void SetUpPositions()
        {
            _labels.Clear();
            _modToggles.Clear();
            _currentItemIndex = 0;

            upArrow = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + width + Game1.tileSize / 4, yPositionOnScreen + Game1.tileSize / 4, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), Game1.pixelZoom);
            downArrow = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + width + Game1.tileSize / 4, yPositionOnScreen + height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), Game1.pixelZoom);
            scrollBar = new ClickableTextureComponent(new Rectangle(upArrow.bounds.X + Game1.pixelZoom * 3, upArrow.bounds.Y + upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), Game1.pixelZoom);
            scrollBarRunner = new Rectangle(scrollBar.bounds.X, upArrow.bounds.Y + upArrow.bounds.Height + Game1.pixelZoom, scrollBar.bounds.Width, height - Game1.tileSize - upArrow.bounds.Height - Game1.pixelZoom * 7);

            var mods = ModRegistry.GetRegisteredItems();
            
            for (var index = 0; index < 4; ++index)
                _optionSlots.Add(new ClickableComponent(new Rectangle(xPositionOnScreen + Game1.tileSize / 4, yPositionOnScreen + Game1.tileSize / 4 + index * (height / 4) - (index * 7), width - Game1.tileSize / 2, (height / 4 + Game1.pixelZoom) - 11), string.Concat(index)));

            foreach (var mod in mods)
            {
                if (Equals(mod.UniqueId, ModLoader1.Instance.ModSettings.UniqueId)) continue;

                var checkbox = new DisableableOptionCheckbox("", 11) {
                    IsChecked = mod.ModState == ModState.Loaded
                };

                if(mod.IsFarmhandMod && mod.ModState != ModState.Loaded && mod.ModState != ModState.Deactivated)
                {
                    ResolveLoadingIssue(checkbox, (ModManifest)mod);
                }
                _modToggles.Add(checkbox);
                _modOptions[checkbox] = mod;
            }            
        }

        private void ResolveLoadingIssue(DisableableOptionCheckbox checkbox, ModManifest mod)
        {
            checkbox.IsDisabled = true;
            if(mod.ModState == ModState.MissingDependency)
            {
                var missingDependencies = mod.Dependencies.Where(n => n.DependencyState == DependencyState.Missing);
                var missingParents = mod.Dependencies.Where(n => n.DependencyState == DependencyState.ParentMissing);
                var tooLow = mod.Dependencies.Where(n => n.DependencyState == DependencyState.TooLowVersion);
                var tooHigh = mod.Dependencies.Where(n => n.DependencyState == DependencyState.TooHighVersion);

                var modDependencies = missingDependencies as ModDependency[] ?? missingDependencies.ToArray();
                if (modDependencies.Any())
                {
                    checkbox.DisableReason =
                        $"Missing Dependencies: : {string.Join(", ", modDependencies.Select(n => n.UniqueId))}";
                }
                else if (modDependencies.Any())
                {
                    checkbox.DisableReason =
                        $"Dependent Mods Missing Dependencies: : {string.Join(", ", missingParents.Select(n => n.UniqueId))}";
                }
                else
                {
                    var tooLowDependencies = tooLow as ModDependency[] ?? tooLow.ToArray();
                    if (tooLowDependencies.Any())
                    {
                        checkbox.DisableReason =
                            $"Dependency Version Too Low: : {string.Join(", ", tooLowDependencies.Select(n => n.UniqueId + $"(Minimum: {n.MinimumVersion.ToString()})"))}";
                    }
                    else
                    {
                        var tooHighDependencies = tooHigh as ModDependency[] ?? tooHigh.ToArray();
                        if (tooHighDependencies.Any())
                        {
                            checkbox.DisableReason =
                                $"Dependency Version Too High: : {string.Join(",", tooHighDependencies.Select(n => n.UniqueId + $"(Maximum: {n.MaximumVersion.ToString()})"))}";
                        }
                    }
                }
            }
            else if(mod.ModState == ModState.Errored)
            {
                var lastException = mod.LastException?.Message ?? "";
                checkbox.DisableReason = $"Error: {lastException}";
            }
        }
        

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {

            if (downArrow.containsPoint(x, y) && _currentItemIndex < Math.Max(0, _modToggles.Count - 4))
                downArrowPressed();
            else if (upArrow.containsPoint(x, y) && _currentItemIndex > 0)
                upArrowPressed();
            else if (scrollBar.containsPoint(x, y))
                scrolling = true;
            else if (!downArrow.containsPoint(x, y) && x > xPositionOnScreen + width && (x < xPositionOnScreen + width + Game1.tileSize * 2 && y > yPositionOnScreen) && y < yPositionOnScreen + height)
            {
                scrolling = true;
                leftClickHeld(x, y);
                releaseLeftClick(x, y);
            }

            for (var index = 0; index < _optionSlots.Count; ++index)
            {
                if (_optionSlots[index].containsPoint(x, y) && index < _modToggles.Count)
                {
                    _modToggles[_currentItemIndex + index].receiveLeftClick(x - _optionSlots[index].bounds.X, y - _optionSlots[index].bounds.Y);

                    OnTogglePress(_modToggles[_currentItemIndex + index]);

                    break;
                }
            }
        }

        private void OnTogglePress(DisableableOptionCheckbox disableableOptionCheckbox)
        {
            if (disableableOptionCheckbox.IsDisabled) return;

            var mod = _modOptions[disableableOptionCheckbox];
            if (mod.IsFarmhandMod)
            {
                Game1.playSound("coin");
                if (disableableOptionCheckbox.IsChecked)
                {
                    Log.Info($"Loaded Mod: {_modOptions[disableableOptionCheckbox].Name}");
                    Farmhand.ModLoader.ReactivateMod((ModManifest)mod);
                }
                else
                {
                    Log.Info($"Unloaded Mod: {_modOptions[disableableOptionCheckbox].Name}");
                    Farmhand.ModLoader.DeactivateMod((ModManifest)mod);
                }
            }
        }

        public override void performHoverAction(int x, int y)
        {
            base.performHoverAction(x, y);
            upArrow.tryHover(x, y);
            downArrow.tryHover(x, y);
            scrollBar.tryHover(x, y);

            hoverText = "";
            
            if (scrolling)
                return;
                
            for (var index = 0; index < _optionSlots.Count; ++index)
            {
                if (_currentItemIndex + index < _modToggles.Count && _optionSlots[index].containsPoint(x, y))
                {
                    _optionSlots[index].scale = Math.Min(_optionSlots[index].scale + 0.03f, 1.1f);

                    _modToggles[_currentItemIndex + index].IsHovered = _optionSlots[index].bounds.Contains(x, y) && _modToggles[_currentItemIndex + index].bounds.Contains(x - _optionSlots[index].bounds.X, y - _optionSlots[index].bounds.Y);

                    hoverText = _modOptions[_modToggles[_currentItemIndex + index]].Description;
                }
                else
                    _optionSlots[index].scale = Math.Max(1f, _optionSlots[index].scale - 0.03f);
            }
        }

        public override void draw(SpriteBatch b)
        {
            drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), xPositionOnScreen, yPositionOnScreen, width, height, Color.White, Game1.pixelZoom, true);

            if (_modToggles.Count == 0)
                SpriteText.drawStringHorizontallyCenteredAt(b, "No Mods Found", Game1.graphics.GraphicsDevice.Viewport.Bounds.Center.X, Game1.graphics.GraphicsDevice.Viewport.Bounds.Center.Y);

            upArrow.draw(b);
            downArrow.draw(b);
            if (_modToggles.Count > 4) {
                drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), scrollBarRunner.X, scrollBarRunner.Y, scrollBarRunner.Width, scrollBarRunner.Height, Color.White, Game1.pixelZoom, false);
                scrollBar.draw(b);
            }

            for (var index = 0; index < _optionSlots.Count; ++index) {
                if (_currentItemIndex + index < _modToggles.Count)
                {
                    drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 396, 15, 15), _optionSlots[index].bounds.X, _optionSlots[index].bounds.Y, _optionSlots[index].bounds.Width, _optionSlots[index].bounds.Height, (_optionSlots[index].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()) && !scrolling) ? Color.Wheat : Color.White, Game1.pixelZoom, false);
                    SpriteText.drawString(b, $"{_currentItemIndex + index + 1}.", _optionSlots[index].bounds.X + Game1.pixelZoom * 7 + Game1.tileSize / 2 - SpriteText.getWidthOfString($"{_currentItemIndex + index + 1}.") / 2, _optionSlots[index].bounds.Y + Game1.pixelZoom * 9);
                    SpriteText.drawString(b, _modOptions[_modToggles[_currentItemIndex + index]].Name, _optionSlots[index].bounds.X + Game1.tileSize + Game1.pixelZoom * 9, _optionSlots[index].bounds.Y + Game1.pixelZoom * 9);

                    Utility.drawTextWithShadow(b, $"by {_modOptions[_modToggles[_currentItemIndex + index]].Author}", Game1.dialogueFont, new Vector2(_optionSlots[index].bounds.X + Game1.tileSize + Game1.pixelZoom + 12, _optionSlots[index].bounds.Y + Game1.tileSize + Game1.pixelZoom * 6), Game1.textColor);
                    Utility.drawTextWithShadow(b, $"v{_modOptions[_modToggles[_currentItemIndex + index]].Version}", Game1.dialogueFont, new Vector2((_optionSlots[index].bounds.X + width - Game1.tileSize * 2) - Game1.dialogueFont.MeasureString($"v{_modOptions[_modToggles[_currentItemIndex + index]].Version}").X, _optionSlots[index].bounds.Y + Game1.pixelZoom * 11), Game1.textColor);

                    _modToggles[_currentItemIndex + index].draw(b, _optionSlots[index].bounds.X, _optionSlots[index].bounds.Y + Game1.pixelZoom * 18);
                }
            }

            var loaded = _modToggles.Count(_ => _.IsChecked);
            var total = _modToggles.Count;
            var loadedTextMeasure = Game1.dialogueFont.MeasureString($"Mods Loaded: {loaded}");
            Utility.drawTextWithShadow(b, $"Mods Loaded: {loaded}", Game1.dialogueFont, new Vector2(xPositionOnScreen + width + 78, height - (78 + loadedTextMeasure.Y)), Color.White, 0.7f);
            Utility.drawTextWithShadow(b, $"Mods Installed: {total}", Game1.dialogueFont, new Vector2(xPositionOnScreen + width + 78, height - (72 + loadedTextMeasure.Y) + 24), Color.White, 0.7f);

            b.Draw(Game1.mouseCursors, new Vector2(Game1.getOldMouseX(), Game1.getOldMouseY()), Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16), Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);

            if (!String.IsNullOrEmpty(hoverText))
                drawHoverText(b, Game1.parseText(hoverText, Game1.dialogueFont, 360), Game1.dialogueFont);
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {

        }

        public override void releaseLeftClick(int x, int y)
        {
            base.releaseLeftClick(x, y);
            scrolling = false;
        }

        public override void leftClickHeld(int x, int y)
        {
            base.leftClickHeld(x, y);
            if (!scrolling)
                return;

            var y1 = scrollBar.bounds.Y;
            scrollBar.bounds.Y = Math.Min(yPositionOnScreen + height - Game1.tileSize - Game1.pixelZoom * 3 - scrollBar.bounds.Height, Math.Max(y, yPositionOnScreen + upArrow.bounds.Height + Game1.pixelZoom * 5));
            _currentItemIndex = Math.Min(_modToggles.Count - 4, Math.Max(0, _modToggles.Count * ((y - scrollBarRunner.Y) / scrollBarRunner.Height)));
            setScrollBarToCurrentIndex();
            if (y1 == scrollBar.bounds.Y)
                return;

            Game1.playSound("shiny4");
        }

        private void setScrollBarToCurrentIndex()
        {
            if (_modToggles.Count <= 0)
                return;

            scrollBar.bounds.Y = scrollBarRunner.Height / Math.Max(1, _modToggles.Count - 4 + 1) * _currentItemIndex + upArrow.bounds.Bottom + Game1.pixelZoom;
            if (_currentItemIndex != _modToggles.Count - 4)
                return;

            scrollBar.bounds.Y = downArrow.bounds.Y - scrollBar.bounds.Height - Game1.pixelZoom;
        }

        public override void receiveScrollWheelAction(int direction)
        {
            base.receiveScrollWheelAction(direction);
            if (direction > 0 && _currentItemIndex > 0) {
                upArrowPressed();
            }
            else {
                if (direction >= 0 || _currentItemIndex >= Math.Max(0, _modToggles.Count - 4))
                    return;
                downArrowPressed();
            }
        }

        private void downArrowPressed()
        {
            downArrow.scale = downArrow.baseScale;
            ++_currentItemIndex;
            Game1.playSound("shwip");
            setScrollBarToCurrentIndex();
        }

        private void upArrowPressed()
        {
            upArrow.scale = upArrow.baseScale;
            --_currentItemIndex;
            Game1.playSound("shwip");
            setScrollBarToCurrentIndex();
        }
    }
}
