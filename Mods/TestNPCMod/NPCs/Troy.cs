using System.Collections.Generic;
using Farmhand.API.NPCs;
using Farmhand.API.NPCs.Dialogues;
using Farmhand.API.NPCs.GiftTastes;
using Farmhand.API.NPCs.Schedules;
using Microsoft.Xna.Framework;
using StardewValley;


namespace TestNPCMod.NPCs
{
    public class Troy : Farmhand.Overrides.Character.NPC
    {
        #region Registration Information
        private static NpcInformation _information;
        public new static NpcInformation Information => _information ?? (_information = new NpcInformation
        {
            Name = "TroyNPCTest",
            DisplayName = "Troy",
            Texture = TestNpcMod.Instance.ModSettings.GetTexture("sprite_TestNPC"),
            Portrait = TestNpcMod.Instance.ModSettings.GetTexture("sprite_TestNPC_Portrait"),
            Age = Farmhand.API.NPCs.Characteristics.Age.Teen,
            Manners = Farmhand.API.NPCs.Characteristics.Manners.Polite,
            SocialAnxiety = Farmhand.API.NPCs.Characteristics.SocialAttitude.Shy,
            Optimism = Farmhand.API.NPCs.Characteristics.Optimism.Positive,
            Gender = Farmhand.API.NPCs.Characteristics.Gender.Male,
            HomeRegion = Farmhand.API.NPCs.Characteristics.HomeRegion.Town,

            BirthdaySeason = Farmhand.API.NPCs.Characteristics.BirthdaySeason.Winter,
            BirthdayDate = 13,

            DefaultX = 64,
            DefaultY = 17,
            DefaultMap = Farmhand.API.Locations.DefaultMaps.Farm,
            DefaultFacingDirection = Farmhand.API.NPCs.Characteristics.Direction.West,

            IsDatable = false,

            Dialogues = new DialogueInformation
            {
                DialogueEntries = new List<DialogueEntry>
                    {
                        new DialogueEntry("Introduction",
                            "Greetings @, I heard you just moved to our little town.#$e#I'm sure you aren't too familiar with the lifestyle here but I'm sure It'll grow on you quickly."),
                        new DialogueEntry("Mon", "Why hello again @, Have you heard the voices yet?"),
                        new DialogueEntry("Tue",
                            "You doing well @? Exploring is a great deal of fun isn't it?#$e#Maybe you'll find something interesting someday!"),
                        new DialogueEntry("Tue16", "Please... Just leave me alone today..."),
                        new DialogueEntry("Wed", "...$a"),
                        new DialogueEntry("Thu",
                            "I think the Wizard is up to something bad... I just can't prove it!"),
                        new DialogueEntry("Fri",
                            "Who are you again? Your name seems to have escaped my mind...#$e#I'm sure we've met before though..."),
                        new DialogueEntry("Sat", "Why hello there Neightbour! Your crops growing well?"),
                        new DialogueEntry("Sun",
                            "Did I ever tell you about my cat Daisy? Cute little kitty.$h#$e#She died a few years ago though.$$neutral"),
                        new DialogueEntry("Sun21",
                            "I need to find my cat Daisy! She seems to have ran away again...#$e#You haven't seen her have you @?")
                    },
                RainyDialogue = new DialogueEntry("Tory", "It sure is pouring out today, Don't ya think @?")
            },
            GiftTastes = new GiftTastesInformation
            {
                LovesResponse = "You got this for me? It's perfect! Thank you @.$h",
                LikesResponse = "You're too kind @, I'm not sure how to repay you though.$h",
                NeutralResponse = "Well... It's the thought that counts right?",
                DislikesResponse = "Why are you giving me this?",
                HatesResponse = "Why on earth would you give me something like this? Completely useless!",

                LovesIds = new List<int> { 382, 536, 563 },
                LikesIds = new List<int> { -28, 107, 109, 119 },
                //NeutralIds = new List<int> {} // Not needed to be initialized if empty
                DislikesIds = new List<int> { -81, -79, 245 },
                HatesIds = new List<int> { -2, 72, 220, 221 }
            },
            Schedules = new ScheduleInformation
            {
                PathInformation = new List<SchedulePathInformation>
                    {
                        new SchedulePathInformation("spring")
                        {
                            Directions = new List<ScheduleDirections>
                            {
                                new ScheduleDirections(1000, Farmhand.API.Locations.DefaultMaps.WizardHouse, 11, 13,
                                    Farmhand.API.NPCs.Characteristics.Direction.East),
                                new ScheduleDirections(1200, Farmhand.API.Locations.DefaultMaps.Mountain, 57, 20,
                                    Farmhand.API.NPCs.Characteristics.Direction.West, "Nothing beats the fresh mountain breeze... Don't you think?"),
                                new ScheduleDirections(1900, Farmhand.API.Locations.DefaultMaps.WizardHouse, 2, 6,
                                    Farmhand.API.NPCs.Characteristics.Direction.West, "I'm not sure what it is, but something doesn't feel right.")
                            }
                        },
                        new SchedulePathInformation("default")
                        {
                            Directions = new List<ScheduleDirections>
                            {
                                new ScheduleDirections(true, "spring")
                            }
                        },
                        new SchedulePathInformation("rain")
                        {
                            Directions = new List<ScheduleDirections>
                            {
                                new ScheduleDirections(1000, Farmhand.API.Locations.DefaultMaps.WizardHouse, 2, 16,
                                    Farmhand.API.NPCs.Characteristics.Direction.West),
                                new ScheduleDirections(1400, Farmhand.API.Locations.DefaultMaps.WizardHouse, 3, 19,
                                    Farmhand.API.NPCs.Characteristics.Direction.South),
                                new ScheduleDirections(1700, Farmhand.API.Locations.DefaultMaps.WizardHouse, 11, 13,
                                    Farmhand.API.NPCs.Characteristics.Direction.North,
                                    "The rain always annoyed me in the past, But now I find it kind of soothing.#$e#Why is that I wonder..."),
                                new ScheduleDirections(2100, Farmhand.API.Locations.DefaultMaps.WizardHouse, 2, 6,
                                    Farmhand.API.NPCs.Characteristics.Direction.West)
                            }
                        }
                    }
            }
        });

        #endregion

        public Troy(Vector2 position) 
            : base(Troy.Information, position) {}

        public Troy() 
            : base(Troy.Information, new Vector2(Troy.Information.DefaultX * Game1.tileSize, Troy.Information.DefaultY * Game1.tileSize)) {}
    }
}
