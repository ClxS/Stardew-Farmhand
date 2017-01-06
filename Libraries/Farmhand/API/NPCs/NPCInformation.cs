namespace Farmhand.API.NPCs
{
    using Farmhand.API.NPCs.Characteristics;
    using Farmhand.API.NPCs.Dialogues;
    using Farmhand.API.NPCs.GiftTastes;
    using Farmhand.API.NPCs.Schedules;
    using Farmhand.Helpers;

    using Microsoft.Xna.Framework.Graphics;

    public class NpcInformation
    {
        /// <summary>
        ///     Gets or sets the NPC Name. This is used by the serialization fix-up method to make sure old saves are updated.
        ///     This should be unique to your mod, ideally including something such as the mod name and author name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the in-game name of the NPC
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///     Gets or sets the in-game spritesheet of the NPC
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        ///     Gets or sets the image to display during conversations
        /// </summary>
        public Texture2D Portrait { get; set; }

        /// <summary>
        ///     Gets or sets the age of the NPC
        /// </summary>
        public Age Age { get; set; }

        /// <summary>
        ///     Gets or sets the manners of the NPC
        /// </summary>
        public Manners Manners { get; set; }

        /// <summary>
        ///     Gets or sets the social anxiety level of the NPC
        /// </summary>
        public SocialAttitude SocialAnxiety { get; set; }

        /// <summary>
        ///     Gets or sets the optimism level of the NPC
        /// </summary>
        public Optimism Optimism { get; set; }

        /// <summary>
        ///     Gets or sets the gender of the NPC
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        ///     Gets or sets the home region of the NPC (i.e. Town/Desert/Other)
        /// </summary>
        public HomeRegion HomeRegion { get; set; }

        /// <summary>
        ///     Gets or sets the season of the NPCs birthday
        /// </summary>
        public BirthdaySeason BirthdaySeason { get; set; }

        /// <summary>
        ///     Gets or sets the day of the NPCs birthday
        /// </summary>
        public int BirthdayDate { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this NPC is a romance option.
        /// </summary>
        public bool IsDatable { get; set; }

        /// <summary>
        ///     Gets or sets the NPC who is the love interest of this NPC
        /// </summary>
        public string LoveInterest { get; set; } = null;

        /// <summary>
        ///     Gets or sets which map this NPC should first appear in
        /// </summary>
        public string DefaultMap { get; set; }

        /// <summary>
        ///     Gets or sets the default X position of this NPC when it is first created
        /// </summary>
        public int DefaultX { get; set; }

        /// <summary>
        ///     Gets or sets the default Y position of this NPC when it is first created
        /// </summary>
        public int DefaultY { get; set; }

        /// <summary>
        ///     Gets or sets the direction this NPC is when it is first created
        /// </summary>
        public Direction DefaultFacingDirection { get; set; }

        /// <summary>
        ///     Gets or sets the family this NPC belongs to
        /// </summary>
        public string Family { get; set; }

        /// <summary>
        ///     Gets or sets information about the gift tastes for this NPC
        /// </summary>
        public GiftTastesInformation GiftTastes { get; set; }

        /// <summary>
        ///     Gets or sets the scheduling information for this NPC
        /// </summary>
        public ScheduleInformation Schedules { get; set; }

        /// <summary>
        ///     Gets or sets the dialogue information for this NPC
        /// </summary>
        public DialogueInformation Dialogues { get; set; }

        private string Birthday => $"{NpcUtility.GetBirthdaySeasonValue(this.BirthdaySeason)} {this.BirthdayDate}";

        private string Datable => this.IsDatable ? "datable" : "not-datable";

        private string DefaultLocation => $"{this.DefaultMap} {this.DefaultX} {this.DefaultY}";

        /// <summary>
        ///     Gets the game compatible string representation of this NPC.
        /// </summary>
        public string DispositionString
            =>
                this.Age.GetEnumName() + "/" + this.Manners.GetEnumName() + "/" + this.SocialAnxiety.GetEnumName() + "/"
                + this.Optimism.GetEnumName() + "/" + this.Gender.GetEnumName() + "/" + this.Datable + "/"
                + this.LoveInterest + "/" + this.HomeRegion.GetEnumName() + "/" + this.Birthday + "/" + this.Family
                + "/" + this.DefaultLocation + "/" + (this.DisplayName ?? this.Name);
    }
}