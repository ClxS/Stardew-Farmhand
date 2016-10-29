using System;
using Farmhand;
using Farmhand.API.Player;
using Farmhand.Events;
using Farmhand.Events.Arguments.ControlEvents;
using Farmhand.Events.Arguments.GameEvents;
using Farmhand.Events.Arguments.PlayerEvents;
using Farmhand.Logging;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using Object = StardewValley.Object;

namespace TestMailMod
{
    public class TestMailMod : Mod
    {
        public override void Entry()
        {
            GameEvents.OnAfterLoadedContent += GameEvents_AfterContentLoaded;
            PlayerEvents.OnItemAddedToInventory += PlayerEvents_ItemAddedToInventory;
            ControlEvents.OnKeyPressed += ControlEvents_KeyPressed;
        }

        private bool SentMail;

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {   
            if (e.KeyPressed == Keys.F8)
            {
                if (!SentMail)
                {
                    SentMail = true;
                    Game1.timeOfDay = 900;
                }
                if (!Game1.player.hasOrWillReceiveMail("testMod_StartTestQuest"))
                {
                    Game1.player.addItemToInventory(new Object(337, 2));
                    Game1.player.addItemToInventory(new Object(789, 1));
                    Game1.player.mailForTomorrow.Add("testMod_StartTestQuest");
                }
            }
        }

        private void PlayerEvents_ItemAddedToInventory(object sender, EventArgsOnItemAddedToInventory e)
        {
            Log.Info(e.Item.Name);
            Console.WriteLine(Game1.player.hasOrWillReceiveMail("testMod_StartTestQuest"));
            if (e.Item.Name == "Dwarf Scroll III" && !Game1.player.hasOrWillReceiveMail("testMod_StartTestQuest"))
            {
                Log.Info("Mail has been sent!");
                Game1.player.mailForTomorrow.Add("testMod_StartTestQuest");
            }
        }

        private void GameEvents_AfterContentLoaded(object sender, EventArgs e)
        {
            Mail.RegisterMail(new MailInformation
            {
                Id = "testMod_StartTestQuest",
                Message = "I want you to go to the Beach! NO QUESTIONS ASKED!^  ???%item quest 200 %%"
            });
        }
    }
}
