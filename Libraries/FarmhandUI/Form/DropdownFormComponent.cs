using System;
using System.Collections.Generic;
using Farmhand.UI.Base;
using Farmhand.UI.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;

namespace Farmhand.UI.Form
{
    public class DropdownFormComponent : BaseFormComponent
    {
        protected class DropdownSelect : BaseKeyboardFormComponent
        {
            protected int MaxScroll;
            protected int HoverOffset;
            protected int KeyboardOffset;
            protected int ScrollOffset;
            protected int Size;
            protected bool ShowHover;
            protected string Value;
            protected DropdownFormComponent Owner;
            protected IComponentContainer Collection;
            protected Rectangle Self;
            protected int GetCursorIndex(Point p, Point o)
            {
                int index = (int)Math.Floor((p.Y - Area.Y) / Game1.pixelZoom / 7D) + ScrollOffset;
                if (index < 0)
                    index = 0;
                if (index >= Owner.Values.Count)
                    index = Owner.Values.Count - 1;
                return index;
            }
            public DropdownSelect(Point position, int width, Rectangle self, DropdownFormComponent owner, IComponentContainer collection)
            {
                MaxScroll = Math.Max(0, owner.Values.Count - 10);
                Size = Math.Min(10, owner.Values.Count);
                Value = owner.Value;
                Owner = owner;
                Collection = collection;
                Self = self;
                Area=new Rectangle(position.X, position.Y, width, Zoom2 + Game1.pixelZoom * Math.Min(7 * owner.Values.Count,70));
                collection.GetAttachedMenu().GiveFocus(this);
                if (!collection.GetAttachedMenu().EventRegion.Contains(Area.X, Area.Y + Area.Height))
                    Area.Y -= Area.Height + Zoom9;
                int index = owner.Values.IndexOf(Value);
                ScrollOffset = Math.Max(0,index - 9);
                KeyboardOffset = index;
            }
            public override bool InBounds(Point p, Point o)
            {
                return true;
            }
            public override void FocusGained()
            {
                Selected = true;
            }
            public override void FocusLost()
            {
                if (Value == Owner.Value)
                    return;
                Owner.Value = Value;
                Owner.Handler?.Invoke(Owner, Collection, Parent.GetAttachedMenu(), Owner.Value);
            }
            public override void LeftClick(Point p, Point o)
            {
                Parent.ResetFocus();
                if (base.InBounds(p, new Point(0, 0)))
                {
                    Value = Owner.Values[GetCursorIndex(p, o)];
                }
                else if (!Self.Contains(p))
                {
                    Parent.GetAttachedMenu().receiveLeftClick(p.X, p.Y, false);
                }
            }
            public override void RightClick(Point p, Point o)
            {
                if (!base.InBounds(p, new Point(0, 0)))
                {
                    Parent.ResetFocus();
                    Parent.GetAttachedMenu().receiveRightClick(p.X, p.Y, false);
                }
            }
            public override void Scroll(int d, Point p, Point o)
            {
                int change = d / 120;
                int oldOffset = ScrollOffset;
                ScrollOffset = Math.Max(0, Math.Min(ScrollOffset - change, MaxScroll));
                if (oldOffset != ScrollOffset)
                    Game1.playSound("drumkit6");
            }
            public override void HoverOver(Point p, Point o)
            {
                ShowHover = base.InBounds(p, new Point(0, 0));
                HoverOffset = GetCursorIndex(p, o);
            }
            public override void Draw(SpriteBatch b, Point o)
            {
                o = new Point(0, 0);
                Color col = Color.Black * 0.25f;
                // Background
                IClickableMenu.drawTextureBox(b, Game1.mouseCursors, Background, o.X + Area.X, o.Y + Area.Y - Game1.pixelZoom, Area.Width - Zoom2, Area.Height, Color.White, Game1.pixelZoom, false);
                for (int c = 0; c < Size; c++)
                {
                    // Selected
                    if (Owner.Values[ScrollOffset + c] == Value)
                        b.Draw(Game1.staminaRect, new Rectangle(o.X + Area.X + Game1.pixelZoom, o.Y + Area.Y + Zoom7 * c, Area.Width - Zoom4, Zoom7), new Rectangle(0, 0, 1, 1), Color.Wheat*0.5f);
                    if (Selected && KeyboardOffset == ScrollOffset + c)
                    {
                        // Top
                        b.Draw(Game1.staminaRect, new Rectangle(o.X + Area.X + Game1.pixelZoom, o.Y + Area.Y + Zoom7 * c, Area.Width - Zoom4, Zoom05), new Rectangle(0, 0, 1, 1), col);
                        // Bottom
                        b.Draw(Game1.staminaRect, new Rectangle(o.X + Area.X + Game1.pixelZoom, o.Y + Area.Y + Zoom7 * c + Zoom6 + Zoom05, Area.Width - Zoom4, Zoom05), new Rectangle(0, 0, 1, 1), col);
                        // Left
                        b.Draw(Game1.staminaRect, new Rectangle(o.X + Area.X + Game1.pixelZoom, o.Y + Area.Y + Zoom7 * c + Zoom05, Zoom05, Zoom6), new Rectangle(0, 0, 1, 1), col);
                        // Right
                        b.Draw(Game1.staminaRect, new Rectangle(o.X + Area.X + Area.Width - Zoom3 - Zoom05, o.Y + Area.Y + Zoom7 * c + Zoom05, Zoom05, Zoom6), new Rectangle(0, 0, 1, 1), col);
                    }
                    // Hover
                    if (ShowHover && HoverOffset==ScrollOffset+c)
                        b.Draw(Game1.staminaRect, new Rectangle(o.X + Area.X + Game1.pixelZoom + Zoom05, o.Y + Area.Y + Zoom05 + Zoom7 * c, Area.Width - Zoom5, Zoom6), new Rectangle(0, 0, 1, 1), Color.Wheat*0.75f);
                    // Text
                    Utility.drawTextWithShadow(b, Owner.Values[ScrollOffset + c], Game1.smallFont, new Vector2(o.X + Area.X + Zoom2, o.Y + Area.Y + Zoom7 * c + Game1.pixelZoom), Game1.textColor * (Disabled ? 0.33f : 1f));
                }
                // ScrollUp
                if (ScrollOffset > 0)
                    b.Draw(Game1.mouseCursors, new Rectangle(o.X + Area.X + Area.Width - Zoom2, o.Y + Area.Y, Zoom7, Zoom7), UpScroll, Color.White);
                // ScrollDown
                if (ScrollOffset < MaxScroll)
                    b.Draw(Game1.mouseCursors, new Rectangle(o.X + Area.X + Area.Width - Zoom2, o.Y + Area.Y + Area.Height - Zoom9, Zoom7, Zoom7), DownScroll, Color.White);
            }
            public override void CommandReceived(char cmd)
            {
                switch ((int)cmd)
                {
                    case 13:
                        Value = Owner.Values[KeyboardOffset];
                        Parent.ResetFocus();
                        break;
                }
            }
            public override void SpecialReceived(Keys key)
            {
                switch (key)
                {
                    case Keys.Down:
                        if (KeyboardOffset < Owner.Values.Count - 1)
                            KeyboardOffset++;
                        if (KeyboardOffset - ScrollOffset > 9 && ScrollOffset < MaxScroll)
                            ScrollOffset++;
                        break;
                    case Keys.Up:
                        if (KeyboardOffset > 0)
                            KeyboardOffset--;
                        if (KeyboardOffset - ScrollOffset < 0 && ScrollOffset > 0)
                            ScrollOffset--;
                        break;
                }
            }
        }
        protected static readonly Rectangle Background = new Rectangle(433, 451, 3, 3);
        protected static readonly Rectangle Button = new Rectangle(438, 450, 9, 11);
        protected static readonly Rectangle UpScroll = new Rectangle(421, 459, 11, 12);
        protected static readonly Rectangle DownScroll = new Rectangle(421, 472, 11, 12);
        public event ValueChanged<string> Handler;

