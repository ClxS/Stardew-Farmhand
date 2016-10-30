using System.Collections.Generic;
using System.Linq;
using Farmhand.API.Generic;

namespace Farmhand.API.Player
{
    public class MailInformation
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public MailAttachment Attachment { get; set; }

        public class MailAttachment { }

        public class ObjectAttachment : MailAttachment
        {
            public int ItemId { get; set; }
            public int Amount { get; set; } = 1;
        }

        public class MultiObjectAttachment : MailAttachment
        {
            public List<ItemQuantityPair> Items { get; set; } = new List<ItemQuantityPair>();
        }

        public class ToolAttachment : MailAttachment
        {
            public string ToolType { get; set; }
        }

        public class BigObjectAttachment : MailAttachment
        {
            public int ItemId { get; set; }
        }

        public class MoneyAttachment : MailAttachment
        {
            public int MinMoney { get; set; }
            public int MaxMoney { get; set; }
        }

        public class QuestAttachment : MailAttachment
        {
            public int QuestId { get; set; }
        }

        public class CookingAttachment : MailAttachment { }

        public class CraftingAttachment : MailAttachment
        {
            public string RecipeName { get; set; }
        }

        private string GetAttachment()
        {
            if (Attachment == null) return "";

            var ret = "%item ";

            if ((Attachment as ObjectAttachment) != null)
            {
                var a = (ObjectAttachment)Attachment;
                ret += $"object {a.ItemId} {a.Amount}";
            }
            if ((Attachment as MultiObjectAttachment) != null)
            {
                var a = (MultiObjectAttachment)Attachment;
                ret += $"object {string.Join(" ", a.Items.Select(item => item.ToString()))}";
            }
            if ((Attachment as ToolAttachment) != null)
            {
                var a = (ToolAttachment)Attachment;
                ret += $"tools {a.ToolType}";
            }
            if ((Attachment as BigObjectAttachment) != null)
            {
                var a = (BigObjectAttachment)Attachment;
                ret += $"bigobject {a.ItemId}";
            }
            if ((Attachment as MoneyAttachment) != null)
            {
                var a = (MoneyAttachment)Attachment;
                var amount = a.MaxMoney > 0 ? Constants.Randomizer.NextInt(a.MinMoney, a.MaxMoney) : a.MinMoney;
                ret += $"money {amount}";
            }
            if ((Attachment as QuestAttachment) != null)
            {
                var a = (QuestAttachment)Attachment;
                ret += $"quest {a.QuestId}";
            }
            if ((Attachment as CookingAttachment) != null)
            {
                var a = (CookingAttachment)Attachment;
                ret += "cookingRecipe";
            }
            if ((Attachment as CraftingAttachment) != null)
            {
                var a = (CraftingAttachment)Attachment;
                ret += $"craftinRecipe {a.RecipeName.Replace(" ", "_")}";
            }

            return $"{ret} %%";
        }

        public override string ToString() => $"{Message}{GetAttachment()}";
    }
}
