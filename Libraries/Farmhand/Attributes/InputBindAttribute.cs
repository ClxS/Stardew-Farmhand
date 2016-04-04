using System;

namespace Farmhand.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class InputBindAttribute : ParameterBindAttribute
    {
        public InputBindAttribute(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        public Type Type { get; set; }
        public string Name { get; set; }
    }
}
