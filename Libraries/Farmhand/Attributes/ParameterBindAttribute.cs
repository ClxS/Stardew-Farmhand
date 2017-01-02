namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Binds to a parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ParameterBindAttribute : Attribute
    {
    }
}