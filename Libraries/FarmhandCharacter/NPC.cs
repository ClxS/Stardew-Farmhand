using System.Collections.Generic;
using Farmhand.API.NPCs;
using Microsoft.Xna.Framework;
using StardewValley;

namespace Farmhand.Overrides.Character
{
    public class NPC : StardewValley.NPC
    {
        protected NPCInformation Information;

        public NPC(NPCInformation Information, Vector2 Position)
            : base(new AnimatedSprite(Information.Texture), Position, Information.DefaultMap, (int)Information.DefaultFacingDirection, Information.DisplayName ?? Information.Name, Information.IsDatable, null, Information.Portrait)
        {
            this.Information = Information;
        }
    }
}