using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class UseOutputBindAttribute : ParameterBindAttribute
    {
    }
}
