using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution
{
    public class Test
    {
        public virtual void Test1()
        {
        }
        public void Test2()
        {
        }
        public void Test3()
        {
            Test1();
            Test2();
        }
    }
}
