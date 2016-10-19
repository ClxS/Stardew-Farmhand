using System;
using System.Collections.Generic;
using System.ComponentModel;
using Farmhand.Helpers;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Farmhand.API.NPCs
{
    public class NPCInformation
    {
        public enum NPCAge {
            [Description("adult")]
            Adult,
            [Description("teen")]
            Teen,
            [Description("child")]
            Child
        }
        public enum NPCManners {
            [Description("neutral")]
            Neutral,
            [Description("polite")]
            Polite,
            [Description("rude")]
            Rude
        }
        public enum NPCSocial {
            [Description("outgoing")]
            Outgoing,
            [Description("shy")]
            Shy
        }
        public enum NPCOptimism {
            [Description("positive")]
            Positive,
            [Description("negative")]
            Negative
        }
        public enum NPCGender {
            [Description("male")]
            Male,
            [Description("female")]
            Female,
            [Description("undefined")]
            Undefined
        }
        public enum NPCHomeRegion {
            Other,
            Desert,
            Town
        }
        public enum NPCDirection {
            South,
            East,
            North,
            West
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Texture2D Texture { get; set; }
        public Texture2D Portrait { get; set; }

        public NPCAge Age { get; set; }
        public NPCManners Manners { get; set; }
        public NPCSocial SocialAnxiety { get; set; }
        public NPCOptimism Optimism { get; set; }
        public NPCGender Gender { get; set; }
        public NPCHomeRegion HomeRegion { get; set; }

        public string BirthdaySeason { get; set; }
        public int BirthdayDate { get; set; }
        private string Birthday => $"{BirthdaySeason} {BirthdayDate}";

        public bool IsDatable { get; set; }
        private string Datable => IsDatable ? "datable" : "not-datable";
        public string LoveInterest { get; set; } = null;

        public string DefaultMap { get; set; }
        public int DefaultX { get; set; }
        public int DefaultY { get; set; }
        public NPCDirection DefaultFacingDirection { get; set; }

        private string DefaultLocation => $"{DefaultMap} {DefaultX} {DefaultY}";
        
        public string Family { get; set; }

        public NPCGiftTastesInformation GiftTastes { get; set; }
        public NPCSchedulesInformation Schedules { get; set; }
        public NPCDialoguesInformation Dialogues { get; set; }

        public string DispositionString => $"{Age.GetEnumName()}/{Manners.GetEnumName()}/{SocialAnxiety.GetEnumName()}/{Optimism.GetEnumName()}/{Gender.GetEnumName()}/{Datable}/{LoveInterest}/{HomeRegion.GetEnumName()}/{Birthday}/{Family}/{DefaultLocation}/{DisplayName ?? Name}";
    }
}