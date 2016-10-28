using Farmhand.API.NPCs;
using Microsoft.Xna.Framework;
using StardewValley;

namespace TestNPCMod
{
    public class TestNPC : Farmhand.Overrides.Character.NPC
    {
        public TestNPC(NPCInformation Information, Vector2 Position) : base(Information, Position) {}
        public TestNPC(NPCInformation Information) : base(Information, new Vector2(Information.DefaultX * Game1.tileSize, Information.DefaultY * Game1.tileSize)) {}
    }
}
