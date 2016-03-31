using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Revolution.Events;
using Revolution.Helpers;
using Revolution.Registries.Containers;
using StardewModdingAPI.Events;
using StardewModdingAPI.Inheritance;
using StardewValley;

namespace StardewModdingAPI
{
    public class Compatibility : ICompatibilityLayer
    {
        public override void AttachEvents(Game1 inst)
        {
            Revolution.Events.ControlEvents.OnControllerButtonPressed += Events.ControlEvents.InvokeButtonPressed;
            Revolution.Events.ControlEvents.OnControllerButtonReleased += Events.ControlEvents.InvokeButtonReleased;
            Revolution.Events.ControlEvents.OnControllerTriggerPressed += Events.ControlEvents.InvokeTriggerPressed;
            Revolution.Events.ControlEvents.OnControllerTriggerReleased += Events.ControlEvents.InvokeTriggerReleased;
            Revolution.Events.ControlEvents.OnKeyPressed += Events.ControlEvents.InvokeKeyPressed;
            Revolution.Events.ControlEvents.OnKeyReleased += Events.ControlEvents.InvokeKeyReleased;
            Revolution.Events.ControlEvents.OnKeyboardChanged += Events.ControlEvents.InvokeKeyboardChanged;
            Revolution.Events.ControlEvents.OnMouseChanged += Events.ControlEvents.InvokeMouseChanged;

            Revolution.Events.GameEvents.OnBeforeGameInitialised += Events.GameEvents.InvokeGameLoaded;
            Revolution.Events.GameEvents.OnAfterGameInitialised += Events.GameEvents.InvokeInitialize;
            Revolution.Events.GameEvents.OnAfterLoadedContent += Events.GameEvents.InvokeLoadContent;
            Revolution.Events.GameEvents.OnAfterUpdateTick += Events.GameEvents.InvokeUpdateTick;

            Revolution.Events.GraphicsEvents.OnAfterDraw += Events.GraphicsEvents.InvokeDrawTick;
            Revolution.Events.GraphicsEvents.OnResize += Events.GraphicsEvents.InvokeResize;

            Revolution.Events.LocationEvents.OnLocationsChanged += Events.LocationEvents.InvokeLocationsChanged;
            Revolution.Events.LocationEvents.OnLocationObjectsChanged += Events.LocationEvents.InvokeOnNewLocationObject;
            Revolution.Events.LocationEvents.OnCurrentLocationChanged += Events.LocationEvents.InvokeCurrentLocationChanged;

            Revolution.Events.MenuEvents.OnMenuChanged += Events.MenuEvents.InvokeMenuChanged;

            Revolution.Events.PlayerEvents.OnFarmerChanged += Events.PlayerEvents.InvokeFarmerChanged;
            Revolution.Events.PlayerEvents.OnItemAddedToInventory += Events.PlayerEvents.InvokeInventoryChanged;
            Revolution.Events.PlayerEvents.OnLevelUp += Events.PlayerEvents.InvokeLeveledUp;

            Revolution.Events.TimeEvents.OnAfterTimeChanged += Events.TimeEvents.InvokeTimeOfDayChanged;
            Revolution.Events.TimeEvents.OnAfterDayChanged += Events.TimeEvents.InvokeDayOfMonthChanged;
            Revolution.Events.TimeEvents.OnAfterSeasonChanged += Events.TimeEvents.InvokeSeasonOfYearChanged;
            Revolution.Events.TimeEvents.OnAfterYearChanged += Events.TimeEvents.InvokeYearOfGameChanged;
            
            //Program.gamePtr = new SGame(inst);
        }

        public override bool ContainsOurModType(Type[] assemblyTypes)
        {
            return assemblyTypes.Any(x => x.BaseType == typeof(StardewModdingAPI.Mod));
        }

        public override object LoadMod(Assembly modAssembly, Type[] assemblyTypes, ModManifest manifest)
        {
            StardewModdingAPI.Mod instance = null;
            try
            {
                var type = assemblyTypes.First(x => x.BaseType == typeof(StardewModdingAPI.Mod));
                instance = (StardewModdingAPI.Mod)modAssembly.CreateInstance(type.ToString());
                if (instance != null)
                {
                    instance.PathOnDisk = manifest.ModDirectory;
                    instance.Entry();
                }
            }
            catch (Exception ex)
            {
                Revolution.Logging.Log.Exception("Error in Entry on SMAPI mod", ex);
            }
            return instance;
        }

        public override IEnumerable<Type> GetEventClasses()
        {
            return OwnAssembly
                .GetTypes()
                .Where(t => string.Equals(t.Namespace, "StardewModdingAPI.Events", StringComparison.Ordinal));
        }
    }
}
