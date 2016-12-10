using System;
using System.Collections.Generic;
using System.Reflection;
using Farmhand.Registries.Containers;
using StardewValley;

namespace Farmhand.Helpers
{
    public abstract class CompatibilityLayer
    {
        public Assembly OwnAssembly { get; set; }
        public virtual Type GameOverrideType { get; set; }
        public string RootDirectory { get; set; }

        public abstract void AttachEvents(Game1 inst);
        public abstract bool ContainsOurModType(Type[] assemblyTypes);
        public abstract void LoadMods();

        public abstract IEnumerable<Type> GetEventClasses();
    }
}
