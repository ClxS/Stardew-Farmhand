namespace Farmhand.API.Monsters
{
    using System.Collections.Generic;

    using Farmhand.API.Generic;

    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    ///     Information about a mod Monster.
    /// </summary>
    public class MonsterInformation
    {
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the texture for the monster.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        ///     Gets or sets the monster's health.
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        ///     Gets or sets the max health of the monster.
        /// </summary>
        public int MaxHealth { get; set; }

        /// <summary>
        ///     Gets or sets the damage caused by the monster.
        /// </summary>
        public int DamageToFarmer { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the monster is a flying monster.
        /// </summary>
        public bool IsGlider { get; set; }

        /// <summary>
        ///     Gets or sets the duration of random movements.
        /// </summary>
        public int DurationOfRandomMovements { get; set; }

        /// <summary>
        ///     Gets or sets the objects to drop, by int IDs. Note, these are guaranteed drops.
        /// </summary>
        public List<ItemChancePair> ObjectsToDrop { get; set; }

        /// <summary>
        ///     Gets or sets the monster armor.
        /// </summary>
        public int Resilience { get; set; }

        /// <summary>
        ///     Gets or sets the jitteriness of the monster's random movements.
        /// </summary>
        public double Jitteriness { get; set; }

        /// <summary>
        ///     Gets or sets the range when to move towards player.
        /// </summary>
        public int MoveTowardsPlayer { get; set; }

        /// <summary>
        ///     Gets or sets the speed this monster moves.
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        ///     Gets or sets the miss chance.
        /// </summary>
        public double MissChance { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this monster spawns in the mine.
        /// </summary>
        public bool MineMonster { get; set; }

        /// <summary>
        ///     Gets or sets the experience gained from killing this monster.
        /// </summary>
        public int ExperienceGained { get; set; }

        /// <summary>
        ///     Converts this information into a game compatible string.
        /// </summary>
        /// <returns>
        ///     The information as a string.
        /// </returns>
        public override string ToString()
        {
            var objectsToDropString = string.Empty;
            for (var i = 0; i < this.ObjectsToDrop.Count; i++)
            {
                objectsToDropString += this.ObjectsToDrop[i].ItemId + " " + this.ObjectsToDrop[i].Chance;

                if (i < this.ObjectsToDrop.Count - 1)
                {
                    objectsToDropString += " ";
                }
            }

            return
                $"{this.MaxHealth}/{this.DamageToFarmer}/0/0/{this.IsGlider}/{this.DurationOfRandomMovements}/{objectsToDropString}/{this.Resilience}/{this.Jitteriness}/{this.MoveTowardsPlayer}/{this.Speed}/{this.MissChance}/{this.MineMonster}/{this.ExperienceGained}";
        }
    }
}