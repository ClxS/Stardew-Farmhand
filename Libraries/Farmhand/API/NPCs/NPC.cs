using System;
using System.Collections.Generic;
using Farmhand.Attributes;
using Farmhand.Events;
using Microsoft.Xna.Framework;
using StardewValley;
using xTile.Dimensions;

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
        
        [Hook(HookType.Exit, "StardewValley.Game1", "loadForNewGame")]
        internal static void InvokeAfterGameLoaded()
        {
            foreach (var npc in NPCs)
            {
                var Information = npc.Value;
                Game1.getLocationFromName(npc.Value.DefaultMap).addCharacter(new StardewValley.NPC(new AnimatedSprite(Information.Texture), new Vector2(Information.DefaultX * Game1.tileSize, Information.DefaultY * Game1.tileSize), Information.DefaultMap, (int)Information.DefaultFacingDirection, Information.DisplayName ?? Information.Name, Information.IsDatable, null, Information.Portrait));
                StardewValley.NPC.populateRoutesFromLocationToLocationList();
            }
        }
    }
}