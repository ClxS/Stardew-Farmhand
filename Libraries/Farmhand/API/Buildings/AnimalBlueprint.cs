namespace Farmhand.API.Buildings
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The animal blueprint.
    /// </summary>
    public class AnimalBlueprint : IBlueprint
    {
        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        public string Description { get; set; } // Animal:[4]

        /// <summary>
        ///     Gets or sets the money required.
        /// </summary>
        public int MoneyRequired { get; set; } // Animal:[1]

        /// <summary>
        ///     Gets or sets the texture source view rect for the blueprint.
        /// </summary>
        public Vector2 SourceViewRect { get; set; } // Animal:[2-3]

        #region IBlueprint Members

        /// <summary>
        ///     Gets the blueprint as a game compatible string.
        /// </summary>
        public string BlueprintString
            =>
                $"animal/{this.MoneyRequired}/{this.SourceViewRect.X}/{this.SourceViewRect.Y}/{this.Description}/null/Farm"
        ;

        /// <summary>
        ///     Gets whether this is a carpenter blueprint. (False)
        /// </summary>
        public bool IsCarpenterBlueprint => false;

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}