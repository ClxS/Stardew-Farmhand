using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace StardewModdingAPI
{
    internal class ModItem : Object
    {
        public Item AsItem
        {
            get { return this; }
        }

        public override string Name { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
        public Texture2D Texture { get; set; }
    }
}