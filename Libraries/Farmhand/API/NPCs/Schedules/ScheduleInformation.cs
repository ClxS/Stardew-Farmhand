using System.Collections.Generic;

namespace Farmhand.API.NPCs.Schedules
{
    public class ScheduleInformation
    {
        public List<SchedulePathInformation> PathInformation { get; set; }

        public Dictionary<string, string> BuildSchedule()
        {
            var ret = new Dictionary<string, string>();

            PathInformation.ForEach(path => ret.Add(path.ScheduleId, path.BuildDirections()));
            return ret;
        }
    }
}
