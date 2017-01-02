namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Just marks that the hook for this has yet to be implemented.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PendingHookAttribute : Attribute
    {
    }
}