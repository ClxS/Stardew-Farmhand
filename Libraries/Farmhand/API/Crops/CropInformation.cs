namespace Farmhand.API.Crops
{
    using System.Collections.Generic;
    
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    ///     Information for a custom crop.
    /// </summary>
    public class CropInformation
    {
        /// <summary>
        ///     Gets or sets the extra harvest chance.
        /// </summary>
        public double ChanceForExtra { get; set; } = 1.0;

        /// <summary>
        ///     Gets the list of possible colors.
        /// </summary>
        public List<Color> Colors { get; } = new List<Color>();

        /// <summary>
        ///     Gets or sets the harvest method.
        /// </summary>
        public int HarvestMethod { get; set; }

        /// <summary>
        ///     Gets crop index ID.
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        ///     Gets or sets the max harvest increase per level.
        /// </summary>
        public int MaxHarvestIncreasePerLevel { get; set; } = 1;

        /// <summary>
        ///     Gets or sets the maximum harvest.
        /// </summary>
        public int MaximumHarvest { get; set; } = 1;

        /// <summary>
        ///     Gets or sets the minimum harvest.
        /// </summary>
        public int MinimumHarvest { get; set; } = 1;

        /// <summary>
        ///     Gets or sets the crop name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the phase days.
        /// </summary>
        public List<int> PhaseDays { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether it's raised from seeds.
        /// </summary>
        public bool RaisedSeeds { get; set; }

        /// <summary>
        ///     Gets or sets the regrow.
        /// </summary>
        public int Regrow { get; set; }

        /// <summary>
        ///     Gets or sets the crop seasons.
        /// </summary>
        public List<string> Seasons { get; set; }

        /// <summary>
        ///     Gets or sets the seed item ID.
        /// </summary>
        public int Seed { get; set; }

        /// <summary>
        ///     Gets or sets the crop texture.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        ///     Gets or sets the yield item ID.
        /// </summary>
        public int Yield { get; set; }

        /// <summary>
        ///     Converts the information into a game compatible string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            // Determine if Min/Max harvest options need to be included
            var includeMinMax = this.MinimumHarvest > 1 || this.MaximumHarvest > 1;

            // Determine if Color options need to be included
            var includeColor = this.Colors.Count > 0;

            // Break Phase Days down into a string
            var phaseDaysString = string.Empty;
            for (var i = 0; i < this.PhaseDays.Count; i++)
            {
                phaseDaysString += this.PhaseDays[i];

                if (i != this.PhaseDays.Count - 1)
                {
                    phaseDaysString += " ";
                }
            }

            // Break Seasons down into a string
            var seasonsString = string.Empty;
            for (var i = 0; i < this.Seasons.Count; i++)
            {
                seasonsString += this.Seasons[i];

                if (i != this.Seasons.Count - 1)
                {
                    seasonsString += " ";
                }
            }

            // Break Min/Max down into a string
            var minMaxString = "false";
            if (includeMinMax)
            {
                minMaxString =
                    $"true {this.MinimumHarvest} {this.MaximumHarvest} {this.MaxHarvestIncreasePerLevel} {this.ChanceForExtra}";
            }

            // Break Colors down into a string
            var colorsString = "false";
            if (includeColor)
            {
                colorsString = "true ";
                for (var i = 0; i < this.Colors.Count; i++)
                {
                    colorsString += $"{this.Colors[i].R} {this.Colors[i].G} {this.Colors[i].B}";

                    if (i != this.Colors.Count - 1)
                    {
                        colorsString += " ";
                    }
                }
            }

            // Put it all together
            return
                $"{phaseDaysString}/{seasonsString}/{this.Id}/{this.Yield}/{this.Regrow}/{this.HarvestMethod}/{minMaxString}/{this.RaisedSeeds}/{colorsString}";
        }
    }
}