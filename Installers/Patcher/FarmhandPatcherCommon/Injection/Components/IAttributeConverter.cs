namespace Farmhand.Installers.Patcher.Injection.Components
{
    using System;

    /// <summary>
    ///     The PropertyProvider interface. This is used due to our attribute mismatches that occur
    ///     from using the same attribute from what is technically two different assemblies.
    /// </summary>
    public interface IAttributeConverter
    {
        /// <summary>
        /// Gets the full name of the attribute this converter handles.
        /// </summary>
        string FullName { get; }

        /// <summary>
        ///     Initialise property provider properties from an attribute.
        /// </summary>
        /// <param name="attribute">
        ///     The attribute.
        /// </param>
        void FromAttribute(Attribute attribute);
    }
}