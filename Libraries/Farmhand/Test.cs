namespace Farmhand
{
    public class Test
    {
        public static bool IsEnabled = false;

        public Mod Test1(Mod @in, int y, int width, int height, bool test)
        {
            object value;
            if (Test2("", "", out value))
            {
                return (Mod)value;
            }
            return @in;
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
