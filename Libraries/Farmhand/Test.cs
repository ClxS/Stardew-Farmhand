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
            if(IsEnabled)
            {
                return "sfes";
            }
            else
            {
                return null;
            }
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
