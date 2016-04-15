using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Items
{
    /// <summary>
    /// Contains general item information
    /// </summary>
    public class ItemInformation
    {
        public int Id { get; internal set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemType Type { get; set; }
        public ItemCategory Category { get; set; }
        public int Price { get; set; }
        public string Texture { get; set; }
        public int Editibility { get; set; } = -300;

        public override string ToString()
        {
            return $"{Name}/{Price}/{Editibility}/{Category}/{Description}";
        }
    }
}
