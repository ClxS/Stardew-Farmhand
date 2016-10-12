using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Farmhand.API.Crops
{
    public class CropInformation
    {
        // Crop name
        public string Name { get; set; }

        // Crop Texture
        public Texture2D Texture { get; set; }

        // Crop Phase Days
        public List<int> PhaseDays { get; set; }

        // Crop Seasons
        public List<string> Seasons { get; set; }

        // Crop Index Id
        public int Id { get; set; }

        // Crop Seed Item Id
        public int Seed { get; set; }

        // Crop Yield Item Id
        public int Yield { get; set; }

        // Regrow after harvest
        public int Regrow { get; set; }

        // Harvest Method
        public int HarvestMethod { get; set; }

        // Minimum Harvest
        public int MinimumHarvest { get; set; } = 1;

        // Maximum Harvest
        public int MaximumHarvest { get; set; } = 1;

        // Maximum Harvest Increase Per Farming Level
        public int MaxHarvestIncreasePerLevel { get; set; } = 1;

        // Extra Harvest Chance
        public double ChanceForExtra { get; set; } = 1.0;

        // Raised Seeds
        public bool RaisedSeeds;

        // Possible Colors
        public List<Color> Colors { get; set; } = new List<Color>();


        // ToString Method
        public override string ToString()
        {
            // Determine if Min/Max harvest options need to be included
            bool IncludeMinMax = (MinimumHarvest > 1 || MaximumHarvest > 1);

            // Determine if Color options need to be included
            bool IncludeColor = Colors.Count > 0;

            // Break Phase Days down into a string
            string PhaseDaysString = "";
            for(int i=0; i<PhaseDays.Count; i++)
            {
                PhaseDaysString += PhaseDays[i];

                if(i != PhaseDays.Count-1)
                {
                    PhaseDaysString += " ";
                }
            }

            // Break Seasons down into a string
            string SeasonsString = "";
            for(int i=0; i<Seasons.Count; i++)
            {
                SeasonsString += Seasons[i];

                if(i != Seasons.Count-1)
                {
                    SeasonsString += " ";
                }
            }

            // Break Min/Max down into a string
            string MinMaxString = "false";
            if(IncludeMinMax)
            {
                MinMaxString = $"true {MinimumHarvest} {MaximumHarvest} {MaxHarvestIncreasePerLevel} {ChanceForExtra}";
            }

            // Break Colors down into a string
            string ColorsString = "false";
            if(IncludeColor)
            {
                ColorsString = "true ";
                for(int i=0; i<Colors.Count; i++)
                {
                    ColorsString += $"{Colors[i].R} {Colors[i].G} {Colors[i].B}";

                    if(i != Colors.Count-1)
                    {
                        ColorsString += " ";
                    }
                }
            }

            // Put it all together
            return $"{PhaseDaysString}/{SeasonsString}/{Id}/{Yield}/{Regrow}/{HarvestMethod}/{MinMaxString}/{RaisedSeeds}/{ColorsString}";
        }
    }
}
