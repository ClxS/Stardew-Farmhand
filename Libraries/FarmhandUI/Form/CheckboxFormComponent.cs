namespace Farmhand.UI.Form
{
    using Farmhand.UI.Base;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    ///     The checkbox form component.
    /// </summary>
    public class CheckboxFormComponent : BaseFormComponent
    {
        /// <summary>
        ///     Texture location of unchecked check box
        /// </summary>
        protected static readonly Rectangle SourceRectUnchecked = new Rectangle(227, 425, 9, 9);

        /// <summary>
        ///     Texture location of checked check box
        /// </summary>
        protected static readonly Rectangle SourceRectChecked = new Rectangle(236, 425, 9, 9);

        /// <summary>
        ///     Initializes a new instance of the <see cref="CheckboxFormComponent" /> class.
        /// </summary>
        /// <param name="offset">
        ///     The offset of this component
        /// </param>
        /// <param name="label">
        ///     The label of the check box
        /// </param>
        /// <param name="handler">
        ///     The check box's value changed handler
        /// </param>
        public CheckboxFormComponent(Point offset, string label, ValueChanged<bool> handler = null)
        {
            this.SetScaledArea(new Rectangle(offset.X, offset.Y, 9 + this.GetStringWidth(label, Game1.smallFont), 9));
            this.Value = false;
            this.Label = label;
            if (handler != null)
            {
                this.Handler += handler;
            }
        }

        /// <summary>
        ///     Gets or sets the check box's label.
        /// </summary>
        protected string Label { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the check box is checked.
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        ///     The ValueChanged handler.
        /// </summary>
        public event ValueChanged<bool> Handler;

        /// <summary>
        ///     Called when the left mouse button is clicked over this component
        /// </summary>
        /// <param name="p">
        ///     The mouse position
        /// </param>
        /// <param name="o">
        ///     The origin of this component
        /// </param>
        public override void LeftClick(Point p, Point o)
        {
            if (this.Disabled)
            {
                return;
            }

            Game1.playSound("drumkit6");
            this.Value = !this.Value;
            this.Handler?.Invoke(this, this.Parent, this.Parent.GetAttachedMenu(), this.Value);
        }

        /// <summary>
        ///     The draw loop for this component
        /// </summary>
        /// <param name="b">
        ///     The sprite batch to use
        /// </param>
        /// <param name="o">
        ///     The origin for this component
        /// </param>
        public override void Draw(SpriteBatch b, Point o)
        {
            if (!this.Visible)
            {
                return;
            }

            b.Draw(
                Game1.mouseCursors,
                new Vector2(o.X + this.Area.X, o.Y + this.Area.Y),
                this.Value ? SourceRectChecked : SourceRectUnchecked,
                Color.White * (this.Disabled ? 0.33f : 1f),
                0.0f,
                Vector2.Zero,
                Game1.pixelZoom,
                SpriteEffects.None,
                0.4f);
            Utility.drawTextWithShadow(
                b,
                this.Label,
                Game1.smallFont,
                new Vector2(o.X + this.Area.X + Zoom10, o.Y + this.Area.Y + Zoom2),
                Game1.textColor * (this.Disabled ? 0.33f : 1f),
                1f,
                0.1f);
        }
    }
}