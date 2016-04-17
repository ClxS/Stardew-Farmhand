using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StardewValley;

namespace Farmhand.API.Buildings
{
    public interface IBlueprint
    {
        string Name { get; set; }
        string BlueprintString { get; }
        bool IsCarpenterBlueprint { get; }
        
    }
}
