namespace Farmhand.Overrides.Menus
{
    using Farmhand.Attributes;

    [HookForceVirtualBase]
    [HookAlterBaseFieldProtection(LowestProtection.Protected)]
    internal class GameMenu : StardewValley.Menus.GameMenu
    {
    }
}