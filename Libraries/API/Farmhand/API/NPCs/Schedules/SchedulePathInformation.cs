namespace Farmhand.API.NPCs.Schedules
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Schedule path information.
    /// </summary>
    public class SchedulePathInformation
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SchedulePathInformation" /> class.
        /// </summary>
        /// <param name="scheduleId">
        ///     The schedule id.
        /// </param>
        public SchedulePathInformation(string scheduleId)
        {
            this.ScheduleId = scheduleId;
        }

        /// <summary>
        ///     Gets or sets the schedule id.
        /// </summary>
        public string ScheduleId { get; set; }

        /// <summary>
        ///     Gets or sets the list of directions in this schedule directions.
        /// </summary>
        public List<ScheduleDirections> Directions { get; set; }

        /// <summary>
        ///     Constructs the schedule directions as a game compatible string.
        /// </summary>
        /// <returns>
        ///     The directions as a string.
        /// </returns>
        public string BuildDirections() => string.Join("/", this.Directions.Select(_ => _.BuildScheduleDirection()));
    }
}