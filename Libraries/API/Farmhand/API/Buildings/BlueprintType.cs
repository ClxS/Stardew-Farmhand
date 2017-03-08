namespace Farmhand.API.Buildings
{
    /// <summary>
    ///     An enumeration of the blueprint type.
    /// </summary>
    public enum BlueprintType
    {
        /// <summary>
        ///     The blueprint is for a new building.
        /// </summary>
        Buildings,

        /// <summary>
        ///     The blueprint if for an upgrade of a lower tier building.
        /// </summary>
        Upgrades
    }
}