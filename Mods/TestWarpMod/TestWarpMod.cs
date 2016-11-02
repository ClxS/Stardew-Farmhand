using Farmhand;
using Farmhand.Events.Arguments.LocationEvents;

namespace TestWarpMod
{
    public class TestWarpMod : Mod
    {
        public static TestWarpMod Instance;

        public override void Entry()
        {
            Instance = this;

            Farmhand.Events.LocationEvents.OnBeforeWarp += LinusPrivacy;
        }

        public static void LinusPrivacy( object sender, EventArgsOnBeforeWarp args )
        {
            if ( args.LocationAfterWarp.Name == "Tent" && ( StardewValley.Game1.timeOfDay < 900 || StardewValley.Game1.timeOfDay >= 2000 ) )
            {
                args.Cancel = true;
                StardewValley.Game1.drawObjectDialogue("Linus is probably asleep right now.");
            }
        }
    }
}
