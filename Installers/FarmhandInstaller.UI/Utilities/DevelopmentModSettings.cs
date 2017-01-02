namespace Farmhand.Installers.Utilities
{
    using System.Text.RegularExpressions;

    internal class DevelopmentModSettings
    {
        public string Name { get; set; } = string.Empty;

        private string nameNoSpace;

        private string nameNoSpaceFrom;

        public string NameNoSpace
        {
            get
            {
                if (this.nameNoSpaceFrom != this.Name)
                {
                    this.nameNoSpaceFrom = this.Name;
                    this.nameNoSpace = Regex.Replace(this.Name, @"[^0-9a-zA-Z]+", string.Empty);
                }

                return this.nameNoSpace;
            }
        }

        public string Author { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool UsesSettingsFile { get; set; } = false;
    }
}
