using Farmhand.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Overrides.Characters
{
    [HookForceVirtualBase]
    [HookAlterBaseFieldProtection(LowestProtection.Protected)]
    internal class Monster : StardewValley.Monsters.Monster
    {

    }
}
