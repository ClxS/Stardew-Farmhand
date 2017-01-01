namespace InstallerPackager
{
    using System.Collections.Generic;

    internal class Package
    {
        public string Name { get; set; }
        
        public List<string> InclusionFilters { get; set; } = new List<string>();

        public List<string> ExclusionFilters { get; set; } = new List<string>();
    }
}
