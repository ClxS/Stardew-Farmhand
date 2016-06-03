using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Tools
{
    public class WeaponInformation
    {
        public int Id { get; set; }

        public Texture2D Texture { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        public float Knockback { get; set; }

        public int Speed { get; set; }

        public int AddedPrecision { get; set; }

        public int AddedDefense { get; set; }

        public int WeaponType { get; set; }

        public int AddedAreaOfEffect { get; set; }

        public float CritChance { get; set; }

        public float CritMultiplier { get; set; }

        public override string ToString()
        {
            return $"{Name}/{Description}/{MinDamage}/{MaxDamage}/{Knockback}/{Speed}/{AddedPrecision}/{AddedDefense}/{WeaponType}/-1/-1/{AddedAreaOfEffect}/{CritChance}/{CritMultiplier}";
        }
    }
}
