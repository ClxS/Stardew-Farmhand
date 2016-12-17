using System.Collections.Generic;
using System.Linq;

namespace Farmhand.API.NPCs.Schedules
{
    public class SchedulePathInformation
    {
        public string ScheduleId { get; set; }

        public List<ScheduleDirections> Directions { get; set; }

        public SchedulePathInformation(string scheduleId)
        {
            ScheduleId = scheduleId;
        }

        public string BuildDirections() => string.Join("/", Directions.Select(_ => _.ToString()));
    }
}
