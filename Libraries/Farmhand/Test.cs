namespace Farmhand
{
    /// <summary>
    /// Used for testing IL generation. Do not use this class since it's entirely useless.
    /// </summary>
    public class Test
    {
        public static bool IsEnabled = false;

        public void Test1(Mod @in, int y, int width, int height, bool test)
        {
            if (Farmhand.Events.GlobalRouteManager.IsEnabled)
            {
                if(Farmhand.Events.GlobalRouteManager.IsBeingListenedTo("Farmhand.Test", "Test"))
                {
                    Farmhand.Events.GlobalRouteManager.GlobalRouteInvoke("Farmhand.Test", "Test");
                }
            }
        }

        public static bool Test2(string type, string method, out object @out, params object[] param)
        {
            @out = null;
            return false;
        }

        public static bool TestFunction()
        {
            return false;
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
