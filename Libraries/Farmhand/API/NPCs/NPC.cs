using System.Collections.Generic;
using StardewValley;

namespace Farmhand.API.NPCs
{
    public class NPC
    {
        public static Dictionary<string, NPCInformation> NPCs { get; } = new Dictionary<string, NPCInformation>();

        public static void RegisterNPC(NPCInformation npcInformation)
        {
            if (NPCs.ContainsKey(npcInformation.Name) && NPCs[npcInformation.Name] != npcInformation)
            {
                Logging.Log.Warning($"Potential conflict registering new NPC. NPC {npcInformation.Name} has been registered by two seperate mods. Only the last registered one will be used.");
            }
            NPCs[npcInformation.Name] = npcInformation;
        }
    }
}