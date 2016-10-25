using System.Collections.Generic;

namespace Farmhand.API.Player
{
    public class Quest
    {
        public static Dictionary<int, QuestInformation> Quests { get; } = new Dictionary<int, QuestInformation>();

        public static void RegisterQuest(QuestInformation questInformation)
        {
            if (Quests.ContainsKey(questInformation.Id) && Quests[questInformation.Id] != questInformation)
            {
                Logging.Log.Warning($"Potential conflict registering new quest. Quest {questInformation.Id} has been registered by two separate mods." +
                                    "Only the last registered one will be used.");
            }
            Quests[questInformation.Id] = questInformation;
        }
    }
}
