namespace Farmhand.API.NPCs.Schedules
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Schedule conditions.
    /// </summary>
    public class ScheduleCondition
    {
        /// <summary>
        ///     Gets or sets the NPC name.
        /// </summary>
        public string NpcName { get; set; }

        /// <summary>
        ///     Gets or sets the friendship level.
        /// </summary>
        public int FriendshipLevel { get; set; }

        /// <summary>
        ///     Gets or sets the path information related to this condition.
        /// </summary>
        public List<SchedulePathInformation> PathInformation { get; set; }

        /// <summary>
        ///     Constructs the schedule condition as a game compatible string.
        /// </summary>
        /// <returns>
        ///     The condition as a string.
        /// </returns>
        public string BuildCondition()
        {
            return
                $"NOT friendship {this.NpcName} {this.FriendshipLevel}/{string.Join(string.Empty, this.PathInformation.Select(_ => _.BuildDirections()))}";
        }
    }
}