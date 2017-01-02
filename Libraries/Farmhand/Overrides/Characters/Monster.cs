namespace Farmhand.Overrides.Characters
{
    using Farmhand.Attributes;

    [HookForceVirtualBase]
    [HookAlterBaseFieldProtection(LowestProtection.Protected)]
    internal class Monster : StardewValley.Monsters.Monster
    {
    }
}