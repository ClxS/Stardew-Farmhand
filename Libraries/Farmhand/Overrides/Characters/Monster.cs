using Farmhand.Attributes;

namespace Farmhand.Overrides.Characters
{
    [HookForceVirtualBase]
    [HookAlterBaseFieldProtection(LowestProtection.Protected)]
    internal class Monster : StardewValley.Monsters.Monster
    {

    }
}
