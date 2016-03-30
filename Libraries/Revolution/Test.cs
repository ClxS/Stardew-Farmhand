namespace Revolution
{
    public class Test
    {
        public static bool IsEnabled = false;

        public T Test1<T>(T @in, int y, int width, int height, bool test)
        {
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
    }
}