        protected List<string> Values;

        private string _value;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if(Values.Contains(value))
                    _value = value;
            }
        }

        public DropdownFormComponent(Point position, List<string> values, ValueChanged<string> handler=null) : this(position, 75, values, handler)
        {

        }
        public DropdownFormComponent(Point position, int width, List<string> values, ValueChanged<string> handler=null)
        {
            SetScaledArea(new Rectangle(position.X, position.Y, width, 11));
            Values = values;
            Value = Values[0];
            if(handler!=null)
                Handler += handler;
        }
        public override void LeftClick(Point p, Point o)
        {
            if (Disabled)
                return;
            var dropdownSelect = new DropdownSelect(new Point(o.X + Area.X, o.Y + Area.Y + Area.Height), Area.Width, new Rectangle(o.X + Area.X, o.Y + Area.Y, Area.Width, Area.Height), this, Parent);
        }
        public override void Draw(SpriteBatch b, Point o)
        {
            if (!Visible)
                return;
            // Selected background
            IClickableMenu.drawTextureBox(b, Game1.mouseCursors, Background, o.X+Area.X, o.Y+Area.Y, Area.Width-Game1.pixelZoom*(Button.Width), Zoom11, Color.White * (Disabled ? 0.33f : 1f), Game1.pixelZoom, false);
            // Selected label
            Utility.drawTextWithShadow(b, Value, Game1.smallFont, new Vector2(o.X + Area.X + Zoom2, o.Y + Area.Y + Zoom3), Game1.textColor * (Disabled ? 0.33f : 1f));
            // Selector button
            b.Draw(Game1.mouseCursors, new Vector2(o.X+Area.X + Area.Width - Game1.pixelZoom * Button.Width, o.Y + Area.Y), Button, Color.White * (Disabled ? 0.33f : 1f), 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.88f);
        }
    }
}
