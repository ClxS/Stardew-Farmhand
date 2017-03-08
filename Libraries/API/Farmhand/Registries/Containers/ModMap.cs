namespace Farmhand.Registries.Containers
{
    using Newtonsoft.Json;

    using xTile;
    using xTile.Format;

    /// <summary>
    ///     Defines a mods custom map information.
    /// </summary>
    public class ModMap
    {
        [JsonIgnore]
        private Map map;

        /// <summary>
        ///     Gets or sets the file for this map.
        /// </summary>
        public string File { get; set; }

        /// <summary>
        ///     Gets or sets the ID for this map.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the absolute file path.
        /// </summary>
        [JsonIgnore]
        public string AbsoluteFilePath { get; set; }

        /// <summary>
        ///     Gets the map.
        /// </summary>
        [JsonIgnore]
        public Map Map
        {
            get
            {
                if (this.map == null && this.Exists())
                {
                    this.map = FormatManager.Instance.LoadMap(this.AbsoluteFilePath);
                }

                return this.map;
            }

            internal set
            {
                this.map = value;
            }
        }

        /// <summary>
        ///     Gets whether this map exists.
        /// </summary>
        /// <returns>
        ///     Whether the map exists.
        /// </returns>
        public bool Exists()
        {
            return !string.IsNullOrEmpty(this.AbsoluteFilePath) && System.IO.File.Exists(this.AbsoluteFilePath);
        }
    }
}