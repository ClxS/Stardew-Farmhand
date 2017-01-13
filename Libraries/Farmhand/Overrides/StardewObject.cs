namespace Farmhand.Overrides
{
    using Farmhand.Attributes;

    using StardewValley;

    [HookForceVirtualBase]
    [HookAlterBaseFieldProtection(LowestProtection.Protected)]
    internal class StardewObject : Object
    {
    }
}