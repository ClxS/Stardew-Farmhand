using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardewValley;

namespace Farmhand.UI
{
    public class ScrollableCollectionComponent : GenericCollectionComponent
    {
        protected static Rectangle UpButton = new Rectangle(421, 459, 11, 12);
        protected static Rectangle DownButton = new Rectangle(421, 472, 11, 12);

        protected int ScrollOffset=0;
        protected int InnerHeight;
        protected int BarOffset;

        protected bool UpActive=false;
        protected bool DownActive=false;

        protected int Counter = 0;
        protected int Limiter = 20;
        public ScrollableCollectionComponent(Rectangle area, List<IMenuComponent> components = null):base(area,components)
        {
            
        }
        protected override void UpdateDrawOrder()
        {
            base.UpdateDrawOrder();
            int height = Area.Height;
            foreach(IMenuComponent c in DrawOrder)
            {
                if (!c.Visible)
                    return;
                Rectangle r = c.GetRegion();
                int b = r.Y + r.Height;
                if (b > height)
                    height = b;
            }
            if (height > Area.Height)
                BarOffset = zoom12;
            else
                BarOffset = 0;
            InnerHeight = (int)Math.Ceiling((height - Area.Height) / (double)zoom10);
        }
        public override void Scroll(int d, Point p, Point o)
        {
            if (!Visible)
                return;
            base.Scroll(d, p, o);
            if (HoverElement != null)
                return;
            int change = d / 120;
            int oldOffset = ScrollOffset;
            ScrollOffset = Math.Max(0, Math.Min(ScrollOffset - change, InnerHeight));
            if(oldOffset!=ScrollOffset)
                Game1.playSound("drumkit6");
        }
        public override void HoverOver(Point p, Point o)
        {
            Rectangle up = new Rectangle(Area.X + o.X + Area.Width - (UpActive ? zoom12 : zoom11 + zoom05), Area.Y + o.Y + (UpActive ? 0 : zoom05), UpActive ? zoom12 : zoom11, UpActive ? zoom13 : zoom12);
            Rectangle down = new Rectangle(Area.X + o.X + Area.Width - (DownActive ? zoom12 : zoom11 + zoom05), Area.Y + o.Y + Area.Height - zoom12 - (DownActive ? zoom05 : 0), DownActive ? zoom12 : zoom11, DownActive ? zoom13 : zoom12);
            UpActive = ScrollOffset > 0 && up.Contains(p);
            DownActive = ScrollOffset < InnerHeight && down.Contains(p);
            base.HoverOver(p, o);
        }
        protected void ArrowClick(Point p, Point o)
        {
            if (UpActive && ScrollOffset > 0)
            {
                ScrollOffset--;
                Game1.playSound("drumkit6");
            }
            else if (DownActive && ScrollOffset < InnerHeight)
            {
                ScrollOffset++;
                Game1.playSound("drumkit6");
            }
        }
        public override void LeftHeld(Point p, Point o)
        {
            base.LeftHeld(p, o);
            if (!UpActive && !DownActive)
                return;
            Counter++;
            if (Counter % Limiter != 0)
                return;
            Limiter = Math.Max(1, Limiter - 1);
            Counter = 0;
            ArrowClick(p,o);
        }
        public override void LeftUp(Point p, Point o)
        {
            Limiter = 10;
            Counter = 0;
            base.LeftUp(p, o);
        }
        public override void LeftClick(Point p, Point o)
        {
            base.LeftClick(p, o);
            Counter = 0;
            Limiter = 10;
            ArrowClick(p,o);
        }
        public override void Draw(SpriteBatch b, Point o)
        {
            if (!Visible)
                return;
            b.End();
            Rectangle reg = EventRegion;
            b.GraphicsDevice.ScissorRectangle = new Rectangle(reg.X+o.X,reg.Y+o.Y,reg.Width,reg.Height);
            b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, new RasterizerState() { ScissorTestEnable = true });
            Point o2=new Point(o.X+reg.X, o.Y+reg.Y-(ScrollOffset * zoom10));
            foreach(IMenuComponent el in DrawOrder)
                el.Draw(b,o2);
            b.End();
            b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);

            // Scrollbar
            if (BarOffset == 0)
                return;
            // Up
            b.Draw(Game1.mouseCursors, new Rectangle(Area.X + o.X + Area.Width - (UpActive ? zoom12 : zoom11 + zoom05), Area.Y + o.Y + (UpActive ? 0 : zoom05), UpActive ? zoom12 : zoom11, UpActive ? zoom13 : zoom12), UpButton, Color.White * (ScrollOffset > 0 ? 1 : 0.5f));
            // down
            b.Draw(Game1.mouseCursors, new Rectangle(Area.X + o.X + Area.Width - (DownActive ? zoom12 : zoom11 + zoom05), Area.Y + o.Y + Area.Height - zoom12 - (DownActive ? zoom05 : 0), DownActive ? zoom12 : zoom11, DownActive ? zoom13 : zoom12), DownButton, Color.White * (ScrollOffset < InnerHeight ? 1 : 0.5f));
        }
    }
}
