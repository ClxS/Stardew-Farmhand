using Farmhand.API.NPCs;
using Microsoft.Xna.Framework;

namespace TestNPCMod
{
    public class TestNPC : Farmhand.Overrides.Character.NPC
    {
        public TestNPC(NPCInformation Information, Vector2 Position)
            : base(Information, Position)
        {
        }
    }
}
