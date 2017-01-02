namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Gets the otherwise outputted variable from a HookReturnable marked exit method
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class MethodOutputBindAttribute : ParameterBindAttribute
    {
    }
}