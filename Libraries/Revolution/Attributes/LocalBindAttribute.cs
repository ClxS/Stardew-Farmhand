using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Attributes
{
    public class LocalBindAttribute : ParameterBindAttribute
    {
        public LocalBindAttribute(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        public Type Type { get; set; }
        public string Name { get; set; }
    }
}
