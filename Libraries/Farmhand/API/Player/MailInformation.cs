namespace Farmhand.API.Player
{
    using System.Collections.Generic;
    using System.Linq;

    using Farmhand.API.Generic;

    /// <summary>
    ///     Information on a piece of mail.
    /// </summary>
    public class MailInformation
    {
        /// <summary>
        ///     Gets or sets the unique identifier for this piece of mail.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the mail message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     Gets or sets the attachment.
        /// </summary>
        public MailAttachment Attachment { get; set; }

        private string GetAttachment()
        {
            if (this.Attachment == null)
            {
                return string.Empty;
            }

            var ret = "%item ";

            var objectAttachment = this.Attachment as ObjectAttachment;
            if (objectAttachment != null)
            {
                var a = objectAttachment;
                ret += $"object {a.ItemId} {a.Amount}";
            }

            var multiObjectAttachment = this.Attachment as MultiObjectAttachment;
            if (multiObjectAttachment != null)
            {
                var a = multiObjectAttachment;
                ret += $"object {string.Join(" ", a.Items.Select(item => item.ToString()))}";
            }

            var toolAttachment = this.Attachment as ToolAttachment;
            if (toolAttachment != null)
            {
                var a = toolAttachment;
                ret += $"tools {a.ToolType}";
            }

            var bigObjectAttachment = this.Attachment as BigObjectAttachment;
            if (bigObjectAttachment != null)
            {
                var a = bigObjectAttachment;
                ret += $"bigobject {a.ItemId}";
            }

            var moneyAttachment = this.Attachment as MoneyAttachment;
            if (moneyAttachment != null)
            {
                var a = moneyAttachment;
                var amount = a.MaxMoney > 0 ? Constants.Randomizer.NextInt(a.MinMoney, a.MaxMoney) : a.MinMoney;
                ret += $"money {amount}";
            }

            var questAttachment = this.Attachment as QuestAttachment;
            if (questAttachment != null)
            {
                var a = questAttachment;
                ret += $"quest {a.QuestId}";
            }

            var cookingAttachment = this.Attachment as CookingAttachment;
            if (cookingAttachment != null)
            {
                var a = cookingAttachment;
                ret += $"fh-recipe {a.RecipeName}";
            }

            var craftingAttachment = this.Attachment as CraftingAttachment;
            if (craftingAttachment != null)
            {
                var a = craftingAttachment;
                ret += $"craftinRecipe {a.RecipeName.Replace(" ", "_")}";
            }

            return $"{ret} %%";
        }

        /// <summary>
        ///     Gets the string representation of this mail.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string ToString() => $"{this.Message}{this.GetAttachment()}";

        #region Nested type: BigObjectAttachment

        /// <summary>
        ///     An attachment which contains a BigObject.
        /// </summary>
        public class BigObjectAttachment : MailAttachment
        {
            /// <summary>
            ///     Gets or sets the item id.
            /// </summary>
            public int ItemId { get; set; }
        }

        #endregion

        #region Nested type: CookingAttachment

        /// <summary>
        ///     An attachment which contains a cooking item.
        /// </summary>
        public class CookingAttachment : MailAttachment
        {
            /// <summary>
            ///     Gets or sets the recipe name.
            /// </summary>
            public string RecipeName { get; set; }
        }

        #endregion

        #region Nested type: CraftingAttachment

        /// <summary>
        ///     An attachment which contains a crafting recipe.
        /// </summary>
        public class CraftingAttachment : MailAttachment
        {
            /// <summary>
            ///     Gets or sets the recipe name.
            /// </summary>
            public string RecipeName { get; set; }
        }

        #endregion

        #region Nested type: MailAttachment

        /// <summary>
        ///     A mail attachment.
        /// </summary>
        public abstract class MailAttachment
        {
        }

        #endregion

        #region Nested type: MoneyAttachment

        /// <summary>
        ///     An attachment which contains money.
        /// </summary>
        public class MoneyAttachment : MailAttachment
        {
            /// <summary>
            ///     Gets or sets the min money.
            /// </summary>
            public int MinMoney { get; set; }

            /// <summary>
            ///     Gets or sets the max money.
            /// </summary>
            public int MaxMoney { get; set; }
        }

        #endregion

        #region Nested type: MultiObjectAttachment

        /// <summary>
        ///     An attachment which contains multiple objects.
        /// </summary>
        public class MultiObjectAttachment : MailAttachment
        {
            /// <summary>
            ///     Gets or sets the attached items.
            /// </summary>
            public List<ItemQuantityPair> Items { get; set; } = new List<ItemQuantityPair>();
        }

        #endregion

        #region Nested type: ObjectAttachment

        /// <summary>
        ///     An attachment which contains an object.
        /// </summary>
        public class ObjectAttachment : MailAttachment
        {
            /// <summary>
            ///     Gets or sets the item ID.
            /// </summary>
            public int ItemId { get; set; }

            /// <summary>
            ///     Gets or sets the amount of items attached.
            /// </summary>
            public int Amount { get; set; } = 1;
        }

        #endregion

        #region Nested type: QuestAttachment

        /// <summary>
        ///     An attachment which contains a quest.
        /// </summary>
        public class QuestAttachment : MailAttachment
        {
            /// <summary>
            ///     Gets or sets the quest id.
            /// </summary>
            public int QuestId { get; set; }
        }

        #endregion

        #region Nested type: ToolAttachment

        /// <summary>
        ///     An attachment which contains a tool.
        /// </summary>
        public class ToolAttachment : MailAttachment
        {
            /// <summary>
            ///     Gets or sets the tool type.
            /// </summary>
            public string ToolType { get; set; }
        }

        #endregion
    }
}