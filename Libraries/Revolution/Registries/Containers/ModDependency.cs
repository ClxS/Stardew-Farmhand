using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Registries.Containers
{
    public class ModDependency
    {
        public string UniqueModId { get; set; }
        public Version MinimumVersion { get; set; }
        public Version MaximumVersion { get; set; }
    }
}
