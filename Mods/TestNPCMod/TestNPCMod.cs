using System;
using System.Collections.Generic;
using System.Linq;
using Farmhand;
using Farmhand.API.NPCs;
using Farmhand.API.NPCs.Dialogues;
using Farmhand.API.NPCs.GiftTastes;
using Farmhand.API.NPCs.Schedules;
using Farmhand.Events;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using TestNPCMod.NPCs;
using static Farmhand.API.NPCs.NpcInformation;

namespace TestNPCMod
{
    public class TestNpcMod : Mod
    {
        public static TestNpcMod Instance;

        private readonly Dictionary<string, NpcInformation> _npcInformation = new Dictionary<string, NpcInformation>();

        public override void Entry()
        {
            GameEvents.OnAfterLoadedContent += GameEvents_OnAfterLoadedContent;
            GameEvents.OnAfterGameLoaded += GameEvents_OnAfterGameLoaded;
            ControlEvents.OnKeyPressed += (obj, ev) =>
            {
                var keyState = Keyboard.GetState();
                if (keyState.IsKeyDown(Keys.F2) && !Game1.oldKBState.IsKeyDown(Keys.F2))
                {
                    Console.WriteLine(string.Join(", ", Utility.getAllCharacters().Select(npc => npc.getName())));
                    Game1.currentLocation.getCharacters().ForEach(chr => Console.WriteLine(chr.getName()));
                }
            };
        }

        private void GameEvents_OnAfterGameLoaded(object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine();
            Game1.locations.FirstOrDefault(location => location.Name == "WizardHouse")?.addCharacter(new Troy(_npcInformation["Troy"]));
            Console.WriteLine(string.Join(", ", Game1.locations.FirstOrDefault(location => location.Name == "WizardHouse")?.getCharacters().Select(npc => npc.getName()) ?? new[] {"None"}));
            Console.WriteLine();
            Console.WriteLine();
            //Game1.locations[21].addCharacter(new TestNPC(npcInformation["Troy"]));
        }
        
        private void GameEvents_OnAfterLoadedContent(object sender, EventArgs e)
        {
            Farmhand.API.NPCs.Npc.RegisterNpc(Troy.Information);
        }
    }
}