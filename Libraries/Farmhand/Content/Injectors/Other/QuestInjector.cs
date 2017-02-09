namespace Farmhand.Content.Injectors.Other
{
    using System;
    using System.Collections.Generic;

    using Farmhand.API.Player;

    internal class QuestInjector : IContentInjector
    {
        #region IContentInjector Members

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\Quests";
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var quests = obj as Dictionary<int, string>;
            if (quests == null)
            {
                throw new Exception($"Unexpected type for {assetName}");
            }

            foreach (var quest in Quest.Quests)
            {
                quests[quest.Value.Id] = quest.Value.ToString();
            }
        }

        #endregion
    }
}