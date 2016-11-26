namespace Farmhand.API.NPCs.Schedules
{
    public class ScheduleDirections
    {
        public bool Not { get; set; }
        public ScheduleCondition Condition { get; set; }

        public bool Goto { get; set; }
        public string NewScheduleId { get; set; }

        public int TimeOfDay { get; set; } = -1;
        public string MapName { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        
        public int FacingDirection { get; set; }

        public string EndBehavior { get; set; }
        public string EndMessage { get; set; }

        public ScheduleDirections(bool not, ScheduleCondition condition)
        {
            Not = not;
            Condition = condition;
        }
        public ScheduleDirections(bool GOTO, string newScheduleId)
        {
            Goto = GOTO;
            NewScheduleId = newScheduleId;
        }
        public ScheduleDirections(int timeOfDay, string mapName, int posX, int posY, int facing, string end = null, bool behavior = false)
        {
            FacingDirection = facing;
            TimeOfDay = timeOfDay;
            MapName = mapName;
            EndX = posX;
            EndY = posY;

            if (end == null) return;
            if (behavior)
                EndBehavior = end;
            else
                EndMessage = end;
        }

        public override string ToString()
        {
            if (Not)
                return Condition.ToString();

            if (Goto)
                return $"GOTO {NewScheduleId}";

            var msg = EndMessage != null || EndBehavior != null;
            var message = msg ? EndMessage != null ? $"\\\" {EndMessage}\\\"" : (EndBehavior != null ? $" {EndBehavior}" : "") : "";
            return $"{TimeOfDay} {MapName} {EndX} {EndY} {FacingDirection}{message}";
        }
    }
}
