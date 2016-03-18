using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Registries.Containers
{
    public class ModInfo
    {
        public string UniqueModId { get; set; }
        public string Name { get; set; }        
        public string Author { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public List<ModDependency> Dependencies { get; set; }
        public Mod Instance { get; set; }
    }
}
