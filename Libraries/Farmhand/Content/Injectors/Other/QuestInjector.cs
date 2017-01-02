namespace Farmhand.Content.Injectors.Other
{
    using System;
    using System.Collections.Generic;

    using Farmhand.API.Player;
    using Farmhand.Logging;

    internal class QuestInjector : IContentInjector
    {
        #region IContentInjector Members

        public bool IsLoader => false;

        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\Quests";
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Log.Error("You shouldn't be here!");
            return default(T);
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