using System;

namespace Revolution.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class HookForceVirtualBaseAttribute : Attribute
    {
    }
}
