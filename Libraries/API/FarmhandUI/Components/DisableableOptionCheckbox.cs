namespace Farmhand.UI.Components
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;
    using StardewValley.Menus;

    /// <summary>
    ///     Custom UI control providing a checkbox which can be disabled.
    /// </summary>
    public class DisableableOptionCheckbox : OptionsElement
    {
        /// <summary>
        ///     Pixel width of the check box
        /// </summary>
        private const int PixelsWide = 9;

        /// <summary>
        ///     The unchecked texture source
        /// </summary>
        private static readonly Rectangle SourceRectUnchecked = new Rectangle(227, 425, 9, 9);

        /// <summary>
        ///     The checked texture source
        /// </summary>
        private static readonly Rectangle SourceRectChecked = new Rectangle(236, 425, 9, 9);

        /// <summary>
        ///     The disabled texture source
        /// </summary>
        private static readonly Rectangle SourceRectDisabled = new Rectangle(338, 494, 10, 10);

        /// <summary>
        ///     Initializes a new instance of the <see cref="DisableableOptionCheckbox" /> class.
        /// </summary>
        /// <param name="label">
        ///     The label for this check box
        /// </param>
        /// <param name="whichOption">
        ///     The index of the option this check box relates to
        /// </param>
        /// <param name="x">
        ///     The x position of the check box
        /// </param>
        /// <param name="y">
        ///     The y position of the check box
        /// </param>
        public DisableableOptionCheckbox(string label, int whichOption, int x = -1, int y = -1)
            : base(label, x, y, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom, whichOption)
        {
            this.IsHovered = false;
        }

        /// <summary>
        ///     Gets or sets the reason the check box is disabled
        /// </summary>
        public string DisableReason { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the check box is hovered.
        /// </summary>
        public bool IsHovered { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the check box is checked.
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the check box is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        ///     Called when checkbox receives a left click
        /// </summary>
        /// <param name="x">
        ///     The x position of the click
        /// </param>
        /// <param name="y">
        ///     The y position of the click
        /// </param>
        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut)
            {
                return;
            }

            Game1.playSound("drumkit6");
            base.receiveLeftClick(x, y);
            this.IsChecked = !this.IsChecked;
        }

        /// <summary>
        ///     The draw method of the check box
        /// </summary>
        /// <param name="b">
        ///     The sprite batch to use
        /// </param>
        /// <param name="slotX">
        ///     The x slot
        /// </param>
        /// <param name="slotY">
        ///     The y slot
        /// </param>
        public override void draw(SpriteBatch b, int slotX, int slotY)
        {
            b.Draw(
                Game1.mouseCursors,
                new Vector2(slotX + this.bounds.X, slotY + this.bounds.Y),
                this.IsDisabled ? SourceRectDisabled : (this.IsChecked ? SourceRectChecked : SourceRectUnchecked),
                Color.White * (this.greyedOut ? 0.33f : 1f),
                0.0f,
                Vector2.Zero,
                Game1.pixelZoom,
                SpriteEffects.None,
                0.4f);

            if (this.IsDisabled && this.IsHovered)
            {
                Utility.drawBoldText(
                    b,
                    this.DisableReason,
                    Game1.dialogueFont,
                    new Vector2(slotX + this.bounds.X + this.bounds.Width + Game1.pixelZoom * 2, slotY + this.bounds.Y),
                    Color.Red,
                    1f,
                    0.1f);
            }
            else
            {
                base.draw(b, slotX, slotY);
            }
        }
    }
}