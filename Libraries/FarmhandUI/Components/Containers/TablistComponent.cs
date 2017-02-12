namespace Farmhand.UI.Components.Containers
{
    using System.Collections.Generic;

    using Farmhand.UI.Base;
    using Farmhand.UI.Components.Interfaces;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    ///     A tab list component.
    /// </summary>
    public class TablistComponent : BaseInteractiveMenuComponent, IComponentContainer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TablistComponent" /> class.
        /// </summary>
        /// <param name="area">
        ///     The area.
        /// </param>
        public TablistComponent(Rectangle area)
        {
            this.SetScaledArea(area);
        }

        /// <summary>
        ///     Gets or sets the currently selected tab index.
        /// </summary>
        public int Index
        {
            get
            {
                return this.Current;
            }

            set
            {
                if (value >= 0 && value < this.Tabs.Count)
                {
                    this.Current = value;
                    this.CurrentTab = this.Tabs[value].Component;
                }
            }
        }

        /// <summary>
        ///     Gets or sets the texture location for the tab background
        /// </summary>
        protected static Rectangle Tab { get; set; } = new Rectangle(16, 368, 16, 16);

        /// <summary>
        ///     Gets or sets the menu this tab list is attached to
        /// </summary>
        protected FrameworkMenu AttachedMenu { get; set; }

        /// <summary>
        ///     Gets or sets the currently selected tab index
        /// </summary>
        protected int Current { get; set; } = -1;

        /// <summary>
        ///     Gets or sets the currently selected tab component
        /// </summary>
        protected IInteractiveMenuComponent CurrentTab { get; set; }

        /// <summary>
        ///     Gets or sets the currently focused element
        /// </summary>
        protected IInteractiveMenuComponent FocusElement { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the mouse button is being held over this control.
        /// </summary>
        protected bool Hold { get; set; } = false;

        /// <summary>
        ///     Gets or sets the currently hovered element.
        /// </summary>
        protected IInteractiveMenuComponent HoverElement { get; set; }

        /// <summary>
        ///     Gets or sets the tabs in the tab list.
        /// </summary>
        protected List<TabInfo> Tabs { get; set; } = new List<TabInfo>();

        #region IComponentContainer Members

        /// <summary>
        ///     Gets the event region.
        /// </summary>
        public Rectangle EventRegion
            =>
                new Rectangle(
                    this.Area.X + Zoom5,
                    this.Area.Y + Zoom22,
                    this.Area.Width - Zoom10,
                    this.Area.Height - Zoom28);

        /// <summary>
        ///     Gets the zoom event region.
        /// </summary>
        public Rectangle ZoomEventRegion
            =>
                new Rectangle(
                    (this.Area.X + Zoom5) / Game1.pixelZoom,
                    (this.Area.Y + Zoom22) / Game1.pixelZoom,
                    (this.Area.Width - Zoom14) / Game1.pixelZoom,
                    (this.Area.Height - Zoom28) / Game1.pixelZoom);

        /// <summary>
        ///     Gets the attached menu.
        /// </summary>
        /// <returns>
        ///     The attached menu. <see cref="FrameworkMenu" />.
        /// </returns>
        public FrameworkMenu GetAttachedMenu()
        {
            return this.Parent.GetAttachedMenu();
        }

        /// <summary>
        ///     Gives focus to the specified component
        /// </summary>
        /// <param name="component">
        ///     The component to give focus to
        /// </param>
        public void GiveFocus(IInteractiveMenuComponent component)
        {
            if (!this.Tabs.Exists(x => x.Component == component) || component == this.FocusElement)
            {
                return;
            }

            this.Parent.GiveFocus(this);
            this.ResetFocus();
            this.FocusElement = component;
            if (this.FocusElement is IKeyboardComponent)
            {
                Game1.keyboardDispatcher.Subscriber = new KeyboardSubscriberProxy((IKeyboardComponent)this.FocusElement);
            }

            component.FocusGained();
        }

        /// <summary>
        ///     Resets the components focus
        /// </summary>
        public void ResetFocus()
        {
            if (this.FocusElement == null)
            {
                return;
            }

            this.FocusElement.FocusLost();
            if (this.FocusElement is IKeyboardComponent)
            {
                Game1.keyboardDispatcher.Subscriber.Selected = false;
                Game1.keyboardDispatcher.Subscriber = null;
            }

            this.FocusElement = null;
        }

        #endregion

        /// <summary>
        ///     Adds a tab to the tab list
        /// </summary>
        /// <param name="icon">
        ///     The icon index for this tab
        /// </param>
        /// <param name="collection">
        ///     The component collection for this tab
        /// </param>
        /// <typeparam name="T">
        ///     The type of the collection to add
        /// </typeparam>
        public void AddTab<T>(int icon, T collection) where T : IComponentCollection, IInteractiveMenuComponent
        {
            this.Tabs.Add(new TabInfo(collection, icon));
            collection.Attach(this);
            if (this.CurrentTab == null)
            {
                this.Current = 0;
                this.CurrentTab = collection;
            }
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

            // Draw chrome
            FrameworkMenu.DrawMenuRect(
                b,
                o.X + this.Area.X - Zoom2,
                o.Y + this.Area.Y + Zoom15,
                this.Area.Width,
                this.Area.Height - Zoom15);

            // Draw tabs
            for (var c = 0; c < this.Tabs.Count; c++)
            {
                b.Draw(
                    Game1.mouseCursors,
                    new Rectangle(
                        o.X + this.Area.X + Zoom4 + c * Zoom16,
                        o.Y + this.Area.Y + (c == this.Current ? Zoom2 : 0),
                        Zoom16,
                        Zoom16),
                    Tab,
                    Color.White);
                b.Draw(
                    Game1.objectSpriteSheet,
                    new Rectangle(
                        o.X + this.Area.X + Zoom8 + c * Zoom16,
                        o.Y + this.Area.Y + Zoom5 + (c == this.Current ? Zoom2 : 0),
                        Zoom8,
                        Zoom8),
                    Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.Tabs[c].Icon, 16, 16),
                    Color.White);
            }

            // Draw body
            this.CurrentTab?.Draw(b, new Point(o.X + this.Area.X + Zoom5, o.Y + this.Area.Y + Zoom22));
        }

        /// <summary>
        ///     Called when focus is lost
        /// </summary>
        public override void FocusLost()
        {
            this.ResetFocus();
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
            this.CurrentTab?.HoverOver(p, new Point(o.X + this.Area.X + Zoom5, o.Y + this.Area.Y + Zoom22));
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
            if (p.Y - o.Y - this.Area.Y < Zoom16)
            {
                var pos = (p.X - o.X - this.Area.X - Zoom4) / Zoom16;
                if (pos < 0 || pos >= this.Tabs.Count || this.Current == pos)
                {
                    return;
                }

                this.Current = pos;
                this.CurrentTab = this.Tabs[pos].Component;
                Game1.playSound("smallSelect");
            }
            else
            {
                this.CurrentTab?.LeftClick(p, new Point(o.X + this.Area.X + Zoom5, o.Y + this.Area.Y + Zoom22));
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
            this.CurrentTab?.LeftHeld(p, new Point(o.X + this.Area.X + Zoom5, o.Y + this.Area.Y + Zoom22));
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
            this.CurrentTab?.LeftUp(p, new Point(o.X + this.Area.X + Zoom5, o.Y + this.Area.Y + Zoom22));
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
            this.CurrentTab?.RightClick(p, new Point(o.X + this.Area.X + Zoom5, o.Y + this.Area.Y + Zoom22));
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
            this.CurrentTab?.Scroll(d, p, new Point(o.X + this.Area.X + Zoom5, o.Y + this.Area.Y + Zoom22));
        }

        /// <summary>
        ///     The update loop for this component
        /// </summary>
        /// <param name="t">
        ///     The elapsed time since the previous update
        /// </param>
        public override void Update(GameTime t)
        {
            this.CurrentTab?.Update(t);
        }

        #region Nested type: TabInfo

        /// <summary>
        ///     Contains tab information
        /// </summary>
        protected class TabInfo
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="TabInfo" /> class.
            /// </summary>
            /// <param name="component">
            ///     The component this tab identifies
            /// </param>
            /// <param name="icon">
            ///     The icon index for this tab
            /// </param>
            public TabInfo(IInteractiveMenuComponent component, int icon)
            {
                this.Component = component;
                this.Icon = icon;
            }

            /// <summary>
            ///     Gets or sets a value which indicates which component this tab identifies
            /// </summary>
            public IInteractiveMenuComponent Component { get; set; }

            /// <summary>
            ///     Gets or sets a value which indicates the icon index for this tab
            /// </summary>
            public int Icon { get; set; }
        }

        #endregion
    }
}