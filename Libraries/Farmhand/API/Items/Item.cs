using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmhand.API.Utilities;
using Farmhand.Registries;
using Microsoft.Xna.Framework;
using StardewValley;

namespace Farmhand.API.Items
{
    /// <summary>
    /// Provides functions relating to items
    /// </summary>
    public static class Item
    {
        public static List<ItemInformation> Items { get; } = new List<ItemInformation>();
        
        /// <summary>
        /// Registers a new item
        /// </summary>
        /// <param name="item">Information of item to register</param>
        public static void RegisterItem(ItemInformation item)
        {
            if (Game1.objectSpriteSheet == null)
            {
                throw new Exception("objectInformation is null! This likely occurs if you try to register an item before AfterContentLoaded");
            }

            item.Id = IdManager.AssignNewIdSequential(Game1.objectInformation); 
            Items.Add(item);
            TextureUtility.AddSpriteToSpritesheet(ref Game1.objectSpriteSheet, TextureRegistry.GetItem(item.Texture)?.Texture, item.Id, 16, 16);
            Game1.objectInformation[item.Id] = item.ToString();
        }

        
    }
}
