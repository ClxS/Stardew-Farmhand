using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Farmhand.API.NPCs
{
    public class NPCSchedulesInformation
    {
        public List<SchedulePathInformation> PathInformation { get; set; }

        public Dictionary<string, string> BuildSchedule()
        {
            var ret = new Dictionary<string, string>();

            PathInformation.ForEach(path => ret.Add(path.ScheduleId, path.BuildDirections()));
            return ret;
        }
    }

    public class SchedulePathInformation
    {
        public string ScheduleId { get; set; }

        public List<ScheduleDirections> Directions { get; set; }

        public string BuildDirections()
        {
            return string.Join("/", Directions.Select(_ => _.ToString()));
        }
    }

    public class ScheduleDirections
    {
        public bool NOT { get; set; }
        public ScheduleCondition Condition { get; set; }

        public bool GOTO { get; set; }
        public string NewScheduleId { get; set; }

        public int TimeOfDay { get; set; } = -1;
        public string MapName { get; set; }
        public int endX { get; set; }
        public int endY { get; set; }

        public int facingDirection { get; set; }

        public string endBehavior { get; set; } = null;
        public string endMessage { get; set; } = null;

        public override string ToString()
        {
            if (NOT)
                return Condition.ToString();

            if (GOTO)
                return $"GOTO {NewScheduleId}";

            var msg = endMessage != null || endBehavior != null;
            var message = msg ? endMessage != null ? $"\\\" {endMessage}\\\"" : (endBehavior != null ? $" {endBehavior}" : "") : "";
            return $"{TimeOfDay} {MapName} {endX} {endY} {facingDirection}{message}";
        }
    }

    public class ScheduleCondition
    {
        public string NPCName { get; set; }
        public int FriendshipLevel { get; set; }
        public List<SchedulePathInformation> PathInformation { get; set; }

        public override string ToString()
        {
            return $"NOT friendship {NPCName} {FriendshipLevel}/{string.Join("", PathInformation.Select(_ => _.BuildDirections()))}";
        }
    }
}