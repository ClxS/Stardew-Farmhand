using System;

namespace Farmhand.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class LocalBindAttribute : ParameterBindAttribute
    {
        public LocalBindAttribute(Type type, int index)
        {
            Type = type;
            Index = index;
        }

        public Type Type { get; set; }
        public int Index { get; set; }
    }
}
