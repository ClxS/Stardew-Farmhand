namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Binds to the 'this' during hooking.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ThisBindAttribute : ParameterBindAttribute
    {
    }
}