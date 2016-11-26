using System.Collections.Generic;

namespace Farmhand.API.NPCs
{
    public class Npc
    {
        public static Dictionary<string, NpcInformation> Npcs { get; } = new Dictionary<string, NpcInformation>();

        public static void RegisterNpc(NpcInformation npcInformation)
        {
            if (Npcs.ContainsKey(npcInformation.Name) && Npcs[npcInformation.Name] != npcInformation)
            {
                Logging.Log.Warning($"Potential conflict registering new NPC. NPC {npcInformation.Name} has been registered by two seperate mods. Only the last registered one will be used.");
            }
            Npcs[npcInformation.Name] = npcInformation;
        }
    }
}
