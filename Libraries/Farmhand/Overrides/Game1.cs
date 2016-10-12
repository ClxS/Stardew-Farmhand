using Farmhand.Attributes;

namespace Farmhand.Overrides
{
    [HookForceVirtualBase]
    [HookAlterBaseFieldProtection(LowestProtection.Protected)]
    public class GameOverrideBase : StardewValley.Game1
    {
    }
}
