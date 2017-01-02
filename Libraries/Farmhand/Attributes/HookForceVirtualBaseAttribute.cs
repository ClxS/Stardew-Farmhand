namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Forces methods in base class to be marked virtual.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class HookForceVirtualBaseAttribute : Attribute
    {
    }
}