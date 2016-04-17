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
            MaterialsRequired = new List<ItemQuantityPair>()
            {
                new ItemQuantityPair() { ItemId = 390, Count = 1}
            },
            TileSize = new Vector2(4, 2),
            HumanDoor = new Vector2(2, 2),
            MapWarpTo = "SlimeHutch",
            Description = "A place to store all your tools!",
            BlueprintType = BlueprintType.Buildings,
            BuildingToUpdate = "none",
            SourceViewRect = new Vector2(48, 48),
            MaxOccupants = 0,
            ActionBehaviour = null,
            SuitableBuildingLocations = new List<string>() {"Farm"},
            MoneyRequired = 100
        });
    }
}

