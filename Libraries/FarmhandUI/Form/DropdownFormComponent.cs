namespace Farmhand.UI.Form
{
    using System;
    using System.Collections.Generic;

    using Farmhand.UI.Base;
    using Farmhand.UI.Interfaces;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using StardewValley;
    using StardewValley.Menus;

    /// <summary>
    ///     The dropdown form component.
    /// </summary>
    public class DropdownFormComponent : BaseFormComponent
    {
        /// <summary>
        ///     Texture location for dropdown background
        /// </summary>
        protected static readonly Rectangle Background = new Rectangle(433, 451, 3, 3);

        /// <summary>
        ///     Texture location for dropdown button
        /// </summary>
        protected static readonly Rectangle Button = new Rectangle(438, 450, 9, 11);

        /// <summary>
        ///     Texture location for dropdown up scroll button
        /// </summary>
        protected static readonly Rectangle UpScroll = new Rectangle(421, 459, 11, 12);

        /// <summary>
        ///     Texture location for dropdown down scroll button
        /// </summary>
        protected static readonly Rectangle DownScroll = new Rectangle(421, 472, 11, 12);
        
        private string value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DropdownFormComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of the dropdown.
        /// </param>
        /// <param name="values">
        ///     A list of possible values.
        /// </param>
        /// <param name="handler">
        ///     The selected value handler.
        /// </param>
        public DropdownFormComponent(Point position, List<string> values, ValueChanged<string> handler = null)
            : this(position, 75, values, handler)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DropdownFormComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of the dropdown.
        /// </param>
        /// <param name="width">
        ///     The dropdown width.
        /// </param>
        /// <param name="values">
        ///     A list of possible values.
        /// </param>
        /// <param name="handler">
        ///     The selected value handler.
        /// </param>
        public DropdownFormComponent(
            Point position,
            int width,
            List<string> values,
            ValueChanged<string> handler = null)
        {
            this.SetScaledArea(new Rectangle(position.X, position.Y, width, 11));
            this.Values = values;
            this.Value = this.Values[0];
            if (handler != null)
            {
                this.Handler += handler;
            }
        }

        /// <summary>
        ///     Gets or sets a list of value entries in the dropdown
        /// </summary>
        protected List<string> Values { get; set; }

        /// <summary>
        ///     Gets or sets the selected value.
        /// </summary>
        public string Value
        {
            get
            {
                return this.value;
            }

            set
            {
                if (this.Values.Contains(value))
                {
                    this.value = value;
                }
            }
        }

        /// <summary>
        ///     The selected value changed handler.
        /// </summary>
        public event ValueChanged<string> Handler;

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

#pragma warning disable 0169
            // ReSharper disable once UnusedVariable
            var dropdownSelect = new DropdownSelect(
                new Point(o.X + this.Area.X, o.Y + this.Area.Y + this.Area.Height),
                this.Area.Width,
                new Rectangle(o.X + this.Area.X, o.Y + this.Area.Y, this.Area.Width, this.Area.Height),
                this,
                this.Parent);
#pragma warning restore 0169
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

            // Selected background
            IClickableMenu.drawTextureBox(
                b,
                Game1.mouseCursors,
                Background,
                o.X + this.Area.X,
                o.Y + this.Area.Y,
                this.Area.Width - Game1.pixelZoom * Button.Width,
                Zoom11,
                Color.White * (this.Disabled ? 0.33f : 1f),
                Game1.pixelZoom,
                false);

            // Selected label
            Utility.drawTextWithShadow(
                b,
                this.Value,
                Game1.smallFont,
                new Vector2(o.X + this.Area.X + Zoom2, o.Y + this.Area.Y + Zoom3),
                Game1.textColor * (this.Disabled ? 0.33f : 1f));

            // Selector button
            b.Draw(
                Game1.mouseCursors,
                new Vector2(o.X + this.Area.X + this.Area.Width - Game1.pixelZoom * Button.Width, o.Y + this.Area.Y),
                Button,
                Color.White * (this.Disabled ? 0.33f : 1f),
                0.0f,
                Vector2.Zero,
                Game1.pixelZoom,
                SpriteEffects.None,
                0.88f);
        }

        #region Nested type: DropdownSelect

        /// <summary>
        ///     A component representing a dropdown's selectable items
        /// </summary>
        protected class DropdownSelect : BaseKeyboardFormComponent
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="DropdownSelect" /> class.
            /// </summary>
            /// <param name="position">
            ///     The position of this item.
            /// </param>
            /// <param name="width">
            ///     The width of this item.
            /// </param>
            /// <param name="self">
            ///     This item's bounding area.
            /// </param>
            /// <param name="owner">
            ///     The owning dropdown for this item.
            /// </param>
            /// <param name="collection">
            ///     The collection containing this item.
            /// </param>
            public DropdownSelect(
                Point position,
                int width,
                Rectangle self,
                DropdownFormComponent owner,
                IComponentContainer collection)
            {
                this.MaxScroll = Math.Max(0, owner.Values.Count - 10);
                this.Size = Math.Min(10, owner.Values.Count);
                this.Value = owner.Value;
                this.Owner = owner;
                this.Collection = collection;
                this.Self = self;
                this.Area = new Rectangle(
                    position.X,
                    position.Y,
                    width,
                    Zoom2 + Game1.pixelZoom * Math.Min(7 * owner.Values.Count, 70));
                collection.GetAttachedMenu().GiveFocus(this);
                if (!collection.GetAttachedMenu().EventRegion.Contains(this.Area.X, this.Area.Y + this.Area.Height))
                {
                    // ReSharper disable once VirtualMemberCallInConstructor
                    this.MoveRegion(0, -(this.Area.Height + Zoom9));
                }

                var index = owner.Values.IndexOf(this.Value);
                this.ScrollOffset = Math.Max(0, index - 9);
                this.KeyboardOffset = index;
            }

            /// <summary>
            ///     Gets he owning collection
            /// </summary>
            protected IComponentContainer Collection { get; }

            /// <summary>
            ///     Gets or sets the hover offset.
            /// </summary>
            protected int HoverOffset { get; set; }

            /// <summary>
            ///     Gets or sets the keyboard offset.
            /// </summary>
            protected int KeyboardOffset { get; set; }

            /// <summary>
            ///     Gets or sets the max scroll.
            /// </summary>
            protected int MaxScroll { get; set; }

            /// <summary>
            ///     Gets or sets the owner of this item.
            /// </summary>
            protected DropdownFormComponent Owner { get; set; }

            /// <summary>
            ///     Gets or sets the scroll offset.
            /// </summary>
            protected int ScrollOffset { get; set; }

            /// <summary>
            ///     Gets this components own rectangle.
            /// </summary>
            protected Rectangle Self { get; }

            /// <summary>
            ///     Gets or sets a value indicating whether to show hover state.
            /// </summary>
            protected bool ShowHover { get; set; }

            /// <summary>
            ///     Gets or sets the size of this item.
            /// </summary>
            protected int Size { get; set; }

            /// <summary>
            ///     Gets or sets the value of this item.
            /// </summary>
            protected string Value { get; set; }

            /// <summary>
            ///     The get cursor index.
            /// </summary>
            /// <param name="p">
            ///     The mouse location
            /// </param>
            /// <param name="o">
            ///     This components origin
            /// </param>
            /// <returns>
            ///     The cursor index <see cref="int" />.
            /// </returns>
            protected int GetCursorIndex(Point p, Point o)
            {
                // ReSharper disable once PossibleLossOfFraction
                var index = (int)Math.Floor((p.Y - this.Area.Y) / Game1.pixelZoom / 7D) + this.ScrollOffset;
                if (index < 0)
                {
                    index = 0;
                }

                if (index >= this.Owner.Values.Count)
                {
                    index = this.Owner.Values.Count - 1;
                }

                return index;
            }

            /// <summary>
            ///     Checks whether a point is within this components bounding area
            /// </summary>
            /// <param name="p">
            ///     The point to check
            /// </param>
            /// <param name="o">
            ///     The origin of this component
            /// </param>
            /// <returns>
            ///     Whether the point is within the bounds
            /// </returns>
            public override bool InBounds(Point p, Point o)
            {
                return true;
            }

            /// <summary>
            ///     Called when this component receives focus
            /// </summary>
            public override void FocusGained()
            {
                this.Selected = true;
            }

            /// <summary>
            ///     Called when this component loses focus
            /// </summary>
            public override void FocusLost()
            {
                if (this.Value == this.Owner.Value)
                {
                    return;
                }

                this.Owner.Value = this.Value;
                this.Owner.Handler?.Invoke(this.Owner, this.Collection, this.Parent.GetAttachedMenu(), this.Owner.Value);
            }

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
                this.Parent.ResetFocus();
                if (base.InBounds(p, new Point(0, 0)))
                {
                    this.Value = this.Owner.Values[this.GetCursorIndex(p, o)];
                }
                else if (!this.Self.Contains(p))
                {
                    this.Parent.GetAttachedMenu().receiveLeftClick(p.X, p.Y, false);
                }
            }

            /// <summary>
            ///     Called when the right mouse button is clicked over this component
            /// </summary>
            /// <param name="p">
            ///     The mouse position
            /// </param>
            /// <param name="o">
            ///     The origin of this component
            /// </param>
            public override void RightClick(Point p, Point o)
            {
                if (!base.InBounds(p, new Point(0, 0)))
                {
                    this.Parent.ResetFocus();
                    this.Parent.GetAttachedMenu().receiveRightClick(p.X, p.Y, false);
                }
            }

            /// <summary>
            ///     Called when the scroll wheel is used over this component
            /// </summary>
            /// <param name="d">
            ///     The delta value of the scroll
            /// </param>
            /// <param name="p">
            ///     The mouse position
            /// </param>
            /// <param name="o">
            ///     The origin of this component
            /// </param>
            public override void Scroll(int d, Point p, Point o)
            {
                var change = d / 120;
                var oldOffset = this.ScrollOffset;
                this.ScrollOffset = Math.Max(0, Math.Min(this.ScrollOffset - change, this.MaxScroll));
                if (oldOffset != this.ScrollOffset)
                {
                    Game1.playSound("drumkit6");
                }
            }

            /// <summary>
            ///     Called when the mouse is hovered over this component
            /// </summary>
            /// <param name="p">
            ///     The mouse position
            /// </param>
            /// <param name="o">
            ///     The origin of this component
            /// </param>
            public override void HoverOver(Point p, Point o)
            {
                this.ShowHover = base.InBounds(p, new Point(0, 0));
                this.HoverOffset = this.GetCursorIndex(p, o);
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
                var origin = new Point(0, 0);
                var col = Color.Black * 0.25f;

                // Background
                IClickableMenu.drawTextureBox(
                    b,
                    Game1.mouseCursors,
                    Background,
                    origin.X + this.Area.X,
                    origin.Y + this.Area.Y - Game1.pixelZoom,
                    this.Area.Width - Zoom2,
                    this.Area.Height,
                    Color.White,
                    Game1.pixelZoom,
                    false);
                for (var c = 0; c < this.Size; c++)
                {
                    // Selected
                    if (this.Owner.Values[this.ScrollOffset + c] == this.Value)
                    {
                        b.Draw(
                            Game1.staminaRect,
                            new Rectangle(
                                origin.X + this.Area.X + Game1.pixelZoom,
                                origin.Y + this.Area.Y + Zoom7 * c,
                                this.Area.Width - Zoom4,
                                Zoom7),
                            new Rectangle(0, 0, 1, 1),
                            Color.Wheat * 0.5f);
                    }

                    if (this.Selected && this.KeyboardOffset == this.ScrollOffset + c)
                    {
                        // Top
                        b.Draw(
                            Game1.staminaRect,
                            new Rectangle(
                                origin.X + this.Area.X + Game1.pixelZoom,
                                origin.Y + this.Area.Y + Zoom7 * c,
                                this.Area.Width - Zoom4,
                                Zoom05),
                            new Rectangle(0, 0, 1, 1),
                            col);

                        // Bottom
                        b.Draw(
                            Game1.staminaRect,
                            new Rectangle(
                                origin.X + this.Area.X + Game1.pixelZoom,
                                origin.Y + this.Area.Y + Zoom7 * c + Zoom6 + Zoom05,
                                this.Area.Width - Zoom4,
                                Zoom05),
                            new Rectangle(0, 0, 1, 1),
                            col);

                        // Left
                        b.Draw(
                            Game1.staminaRect,
                            new Rectangle(
                                origin.X + this.Area.X + Game1.pixelZoom,
                                origin.Y + this.Area.Y + Zoom7 * c + Zoom05,
                                Zoom05,
                                Zoom6),
                            new Rectangle(0, 0, 1, 1),
                            col);

                        // Right
                        b.Draw(
                            Game1.staminaRect,
                            new Rectangle(
                                origin.X + this.Area.X + this.Area.Width - Zoom3 - Zoom05,
                                origin.Y + this.Area.Y + Zoom7 * c + Zoom05,
                                Zoom05,
                                Zoom6),
                            new Rectangle(0, 0, 1, 1),
                            col);
                    }

                    // Hover
                    if (this.ShowHover && this.HoverOffset == this.ScrollOffset + c)
                    {
                        b.Draw(
                            Game1.staminaRect,
                            new Rectangle(
                                origin.X + this.Area.X + Game1.pixelZoom + Zoom05,
                                origin.Y + this.Area.Y + Zoom05 + Zoom7 * c,
                                this.Area.Width - Zoom5,
                                Zoom6),
                            new Rectangle(0, 0, 1, 1),
                            Color.Wheat * 0.75f);
                    }

                    // Text
                    Utility.drawTextWithShadow(
                        b,
                        this.Owner.Values[this.ScrollOffset + c],
                        Game1.smallFont,
                        new Vector2(
                            origin.X + this.Area.X + Zoom2,
                            origin.Y + this.Area.Y + Zoom7 * c + Game1.pixelZoom),
                        Game1.textColor * (this.Disabled ? 0.33f : 1f));
                }

                // ScrollUp
                if (this.ScrollOffset > 0)
                {
                    b.Draw(
                        Game1.mouseCursors,
                        new Rectangle(
                            origin.X + this.Area.X + this.Area.Width - Zoom2,
                            origin.Y + this.Area.Y,
                            Zoom7,
                            Zoom7),
                        UpScroll,
                        Color.White);
                }

                // ScrollDown
                if (this.ScrollOffset < this.MaxScroll)
                {
                    b.Draw(
                        Game1.mouseCursors,
                        new Rectangle(
                            origin.X + this.Area.X + this.Area.Width - Zoom2,
                            origin.Y + this.Area.Y + this.Area.Height - Zoom9,
                            Zoom7,
                            Zoom7),
                        DownScroll,
                        Color.White);
                }
            }

            /// <summary>
            ///     Called when this component receives a command
            /// </summary>
            /// <param name="cmd">
            ///     The command received
            /// </param>
            public override void CommandReceived(char cmd)
            {
                switch ((int)cmd)
                {
                    case 13:
                        this.Value = this.Owner.Values[this.KeyboardOffset];
                        this.Parent.ResetFocus();
                        break;
                }
            }

            /// <summary>
            ///     Called when this component receives a special key
            /// </summary>
            /// <param name="key">
            ///     The key received
            /// </param>
            public override void SpecialReceived(Keys key)
            {
                switch (key)
                {
                    case Keys.Down:
                        if (this.KeyboardOffset < this.Owner.Values.Count - 1)
                        {
                            this.KeyboardOffset++;
                        }

                        if (this.KeyboardOffset - this.ScrollOffset > 9 && this.ScrollOffset < this.MaxScroll)
                        {
                            this.ScrollOffset++;
                        }

                        break;
                    case Keys.Up:
                        if (this.KeyboardOffset > 0)
                        {
                            this.KeyboardOffset--;
                        }

                        if (this.KeyboardOffset - this.ScrollOffset < 0 && this.ScrollOffset > 0)
                        {
                            this.ScrollOffset--;
                        }

                        break;
                }
            }
        }

        #endregion
    }
}