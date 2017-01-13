namespace Farmhand.Character
{
    using System.Xml.Serialization;

    using Farmhand.API.NPCs;

    using Microsoft.Xna.Framework;

    using StardewValley;

    /// <summary>
    ///     Acts as a base class for custom NPCs.
    /// </summary>
    [XmlType("NPCOverride")]
    public class NPC : StardewValley.NPC
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NPC" /> class.
        /// </summary>
        /// <param name="information">
        ///     The NPC information.
        /// </param>
        /// <param name="position">
        ///     The NPC position.
        /// </param>
        public NPC(NpcInformation information, Vector2 position)
            : base(
                new AnimatedSprite(information.Texture, 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4),
                position,
                information.DefaultMap,
                (int)information.DefaultFacingDirection,
                information.Name,
                information.IsDatable,
                null,
                information.Portrait)
        {
            this.Information = information;
        }

        /// <summary>
        ///     Gets or sets the NPC information.
        /// </summary>
        protected NpcInformation Information { get; set; }
    }
}