using Farmhand;
using Farmhand.API.Generic;
using Farmhand.API.Monsters;
using Farmhand.Events;
using Farmhand.Events.Arguments.GlobalRoute;
using Farmhand.Overrides;
using Farmhand.Registries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMonsterMod.Monsters;

namespace TestMonsterMod
{
    public class TestMonsterMod : Mod
    {
        public static TestMonsterMod Instance;

        public override void Entry()
        {
            Farmhand.Events.GameEvents.OnAfterLoadedContent += GameEvents_OnAfterLoadedContent;
            
            // TODO once returnable events are working, this can be replaced with a proper event
            Farmhand.Events.GlobalRouteManager.Listen(ListenerType.Pre, "StardewValley.Locations.MineShaft", "getMonsterForThisLevel", MineshaftEvents_OnGetMonsterForThisLevel);
        }

        private void GameEvents_OnAfterLoadedContent(object sender, System.EventArgs e)
        {
            Farmhand.API.Monsters.Monster.RegisterMonster(new MonsterInformation
            {
                Name = "TestMonster",
                Texture = ModSettings.GetTexture("sprite_TestMonster"),
                Health = 20,
                MaxHealth = 20,
                DamageToFarmer = 1,
                IsGlider = false,
                DurationOfRandomMovements = 1000,
                ObjectsToDrop = new List<ItemChancePair>
                {
                    new ItemChancePair() { ItemId = 167, Chance = .50},
                    new ItemChancePair() { ItemId = 167, Chance = .05},
                    new ItemChancePair() { ItemId = 472, Chance = .10}
                },
                Resilience = 0,
                Jitteriness = 0,
                MoveTowardsPlayer = 4,
                Speed = 3,
                MissChance = 0,
                MineMonster = true,
                ExperienceGained = 5
            });
        }

        private void MineshaftEvents_OnGetMonsterForThisLevel(EventArgsGlobalRoute e)
        {
            // Essentially, this is giving a 30% chance to spawn TestMonster instead of another monster
            if (Game1.random.NextDouble() < .3)
            {
                var args = e as EventArgsGlobalRouteReturnable;

                args.Output = new TestMonster(Farmhand.API.Monsters.Monster.Monsters["TestMonster"],
                    new Vector2((int)e.Parameters[2], (int)e.Parameters[3]) * (float)Game1.tileSize);
            }
        }
    }
}
