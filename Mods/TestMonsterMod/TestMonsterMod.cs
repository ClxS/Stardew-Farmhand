using Farmhand;
using Farmhand.API.Generic;
using Farmhand.API.Monsters;
using System.Collections.Generic;
using TestMonsterMod.Monsters;

namespace TestMonsterMod
{
    public class TestMonsterMod : Mod
    {
        public static TestMonsterMod Instance;

        public override void Entry()
        {
            Farmhand.Events.GameEvents.AfterLoadedContent += GameEvents_OnAfterLoadedContent;
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
                    new ItemChancePair { ItemId = 167, Chance = .50},
                    new ItemChancePair { ItemId = 167, Chance = .05},
                    new ItemChancePair { ItemId = 472, Chance = .10}
                },
                Resilience = 0,
                Jitteriness = 0,
                MoveTowardsPlayer = 4,
                Speed = 3,
                MissChance = 0,
                MineMonster = true,
                ExperienceGained = 5
            });

            Farmhand.API.Locations.MineShaft.AddMonsterSpawnChance(typeof(TestMonster), Farmhand.API.Monsters.Monster.Monsters["TestMonster"], 1.0, 1, 200);
        }
    }
}
