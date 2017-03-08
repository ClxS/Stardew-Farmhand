namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Gets the otherwise outputted variable from a hook exit method
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class MethodOutputBindAttribute : ParameterBindAttribute
    {
    }
}