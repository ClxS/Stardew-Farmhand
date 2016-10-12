using Farmhand.Attributes;

namespace Farmhand.Menus
{
    [HookForceVirtualBase]
    [HookAlterBaseFieldProtection(LowestProtection.Protected)]
    class GameMenu : StardewValley.Menus.GameMenu
    {
    }
}
