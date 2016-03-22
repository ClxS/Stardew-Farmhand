using Revolution.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution
{
    public static class Test
    {
        public static bool Test2()
        {
            Random rnd = new Random();
            int month = rnd.Next(1, 13); // creates a number between 1 and 12
            return month > 6;
        }

        public static void Test3()
        {
            Console.WriteLine("Test 1");
            if (Test2())
            {
                Console.WriteLine("Test 2");
                return;
            }
            Console.WriteLine("Test 3");            
            return;
        }
    }
}
