namespace Farmhand.API.Locations
{
    using Farmhand.Logging;

    using xTile;

    /// <summary>
    ///     Detailed information about a custom map.
    /// </summary>
    public class MapInformation
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MapInformation" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The owning mod.
        /// </param>
        /// <param name="map">
        ///     The map.
        /// </param>
        /// <param name="override">
        ///     The map override mode. See <see cref="MapOverride" />
        /// </param>
        public MapInformation(Mod owner, Map map, MapOverride @override = MapOverride.NormalMerge)
        {
            this.Owner = owner;
            this.Map = map;
            this.Override = @override;

            this.CheckWarnings();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MapInformation" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The owning mod.
        /// </param>
        /// <param name="map">
        ///     The map.
        /// </param>
        /// <param name="offsetX">
        ///     The map origin offset x.
        /// </param>
        /// <param name="offsetY">
        ///     The map origin offset y.
        /// </param>
        /// <param name="override">
        ///     The map override mode. See <see cref="MapOverride" />
        /// </param>
        public MapInformation(
            Mod owner,
            Map map,
            int offsetX,
            int offsetY,
            MapOverride @override = MapOverride.NormalMerge)
        {
            this.Owner = owner;
            this.Map = map;
            this.Override = @override;
            this.OffsetX = offsetX;
            this.OffsetY = offsetY;

            this.CheckWarnings();
        }

        /// <summary>
        ///     Gets or sets the map.
        /// </summary>
        public Map Map { get; set; }

        /// <summary>
        ///     Gets or sets the origin offset x.
        /// </summary>
        public int OffsetX { get; set; }

        /// <summary>
        ///     Gets or sets the origin offset y.
        /// </summary>
        public int OffsetY { get; set; }

        /// <summary>
        ///     Gets or sets the map override mode.
        /// </summary>
        public MapOverride Override { get; set; }

        /// <summary>
        ///     Gets or sets the owning mod.
        /// </summary>
        public Mod Owner { get; set; }

        /// <summary>
        ///     Get the height for this map.
        /// </summary>
        /// <returns>
        ///     The total height for this map.
        /// </returns>
        public int TotalHeight()
        {
            var height = 0;

            foreach (var layer in this.Map.Layers)
            {
                if (layer.LayerHeight > height)
                {
                    height = layer.LayerHeight;
                }
            }

            return height;
        }

        /// <summary>
        ///     Get the width for this map.
        /// </summary>
        /// <returns>
        ///     The total width for this map.
        /// </returns>
        public int TotalWidth()
        {
            var width = 0;

            foreach (var layer in this.Map.Layers)
            {
                if (layer.LayerWidth > width)
                {
                    width = layer.LayerWidth;
                }
            }

            return width;
        }

        // Check for warnings about this map
        private void CheckWarnings()
        {
            // Check for a negative offset, and issue a warning
            if (this.OffsetX < 0 || this.OffsetY < 0)
            {
                Log.Warning(
                    $"Map information registered with a negative offset by mod {this.Owner.ModSettings.Name}, I hope {this.Owner.ModSettings.Author} knows what they're doing...");
            }

            // Check for a map that's too large, and issue a warning
            if (this.TotalWidth() > 155 || this.TotalHeight() > 155 || this.TotalWidth() + this.OffsetX > 155
                || this.TotalHeight() + this.OffsetY > 155)
            {
                Log.Warning(
                    $"Map information registered with a size larger than 155 tiles by mod {this.Owner.ModSettings.Name}, Using this map will probably crash the game!");
            }
        }
    }
}