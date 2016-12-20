using System.Collections.Generic;
using Farmhand.UI.Base;
using Farmhand.UI.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Farmhand.UI.Containers
{
    public class TablistComponent : BaseInteractiveMenuComponent, IComponentContainer
    {
        protected class TabInfo
        {
            public IInteractiveMenuComponent Component;
            public int Icon;
            public TabInfo(IInteractiveMenuComponent component, int icon)
            {
                Component = component;
                Icon = icon;
            }
        }
        protected static Rectangle Tab = new Rectangle(16, 368, 16, 16);

        protected IInteractiveMenuComponent HoverElement;
        protected IInteractiveMenuComponent FocusElement;
        protected bool Hold = false;

        protected FrameworkMenu AttachedMenu;

        protected List<TabInfo> Tabs=new List<TabInfo>();
        protected int Current=-1;
        protected IInteractiveMenuComponent CurrentTab;

        // IComponentCollection proxy
        public Rectangle EventRegion => new Rectangle(Area.X + Zoom5, Area.Y + Zoom22, Area.Width - Zoom10, Area.Height - Zoom28);

        public Rectangle ZoomEventRegion => new Rectangle((Area.X + Zoom5) / Game1.pixelZoom, (Area.Y + Zoom22) / Game1.pixelZoom, (Area.Width - Zoom14) / Game1.pixelZoom, (Area.Height - Zoom28) / Game1.pixelZoom);

        public int Index
        {
            get
            {
                return Current;
            }
            set
            {
                if(value >= 0 && value < Tabs.Count)
                {
                    Current = value;
                    CurrentTab = Tabs[value].Component;
                }
            }
        }

        public TablistComponent(Rectangle area)
        {
            SetScaledArea(area);
        }
        public void AddTab<T>(int icon, T collection) where T : IComponentCollection, IInteractiveMenuComponent
        {
            Tabs.Add(new TabInfo(collection, icon));
            collection.Attach(this);
            if(CurrentTab==null)
            {
                Current = 0;
                CurrentTab = collection;
            }
        }
        // IInteractiveMenuComponent
        public override void FocusLost()
        {
            ResetFocus();
        }
        public override void LeftUp(Point p, Point o)
        {
            CurrentTab?.LeftUp(p, new Point(o.X + Area.X + Zoom5, o.Y + Area.Y + Zoom22));
        }
        public override void LeftHeld(Point p, Point o)
        {
            CurrentTab?.LeftHeld(p, new Point(o.X + Area.X + Zoom5, o.Y + Area.Y + Zoom22));
        }
        public override void LeftClick(Point p, Point o)
        {
            if (p.Y - o.Y - Area.Y < Zoom16)
            {
                int pos = (p.X - o.X - Area.X - Zoom4) / Zoom16;
                if (pos < 0 || pos >= Tabs.Count || Current == pos)
                    return;
                Current = pos;
                CurrentTab = Tabs[pos].Component;
                Game1.playSound("smallSelect");
            }
            else
                CurrentTab?.LeftClick(p, new Point(o.X + Area.X + Zoom5, o.Y + Area.Y + Zoom22));
        }
        public override void RightClick(Point p, Point o)
        {
            CurrentTab?.RightClick(p, new Point(o.X + Area.X + Zoom5, o.Y + Area.Y + Zoom22));
        }
        public override void HoverOver(Point p, Point o)
        {
            CurrentTab?.HoverOver(p, new Point(o.X + Area.X + Zoom5, o.Y + Area.Y + Zoom22));
        }
        public override void Scroll(int d, Point p, Point o)
        {
            CurrentTab?.Scroll(d, p, new Point(o.X + Area.X + Zoom5, o.Y + Area.Y + Zoom22));
        }
        public override void Update(GameTime t)
        {
            CurrentTab?.Update(t);
        }
        public override void Draw(SpriteBatch b, Point o)
        {
            if (!Visible)
                return;
            // Draw chrome
            FrameworkMenu.DrawMenuRect(b, o.X + Area.X - Zoom2, o.Y + Area.Y + Zoom15, Area.Width, Area.Height - Zoom15);
            // Draw tabs
            for(var c=0;c<Tabs.Count;c++)
            {
                b.Draw(Game1.mouseCursors, new Rectangle(o.X + Area.X + Zoom4 + c * Zoom16, o.Y + Area.Y + (c == Current ? Zoom2 : 0), Zoom16, Zoom16), Tab, Color.White);
                b.Draw(Game1.objectSpriteSheet, new Rectangle(o.X + Area.X + Zoom8 + c * Zoom16, o.Y + Area.Y + Zoom5 + (c == Current ? Zoom2 : 0), Zoom8, Zoom8), Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, Tabs[c].Icon, 16, 16), Color.White);
            }
            // Draw body
            CurrentTab?.Draw(b, new Point(o.X + Area.X + Zoom5, o.Y + Area.Y + Zoom22));
        }
        
        public FrameworkMenu GetAttachedMenu()
        {
            return Parent.GetAttachedMenu();
        }
        public void ResetFocus()
        {
            if (FocusElement == null)
                return;
            FocusElement.FocusLost();
            if (FocusElement is IKeyboardComponent)
            {
                Game1.keyboardDispatcher.Subscriber.Selected = false;
                Game1.keyboardDispatcher.Subscriber = null;
            }
            FocusElement = null;
        }
        public void GiveFocus(IInteractiveMenuComponent component)
        {
            if (!Tabs.Exists(x => x.Component==component) || component == FocusElement)
                return;
            Parent.GiveFocus(this);
            ResetFocus();
            FocusElement = component;
            if (FocusElement is IKeyboardComponent)
                Game1.keyboardDispatcher.Subscriber = new KeyboardSubscriberProxy((IKeyboardComponent)FocusElement);
            component.FocusGained();
        }
    }
}
