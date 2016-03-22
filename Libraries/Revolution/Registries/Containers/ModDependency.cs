using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Registries.Containers
{
    public enum DependencyState
    {
        OK,
        Missing,
        ParentMissing,
        TooLowVersion,
        TooHighVersion
    }

    public class ModDependency
    {
        public string UniqueId { get; set; }
        public Version MinimumVersion { get; set; }
        public Version MaximumVersion { get; set; }
        public DependencyState DependencyState { get; set; }
    }
}
