using Farmhand.API.Generic;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Monsters
{
    public class MonsterInformation
    {
        // Monster name
        public string Name { get; set; }

        // Monster Texture
        public Texture2D Texture { get; set; }

        // Current health of the monster
        public int Health { get; set; }

        // Max health of the monster
        public int MaxHealth { get; set; }

        // Damage this monster deals
        public int DamageToFarmer { get; set; }

        // Is this monster a flier?
        public bool IsGlider { get; set; }

        //
        public int DurationOfRandomMovements { get; set; }

        // Objects to drop, by int IDs. Note, these are guaranted drops
        public List<ItemChancePair> ObjectsToDrop { get; set; }

        // Armor, I think
        public int Resilience { get; set; }

        //
        public double Jitteriness { get; set; }

        //
        public int MoveTowardsPlayer { get; set; }

        // Speed this monster moves
        public int Speed { get; set; }

        //
        public double MissChance { get; set; }

        //
        public bool MineMonster { get; set; }

        // Experience gained from killing
        public int ExperienceGained { get; set; }

        public override string ToString()
        {
            string ObjectsToDropString = "";
            for(int i=0; i<ObjectsToDrop.Count; i++)
            {
                ObjectsToDropString += ObjectsToDrop[i].ItemId + " " + ObjectsToDrop[i].Chance;

                if(i < (ObjectsToDrop.Count-1))
                {
                    ObjectsToDropString += " ";
                }
            }

            return $"{MaxHealth}/{DamageToFarmer}/0/0/{IsGlider}/{DurationOfRandomMovements}/{ObjectsToDropString}/{Resilience}/{Jitteriness}/{MoveTowardsPlayer}/{Speed}/{MissChance}/{MineMonster}/{ExperienceGained}";
        }
    }
}
