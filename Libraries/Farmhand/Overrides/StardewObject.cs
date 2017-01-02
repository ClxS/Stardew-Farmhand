namespace Farmhand.Overrides
{
    using Farmhand.Attributes;
    
    [HookForceVirtualBase]
    [HookAlterBaseFieldProtection(LowestProtection.Protected)]
    internal class StardewObject : StardewValley.Object
    {
    }
}