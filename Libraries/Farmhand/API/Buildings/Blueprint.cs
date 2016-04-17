using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Farmhand.Attributes;
using StardewValley;
using StardewValley.Menus;

namespace Farmhand.API.Buildings
{
    public static class Blueprint
    {
        public static List<IBlueprint> Blueprints = new List<IBlueprint>();

        [Hook(HookType.Exit, "StardewValley.Menus.CarpenterMenu", ".ctor")]
        internal static void ConstructorMenuCreated([ThisBind] object @this)
        {
            var bpField = typeof(CarpenterMenu).GetField("blueprints", BindingFlags.NonPublic |
                         BindingFlags.Instance);
            if (bpField != null)
            {
                Logging.Log.Success("Injecting custom BluePrints");
                var blueprints = (List<StardewValley.BluePrint>)bpField.GetValue(@this);
                blueprints.AddRange(Blueprints.Where(n => n.IsCarpenterBlueprint).Select(n => new BluePrint(n.Name)));
            }
            else
            {
                Logging.Log.Error("Could not find carpenter blueprint list");
            }
        }

        public static void RegisterBlueprint(IBlueprint blueprint)
        {
            Blueprints.Add(blueprint);
        }
    }
}
