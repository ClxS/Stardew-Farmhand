using System;

namespace Farmhand.API.Player
{
    public class QuestInformation
    {
        public int Id { get; set; }

        public enum QuestType
        {
            Building,
            Crafting,
            ItemHarvest,
            ItemDelivery,
            Location,
            LostItem,
            Monster
        }

        public QuestType Type { get; set; }

        public string QuestTitle { get; set; }
        public string QuestDescription { get; set; }

        public Objective QuestObjective { get; set; }
        
        public class Objective
        {
            public string ObjectiveDescription { get; set; }
        }

        public class BuildingObjective : Objective
        {
            public string BuildingName { get; set; }
        }

        public class CraftingObjective : Objective
        {
            public int ItemId { get; set; }
            public bool BigCraftable { get; set; }
        }

        public class ItemDeliveryObjective : Objective
        {
            public string NpcName { get; set; }
            public int ItemId { get; set; }
            public int Amount { get; set; } = 1;

            public string CompletionMessage { get; set; }
        }

        public class ItemHarvestObjective : Objective
        {
            public int ItemId { get; set; }
            public int Amount { get; set; } = 1;
        }

        public class LocationObjective : Objective
        {
            public string MapName { get; set; }
        }

        public class LostItemObjective : Objective
        {
            public string NpcName { get; set; }
            public int ItemId { get; set; }
            public string MapName { get; set; }
            public int ItemXPos { get; set; }
            public int ItemYPos { get; set; }

            public string CompletionMessage { get; set; }
        }

        public class MonsterObjective : Objective
        {
            public string MonsterName { get; set; }
            public string Amount { get; set; }
        }

        public int[] NextQuests { get; set; } = { -1 };

        public int MoneyReward { get; set; } = -1;
        public string RewardDescription { get; set; } = "-1";

        public bool CanBeCancelled { get; set; }

        public string GetObjective()
        {
            switch (Type)
            {
                case QuestType.Building:
                    var buildObj = (BuildingObjective)QuestObjective;
                    return $"{buildObj.BuildingName}";
                case QuestType.Crafting:
                    var craftObj = (CraftingObjective)QuestObjective;
                    return $"{craftObj.ItemId} {craftObj.BigCraftable}";
                case QuestType.ItemHarvest:
                    var harvObj = (ItemHarvestObjective)QuestObjective;
                    return $"{harvObj.ItemId} {harvObj.Amount}";
                case QuestType.ItemDelivery:
                    var delObj = (ItemDeliveryObjective)QuestObjective;
                    return $"{delObj.NpcName} {delObj.ItemId} {delObj.Amount}";
                case QuestType.Location:
                    var locObj = (LocationObjective)QuestObjective;
                    return $"{locObj.MapName}";
                case QuestType.LostItem:
                    var lostObj = (LostItemObjective)QuestObjective;
                    return $"{lostObj.NpcName} {lostObj.ItemId} {lostObj.MapName} {lostObj.ItemXPos} {lostObj.ItemYPos}";
                case QuestType.Monster:
                    var monsterObj = (MonsterObjective)QuestObjective;
                    return $"{monsterObj.MonsterName} {monsterObj.Amount}";
                default:
                    throw new Exception($"Invalid QuestType {Type}");
            }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case QuestType.ItemDelivery:
                case QuestType.LostItem:
                    var objective = QuestObjective as LostItemObjective;
                    var msg = objective != null
                        ? objective.CompletionMessage
                        : ((ItemDeliveryObjective) QuestObjective).CompletionMessage;

                    return $"{Type}/{QuestTitle}/{QuestDescription}/{QuestObjective.ObjectiveDescription}/{GetObjective()}/{string.Join(" ", NextQuests)}/{MoneyReward}/{RewardDescription}/{CanBeCancelled}/{msg}";
            }
            
            return $"{Type}/{QuestTitle}/{QuestDescription}/{QuestObjective.ObjectiveDescription}/{GetObjective()}/{string.Join(" ", NextQuests)}/{MoneyReward}/{RewardDescription}/{CanBeCancelled}";
        }
    }
}
