using Farmhand.Attributes;

namespace Farmhand.Overrides.Characters
{
    [HookForceVirtualBase]
    [HookAlterBaseFieldProtection(LowestProtection.Protected)]
    internal class NPC : StardewValley.NPC
    {
        
    }
}
