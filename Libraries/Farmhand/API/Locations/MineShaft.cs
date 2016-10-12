using Farmhand.API.Monsters;
using Farmhand.Attributes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmhand.API.Locations
{
    /// <summary>
    /// Provides functions relating to the MineShaft location
    /// </summary>
    public class MineShaft
    {
        public static List<MineshaftMonsterSpawnChance> MonsterSpawnChances = new List<MineshaftMonsterSpawnChance>();

        private static readonly Random Random = new Random();

        /// <summary>
        /// Adds the chance for a given monster to spawn
        /// </summary>
        /// <param name="monsterType">Type of monster that will spawn</param>
        /// <param name="monsterInformation">Information of monster that will spawn</param>
        /// <param name="spawnWeight">Chance that this monster will spawn, the higher the number, the higher the chance, with 1 being the weight of the default monster</param>
        /// <param name="minLevel">earliest mineshaft level that this spawn chance applies</param>
        /// <param name="maxLevel">latest minehsaft level that this spawn chance applies</param>
        public static void AddMonsterSpawnChance(Type monsterType, MonsterInformation monsterInformation, double spawnWeight, int minLevel, int maxLevel)
        {
            // Create the new spawn chance data object
            MineshaftMonsterSpawnChance newSpawnChance = new MineshaftMonsterSpawnChance(monsterType, monsterInformation, spawnWeight, minLevel, maxLevel);

            // Add it to the MonsterSpawnChances list
            MonsterSpawnChances.Add(newSpawnChance);
        }

        [HookReturnable(HookType.Exit, "StardewValley.Locations.MineShaft", "getMonsterForThisLevel")]
        internal static StardewValley.Monsters.Monster GetMonsterForThisLevel(
            [UseOutputBind] ref bool useOutput,
            [InputBind(typeof(int), "level")] int level,
            [InputBind(typeof(int), "xTile")] int xTile,
            [InputBind(typeof(int), "yTile")] int yTile)
        {
            // Create the spawning location vector the same way it's handled in the hooked method
            Vector2 vector = new Vector2(xTile, yTile) * StardewValley.Game1.tileSize;

            // Create a new sub-list of spawn chances that apply to this level
            var ApplicableChances = MonsterSpawnChances.Where(c => c.MinLevel <= level && c.MaxLevel >= level);
            // Get the sum of all chance weights that apply. Starts at 1 for the default value's weight
            double weightSum = 1.0;
            foreach (MineshaftMonsterSpawnChance chance in ApplicableChances)
            {
                weightSum += chance.SpawnWeight;
            }

            // Get a random double between 0 and 1
            double randomValue = Random.NextDouble();
            // Current Evaluated Weight for the following loop
            double currentWeight = 0.0;
            // Loop through the applicable chances, and determine if luck has shined upon the monster it contains
            foreach (MineshaftMonsterSpawnChance chance in ApplicableChances)
            {
                if( currentWeight < (randomValue*weightSum) && (currentWeight + chance.SpawnWeight) >= (randomValue*weightSum) )
                {
                    // Create the monster object and pass it back to be used
                    StardewValley.Monsters.Monster newMonsterObject = (StardewValley.Monsters.Monster)Activator.CreateInstance(chance.MonsterType, chance.Information, vector);

                    useOutput = true;
                    return newMonsterObject;
                }

                currentWeight += chance.SpawnWeight;
            }

            // Luck did not shine on any custom spawn chances, use the default
            useOutput = false;
            return null;
        }

        /// <summary>
        /// A data class which holds monster spawn chance information for the mineshaft
        /// </summary>
        public class MineshaftMonsterSpawnChance
        {
            // Monster type
            public Type MonsterType { get; set; }

            // Monster name
            public MonsterInformation Information { get; set; }

            // Spawn weight
            public double SpawnWeight { get; set; }

            // Minimum mineshaft level
            public int MinLevel { get; set; }

            // Maximum mineshaft level
            public int MaxLevel { get; set; }

            public MineshaftMonsterSpawnChance(Type monsterType, MonsterInformation information, double spawnWeight, int minLevel, int maxLevel)
            {
                MonsterType = monsterType;
                Information = information;
                SpawnWeight = spawnWeight;
                MinLevel = minLevel;
                MaxLevel = maxLevel;
            }
        }
    }
}
