using System;

namespace Farmhand.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class InputBindAttribute : ParameterBindAttribute
    {
        public InputBindAttribute(Type type, string name, bool @ref = false)
        {
            Type = type;
            Name = name;
            Ref = @ref;
        }

        public Type Type { get; set; }
        public string Name { get; set; }
        public bool Ref { get; set; } = false;
    }
}
