using System;

namespace Farmhand.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PendingHookAttribute : Attribute
    {
    }
}
