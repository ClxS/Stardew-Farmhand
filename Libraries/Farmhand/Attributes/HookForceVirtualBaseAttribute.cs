using System;

namespace Farmhand.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class HookForceVirtualBaseAttribute : Attribute
    {
    }
}
