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
            // TODO this is a primitive patch fix to allow the parsing of categorization for a few items.
            // I wasn't sure why the Category and Type systems were set up the way they were, or if there
            // was a reason for not passing the category number along, but it broke a few things when
            // trying to create seeds.
            string CategoryNumberString = "";
            if (Category == ItemCategory.Seeds)
            {
                CategoryNumberString += "Seeds -74";
            }
            if (Category == ItemCategory.Fruit)
            {
                CategoryNumberString += "Basic -79";
            }

            if (CategoryNumberString != "")
            { 
                return $"{Name}/{Price}/{Editibility}/{CategoryNumberString}/{Description}";
            }
            else
            {
                return $"{Name}/{Price}/{Editibility}/{Category}/{Description}";
            }
        }
    }
}
