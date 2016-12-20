using System;
using Farmhand.UI.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Farmhand.UI.Base
{
    public abstract class BaseMenuComponent : IMenuComponent
    {
        protected static readonly int Zoom05 = Game1.pixelZoom / 2;
        protected static readonly int Zoom2 = 2 * Game1.pixelZoom;
        protected static readonly int Zoom3 = 3 * Game1.pixelZoom;
        protected static readonly int Zoom4 = 4 * Game1.pixelZoom;
        protected static readonly int Zoom5 = 5 * Game1.pixelZoom;
        protected static readonly int Zoom6 = 6 * Game1.pixelZoom;
        protected static readonly int Zoom7 = 7 * Game1.pixelZoom;
        protected static readonly int Zoom8 = 8 * Game1.pixelZoom;
        protected static readonly int Zoom9 = 9 * Game1.pixelZoom;
        protected static readonly int Zoom10 = 10 * Game1.pixelZoom;
        protected static readonly int Zoom11 = 11 * Game1.pixelZoom;
        protected static readonly int Zoom12 = 12 * Game1.pixelZoom;
        protected static readonly int Zoom13 = 13 * Game1.pixelZoom;
        protected static readonly int Zoom14 = 14 * Game1.pixelZoom;
        protected static readonly int Zoom15 = 15 * Game1.pixelZoom;
        protected static readonly int Zoom16 = 16 * Game1.pixelZoom;
        protected static readonly int Zoom17 = 17 * Game1.pixelZoom;
        protected static readonly int Zoom20 = 20 * Game1.pixelZoom;
        protected static readonly int Zoom22 = 22 * Game1.pixelZoom;
        protected static readonly int Zoom28 = 28 * Game1.pixelZoom;

        protected Rectangle Area;
        protected Texture2D Texture;
        protected Rectangle Crop;

        private IComponentContainer _parent;
        public IComponentContainer Parent
        {
            get
            {
                if (_parent == null)
                    throw new NullReferenceException("Component attempted to reference its parent while not attached");
                return _parent;
            }
        }

        public bool Visible { get; set; } = true;
        public int Layer { get; set; } = 0;

        protected void SetScaledArea(Rectangle area)
        {
            Area = new Rectangle(area.X * Game1.pixelZoom, area.Y * Game1.pixelZoom, area.Width * Game1.pixelZoom, area.Height * Game1.pixelZoom);
        }

        protected int GetStringWidth(string text, SpriteFont font, float scale = 1f)
        {
            return (int)Math.Ceiling(font.MeasureString(text).X / Game1.pixelZoom * scale);
        }

        protected BaseMenuComponent()
        {

        }

        protected BaseMenuComponent(Rectangle area, Texture2D texture, Rectangle? crop = null)
        {
            if (crop == null)
                crop = new Rectangle(0, 0, texture.Width, texture.Height);
            Texture = texture;
            Crop = (Rectangle)crop;
            SetScaledArea(area);
        }

        public void Attach(IComponentContainer collection)
        {
            if (_parent!=null)
                throw new Exception("Component is already attached and must be detached first before it can be attached again");
            _parent = collection;
        }

        public void Detach(IComponentContainer collection)
        {
            if (_parent==null)
                throw new Exception("Component is not attached and must be attached first before it can be detached");
            _parent = null;
        }

        public virtual Point GetPosition()
        {
            return new Point(Area.X, Area.Y);
        }

        public virtual Rectangle GetRegion()
        {
            return Area;
        }

        public virtual void Update(GameTime t)
        {

        }

        public virtual void Draw(SpriteBatch b, Point o)
        {
            if (Visible)
                b.Draw(Texture, new Rectangle(Area.X + o.X, Area.Y + o.Y,Area.Width,Area.Height), Crop, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
        }

    }
}
