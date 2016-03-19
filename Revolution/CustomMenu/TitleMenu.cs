using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Revolution.Attributes;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.CustomMenu
{
    [HookAlterBaseProtection(LowestProtection.Protected)]
    class TitleMenu : StardewValley.Menus.TitleMenu
    {
        protected int test;
        public override void draw(SpriteBatch b)
        {
        }
    }
}
