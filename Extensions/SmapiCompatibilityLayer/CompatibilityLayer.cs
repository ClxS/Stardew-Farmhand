using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Farmhand.Registries.Containers;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace SmapiCompatibilityLayer
{
    class CompatibilityLayer : Farmhand.Helpers.CompatibilityLayer
    {
        public override string ModSubdirectory
        {
            get { return "SMAPI"; }
        }

        public override void Initialise()
        {
            Game1.graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }
        
        public override void LoadMods(string modsDirectory)
        {
            var projectType = typeof(StardewModdingAPI.Program);
            var smapiModsDirectory = Path.Combine(modsDirectory, "SMAPI");
            FieldInfo modPathField = projectType.GetField("ModPath", BindingFlags.Instance | BindingFlags.NonPublic);
            modPathField?.SetValue(null, smapiModsDirectory);

            MethodInfo loadModsMethod = projectType.GetMethod("LoadMods", BindingFlags.Instance | BindingFlags.NonPublic);
            loadModsMethod.Invoke(null, new object[] {});
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
