using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardewValley;

namespace Farmhand.UI
{
    abstract public class BaseMenuComponent : IMenuComponent
    {
        protected readonly static int zoom05 = Game1.pixelZoom / 2;
        protected readonly static int zoom2 = 2 * Game1.pixelZoom;
        protected readonly static int zoom3 = 3 * Game1.pixelZoom;
        protected readonly static int zoom4 = 4 * Game1.pixelZoom;
        protected readonly static int zoom5 = 5 * Game1.pixelZoom;
        protected readonly static int zoom6 = 6 * Game1.pixelZoom;
        protected readonly static int zoom7 = 7 * Game1.pixelZoom;
        protected readonly static int zoom8 = 8 * Game1.pixelZoom;
        protected readonly static int zoom9 = 9 * Game1.pixelZoom;
        protected readonly static int zoom10 = 10 * Game1.pixelZoom;
        protected readonly static int zoom11 = 11 * Game1.pixelZoom;
        protected readonly static int zoom12 = 12 * Game1.pixelZoom;
        protected readonly static int zoom13 = 13 * Game1.pixelZoom;
        protected readonly static int zoom14 = 14 * Game1.pixelZoom;
        protected readonly static int zoom15 = 15 * Game1.pixelZoom;
        protected readonly static int zoom16 = 16 * Game1.pixelZoom;
        protected readonly static int zoom17 = 17 * Game1.pixelZoom;
        protected readonly static int zoom20 = 20 * Game1.pixelZoom;
        protected readonly static int zoom22 = 22 * Game1.pixelZoom;
        protected readonly static int zoom28 = 28 * Game1.pixelZoom;
        protected Rectangle Area;
        protected Texture2D Texture;
        protected Rectangle Crop;
        public IComponentContainer Parent
        {
            get
            {
                if (_Parent == null)
                    throw new NullReferenceException("Component attempted to reference its parent while not attached");
                return _Parent;
            }
            private set
            {
                _Parent = value;
            }
        }
        private IComponentContainer _Parent=null;
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
        public BaseMenuComponent(Rectangle area, Texture2D texture, Rectangle? crop = null)
        {
            if (crop == null)
                crop = new Rectangle(0, 0, texture.Width, texture.Height);
            Texture = texture;
            Crop = (Rectangle)crop;
            SetScaledArea(area);
        }
        public void Attach(IComponentContainer collection)
        {
            if (_Parent!=null)
                throw new Exception("Component is already attached and must be detached first before it can be attached again");
            _Parent = collection;
        }
        public void Detach(IComponentContainer collection)
        {
            if (_Parent==null)
                throw new Exception("Component is not attached and must be attached first before it can be detached");
            _Parent = null;
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
