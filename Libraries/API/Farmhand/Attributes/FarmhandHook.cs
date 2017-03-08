namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Acts as the base class for all Farmhand hooks. This serves no purposes other
    ///     than to optimize the installer attribute finding.
    /// </summary>
    public abstract class FarmhandHook : Attribute
    {
    }
}