namespace Farmhand.API.NPCs.Schedules
{
    using Farmhand.API.NPCs.Characteristics;

    /// <summary>
    ///     The schedule directions.
    /// </summary>
    public class ScheduleDirections
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ScheduleDirections" /> class.
        /// </summary>
        /// <param name="useFriendshipCondition">
        ///     Whether the condition is used.
        /// </param>
        /// <param name="condition">
        ///     The condition to use.
        /// </param>
        public ScheduleDirections(bool useFriendshipCondition, ScheduleCondition condition)
        {
            this.UseFriendshipCondition = useFriendshipCondition;
            this.Condition = condition;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScheduleDirections" /> class.
        /// </summary>
        /// <param name="useGoto">
        ///     Whether the schedule should goto another schedule.
        /// </param>
        /// <param name="newScheduleId">
        ///     The new schedule id.
        /// </param>
        public ScheduleDirections(bool useGoto, string newScheduleId)
        {
            this.UseGoto = useGoto;
            this.NewScheduleId = newScheduleId;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScheduleDirections" /> class.
        /// </summary>
        /// <param name="timeOfDay">
        ///     The time of day for this schedule to execute.
        /// </param>
        /// <param name="mapName">
        ///     The map name where this schedule should execute
        /// </param>
        /// <param name="posX">
        ///     The end X position.
        /// </param>
        /// <param name="posY">
        ///     The end Y position.
        /// </param>
        /// <param name="facing">
        ///     The final facing direction of the schedule.
        /// </param>
        /// <param name="end">
        ///     The message of behaviour to execute at the end of the schedule..
        /// </param>
        /// <param name="behavior">
        ///     Whether parameter end prefers to a behaviour, or a message.
        /// </param>
        public ScheduleDirections(
            int timeOfDay,
            string mapName,
            int posX,
            int posY,
            Direction facing,
            string end = null,
            bool behavior = false)
        {
            this.FacingDirection = facing;
            this.TimeOfDay = timeOfDay;
            this.MapName = mapName;
            this.EndX = posX;
            this.EndY = posY;

            if (end == null)
            {
                return;
            }

            if (behavior)
            {
                this.EndBehavior = end;
            }
            else
            {
                this.EndMessage = end;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether not.
        /// </summary>
        public bool UseFriendshipCondition { get; set; }

        /// <summary>
        ///     Gets or sets the condition.
        /// </summary>
        public ScheduleCondition Condition { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this direction goes to another schedule.
        /// </summary>
        public bool UseGoto { get; set; }

        /// <summary>
        ///     Gets or sets the new schedule id.
        /// </summary>
        public string NewScheduleId { get; set; }

        /// <summary>
        ///     Gets or sets the time of day.
        /// </summary>
        public int TimeOfDay { get; set; } = -1;

        /// <summary>
        ///     Gets or sets the map name.
        /// </summary>
        public string MapName { get; set; }

        /// <summary>
        ///     Gets or sets the end X.
        /// </summary>
        public int EndX { get; set; }

        /// <summary>
        ///     Gets or sets the end Y.
        /// </summary>
        public int EndY { get; set; }

        /// <summary>
        ///     Gets or sets the facing direction.
        /// </summary>
        public Direction FacingDirection { get; set; }

        /// <summary>
        ///     Gets or sets the end behavior.
        /// </summary>
        public string EndBehavior { get; set; }

        /// <summary>
        ///     Gets or sets the end message.
        /// </summary>
        public string EndMessage { get; set; }

        /// <summary>
        ///     Constructs the schedule direction as a game compatible string.
        /// </summary>
        /// <returns>
        ///     The direction as a string.
        /// </returns>
        public string BuildScheduleDirection()
        {
            if (this.UseFriendshipCondition)
            {
                return this.Condition.BuildCondition();
            }

            if (this.UseGoto)
            {
                return $"GOTO {this.NewScheduleId}";
            }

            var msg = this.EndMessage != null || this.EndBehavior != null;
            var message = msg
                              ? this.EndMessage != null
                                    ? $"\\\" {this.EndMessage}\\\""
                                    : (this.EndBehavior != null ? $" {this.EndBehavior}" : string.Empty)
                              : string.Empty;
            return $"{this.TimeOfDay} {this.MapName} {this.EndX} {this.EndY} {(int)this.FacingDirection}{message}";
        }
    }
}