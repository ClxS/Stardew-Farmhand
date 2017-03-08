namespace Farmhand.API.Buildings
{
    using System.Collections.Generic;

    using Farmhand.API.Generic;
    using Farmhand.Helpers;

    using Microsoft.Xna.Framework;

    /// <summary>
    ///     A building blueprint.
    /// </summary>
    public class BuildingBlueprint : IBlueprint
    {
        /// <summary>
        ///     Gets or sets the action behaviour.
        /// </summary>
        public virtual string ActionBehaviour { get; set; } // [14]

        /// <summary>
        ///     Gets or sets the animal door.
        /// </summary>
        public virtual Vector2 AnimalDoor { get; set; } = -Vector2.One; // [5-6]

        /// <summary>
        ///     Gets or sets the blueprint type.
        /// </summary>
        public virtual BlueprintType BlueprintType { get; set; } // [9]

        /// <summary>
        ///     Gets or sets the building to update.
        /// </summary>
        public virtual string BuildingToUpdate { get; set; } // [10]

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        public virtual string Description { get; set; } // [8];

        /// <summary>
        ///     Gets or sets the position of the human door.
        /// </summary>
        public virtual Vector2 HumanDoor { get; set; } = -Vector2.One; // [3-4]

        /// <summary>
        ///     Gets or sets the map warp to.
        /// </summary>
        public virtual string MapWarpTo { get; set; } // [7]

        /// <summary>
        ///     Gets or sets the materials required.
        /// </summary>
        public virtual List<ItemQuantityPair> MaterialsRequired { get; set; } // [0]

        /// <summary>
        ///     Gets or sets the max occupants.
        /// </summary>
        public virtual int MaxOccupants { get; set; } // [13]

        /// <summary>
        ///     Gets or sets the money required.
        /// </summary>
        public virtual int MoneyRequired { get; set; } = -1;

        /// <summary>
        ///     Gets or sets the texture source of the building.
        /// </summary>
        public virtual Vector2 SourceViewRect { get; set; } // [11-12]

        /// <summary>
        ///     Gets or sets the suitable building locations.
        /// </summary>
        public virtual List<string> SuitableBuildingLocations { get; set; } // [15]

        /// <summary>
        ///     Gets or sets the tile size.
        /// </summary>
        public virtual Vector2 TileSize { get; set; } // [1-2]

        #region IBlueprint Members

        /// <summary>
        ///     The blueprint string.
        /// </summary>
        public string BlueprintString
            =>
                $"{this.MaterialsRequired.ToItemSetString()}/{this.TileSize.X}/{this.TileSize.Y}/{this.HumanDoor.X}/{this.HumanDoor.Y}/{this.AnimalDoor.X}/{this.AnimalDoor.Y}/{this.MapWarpTo}/"
                + $"{this.Description}/{this.BlueprintType}/{this.BuildingToUpdate}/{this.SourceViewRect.X}/{this.SourceViewRect.Y}/{this.MaxOccupants}/{this.ActionBehaviour ?? "null"}/"
                + $"{this.SuitableBuildingLocations.ToSpaceSeparatedString()}"
                + (this.MoneyRequired >= 0 ? $"/{this.MoneyRequired}" : string.Empty);

        /// <summary>
        ///     The is carpenter blueprint.
        /// </summary>
        public virtual bool IsCarpenterBlueprint => this.MoneyRequired >= 0;

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public virtual string Name { get; set; }

        #endregion
    }
}