using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xTile;
using xTile.Layers;

namespace Farmhand.API.Locations
{
    public enum MapOverride
    {
        SoftMerge,          // When merging, allow other maps to override this maps changes without conflict
        NormalMerge,        // When merging, use regular merging rules
        FullOverride        // When merging, use ONLY this map, ignore other maps
    }

    public class MapInformation
    {
        public Mod Owner { get; set; }

        public Map Map { get; set; }

        public MapOverride Override { get; set; } = MapOverride.NormalMerge;

        public int OffsetX { get; set; } = 0;

        public int OffsetY { get; set; } = 0;

        public MapInformation(Mod owner, Map map, MapOverride @override = MapOverride.NormalMerge)
        {
            Owner = owner;
            Map = map;
            Override = @override;

            CheckWarnings();
        }

        public MapInformation(Mod owner, Map map, int offsetX, int offsetY, MapOverride @override = MapOverride.NormalMerge)
        {
            Owner = owner;
            Map = map;
            Override = @override;
            OffsetX = offsetX;
            OffsetY = offsetY;

            CheckWarnings();
        }

        // Check for warnings about this map
        private void CheckWarnings()
        {
            // Check for a negative offset, and issue a warning
            if (OffsetX < 0 || OffsetY < 0)
            {
                Farmhand.Logging.Log.Warning($"Map information registered with a negative offset by mod {Owner.ModSettings.Name}, I hope {Owner.ModSettings.Author} knows what they're doing...");
            }

            // Check for a map that's too large, and issue a warning
            if (TotalWidth() > 155 || TotalHeight() > 155 || TotalWidth() + OffsetX > 155 || TotalHeight() + OffsetY > 155)
            {
                Farmhand.Logging.Log.Warning($"Map information registered with a size larger than 155 tiles by mod {Owner.ModSettings.Name}, Using this map will probably crash the game!");
            }
        }

        // Get the width for this map
        public int TotalWidth()
        {
            int width = 0;

            foreach(Layer layer in Map.Layers)
            {
                if(layer.LayerWidth > width)
                {
                    width = layer.LayerWidth;
                }
            }

            return width;
        }

        // Get the height for this map
        public int TotalHeight()
        {
            int height = 0;

            foreach (Layer layer in Map.Layers)
            {
                if (layer.LayerHeight > height)
                {
                    height = layer.LayerHeight;
                }
            }

            return height;
        }
    }
}
