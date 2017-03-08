namespace Farmhand.API.NPCs.Schedules
{
    using System.Collections.Generic;

    /// <summary>
    ///     Schedule information.
    /// </summary>
    public class ScheduleInformation
    {
        /// <summary>
        ///     Gets or sets the path information used to construct a schedule.
        /// </summary>
        public List<SchedulePathInformation> PathInformation { get; set; }

        /// <summary>
        ///     Constructs the schedule information as a game compatible string.
        /// </summary>
        /// <returns>
        ///     The schedule as a string.
        /// </returns>
        public Dictionary<string, string> BuildSchedule()
        {
            var ret = new Dictionary<string, string>();

            this.PathInformation.ForEach(path => ret.Add(path.ScheduleId, path.BuildDirections()));
            return ret;
        }
    }
}