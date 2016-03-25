using Microsoft.Xna.Framework.Graphics;
using StardewValley;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
namespace StardewModdingAPI
{
    class ModItem : Object
    {
        public Item AsItem => this as Item;
        public override string Name { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
        public Texture2D Texture { get; set; }
    }
}
