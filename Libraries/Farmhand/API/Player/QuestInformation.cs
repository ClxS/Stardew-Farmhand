namespace Farmhand.API.Player
{
    using System;

    /// <summary>
    ///     Information about a quest.
    /// </summary>
    public class QuestInformation
    {
        #region QuestType enum

        /// <summary>
        ///     An enumeration of quest types.
        /// </summary>
        public enum QuestType
        {
            /// <summary>
            ///     Quest to build something.
            /// </summary>
            Building,

            /// <summary>
            ///     Quest to craft something.
            /// </summary>
            Crafting,

            /// <summary>
            ///     Quest to harvest something.
            /// </summary>
            ItemHarvest,

            /// <summary>
            ///     Quest to deliver something.
            /// </summary>
            ItemDelivery,

            /// <summary>
            ///     Quest to visit a location.
            /// </summary>
            Location,

            /// <summary>
            ///     Quest to find a lost item.
            /// </summary>
            LostItem,

            /// <summary>
            ///     Quest to kill a monster.
            /// </summary>
            Monster
        }

        #endregion

        /// <summary>
        ///     Gets the quest's unique ID. This is assigned on registering this request.
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        ///     Gets or sets the quest type.
        /// </summary>
        public QuestType Type { get; set; }

        /// <summary>
        ///     Gets or sets the quest title.
        /// </summary>
        public string QuestTitle { get; set; }

        /// <summary>
        ///     Gets or sets the quest description.
        /// </summary>
        public string QuestDescription { get; set; }

        /// <summary>
        ///     Gets or sets the quest objective.
        /// </summary>
        public Objective QuestObjective { get; set; }

        /// <summary>
        ///     Gets or sets the following quests.
        /// </summary>
        public int[] NextQuests { get; set; } = { -1 };

        /// <summary>
        ///     Gets or sets the money reward for this request.
        /// </summary>
        public int MoneyReward { get; set; } = -1;

        /// <summary>
        ///     Gets or sets the reward description.
        /// </summary>
        public string RewardDescription { get; set; } = "-1";

        /// <summary>
        ///     Gets or sets a value indicating whether the quest can be canceled.
        /// </summary>
        public bool CanBeCancelled { get; set; }

        /// <summary>
        ///     Gets the quest objective.
        /// </summary>
        /// <returns>
        ///     The object in the game's expected format.
        /// </returns>
        /// <exception cref="Exception">
        ///     Throws an exception when this.Type is not a valid QuestType.
        /// </exception>
        public string GetObjective()
        {
            switch (this.Type)
            {
                case QuestType.Building:
                    var buildObj = (BuildingObjective)this.QuestObjective;
                    return $"{buildObj.BuildingName}";
                case QuestType.Crafting:
                    var craftObj = (CraftingObjective)this.QuestObjective;
                    return $"{craftObj.ItemId} {craftObj.BigCraftable}";
                case QuestType.ItemHarvest:
                    var harvObj = (ItemHarvestObjective)this.QuestObjective;
                    return $"{harvObj.ItemId} {harvObj.Amount}";
                case QuestType.ItemDelivery:
                    var delObj = (ItemDeliveryObjective)this.QuestObjective;
                    return $"{delObj.NpcName} {delObj.ItemId} {delObj.Amount}";
                case QuestType.Location:
                    var locObj = (LocationObjective)this.QuestObjective;
                    return $"{locObj.MapName}";
                case QuestType.LostItem:
                    var lostObj = (LostItemObjective)this.QuestObjective;
                    return $"{lostObj.NpcName} {lostObj.ItemId} {lostObj.MapName} {lostObj.ItemXPos} {lostObj.ItemYPos}";
                case QuestType.Monster:
                    var monsterObj = (MonsterObjective)this.QuestObjective;
                    return $"{monsterObj.MonsterName} {monsterObj.Amount}";
                default:
                    throw new Exception($"Invalid QuestType {this.Type}");
            }
        }

        /// <summary>
        ///     Gets the quest information in the game's expected format.
        /// </summary>
        /// <returns>
        ///     The quest information as a string.
        /// </returns>
        public override string ToString()
        {
            switch (this.Type)
            {
                case QuestType.ItemDelivery:
                case QuestType.LostItem:
                    var objective = this.QuestObjective as LostItemObjective;
                    var msg = objective != null
                                  ? objective.CompletionMessage
                                  : ((ItemDeliveryObjective)this.QuestObjective).CompletionMessage;

                    return
                        $"{this.Type}/{this.QuestTitle}/{this.QuestDescription}/{this.QuestObjective.ObjectiveDescription}/{this.GetObjective()}/{string.Join(" ", this.NextQuests)}/{this.MoneyReward}/{this.RewardDescription}/{this.CanBeCancelled}/{msg}";
            }

            return
                $"{this.Type}/{this.QuestTitle}/{this.QuestDescription}/{this.QuestObjective.ObjectiveDescription}/{this.GetObjective()}/{string.Join(" ", this.NextQuests)}/{this.MoneyReward}/{this.RewardDescription}/{this.CanBeCancelled}";
        }

        #region Nested type: BuildingObjective

        /// <summary>
        ///     An objective which involves building something.
        /// </summary>
        public class BuildingObjective : Objective
        {
            /// <summary>
            ///     Gets or sets the building name.
            /// </summary>
            public string BuildingName { get; set; }
        }

        #endregion

        #region Nested type: CraftingObjective

        /// <summary>
        ///     An objective which involves crafting something.
        /// </summary>
        public class CraftingObjective : Objective
        {
            /// <summary>
            ///     Gets or sets the item id.
            /// </summary>
            public int ItemId { get; set; }

            /// <summary>
            ///     Gets or sets a value indicating whether the item is a big craftable.
            /// </summary>
            public bool BigCraftable { get; set; }
        }

        #endregion

        #region Nested type: ItemDeliveryObjective

        /// <summary>
        ///     An objective which involves delivering something.
        /// </summary>
        public class ItemDeliveryObjective : Objective
        {
            /// <summary>
            ///     Gets or sets the NPC to deliver to.
            /// </summary>
            public string NpcName { get; set; }

            /// <summary>
            ///     Gets or sets the item id.
            /// </summary>
            public int ItemId { get; set; }

            /// <summary>
            ///     Gets or sets the amount desired.
            /// </summary>
            public int Amount { get; set; } = 1;

            /// <summary>
            ///     Gets or sets the completion message.
            /// </summary>
            public string CompletionMessage { get; set; }
        }

        #endregion

        #region Nested type: ItemHarvestObjective

        /// <summary>
        ///     An objective which involves harvesting something.
        /// </summary>
        public class ItemHarvestObjective : Objective
        {
            /// <summary>
            ///     Gets or sets the item id.
            /// </summary>
            public int ItemId { get; set; }

            /// <summary>
            ///     Gets or sets the amount.
            /// </summary>
            public int Amount { get; set; } = 1;
        }

        #endregion

        #region Nested type: LocationObjective

        /// <summary>
        ///     An objective which involves visiting a location.
        /// </summary>
        public class LocationObjective : Objective
        {
            /// <summary>
            ///     Gets or sets the map name.
            /// </summary>
            public string MapName { get; set; }
        }

        #endregion

        #region Nested type: LostItemObjective

        /// <summary>
        ///     An objective which involves finding something.
        /// </summary>
        public class LostItemObjective : Objective
        {
            /// <summary>
            ///     Gets or sets the NPC to deliver to.
            /// </summary>
            public string NpcName { get; set; }

            /// <summary>
            ///     Gets or sets the item id.
            /// </summary>
            public int ItemId { get; set; }

            /// <summary>
            ///     Gets or sets the map name.
            /// </summary>
            public string MapName { get; set; }

            /// <summary>
            ///     Gets or sets the item X pos.
            /// </summary>
            public int ItemXPos { get; set; }

            /// <summary>
            ///     Gets or sets the item Y pos.
            /// </summary>
            public int ItemYPos { get; set; }

            /// <summary>
            ///     Gets or sets the completion message.
            /// </summary>
            public string CompletionMessage { get; set; }
        }

        #endregion

        #region Nested type: MonsterObjective

        /// <summary>
        ///     An objective which involves killing something.
        /// </summary>
        public class MonsterObjective : Objective
        {
            /// <summary>
            ///     Gets or sets the monster name.
            /// </summary>
            public string MonsterName { get; set; }

            /// <summary>
            ///     Gets or sets the amount.
            /// </summary>
            public string Amount { get; set; }
        }

        #endregion

        #region Nested type: Objective

        /// <summary>
        ///     A quest objective.
        /// </summary>
        public abstract class Objective
        {
            /// <summary>
            ///     Gets or sets the objective description.
            /// </summary>
            public string ObjectiveDescription { get; set; }
        }

        #endregion
    }
}