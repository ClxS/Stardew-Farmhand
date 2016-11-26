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

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Texture2D Texture { get; set; }
        public Texture2D Portrait { get; set; }

        public NpcAge Age { get; set; }
        public NpcManners Manners { get; set; }
        public NpcSocial SocialAnxiety { get; set; }
        public NpcOptimism Optimism { get; set; }
        public NpcGender Gender { get; set; }
        public NpcHomeRegion HomeRegion { get; set; }

        public string BirthdaySeason { get; set; }
        public int BirthdayDate { get; set; }
        private string Birthday => $"{BirthdaySeason} {BirthdayDate}";

        public bool IsDatable { get; set; }
        private string Datable => IsDatable ? "datable" : "not-datable";
        public string LoveInterest { get; set; } = null;

        public string DefaultMap { get; set; }
        public int DefaultX { get; set; }
        public int DefaultY { get; set; }
        public NpcDirection DefaultFacingDirection { get; set; }

        private string DefaultLocation => $"{DefaultMap} {DefaultX} {DefaultY}";

        public string Family { get; set; }

        public GiftTastesInformation GiftTastes { get; set; }
        public ScheduleInformation Schedules { get; set; }
        public DialogueInformation Dialogues { get; set; }

        public string DispositionString => $"{Age.GetEnumName()}/{Manners.GetEnumName()}/{SocialAnxiety.GetEnumName()}/{Optimism.GetEnumName()}/{Gender.GetEnumName()}/{Datable}/{LoveInterest}/{HomeRegion.GetEnumName()}/{Birthday}/{Family}/{DefaultLocation}/{DisplayName ?? Name}";
    }
}
