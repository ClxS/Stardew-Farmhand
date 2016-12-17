using System.Collections.Generic;
using System.Linq;

namespace Farmhand.API.NPCs.Schedules
{
    public class ScheduleCondition
    {
        public string NpcName { get; set; }
        public int FriendshipLevel { get; set; }
        public List<SchedulePathInformation> PathInformation { get; set; }

        public override string ToString()
        {
            return $"NOT friendship {NpcName} {FriendshipLevel}/{string.Join("", PathInformation.Select(_ => _.BuildDirections()))}";
        }
    }

}
