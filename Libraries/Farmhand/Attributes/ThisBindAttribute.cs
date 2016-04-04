using System;

namespace Farmhand.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ThisBindAttribute : ParameterBindAttribute
    {
    }
}
