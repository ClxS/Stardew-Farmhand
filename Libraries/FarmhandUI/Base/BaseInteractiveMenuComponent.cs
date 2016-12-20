using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmhand.UI
{
    abstract public class BaseInteractiveMenuComponent : BaseMenuComponent, IInteractiveMenuComponent
    {
        public delegate void ValueChanged<T>(IInteractiveMenuComponent component, IComponentContainer collection, FrameworkMenu menu, T value);
        public delegate void ClickHandler(IInteractiveMenuComponent component, IComponentContainer collection, FrameworkMenu menu);
        protected BaseInteractiveMenuComponent()
        {

        }
        public BaseInteractiveMenuComponent(Rectangle area, Texture2D texture, Rectangle? crop = null) : base(area, texture, crop)
        {

        }
        public virtual bool InBounds(Point p, Point o)
        {
            return Visible ? new Rectangle(Area.X + o.X, Area.Y + o.Y, Area.Width, Area.Height).Contains(p) : false;
        }
        public virtual void RightClick(Point p, Point o)
        {

        }
        public virtual void LeftClick(Point p, Point o)
        {

        }
        public virtual void LeftHeld(Point p, Point o)
        {

        }
        public virtual void LeftUp(Point p, Point o)
        {

        }
        public virtual void HoverIn(Point p, Point o)
        {

        }
        public virtual void HoverOut(Point p, Point o)
        {

        }
        public virtual void HoverOver(Point p, Point o)
        {

        }
        public virtual void FocusLost()
        {

        }
        public virtual void FocusGained()
        {

        }
        public virtual void Scroll(int d, Point p, Point o)
        {

        }
    }
}
