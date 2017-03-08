namespace Farmhand.Character
{
    using System.Xml.Serialization;

    using Farmhand.API.Monsters;

    using Microsoft.Xna.Framework;

    /// <summary>
    ///     Acts as a base class for custom monsters.
    /// </summary>
    [XmlType("MonsterOverride")]
    public class Monster : StardewValley.Monsters.Monster
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Monster" /> class.
        /// </summary>
        /// <param name="information">
        ///     The information.
        /// </param>
        /// <param name="pos">
        ///     The position.
        /// </param>
        public Monster(MonsterInformation information, Vector2 pos)
            : base(information.Name, pos)
        {
            this.Information = information;
        }

        /// <summary>
        ///     Gets or sets the monster information.
        /// </summary>
        protected MonsterInformation Information { get; set; }
    }
}