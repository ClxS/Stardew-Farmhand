using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmhand.API.Generic;
using Farmhand.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;

namespace Farmhand.API.Buildings
{
    public class BuildingBlueprint : IBlueprint
    {
        public virtual string Name { get; set; }
        public virtual List<ItemQuantityPair> MaterialsRequired { get; set; } //[0]
        public virtual Vector2 TileSize { get; set; } //[1-2]
        public virtual Vector2 HumanDoor { get; set; } = -Vector2.One; //[3-4]
        public virtual Vector2 AnimalDoor { get; set; } = -Vector2.One; //[5-6]
        public virtual string MapWarpTo { get; set; } //[7]
        public virtual string Description { get; set; } //[8];
        public virtual BlueprintType BlueprintType { get; set; } //[9]
        public virtual string BuildingToUpdate { get; set; } //[10]
        public virtual Vector2 SourceViewRect { get; set; } //[11-12]
        public virtual int MaxOccupants { get; set; } //[13]
        public virtual string ActionBehaviour { get; set; } //[14]
        public virtual List<string> SuitableBuildingLocations { get; set; } //[15]
        public virtual int MoneyRequired { get; set; } = -1;

        public string BlueprintString => $"{MaterialsRequired.ToItemSetString()}/{TileSize.X}/{TileSize.Y}/{HumanDoor.X}/{HumanDoor.Y}/{AnimalDoor.X}/{AnimalDoor.Y}/{MapWarpTo}/" +
                                      $"{Description}/{BlueprintType.ToString()}/{BuildingToUpdate}/{SourceViewRect.X}/{SourceViewRect.Y}/{MaxOccupants}/{ActionBehaviour}/" +
                                      $"{SuitableBuildingLocations.ToSpaceSeparatedString()}" + ((MoneyRequired >= 0) ? $"/{MoneyRequired}" : "");
        
        public virtual bool IsCarpenterBlueprint => MoneyRequired >= 0;
    }
}
