using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StardewValley;

namespace Farmhand.API
{
    public class CraftingRecipe
    {
        public int Id { get; set; }
        public int PrivateId { get; private set; } = -1;
        public bool IsAdded => PrivateId != -1;
        public bool IsUnlocked => false; //TODO

        public string Name { get; set; } = string.Empty;
        public SerializableDictionary<string, int> RequiredSkills = new SerializableDictionary<string, int>(); 
        public RecipeUnlockType RecipeUnlockType = RecipeUnlockType.SkillBased;
    }
}
