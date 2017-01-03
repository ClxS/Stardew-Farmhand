using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farmhand;
using Farmhand.API.Player;
using Farmhand.Events;
using Farmhand.Events.Arguments.GameEvents;
using static Farmhand.API.Player.QuestInformation;

namespace TestQuestMod
{
    public class TestQuestMod : Mod
    {
        public override void Entry()
        {
            GameEvents.OnAfterLoadedContent += GameEvents_AfterContentLoaded;
        }

        private void GameEvents_AfterContentLoaded(object sender, EventArgs e)
        {
            Quest.RegisterQuest(new QuestInformation
            {
                Type = QuestType.Location,
                QuestTitle = "Who Was That?",
                QuestDescription = "You've been told by a stranger to go to the Beach, Wonder why...",
                QuestObjective = new LocationObjective
                {
                    ObjectiveDescription = "Go to the Beach",
                    MapName = "Beach"
                },
                NextQuests = new []{ 201 }
            });

            Quest.RegisterQuest(new QuestInformation
            {
                Type = QuestType.ItemDelivery,
                QuestTitle = "Clint's Strange Need",
                QuestDescription = "You found this note as soon as you got the the Beach, It seems that Clint wants Lewis' \"Shorts\"... This is starting to get weird...",
                QuestObjective = new ItemDeliveryObjective
                {
                    NpcName = "Clint",
                    ItemId = 789,

                    ObjectiveDescription = "Deliver the Mayor's \"Shorts\" to Clint",
                    CompletionMessage = "Thank you @, I am sorry I can not tell you what is it I Need these for. But I want you to go get me 2 Iridium Bars! I shall reward you handsomely!$h"
                },
                NextQuests = new []{ 202 }
            });

            Quest.RegisterQuest(new QuestInformation
            {
                Type = QuestType.ItemDelivery,
                QuestTitle = "Iridium Bars?",
                QuestDescription = "What in the world is Clint planning to do with the Mayor's \"Shorts\" and Iridium Bars... Maybe I should stop thinking about it.",
                QuestObjective = new ItemDeliveryObjective
                {
                    NpcName = "Clint",
                    ItemId = 337,
                    Amount = 2,

                    ObjectiveDescription = "Deliver 2 Iridium Bars to Clint",
                    CompletionMessage = "Oh you're back! Thank you for your help @, I must ask you to leave so I can do what I must! Please, Do not tell anyone about what you've done here today!$h"
                },
                MoneyReward = 15000
            });
        }
    }
}
