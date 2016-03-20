using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class HookForceVirtualBase : Attribute
    {
        public HookForceVirtualBase()
        {
        }    
    }
}
