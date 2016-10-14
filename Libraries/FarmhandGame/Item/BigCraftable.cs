using Farmhand.API.Items;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using xTile.Dimensions;

namespace Farmhand.Overrides.Game.Item
{
    public class BigCraftable : StardewObject
    {
        // Big Craftable Information
        public BigCraftableInformation Information { get; set; }

        public BigCraftable(BigCraftableInformation information) :
            base(Vector2.Zero, information.Id, false)
        {
            Information = information;
        }

        public BigCraftable(BigCraftableInformation information, Vector2 vector, int Id, bool isRecipe = false) :
            base(vector, Id, isRecipe)
        {
            Information = information;
        }
    }
}
