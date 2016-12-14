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

        // Default constructor for serialization
        protected BigCraftable() :
            base()
        {

        }

        public BigCraftable(BigCraftableInformation information, Vector2 vector, bool isRecipe = false) :
            base(vector, information.Id, isRecipe)
        {
            this.Information = information;
        }
    }
}
