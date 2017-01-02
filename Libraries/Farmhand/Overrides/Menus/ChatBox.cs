namespace Farmhand.Overrides.Menus
{
    using Farmhand.Attributes;

    [HookAlterBaseFieldProtection(LowestProtection.Public)]
    [HookAlterProtection(LowestProtection.Public, "StardewValley.Menus.ChatMessage")]
    internal class ChatBox : StardewValley.Menus.ChatBox
    {
    }
}