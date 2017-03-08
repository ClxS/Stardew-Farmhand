namespace Farmhand.UI.Components.Containers
{
    using System;
    using System.Collections.Generic;

    using Farmhand.UI.Components.Interfaces;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    ///     The scrollable collection component.
    /// </summary>
    public class ScrollableCollectionComponent : GenericCollectionComponent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ScrollableCollectionComponent" /> class.
        /// </summary>
        /// <param name="area">
        ///     The area.
        /// </param>
        /// <param name="components">
        ///     The components.
        /// </param>
        public ScrollableCollectionComponent(Rectangle area, List<IMenuComponent> components = null)
            : base(area, components)
        {
        }

        /// <summary>
        ///     Gets or sets the texture position of the down button
        /// </summary>
        protected static Rectangle DownButton { get; set; } = new Rectangle(421, 472, 11, 12);

        /// <summary>
        ///     Gets or sets the texture position of the up button
        /// </summary>
        protected static Rectangle UpButton { get; set; } = new Rectangle(421, 459, 11, 12);

        /// <summary>
        ///     Gets or sets the bar offset
        /// </summary>
        protected int BarOffset { get; set; }

        /// <summary>
        ///     Gets or sets the counter
        /// </summary>
        protected int Counter { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the down is active
        /// </summary>
        protected bool DownActive { get; set; }

        /// <summary>
        ///     Gets or sets the inner height
        /// </summary>
        protected int InnerHeight { get; set; }

        /// <summary>
        ///     Gets or sets the limiter.
        /// </summary>
        protected int Limiter { get; set; } = 20;

        /// <summary>
        ///     Gets or sets the scroll offset.
        /// </summary>
        protected int ScrollOffset { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the up is active.
        /// </summary>
        protected bool UpActive { get; set; }

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

            b.End();
            var reg = this.EventRegion;
            b.GraphicsDevice.ScissorRectangle = new Rectangle(reg.X + o.X, reg.Y + o.Y, reg.Width, reg.Height);
            b.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                new RasterizerState { ScissorTestEnable = true });
            var o2 = new Point(o.X + reg.X, o.Y + reg.Y - (this.ScrollOffset * Zoom10));
            if (this.DrawOrder != null)
            {
                foreach (var el in this.DrawOrder)
                {
                    el.Draw(b, o2);
                }
            }

            b.End();
            b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);

            // Scrollbar
            if (this.BarOffset == 0)
            {
                return;
            }

            // Up
            b.Draw(
                Game1.mouseCursors,
                new Rectangle(
                    this.Area.X + o.X + this.Area.Width - (this.UpActive ? Zoom12 : Zoom11 + Zoom05),
                    this.Area.Y + o.Y + (this.UpActive ? 0 : Zoom05),
                    this.UpActive ? Zoom12 : Zoom11,
                    this.UpActive ? Zoom13 : Zoom12),
                UpButton,
                Color.White * (this.ScrollOffset > 0 ? 1 : 0.5f));

            // down
            b.Draw(
                Game1.mouseCursors,
                new Rectangle(
                    this.Area.X + o.X + this.Area.Width - (this.DownActive ? Zoom12 : Zoom11 + Zoom05),
                    this.Area.Y + o.Y + this.Area.Height - Zoom12 - (this.DownActive ? Zoom05 : 0),
                    this.DownActive ? Zoom12 : Zoom11,
                    this.DownActive ? Zoom13 : Zoom12),
                DownButton,
                Color.White * (this.ScrollOffset < this.InnerHeight ? 1 : 0.5f));
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
            var up = new Rectangle(
                this.Area.X + o.X + this.Area.Width - (this.UpActive ? Zoom12 : Zoom11 + Zoom05),
                this.Area.Y + o.Y + (this.UpActive ? 0 : Zoom05),
                this.UpActive ? Zoom12 : Zoom11,
                this.UpActive ? Zoom13 : Zoom12);
            var down = new Rectangle(
                this.Area.X + o.X + this.Area.Width - (this.DownActive ? Zoom12 : Zoom11 + Zoom05),
                this.Area.Y + o.Y + this.Area.Height - Zoom12 - (this.DownActive ? Zoom05 : 0),
                this.DownActive ? Zoom12 : Zoom11,
                this.DownActive ? Zoom13 : Zoom12);
            this.UpActive = this.ScrollOffset > 0 && up.Contains(p);
            this.DownActive = this.ScrollOffset < this.InnerHeight && down.Contains(p);
            base.HoverOver(p, o);
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
            if (!this.Visible)
            {
                return;
            }

            if (!this.UpActive && !this.DownActive)
            {
                var o2 = new Point(
                    this.Area.X + o.X,
                    this.Area.Y + o.Y - (this.ScrollOffset * Zoom10));
                if (this.EventOrder != null)
                {
                    foreach (var el in this.EventOrder)
                    {
                        if (el.InBounds(p, o2))
                        {
                            this.GiveFocus(el);
                            el.LeftClick(p, o2);
                            return;
                        }
                    }
                }

                this.ResetFocus();
            }
            else
            {
                this.Counter = 0;
                this.Limiter = 10;
                this.ArrowClick(p, o);
            }
        }

        /// <summary>
        ///     Called when the left mouse button is held over this component
        /// </summary>
        /// <param name="p">
        ///     The mouse position
        /// </param>
        /// <param name="o">
        ///     The origin of this component
        /// </param>
        public override void LeftHeld(Point p, Point o)
        {
            base.LeftHeld(p, o);
            if (!this.UpActive && !this.DownActive)
            {
                return;
            }

            this.Counter++;
            if (this.Counter % this.Limiter != 0)
            {
                return;
            }

            this.Limiter = Math.Max(1, this.Limiter - 1);
            this.Counter = 0;
            this.ArrowClick(p, o);
        }

        /// <summary>
        ///     Called when the left mouse button is released over this component
        /// </summary>
        /// <param name="p">
        ///     The mouse position
        /// </param>
        /// <param name="o">
        ///     The origin of this component
        /// </param>
        public override void LeftUp(Point p, Point o)
        {
            this.Limiter = 10;
            this.Counter = 0;
            base.LeftUp(p, o);
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
            if (!this.Visible)
            {
                return;
            }
            
            if (this.InBounds(p, o))
            {
                base.Scroll(d, p, o);
                var change = d / 120;
                var oldOffset = this.ScrollOffset;
                this.ScrollOffset = Math.Max(0, Math.Min(this.ScrollOffset - change, this.InnerHeight));
                if (oldOffset != this.ScrollOffset)
                {
                    Game1.playSound("drumkit6");
                }
            }
        }

        /// <summary>
        ///     Called when one of the scroll bar arrows are clicked
        /// </summary>
        /// <param name="p">
        ///     The mouse position
        /// </param>
        /// <param name="o">
        ///     The origin of this component
        /// </param>
        protected void ArrowClick(Point p, Point o)
        {
            if (this.UpActive && this.ScrollOffset > 0)
            {
                this.ScrollOffset--;
                Game1.playSound("drumkit6");
            }
            else if (this.DownActive && this.ScrollOffset < this.InnerHeight)
            {
                this.ScrollOffset++;
                Game1.playSound("drumkit6");
            }
        }

        /// <summary>
        ///     Updates the component draw order.
        /// </summary>
        protected override void UpdateDrawOrder()
        {
            base.UpdateDrawOrder();
            var height = this.Area.Height;
            if (this.DrawOrder != null)
            {
                foreach (var c in this.DrawOrder)
                {
                    if (!c.Visible)
                    {
                        return;
                    }

                    var r = c.GetRegion();
                    var b = r.Y + r.Height;
                    if (b > height)
                    {
                        height = b;
                    }
                }
            }

            this.BarOffset = height > this.Area.Height ? Zoom12 : 0;
            this.InnerHeight = (int)Math.Ceiling((height - this.Area.Height) / (double)Zoom10);
        }
    }
}