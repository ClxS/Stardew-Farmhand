namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Creates a bool specifying whether to use the event return output.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class UseOutputBindAttribute : ParameterBindAttribute
    {
    }
}