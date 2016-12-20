namespace Farmhand.UI.Base
{
    using System;

    using Interfaces;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    /// The base menu component.
    /// </summary>
    public abstract class BaseMenuComponent : IMenuComponent
    {
        /// <summary>
        /// Zoom level 0.5 (pixelZoom / 2).
        /// </summary>
        protected static readonly int Zoom05 = Game1.pixelZoom / 2;

        /// <summary>
        /// Zoom level 2 (pixelZoom * 2).
        /// </summary>
        protected static readonly int Zoom2 = 2 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 3 (pixelZoom * 3).
        /// </summary>
        protected static readonly int Zoom3 = 3 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 4 (pixelZoom * 4).
        /// </summary>
        protected static readonly int Zoom4 = 4 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 5 (pixelZoom * 5).
        /// </summary>
        protected static readonly int Zoom5 = 5 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 6 (pixelZoom * 6).
        /// </summary>
        protected static readonly int Zoom6 = 6 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 7 (pixelZoom * 7).
        /// </summary>
        protected static readonly int Zoom7 = 7 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 8 (pixelZoom * 8).
        /// </summary>
        protected static readonly int Zoom8 = 8 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 9 (pixelZoom * 9).
        /// </summary>
        protected static readonly int Zoom9 = 9 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 10 (pixelZoom * 10).
        /// </summary>
        protected static readonly int Zoom10 = 10 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 11 (pixelZoom * 11).
        /// </summary>
        protected static readonly int Zoom11 = 11 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 12 (pixelZoom * 12).
        /// </summary>
        protected static readonly int Zoom12 = 12 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 13 (pixelZoom * 13).
        /// </summary>
        protected static readonly int Zoom13 = 13 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 14 (pixelZoom * 14).
        /// </summary>
        protected static readonly int Zoom14 = 14 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 15 (pixelZoom * 15).
        /// </summary>
        protected static readonly int Zoom15 = 15 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 16 (pixelZoom * 16).
        /// </summary>
        protected static readonly int Zoom16 = 16 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 17 (pixelZoom * 17).
        /// </summary>
        protected static readonly int Zoom17 = 17 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 20 (pixelZoom * 20).
        /// </summary>
        protected static readonly int Zoom20 = 20 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 22 (pixelZoom * 22).
        /// </summary>
        protected static readonly int Zoom22 = 22 * Game1.pixelZoom;

        /// <summary>
        /// Zoom level 28 (pixelZoom * 28).
        /// </summary>
        protected static readonly int Zoom28 = 28 * Game1.pixelZoom;

        /// <summary>
        /// The parent container of this component
        /// </summary>
        private IComponentContainer parent;

        /// <summary>
        /// The bounding area of this component
        /// </summary>
        private Rectangle area;
        
        /// <summary>
        /// Gets the parent of this component
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// Triggered when parent is null
        /// </exception>
        public IComponentContainer Parent
        {
            get
            {
                if (this.parent == null)
                {
                    throw new NullReferenceException("Component attempted to reference its parent while not attached");
                }

                return this.parent;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the component is visible.
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Gets or sets the layer of this component.
        /// </summary>
        public int Layer { get; set; } = 0;

        /// <summary>
        /// Gets or sets the area of this component.
        /// </summary>
        protected Rectangle Area
        {
            get
            {
                return this.area;
            }
            set
            {
                this.area = value;
            }
        }

        /// <summary>
        /// Gets the backing texture of this component
        /// </summary>
        protected Texture2D Texture { get; }

        /// <summary>
        /// Gets the cropped area of this component
        /// </summary>
        protected Rectangle Crop { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMenuComponent"/> class.
        /// </summary>
        protected BaseMenuComponent()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMenuComponent"/> class.
        /// </summary>
        /// <param name="area">
        /// The bounding area of the component
        /// </param>
        /// <param name="texture">
        /// The backing texture of the component
        /// </param>
        /// <param name="crop">
        /// The cropped area of the component
        /// </param>
        protected BaseMenuComponent(Rectangle area, Texture2D texture, Rectangle? crop = null)
        {
            if (crop == null)
            {
                crop = new Rectangle(0, 0, texture.Width, texture.Height);
            }

            this.Texture = texture;
            this.Crop = (Rectangle)crop;
            this.SetScaledArea(area);
        }
        
        /// <summary>
        /// Attaches this component to another
        /// </summary>
        /// <param name="collection">
        /// The collection to attach to
        /// </param>
        /// <exception cref="Exception">
        /// Thrown if this component already has a parent
        /// </exception>
        public void Attach(IComponentContainer collection)
        {
            if (this.parent != null)
            {
                throw new Exception(
                    "Component is already attached and must be detached first before it can be attached again");
            }

            this.parent = collection;
        }

        /// <summary>
        /// Detaches this component from it's parent
        /// </summary>
        /// <param name="collection">
        /// Unused
        /// </param>
        /// <exception cref="Exception">
        /// Thrown if this component does not have a parent
        /// </exception>
        public void Detach(IComponentContainer collection)
        {
            if (this.parent == null)
            {
                throw new Exception("Component is not attached and must be attached first before it can be detached");
            }

            this.parent = null;
        }

        /// <summary>
        /// Gets the position of this component
        /// </summary>
        /// <returns>
        /// The position of this component as a <see cref="Point"/>.
        /// </returns>
        public virtual Point GetPosition()
        {
            return new Point(this.area.X, this.area.Y);
        }

        /// <summary>
        /// Gets the bounding region of this component
        /// </summary>
        /// <returns>
        /// The region of this component as a <see cref="Rectangle"/>.
        /// </returns>
        public virtual Rectangle GetRegion()
        {
            return this.area;
        }

        /// <summary>
        /// Inflates the components bounding area by a specified amount
        /// </summary>
        /// <param name="horizontalAmount">
        /// The horizontal inflation
        /// </param>
        /// <param name="verticalAmount">
        /// The vertical inflation
        /// </param>
        /// <returns>
        /// The inflated region as a <see cref="Rectangle"/>.
        /// </returns>
        public virtual Rectangle InflateRegion(int horizontalAmount, int verticalAmount)
        {
            this.area.Inflate(horizontalAmount, verticalAmount);
            return this.area;
        }

        /// <summary>
        /// Moves the bounding region by a specified amount
        /// </summary>
        /// <param name="x">
        /// The x translation
        /// </param>
        /// <param name="y">
        /// The y translation
        /// </param>
        /// <returns>
        /// The moved region as a <see cref="Rectangle"/>.
        /// </returns>
        public virtual Rectangle MoveRegion(int x, int y)
        {
            this.area.X += x;
            this.area.Y += y;
            return this.area;
        }

        /// <summary>
        /// Called every frame on each component, responsible for updating behavior
        /// </summary>
        /// <param name="t">
        /// The time elapsed since the previous frame
        /// </param>
        public virtual void Update(GameTime t)
        {
        }

        /// <summary>
        /// Called each frame on each component, responsible for drawing it and it's children
        /// </summary>
        /// <param name="b">
        /// The sprite batch to use for drawing
        /// </param>
        /// <param name="o">
        /// The origin of this component
        /// </param>
        public virtual void Draw(SpriteBatch b, Point o)
        {
            if (this.Visible)
            {
                b.Draw(
                    this.Texture,
                    new Rectangle(this.area.X + o.X, this.area.Y + o.Y, this.area.Width, this.area.Height),
                    this.Crop,
                    Color.White,
                    0,
                    Vector2.Zero,
                    SpriteEffects.None,
                    1f);
            }
        }

        /// <summary>
        /// Set the scaled area.
        /// </summary>
        /// <param name="area">
        /// The scaled area
        /// </param>
        protected void SetScaledArea(Rectangle area)
        {
            this.area = new Rectangle(area.X * Game1.pixelZoom, area.Y * Game1.pixelZoom, area.Width * Game1.pixelZoom, area.Height * Game1.pixelZoom);
        }

        /// <summary>
        /// Calculates the width of a string
        /// </summary>
        /// <param name="text">
        /// The string to calculate the width of.
        /// </param>
        /// <param name="font">
        /// The font used.
        /// </param>
        /// <param name="scale">
        /// The scaling factor of the text.
        /// </param>
        /// <returns>
        /// The width of the string.
        /// </returns>
        protected int GetStringWidth(string text, SpriteFont font, float scale = 1f)
        {
            return (int)Math.Ceiling(font.MeasureString(text).X / Game1.pixelZoom * scale);
        }
    }
}