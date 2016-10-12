using Farmhand.Attributes;

namespace Farmhand.Menus
{
    [HookAlterBaseFieldProtection(LowestProtection.Public)]
    [HookAlterProtection(LowestProtection.Public, "StardewValley.Menus.ChatMessage")]
    internal class ChatBox : StardewValley.Menus.ChatBox
    {
    }
}
