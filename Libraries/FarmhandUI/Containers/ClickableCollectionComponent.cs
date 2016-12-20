using System.Collections.Generic;
using Farmhand.UI.Interfaces;
using Microsoft.Xna.Framework;

namespace Farmhand.UI.Containers
{
    class ClickableCollectionComponent : GenericCollectionComponent
    {
        public event ClickHandler Handler;

        public ClickableCollectionComponent(Rectangle area, ClickHandler handler = null, List<IMenuComponent> components = null) : base(area, components)
        {
            if (handler != null)
                Handler += handler;
        }

        public override void LeftHeld(Point p, Point o)
        {
            
        }

        public override void LeftUp(Point p, Point o)
        {
            
        }

        public override void LeftClick(Point p, Point o)
        {
            Handler?.Invoke(this, Parent, Parent.GetAttachedMenu());
        }

        public override void RightClick(Point p, Point o)
        {
            
        }
    }
}