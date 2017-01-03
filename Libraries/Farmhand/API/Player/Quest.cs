namespace Farmhand.API.Player
{
    using System.Collections.Generic;

    using Farmhand.API.Utilities;
    using Farmhand.Logging;

    using StardewValley;

    /// <summary>
    ///     Quest-related API functionality.
    /// </summary>
    public static class Quest
    {
        internal static Dictionary<int, QuestInformation> Quests { get; } = new Dictionary<int, QuestInformation>();

        /// <summary>
        ///     Registers a quest to be inserted into the game.
        /// </summary>
        /// <param name="questInformation">
        ///     The information on the quest to insert.
        /// </param>
        public static void RegisterQuest(QuestInformation questInformation)
        {
            if (Quests.ContainsKey(questInformation.Id) && Quests[questInformation.Id] != questInformation)
            {
                Log.Warning(
                    $"Potential conflict registering new quest. Quest {questInformation.Id} has been registered by two separate mods."
                    + "Only the last registered one will be used.");
            }

            questInformation.Id =
                IdManager.AssignNewIdSequential(Game1.content.Load<Dictionary<int, string>>("Data\\Quests"));
            Quests[questInformation.Id] = questInformation;
        }

        // TODO: Fix up quests from loaded player
    }
}