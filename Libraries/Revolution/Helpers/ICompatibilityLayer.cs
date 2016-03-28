using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Revolution.Registries.Containers;

namespace Revolution.Helpers
{
    public abstract class ICompatibilityLayer
    {
        public abstract void AttachEvents();
        public abstract bool ContainsOurModType(Type[] assemblyTypes);
        public abstract object LoadMod(Assembly modAssembly, Type[] assemblyTypes, ModManifest manifest);
    }
}
