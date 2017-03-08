namespace Farmhand.API.Locations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Farmhand.API.Monsters;
    using Farmhand.Attributes;

    using Microsoft.Xna.Framework;

    using StardewValley;

    using Monster = StardewValley.Monsters.Monster;

    /// <summary>
    ///     Provides functions relating to the MineShaft location
    /// </summary>
    public static class MineShaft
    {
        private static readonly Random Random = new Random();

        /// <summary>
        ///     Gets the monster spawn chances.
        /// </summary>
        public static List<MineshaftMonsterSpawnChance> MonsterSpawnChances { get; } =
            new List<MineshaftMonsterSpawnChance>();

        /// <summary>
        ///     Adds the chance for a given monster to spawn
        /// </summary>
        /// <param name="monsterType">Type of monster that will spawn</param>
        /// <param name="monsterInformation">Information of monster that will spawn</param>
        /// <param name="spawnWeight">
        ///     Chance that this monster will spawn, the higher the number, the higher the chance, with 1
        ///     being the weight of the default monster
        /// </param>
        /// <param name="minLevel">earliest mineshaft level that this spawn chance applies</param>
        /// <param name="maxLevel">latest mineshaft level that this spawn chance applies</param>
        public static void AddMonsterSpawnChance(
            Type monsterType,
            MonsterInformation monsterInformation,
            double spawnWeight,
            int minLevel,
            int maxLevel)
        {
            // Create the new spawn chance data object
            var newSpawnChance = new MineshaftMonsterSpawnChance(
                monsterType,
                monsterInformation,
                spawnWeight,
                minLevel,
                maxLevel);

            // Add it to the MonsterSpawnChances list
            MonsterSpawnChances.Add(newSpawnChance);
        }

        [HookReturnable(HookType.Exit, "StardewValley.Locations.MineShaft", "getMonsterForThisLevel")]
        internal static Monster GetMonsterForThisLevel(
            [UseOutputBind] out bool useOutput,
            [InputBind(typeof(int), "level")] int level,
            [InputBind(typeof(int), "xTile")] int xTile,
            [InputBind(typeof(int), "yTile")] int yTile)
        {
            // Create the spawning location vector the same way it's handled in the hooked method
            var vector = new Vector2(xTile, yTile) * Game1.tileSize;

            // Create a new sub-list of spawn chances that apply to this level
            var applicableChances = MonsterSpawnChances.Where(c => c.MinLevel <= level && c.MaxLevel >= level).ToArray();

            // Get the sum of all chance weights that apply. Starts at 1 for the default value's weight
            var weightSum = 1.0;
            foreach (var chance in applicableChances)
            {
                weightSum += chance.SpawnWeight;
            }

            // Get a random double between 0 and 1
            var randomValue = Random.NextDouble();

            // Current Evaluated Weight for the following loop
            var currentWeight = 0.0;

            // Loop through the applicable chances, and determine if luck has shined upon the monster it contains
            foreach (var chance in applicableChances)
            {
                if (currentWeight < randomValue * weightSum
                    && currentWeight + chance.SpawnWeight >= randomValue * weightSum)
                {
                    // Create the monster object and pass it back to be used
                    var newMonsterObject =
                        (Monster)Activator.CreateInstance(chance.MonsterType, chance.Information, vector);

                    useOutput = true;
                    return newMonsterObject;
                }

                currentWeight += chance.SpawnWeight;
            }

            // Luck did not shine on any custom spawn chances, use the default
            useOutput = false;
            return null;
        }

        #region Nested type: MineshaftMonsterSpawnChance

        /// <summary>
        ///     A data class which holds monster spawn chance information for the mineshaft
        /// </summary>
        public class MineshaftMonsterSpawnChance
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="MineshaftMonsterSpawnChance" /> class.
            /// </summary>
            /// <param name="monsterType">
            ///     The monster type.
            /// </param>
            /// <param name="information">
            ///     The monster information.
            /// </param>
            /// <param name="spawnWeight">
            ///     The spawn chance weighting.
            /// </param>
            /// <param name="minLevel">
            ///     The min level.
            /// </param>
            /// <param name="maxLevel">
            ///     The max level.
            /// </param>
            public MineshaftMonsterSpawnChance(
                Type monsterType,
                MonsterInformation information,
                double spawnWeight,
                int minLevel,
                int maxLevel)
            {
                this.MonsterType = monsterType;
                this.Information = information;
                this.SpawnWeight = spawnWeight;
                this.MinLevel = minLevel;
                this.MaxLevel = maxLevel;
            }

            /// <summary>
            ///     Gets or sets the monster information.
            /// </summary>
            public MonsterInformation Information { get; set; }

            /// <summary>
            ///     Gets or sets the maximum mineshaft level.
            /// </summary>
            public int MaxLevel { get; set; }

            /// <summary>
            ///     Gets or sets the minimum mineshaft level.
            /// </summary>
            public int MinLevel { get; set; }

            /// <summary>
            ///     Gets or sets the Monster type.
            /// </summary>
            public Type MonsterType { get; set; }

            /// <summary>
            ///     Gets or sets the spawn weighting.
            /// </summary>
            public double SpawnWeight { get; set; }
        }

        #endregion
    }
}