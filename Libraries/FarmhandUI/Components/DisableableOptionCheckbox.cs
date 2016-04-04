using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace Farmhand.UI.Components
{    
    public class DisableableOptionCheckbox : OptionsElement
    {
        public static Rectangle SourceRectUnchecked = new Rectangle(227, 425, 9, 9);
        public static Rectangle SourceRectChecked = new Rectangle(236, 425, 9, 9);
        public static Rectangle SourceRectDisabled = new Rectangle(338, 494, 10, 10);
        public const int PixelsWide = 9;
        public string DisableReason;
        public bool IsHovered;
        public bool IsChecked;
        public bool IsDisabled;

        public DisableableOptionCheckbox(string label, int whichOption, int x = -1, int y = -1)
          : base(label, x, y, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom, whichOption)
        {
            IsHovered = false;
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (greyedOut)
                return;
            Game1.playSound("drumkit6");
            base.receiveLeftClick(x, y);
            IsChecked = !IsChecked;
        }

        public override void draw(SpriteBatch b, int slotX, int slotY)
        {
            b.Draw(Game1.mouseCursors, new Vector2(slotX + bounds.X, slotY + bounds.Y), 
                IsDisabled ? SourceRectDisabled : (IsChecked ? SourceRectChecked : SourceRectUnchecked), Color.White * (greyedOut ? 0.33f : 1f), 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.4f);


            if(IsDisabled && IsHovered)
            {
                Utility.drawBoldText(b, DisableReason, Game1.dialogueFont, new Vector2(slotX + bounds.X + bounds.Width + Game1.pixelZoom * 2, slotY + bounds.Y), Color.Red, 1f, 0.1f);
            }
            else
            {
                base.draw(b, slotX, slotY);
            }
        }
    }
}
