namespace Farmhand.Game.Item
{
    using Farmhand.API.Items;

    using Microsoft.Xna.Framework;

    /// <summary>
    ///     Acts as a base class for BigCraftable objects.
    /// </summary>
    public class BigCraftable : StardewObject
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BigCraftable" /> class.
        /// </summary>
        protected BigCraftable()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BigCraftable" /> class.
        /// </summary>
        /// <param name="information">
        ///     The BigCraftable information.
        /// </param>
        /// <param name="tileLocation">
        ///     The tile location.
        /// </param>
        /// <param name="isRecipe">
        ///     Whether it is instantiated by a recipe.
        /// </param>
        public BigCraftable(BigCraftableInformation information, Vector2 tileLocation, bool isRecipe = false)
            : base(tileLocation, information.Id, isRecipe)
        {
            this.Information = information;
        }

        /// <summary>
        ///     Gets or sets Big Craftable information.
        /// </summary>
        public BigCraftableInformation Information { get; set; }
    }
}