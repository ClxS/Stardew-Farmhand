namespace Farmhand.API.Tools
{
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    ///     Contains custom weapon information.
    /// </summary>
    public class WeaponInformation
    {
        /// <summary>
        ///     Gets the ID for the weapon.
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        ///     Gets or sets the texture for the weapon.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        ///     Gets or sets the name of the weapon.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the weapon description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the minimum damage.
        /// </summary>
        public int MinDamage { get; set; }

        /// <summary>
        ///     Gets or sets the maximum damage.
        /// </summary>
        public int MaxDamage { get; set; }

        /// <summary>
        ///     Gets or sets the knockback.
        /// </summary>
        public float Knockback { get; set; }

        /// <summary>
        ///     Gets or sets the attack speed.
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        ///     Gets or sets the added precision.
        /// </summary>
        public int AddedPrecision { get; set; }

        /// <summary>
        ///     Gets or sets the added defense.
        /// </summary>
        public int AddedDefense { get; set; }

        /// <summary>
        ///     Gets or sets the weapon type.
        /// </summary>
        public WeaponType WeaponType { get; set; }

        /// <summary>
        ///     Gets or sets the added area of effect.
        /// </summary>
        public int AddedAreaOfEffect { get; set; }

        /// <summary>
        ///     Gets or sets the critical hit chance.
        /// </summary>
        public float CritChance { get; set; }

        /// <summary>
        ///     Gets or sets the critical hit multiplier.
        /// </summary>
        public float CritMultiplier { get; set; }

        /// <summary>
        ///     The to string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            return
                $"{this.Name}/{this.Description}/{this.MinDamage}/{this.MaxDamage}/{this.Knockback}/{this.Speed}/{this.AddedPrecision}/{this.AddedDefense}/{(int)this.WeaponType}/-1/-1/{this.AddedAreaOfEffect}/{this.CritChance}/{this.CritMultiplier}";
        }
    }
}