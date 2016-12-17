using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using StardewValley;
using Farmhand.API.NPCs;

namespace Farmhand.Overrides.Character
{
    [XmlType("NPCOverride")]
    public class NPC : StardewValley.NPC
    {
        protected NpcInformation Information;

        public NPC(NpcInformation information, Vector2 position)
            : base(new AnimatedSprite(information.Texture, 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), position, information.DefaultMap, (int)information.DefaultFacingDirection, information.Name, information.IsDatable, null, information.Portrait)
        {
            Information = information;
        }
    }
}
