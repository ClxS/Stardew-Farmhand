using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmhand.API.Buildings;
using Farmhand.API.Generic;
using Microsoft.Xna.Framework;

namespace TestBuildingMod.Blueprints
{
    public class Shed : StardewValley.Buildings.Building
    {
        private static BuildingBlueprint _information;

        public static BuildingBlueprint Information => _information ?? (_information = new BuildingBlueprint()
        {
            Name = "Shed",
            MaterialsRequired = new List<ItemQuantityPair>(),
            TileSize = new Vector2(3, 3),
            HumanDoor = new Vector2(6, 3),
            MapWarpTo = "Farm",
            Description = "A place to store all your tools!",
            BlueprintType = BlueprintType.Buildings,
            BuildingToUpdate = "none",
            SourceViewRect = new Vector2(384, 320),
            ActionBehaviour = null,
            SuitableBuildingLocations = new List<string>() {"Farm"},
            MoneyRequired = 1000
        });
    }
}
