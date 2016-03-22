using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.UI.Components
{    
    public class DisableableOptionCheckbox : OptionsElement
    {
        public static Rectangle sourceRectUnchecked = new Rectangle(227, 425, 9, 9);
        public static Rectangle sourceRectChecked = new Rectangle(236, 425, 9, 9);
        public static Rectangle sourceRectDisabled = new Rectangle(338, 494, 10, 10);
        public const int pixelsWide = 9;
        public string disableReason;
        public bool isHovered;
        public bool isChecked;
        public bool isDisabled;

        public DisableableOptionCheckbox(string label, int whichOption, int x = -1, int y = -1)
          : base(label, x, y, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom, whichOption)
        {
            isHovered = false;
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut)
                return;
            Game1.playSound("drumkit6");
            base.receiveLeftClick(x, y);
            this.isChecked = !this.isChecked;
        }

        public override void draw(SpriteBatch b, int slotX, int slotY)
        {
            b.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.bounds.X), (float)(slotY + this.bounds.Y)), 
                new Rectangle?(this.isDisabled ? DisableableOptionCheckbox.sourceRectDisabled : (this.isChecked ? DisableableOptionCheckbox.sourceRectChecked : DisableableOptionCheckbox.sourceRectUnchecked)), Color.White * (this.greyedOut ? 0.33f : 1f), 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.4f);


            if(isDisabled && isHovered)
            {
                Utility.drawBoldText(b, disableReason, Game1.dialogueFont, new Vector2((float)(slotX + this.bounds.X + this.bounds.Width + Game1.pixelZoom * 2), (float)(slotY + this.bounds.Y)), Color.Red, 1f, 0.1f);
            }
            else
            {
                base.draw(b, slotX, slotY);
            }
        }
    }
}
