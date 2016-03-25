using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ThisBindAttribute : ParameterBindAttribute
    {
    }
}
