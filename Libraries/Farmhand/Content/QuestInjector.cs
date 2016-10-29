using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Content
{
    public class QuestInjector : IContentInjector
    {
        public bool IsLoader => false;
        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\Quests";
        }
        
        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Logging.Log.Error("You shouldn't be here!");
            return default(T);
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var quests = obj as Dictionary<int, string>;
            if (quests == null)
                throw new Exception($"Unexpected type for {assetName}");

            foreach (var quest in Farmhand.API.Player.Quest.Quests)
            {
                quests[quest.Value.Id] = quest.Value.ToString();
            }
        }
    }
}
