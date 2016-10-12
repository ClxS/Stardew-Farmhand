using Farmhand.Events;
using StardewValley;

namespace Farmhand
{
    /// <summary>
    /// Used for testing IL generation. Do not use this class since it's entirely useless.
    /// </summary>
    public class Test
    {
        public bool IsEnabled = false;
        protected bool IsEnabled2 = false;

        public string Test1(Mod @in, int y, int width, int height, bool test)
        {
            return IsEnabled ? "sfes" : null;
        }

        public static long TesttttgetNewID()
        {
            if (!GlobalRouteManager.IsEnabled || !GlobalRouteManager.IsBeingPostListenedTo(1557))
                return MultiplayerUtility.latestID++;

            object output = MultiplayerUtility.latestID++;
            GlobalRouteManager.GlobalRoutePostInvoke(1557, "StardewValley.MultiplayerUtility", "getNewID", ref output);
            return (long)output;
        }

        public static bool TestFunction(out object output)
        {
            output = "sfesfse";
            return true;
        }

        public static void VoidFunction(string someTest)
        {

        }

        public static string TestString()
        {
            return "Some test value";
        }
    }
}
