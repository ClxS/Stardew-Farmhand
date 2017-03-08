namespace Farmhand.API.Buildings
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Farmhand.Attributes;
    using Farmhand.Logging;

    using StardewValley;
    using StardewValley.Menus;

    /// <summary>
    ///     Blueprint-related API functionality.
    /// </summary>
    public static class Blueprint
    {
        /// <summary>
        ///     Gets the list of registered blueprints.
        /// </summary>
        public static List<IBlueprint> Blueprints { get; } = new List<IBlueprint>();

        /// <summary>
        ///     Registers a blueprint to the API.
        /// </summary>
        /// <param name="blueprint">
        ///     The blueprint to register.
        /// </param>
        public static void RegisterBlueprint(IBlueprint blueprint)
        {
            Blueprints.Add(blueprint);
        }

        [Hook(HookType.Exit, "StardewValley.Menus.CarpenterMenu", ".ctor")]
        internal static void ConstructorMenuCreated([ThisBind] object @this)
        {
            var blueprintsField = typeof(CarpenterMenu).GetField(
                "blueprints",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (blueprintsField != null)
            {
                Log.Success("Injecting custom BluePrints");
                var blueprints = (List<BluePrint>)blueprintsField.GetValue(@this);
                blueprints.AddRange(Blueprints.Where(n => n.IsCarpenterBlueprint).Select(n => new BluePrint(n.Name)));
            }
            else
            {
                Log.Error("Could not find carpenter blueprint list");
            }
        }
    }
}