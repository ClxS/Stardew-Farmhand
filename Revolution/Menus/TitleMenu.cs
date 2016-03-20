using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Revolution.Attributes;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Menus
{
    [HookForceVirtualBase]
    [HookAlterBaseProtection(LowestProtection.Protected)]
    internal class TitleMenu : StardewValley.Menus.TitleMenu
    {

    }
}
