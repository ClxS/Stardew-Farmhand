using System;
using System.Collections.Generic;
using System.Linq;
using Farmhand;
using Farmhand.API.Generic;
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
            ControlEvents.OnKeyPressed += ControlEvents_KeyPressed;
        }

        private bool SentMail;

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {   
            if (e.KeyPressed == Keys.F3)
            {
                if (!SentMail)
                {
                    SentMail = true;
                    Game1.timeOfDay = 900;
                    
                    var mail = Mail.MailBox.Select(_ =>
                    {
                        if (Game1.player.hasOrWillReceiveMail(_.Key))
                            return null;

                        return _.Key;
                    }).Where(n => n != null).ToArray();
                    Game1.player.mailForTomorrow.AddRange(mail);
                    Game1.drawObjectDialogue($"Mail Sent: {string.Join(", ", mail)}");
                }
            }
        }
        
        private void GameEvents_AfterContentLoaded(object sender, EventArgs e)
        {
            Mail.RegisterMail(new MailInformation
            {
                Id = "testMod_StartTestQuest",
                Message = "I want you to go to the Beach! NO QUESTIONS ASKED!^  ???",
                Attachment = new MailInformation.QuestAttachment
                {
                    QuestId = 200
                }
            });

            Mail.RegisterMail(new MailInformation
            {
                Id = "testMod_MultiObjectAttachment",
                Message = "Here's that one of those items we promised you^  ???",
                Attachment = new MailInformation.MultiObjectAttachment
                {
                    Items = new List<ItemQuantityPair>()
                    {
                        new ItemQuantityPair { ItemId = 337, Count = 2 },
                        new ItemQuantityPair { ItemId = 651, Count = 9 },
                        new ItemQuantityPair { ItemId = 796, Count = 1 }
                    }
                }
            });

            Mail.RegisterMail(new MailInformation
            {
                Id = "testMod_ObjectAttachment",
                Message = "Here's that item we promised you^  ???",
                Attachment = new MailInformation.ObjectAttachment
                {
                    ItemId = 795,
                    Amount = 10 // 1 is the default is no Amonut has been set
                }
            });

            Mail.RegisterMail(new MailInformation
            {
                Id = "testMod_BigObjectAttachment",
                Message = "Here's that large item we promised you^  ???",
                Attachment = new MailInformation.BigObjectAttachment
                {
                    ItemId = 72
                }
            });

            Mail.RegisterMail(new MailInformation
            {
                Id = "testMod_MoneyAttachment",
                Message = "Here's that money we promised you^  ???",
                Attachment = new MailInformation.MoneyAttachment
                {
                    MinMoney = 14000000
                }
            });

            Mail.RegisterMail(new MailInformation
            {
                Id = "testMod_CookingAttachment2",
                Message = "Here's some cooking recipe we found for you^  ???",
                Attachment = new MailInformation.CookingAttachment()
                {
                    RecipeName = "Complete Breakfast"
                }
            });

            Mail.RegisterMail(new MailInformation
            {
                Id = "testMod_CraftingAttachment",
                Message = "We thought this might come in handy for you. It's the recipe for a Tall Torch!^  ???",
                Attachment = new MailInformation.CraftingAttachment
                {
                    RecipeName = "Tub o' Flowers"
                }
            });
        }
    }
}
