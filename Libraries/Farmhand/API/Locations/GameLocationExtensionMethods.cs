using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StardewValley;

namespace Farmhand.API.Locations
{
    public static class GameLocationExtensionMethods
    {
        public static void AddCharacter(this GameLocation @this, NPC npc)
        {
            @this.addCharacter(npc);
        }
    }
}
