using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xTile;

namespace Farmhand.API.Locations
{
    public enum MapOverride
    {
        PartialOverride,
        FullOverride
    }

    public class MapInformation
    {
        public Mod Owner { get; set; }

        public Map Map { get; set; }

        public MapOverride Override { get; set; } = MapOverride.PartialOverride;

        public MapInformation(Mod owner, Map map, MapOverride @override = MapOverride.PartialOverride)
        {
            Owner = owner;
            Map = map;
            Override = @override;
        }
    }
}
