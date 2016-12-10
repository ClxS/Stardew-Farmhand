using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Farmhand.Registries.Containers;
using StardewValley;

namespace SmapiCompatibilityLayer
{
    class CompatibilityLayer : Farmhand.Helpers.CompatibilityLayer
    {
        public override void AttachEvents(Game1 inst)
        {
        }

        public override bool ContainsOurModType(Type[] assemblyTypes)
        {
            return assemblyTypes.Any(n => n == typeof(StardewModdingAPI.Mod));
        }

        public override void LoadMods()
        {
            
        }

        public override IEnumerable<Type> GetEventClasses()
        {
            return Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => string.Equals(t.Namespace, "StardewModdingAPI.Events", StringComparison.Ordinal))
                    .ToList();
        }
    }
}
