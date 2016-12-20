using System;
using System.Collections.Generic;
using System.Linq;
using Farmhand.UI.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

// ReSharper disable StaticMemberInGenericType

namespace Farmhand.UI.Form
{
    public class SliderFormComponent<T> : BaseFormComponent
    {
        protected static readonly Rectangle Background = new Rectangle(403, 383, 6, 6);
        protected static readonly Rectangle Button = new Rectangle(420, 441, 10, 6);

        private T _value;
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                int i = Values.IndexOf(value);
                if (i == -1)
                    return;
                Index = i;
                _value = value;
            }
        }

        protected int OldIndex;
        protected int Offset;
        protected int Index;
        protected List<T> Values;
        protected int OptionKey;
        protected ValueChanged<T> Handler;
        public SliderFormComponent(Point position, List<T> values, ValueChanged<T> handler=null) : this(position, 100, values, handler)
        {
        }
        public SliderFormComponent(Point position, int width, List<T> values, ValueChanged<T> handler=null)
        {
            Offset = (int)Math.Round((width - 10)*Game1.pixelZoom / (values.Count-1D));
            SetScaledArea(new Rectangle(position.X, position.Y, width, 6));
            Values = values;
            Value = values[0];
            Index = 0;
            OldIndex = 0;
            if(handler!=null)
                Handler += handler;
        }
        public override void LeftHeld(Point p, Point o)
        {
            if (Disabled)
                return;
            Index = Math.Max(Math.Min((int)Math.Floor((double)(p.X - (o.X+Area.X)) / Offset), Values.Count - 1), 0);
            Value = Values[Index];
        }
        public override void LeftUp(Point p, Point o)
        {
            if (OldIndex == Index)
                return;
            OldIndex = Index;
            Handler?.Invoke(this, Parent, Parent.GetAttachedMenu(), Value);
        }
        public override void LeftClick(Point p, Point o)
        {
            LeftHeld(p, o);
        }
        public override void Draw(SpriteBatch b, Point o)
        {
            if (!Visible)
                return;
            IClickableMenu.drawTextureBox(b, Game1.mouseCursors, Background, o.X + Area.X, o.Y + Area.Y, Area.Width, Area.Height, Color.White * (Disabled ? 0.33f : 1), Game1.pixelZoom, false);
            b.Draw(Game1.mouseCursors, new Vector2(o.X + Area.X + (Index == Values.Count - 1 ? Area.Width - Zoom10 : Index * Offset), o.Y + Area.Y), Button, Color.White * (Disabled ? 0.33f : 1), 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.9f);
        }
    }
    public class SliderFormComponent : SliderFormComponent<int>
    {
        public SliderFormComponent(Point position, int steps, ValueChanged<int> handler=null) : this(position, 100, steps, handler)
        {
        }
        public SliderFormComponent(Point position, int width, int steps, ValueChanged<int> handler=null) : base(position, width, Enumerable.Range(0, steps).ToList(), handler)
        {
        }
    }
}
