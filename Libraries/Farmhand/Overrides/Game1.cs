namespace Farmhand.Overrides
{
    using Farmhand.Attributes;

    [HookForceVirtualBase]
    [HookAlterBaseFieldProtection(LowestProtection.Protected)]
    internal class GameOverrideBase : StardewValley.Game1
    {
    }
}
