namespace Farmhand.API.Buildings
{
    /// <summary>
    ///     Interface for a custom blueprint.
    /// </summary>
    public interface IBlueprint
    {
        /// <summary>
        ///     Gets the blueprint as a game compatible string.
        /// </summary>
        string BlueprintString { get; }

        /// <summary>
        ///     Gets a value indicating whether is this is a carpenter blueprint.
        /// </summary>
        bool IsCarpenterBlueprint { get; }

        /// <summary>
        ///     Gets or sets the blueprint name.
        /// </summary>
        string Name { get; set; }
    }
}