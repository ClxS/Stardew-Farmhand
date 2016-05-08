using Farmhand.Events;
namespace Farmhand
{
    /// <summary>
    /// Used for testing IL generation. Do not use this class since it's entirely useless.
    /// </summary>
    public class Test
    {
        public bool IsEnabled = false;
                
        public string Test1(Mod @in, int y, int width, int height, bool test)
        {
            if(GlobalRouteManager.IsEnabled)
            {
                if(GlobalRouteManager.IsBeingPreListenedTo(0))
                {
                    object output;
                    if(GlobalRouteManager.GlobalRoutePreInvoke(0, "F", "2", out output, @in, y, width, height, test))
                    {
                        return (string)output;
                    }
                }
            }
            return !this.IsEnabled ? "her" : "his";
        }

        public static bool Test2(string type, string method, out object @out, params object[] param)
        {
            @out = null;
            return false;
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
