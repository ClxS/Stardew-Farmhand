using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmhand.Attributes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Tools;
using xTile.Dimensions;

namespace Farmhand.Overrides
{
    [HookForceVirtualBase]
    [HookAlterBaseProtection(LowestProtection.Protected)]
    public class GameOverrideBase : StardewValley.Game1
    {
    }
}
