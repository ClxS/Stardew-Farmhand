using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StardewValley;

namespace Farmhand.Content.Injectors.NPCs
{
    class ScheduleLoader : IContentInjector
    {
        public bool IsLoader => true;
        public bool IsInjector => false;

        public List<string> SchedulesExceptions
            => Directory.GetFiles($"{Game1.content.RootDirectory}\\Characters\\schedules")
                .Select(file => file?.Replace("Content\\", "").Replace(".xnb", ""))
                .ToList();

        public bool HandlesAsset(Type type, string assetName)
        {
            var baseName = assetName.Replace("Characters\\schedules\\", "");
            return API.NPCs.Npc.Npcs.ContainsKey(baseName);
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            var baseName = assetName.Replace("Characters\\schedules\\", "");
            var schedule = API.NPCs.Npc.Npcs[baseName].Item1.Schedules.BuildSchedule();

            return (T)Convert.ChangeType(schedule, typeof(T));
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            throw new NotImplementedException();
        }
    }
}
