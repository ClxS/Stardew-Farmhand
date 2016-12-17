using System.ComponentModel;
using Farmhand.API.NPCs.Dialogues;
using Farmhand.API.NPCs.GiftTastes;
using Farmhand.API.NPCs.Schedules;
using Farmhand.Helpers;
using Microsoft.Xna.Framework.Graphics;

namespace Farmhand.API.NPCs
{
    public class NpcInformation
    {
        public enum NpcAge
        {
            [Description("adult")]
            Adult,
            [Description("teen")]
            Teen,
            [Description("child")]
            Child
        }
        public enum NpcManners
        {
            [Description("neutral")]
            Neutral,
            [Description("polite")]
            Polite,
            [Description("rude")]
            Rude
        }
        public enum NpcSocial
        {
            [Description("outgoing")]
            Outgoing,
            [Description("shy")]
            Shy
        }
        public enum NpcOptimism
        {
            [Description("positive")]
            Positive,
            [Description("negative")]
            Negative
        }
        public enum NpcGender
        {
            [Description("male")]
            Male,
            [Description("female")]
            Female,
            [Description("undefined")]
            Undefined
        }
        public enum NpcHomeRegion
        {
            Other,
            Desert,
            Town
        }
        public enum NpcDirection
        {
            South,
            East,
            North,
            West
        }

        /// <summary>
        /// Used by the serialization fixup tool to make sure old saves are updated. This should be unique to your mod,
        /// ideally including something such as the mod name and author name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The in-game name of the NPC
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The in-game spritesheet of the NPC
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The image to display during conversations
        /// </summary>
        public Texture2D Portrait { get; set; }

        /// <summary>
        /// The age of the NPC
        /// </summary>
        public NpcAge Age { get; set; }

        /// <summary>
        /// The manners of the NPC
        /// </summary>
        public NpcManners Manners { get; set; }

        /// <summary>
        /// The social anxiety level of the NPC
        /// </summary>
        public NpcSocial SocialAnxiety { get; set; }

        /// <summary>
        /// The optimism level of the NPC
        /// </summary>
        public NpcOptimism Optimism { get; set; }

        /// <summary>
        /// The gender of the NPC
        /// </summary>
        public NpcGender Gender { get; set; }

        /// <summary>
        /// The home region of the NPC (i.e. Town/Desert/Other)
        /// </summary>
        public NpcHomeRegion HomeRegion { get; set; }

        /// <summary>
        /// The season of the NPCs birthday
        /// </summary>
        public string BirthdaySeason { get; set; }

        /// <summary>
        /// The day of the NPCs birthday
        /// </summary>
        public int BirthdayDate { get; set; }

        /// <summary>
        /// Whether this NPC is a romance option
        /// </summary>
        public bool IsDatable { get; set; }

        /// <summary>
        /// The NPC who is the love interest of this NPC
        /// </summary>
        public string LoveInterest { get; set; } = null;

        /// <summary>
        /// Which map this NPC should first appear in
        /// </summary>
        public string DefaultMap { get; set; }

        /// <summary>
        /// The default X position of this NPC when it is first created
        /// </summary>
        public int DefaultX { get; set; }

        /// <summary>
        /// The default Y position of this NPC when it is first creatred
        /// </summary>
        public int DefaultY { get; set; }

        /// <summary>
        /// The direction this NPC is when it is first created 
        /// </summary>
        public NpcDirection DefaultFacingDirection { get; set; }

        /// <summary>
        /// The family this NPC belongs to
        /// </summary>
        public string Family { get; set; }

        /// <summary>
        /// Information about the gift tastes for this NPC
        /// </summary>
        public GiftTastesInformation GiftTastes { get; set; }

        /// <summary>
        /// Scheduling information for this NPC
        /// </summary>
        public ScheduleInformation Schedules { get; set; }

        /// <summary>
        /// Dialogue information for this NPC
        /// </summary>
        public DialogueInformation Dialogues { get; set; }

        private string Birthday => $"{BirthdaySeason} {BirthdayDate}";

        private string Datable => IsDatable ? "datable" : "not-datable";
        
        private string DefaultLocation => $"{DefaultMap} {DefaultX} {DefaultY}";
        
        public string DispositionString => $"{Age.GetEnumName()}/{Manners.GetEnumName()}/{SocialAnxiety.GetEnumName()}/{Optimism.GetEnumName()}/{Gender.GetEnumName()}/{Datable}/{LoveInterest}/{HomeRegion.GetEnumName()}/{Birthday}/{Family}/{DefaultLocation}/{DisplayName ?? Name}";
    }
}
