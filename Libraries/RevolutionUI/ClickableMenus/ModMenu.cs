using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Revolution.Registries;
using Revolution.Registries.Containers;
using Revolution.UI.Components;

namespace Revolution.UI.ClickableMenus
{
    public class ModMenu : IClickableMenu
    {
        private List<ClickableComponent> labels = new List<ClickableComponent>();
        private List<DisableableOptionCheckbox> modToggles = new List<DisableableOptionCheckbox>();
        private List<ClickableComponent> optionSlots = new List<ClickableComponent>();
        private Dictionary<DisableableOptionCheckbox, ModInfo> modOptions = new Dictionary<DisableableOptionCheckbox, ModInfo>();
        private int currentItemIndex;

        public ModMenu()
          : base(Game1.viewport.Width / 2 - (632 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2 - Game1.tileSize, 632 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2 + Game1.tileSize, false)
        {           
            this.setUpPositions();
            Game1.player.faceDirection(2);
            Game1.player.FarmerSprite.StopAnimation();
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);
            this.xPositionOnScreen = Game1.viewport.Width / 2 - (632 + IClickableMenu.borderWidth * 2) / 2;
            this.yPositionOnScreen = Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2 - Game1.tileSize;
            this.setUpPositions();
        }

        private void setUpPositions()
        {
            this.labels.Clear();
            this.modToggles.Clear();
            currentItemIndex = 0;
                        
            var mods = ModRegistry.GetRegisteredItems();

            for (int index = 0; index < 7; ++index)
                this.optionSlots.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize * 6 / 4 + Game1.pixelZoom + index * ((height - Game1.tileSize * 2) / 7), width - Game1.tileSize / 2, (height - Game1.tileSize * 2) / 7 + Game1.pixelZoom), string.Concat((object)index)));

            foreach (var mod in mods)
            {
                var checkbox = new DisableableOptionCheckbox(string.Format("{0} by {1}", mod.Name, mod.Author), 11, -1, -1);
                checkbox.isChecked = mod.ModState == ModState.Loaded;
                checkbox.isDisabled = mod.ModState != ModState.Loaded && mod.ModState != ModState.Inactive;                
                modToggles.Add(checkbox);
                modOptions[checkbox] = mod;
            }
            modToggles[0].isDisabled = true;
            modToggles[0].disableReason = "Missing Dependency: \"Test\"";
        }

        private void optionButtonClick(string name)
        {           
            Game1.playSound("coin");
        }
        
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            for (int index = 0; index < Enumerable.Count<ClickableComponent>((IEnumerable<ClickableComponent>)this.optionSlots); ++index)
            {
                if (this.optionSlots[index].bounds.Contains(x, y) && this.currentItemIndex + index < Enumerable.Count<OptionsElement>((IEnumerable<OptionsElement>)this.modToggles) && this.modToggles[this.currentItemIndex + index].bounds.Contains(x - this.optionSlots[index].bounds.X, y - this.optionSlots[index].bounds.Y))
                {
                    this.modToggles[this.currentItemIndex + index].receiveLeftClick(x - this.optionSlots[index].bounds.X, y - this.optionSlots[index].bounds.Y);
                    break;
                }
            }
        }

        public override void performHoverAction(int x, int y)
        {
            for (int index = 0; index < Enumerable.Count<ClickableComponent>((IEnumerable<ClickableComponent>)this.optionSlots); ++index)
            {
                if (this.currentItemIndex + index < Enumerable.Count<OptionsElement>((IEnumerable<OptionsElement>)this.modToggles))
                {
                    this.modToggles[this.currentItemIndex + index].isHovered = this.optionSlots[index].bounds.Contains(x, y) && this.modToggles[this.currentItemIndex + index].bounds.Contains(x - this.optionSlots[index].bounds.X, y - this.optionSlots[index].bounds.Y);
                }
            }
        }

        public override void draw(SpriteBatch b)
        {
            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, (string)null, false);

            //foreach (ClickableTextureComponent textureComponent in this.leftSelectionButtons)
            //    textureComponent.draw(b);

            for (int index = 0; index < Enumerable.Count<ClickableComponent>((IEnumerable<ClickableComponent>)this.optionSlots); ++index)
            {
                if (this.currentItemIndex >= 0 && this.currentItemIndex + index < Enumerable.Count<OptionsElement>((IEnumerable<OptionsElement>)this.modToggles))
                    this.modToggles[this.currentItemIndex + index].draw(b, this.optionSlots[index].bounds.X, this.optionSlots[index].bounds.Y);
            }
            

            foreach (ClickableComponent clickableComponent in this.labels)
            {
                string text = "";
                Color color = Game1.textColor;
                
                Utility.drawTextWithShadow(b, clickableComponent.name, Game1.smallFont, new Vector2((float)clickableComponent.bounds.X, (float)clickableComponent.bounds.Y), color, 1f, -1f, -1, -1, 1f, 3);
                if (Enumerable.Count<char>((IEnumerable<char>)text) > 0)
                    Utility.drawTextWithShadow(b, text, Game1.smallFont, new Vector2((float)(clickableComponent.bounds.X + Game1.tileSize / 3) - Game1.smallFont.MeasureString(text).X / 2f, (float)(clickableComponent.bounds.Y + Game1.tileSize / 2)), color, 1f, -1f, -1, -1, 1f, 3);
            }

            //foreach (ClickableTextureComponent textureComponent in this.rightSelectionButtons)
            //    textureComponent.draw(b);
                       
            b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {

        }
    }
}
