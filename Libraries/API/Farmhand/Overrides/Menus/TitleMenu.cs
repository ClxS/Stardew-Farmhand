namespace Farmhand.Overrides.Menus
{
    using Farmhand.Attributes;

    [HookForceVirtualBase]
    [HookAlterBaseFieldProtection(LowestProtection.Protected)]
    internal class TitleMenu : StardewValley.Menus.TitleMenu
    {
    }
}