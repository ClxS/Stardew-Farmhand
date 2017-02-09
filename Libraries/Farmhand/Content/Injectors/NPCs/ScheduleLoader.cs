namespace Farmhand.Content.Injectors.NPCs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Farmhand.API.NPCs;

    using StardewValley;

    internal class ScheduleLoader : IContentLoader
    {
        public List<string> SchedulesExceptions
            =>
                Directory.GetFiles($"{Game1.content.RootDirectory}\\Characters\\schedules")
                    .Select(file => file?.Replace("Content\\", string.Empty).Replace(".xnb", string.Empty))
                    .ToList();

        #region IContentLoader Members

        public bool HandlesAsset(Type type, string assetName)
        {
            var baseName = assetName.Replace("Characters\\schedules\\", string.Empty);
            return Npc.Npcs.ContainsKey(baseName);
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            var baseName = assetName.Replace("Characters\\schedules\\", string.Empty);
            var schedule = Npc.Npcs[baseName].Item1.Schedules.BuildSchedule();

            return (T)Convert.ChangeType(schedule, typeof(T));
        }

        #endregion
    }
}