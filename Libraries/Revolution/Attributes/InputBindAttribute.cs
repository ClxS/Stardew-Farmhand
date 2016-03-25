using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Attributes
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
