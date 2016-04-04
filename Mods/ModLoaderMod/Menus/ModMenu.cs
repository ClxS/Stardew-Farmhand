using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using System.Linq;
using Farmhand;
using Farmhand.Registries;
using Farmhand.Registries.Containers;
using Farmhand.UI.Components;
using Farmhand.Logging;

namespace ModLoaderMod.Menus
{
    public class ModMenu : IClickableMenu
    {
        private readonly List<ClickableComponent> _labels = new List<ClickableComponent>();
        private readonly List<DisableableOptionCheckbox> _modToggles = new List<DisableableOptionCheckbox>();
        private readonly List<ClickableComponent> _optionSlots = new List<ClickableComponent>();
        private readonly Dictionary<DisableableOptionCheckbox, ModManifest> _modOptions = new Dictionary<DisableableOptionCheckbox, ModManifest>();
        private int _currentItemIndex;

        public ModMenu()
          : base(Game1.viewport.Width / 2 - (632 + borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + borderWidth * 2) / 2 - Game1.tileSize, 632 + borderWidth * 2, 600 + borderWidth * 2 + Game1.tileSize, false)
        {
            SetUpPositions();
            Game1.player.faceDirection(2);
            Game1.player.FarmerSprite.StopAnimation();
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);
            xPositionOnScreen = Game1.viewport.Width / 2 - (632 + borderWidth * 2) / 2;
            yPositionOnScreen = Game1.viewport.Height / 2 - (600 + borderWidth * 2) / 2 - Game1.tileSize;
            SetUpPositions();
        }

        private void SetUpPositions()
        {
            _labels.Clear();
            _modToggles.Clear();
            _currentItemIndex = 0;

            var mods = ModRegistry.GetRegisteredItems();

            for (var index = 0; index < 7; ++index)
                _optionSlots.Add(new ClickableComponent(new Rectangle(xPositionOnScreen + Game1.tileSize / 4, yPositionOnScreen + Game1.tileSize * 6 / 4 + Game1.pixelZoom + index * ((height - Game1.tileSize * 2) / 7), width - Game1.tileSize / 2, (height - Game1.tileSize * 2) / 7 + Game1.pixelZoom), string.Concat(index)));

            foreach (var mod in mods)
            {
                if (mod.UniqueId == ModLoader1.Instance.ModSettings.UniqueId) continue;

                var checkbox = new DisableableOptionCheckbox($"{mod.Name} by {mod.Author}", 11)
                {
                    IsChecked = mod.ModState == ModState.Loaded
                };

                if(mod.ModState != ModState.Loaded && mod.ModState != ModState.Deactivated)
                {
                    ResolveLoadingIssue(checkbox, mod);
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
                        $"{"Missing Dependencies: "}: {string.Join(", ", modDependencies.Select(n => n.UniqueId))}";
                }
                else if (modDependencies.Any())
                {
                    checkbox.DisableReason =
                        $"{"Dependent Mods Missing Dependencies: "}: {string.Join(", ", missingParents.Select(n => n.UniqueId))}";
                }
                else
                {
                    var tooLowDependencies = tooLow as ModDependency[] ?? tooLow.ToArray();
                    if (tooLowDependencies.Any())
                    {
                        checkbox.DisableReason =
                            $"{"Dependency Version Too Low: "}: {string.Join(", ", tooLowDependencies.Select(n => n.UniqueId + $"(Minimum: {n.MinimumVersion.ToString()})"))}";
                    }
                    else
                    {
                        var tooHighDependencies = tooHigh as ModDependency[] ?? tooHigh.ToArray();
                        if (tooHighDependencies.Any())
                        {
                            checkbox.DisableReason =
                                $"{"Dependency Version Too High: "}: {string.Join(",", tooHighDependencies.Select(n => n.UniqueId + $"(Maximum: {n.MaximumVersion.ToString()})"))}";
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
            for (var index = 0; index < _optionSlots.Count; ++index)
            {
                if (_optionSlots[index].bounds.Contains(x, y) && _currentItemIndex + index < _modToggles.Count && _modToggles[_currentItemIndex + index].bounds.Contains(x - _optionSlots[index].bounds.X, y - _optionSlots[index].bounds.Y))
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

            Game1.playSound("coin");
            if (disableableOptionCheckbox.IsChecked)
            {
                Log.Info($"Loaded Mod: {_modOptions[disableableOptionCheckbox].Name}");
                Farmhand.ModLoader.ReactivateMod(_modOptions[disableableOptionCheckbox]);
            }
            else
            {
                Log.Info($"Unloaded Mod: {_modOptions[disableableOptionCheckbox].Name}");
                Farmhand.ModLoader.DeactivateMod(_modOptions[disableableOptionCheckbox]);
            }
        }

        public override void performHoverAction(int x, int y)
        {
            for (var index = 0; index < _optionSlots.Count; ++index)
            {
                if (_currentItemIndex + index < _modToggles.Count)
                {
                    _modToggles[_currentItemIndex + index].IsHovered = _optionSlots[index].bounds.Contains(x, y) && _modToggles[_currentItemIndex + index].bounds.Contains(x - _optionSlots[index].bounds.X, y - _optionSlots[index].bounds.Y);
                }
            }
        }

        public override void draw(SpriteBatch b)
        {
            Game1.drawDialogueBox(xPositionOnScreen, yPositionOnScreen, width, height, false, true);
            
            for (var index = 0; index < _optionSlots.Count; ++index)
            {
                if (_currentItemIndex >= 0 && _currentItemIndex + index < _modToggles.Count)
                    _modToggles[_currentItemIndex + index].draw(b, _optionSlots[index].bounds.X, _optionSlots[index].bounds.Y);
            }


            foreach (var clickableComponent in _labels)
            {
                const string text = "";
                var color = Game1.textColor;

                Utility.drawTextWithShadow(b, clickableComponent.name, Game1.smallFont, new Vector2(clickableComponent.bounds.X, clickableComponent.bounds.Y), color);
                if (text.Any())
                    Utility.drawTextWithShadow(b, text, Game1.smallFont, new Vector2(clickableComponent.bounds.X + Game1.tileSize / 3 - Game1.smallFont.MeasureString(text).X / 2f, clickableComponent.bounds.Y + Game1.tileSize / 2), color);
            }

            b.Draw(Game1.mouseCursors, new Vector2(Game1.getOldMouseX(), Game1.getOldMouseY()), Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16), Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {

        }
    }
}
