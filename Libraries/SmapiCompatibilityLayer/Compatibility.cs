using System;
using System.Linq;
using System.Reflection;
using Revolution.Events;
using Revolution.Helpers;
using Revolution.Registries.Containers;
using StardewModdingAPI.Inheritance;
using StardewValley;

namespace StardewModdingAPI
{
    public class Compatibility : ICompatibilityLayer
    {
        public override void AttachEvents()
        {
            GameEvents.OnBeforeGameInitialised += Events.GameEvents.InvokeGameLoaded;
            GameEvents.OnAfterGameInitialised += Events.GameEvents.InvokeInitialize;
            GameEvents.OnAfterLoadedContent += Events.GameEvents.InvokeLoadContent;
            GameEvents.OnAfterUpdateTick += Events.GameEvents.InvokeUpdateTick;

            GraphicsEvents.OnAfterDraw += Events.GraphicsEvents.InvokeDrawTick;
            GraphicsEvents.OnResize += Events.GraphicsEvents.InvokeResize;

            LocationEvents.OnLocationsChanged += Events.LocationEvents.InvokeLocationsChanged;
            LocationEvents.OnLocationObjectsChanged += Events.LocationEvents.InvokeOnNewLocationObject;
            LocationEvents.OnCurrentLocationChanged += Events.LocationEvents.InvokeCurrentLocationChanged;

            MenuEvents.OnMenuChanged += Events.MenuEvents.InvokeMenuChanged;

            PlayerEvents.OnFarmerChanged += Events.PlayerEvents.InvokeFarmerChanged;
            PlayerEvents.OnItemAddedToInventory += Events.PlayerEvents.InvokeInventoryChanged;
            PlayerEvents.OnLevelUp += Events.PlayerEvents.InvokeLeveledUp;

            TimeEvents.OnAfterTimeChanged += Events.TimeEvents.InvokeTimeOfDayChanged;
            TimeEvents.OnAfterDayChanged += Events.TimeEvents.InvokeDayOfMonthChanged;
            TimeEvents.OnAfterSeasonChanged += Events.TimeEvents.InvokeSeasonOfYearChanged;
            TimeEvents.OnAfterYearChanged += Events.TimeEvents.InvokeYearOfGameChanged;
            
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
                instance.PathOnDisk = manifest.ModDirectory;
                instance?.Entry();
            }
            catch (Exception ex)
            {
                Revolution.Logging.Log.Exception("Error in Entry on SMAPI mod", ex);
            }
            return instance;
        }
    }
}
