using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Revolution.Helpers
{
    public abstract class ICompatibilityLayer
    {
        public abstract void AttachEvents();
        public abstract bool ContainsOurModType(Type[] assemblyTypes);
        public abstract object LoadMod(Assembly modAssembly, Type[] assemblyTypes);
    }
}
