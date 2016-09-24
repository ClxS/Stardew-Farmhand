using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmhand.Attributes;

namespace Farmhand.Menus
{
    [HookForceVirtualBase]
    [HookAlterBaseFieldProtection(LowestProtection.Protected)]
    class GameMenu : StardewValley.Menus.GameMenu
    {
    }
}
