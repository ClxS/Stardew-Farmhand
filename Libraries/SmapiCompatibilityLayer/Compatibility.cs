﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Farmhand.Helpers;
using Farmhand.Registries.Containers;
using StardewModdingAPI.Inheritance;
using StardewValley;

namespace StardewModdingAPI
{
    public class Compatibility : CompatibilityLayer
    {
        public override Type GameOverrideType => typeof(SGame);
        //public override Type GameOverrideType => null;

        public override void AttachEvents(Game1 inst)
        {
            Farmhand.Events.ControlEvents.OnControllerButtonPressed += Events.ControlEvents.InvokeButtonPressed;
            Farmhand.Events.ControlEvents.OnControllerButtonReleased += Events.ControlEvents.InvokeButtonReleased;
            Farmhand.Events.ControlEvents.OnControllerTriggerPressed += Events.ControlEvents.InvokeTriggerPressed;
            Farmhand.Events.ControlEvents.OnControllerTriggerReleased += Events.ControlEvents.InvokeTriggerReleased;
            Farmhand.Events.ControlEvents.OnKeyPressed += Events.ControlEvents.InvokeKeyPressed;
            Farmhand.Events.ControlEvents.OnKeyReleased += Events.ControlEvents.InvokeKeyReleased;
            Farmhand.Events.ControlEvents.OnKeyboardChanged += Events.ControlEvents.InvokeKeyboardChanged;
            Farmhand.Events.ControlEvents.OnMouseChanged += Events.ControlEvents.InvokeMouseChanged;

            Farmhand.Events.GameEvents.OnBeforeGameInitialised += Events.GameEvents.InvokeGameLoaded;
            Farmhand.Events.GameEvents.OnAfterGameInitialised += Events.GameEvents.InvokeInitialize;
            Farmhand.Events.GameEvents.OnAfterLoadedContent += Events.GameEvents.InvokeLoadContent;
            Farmhand.Events.GameEvents.OnAfterUpdateTick += Events.GameEvents.InvokeUpdateTick;
            
            Farmhand.Events.GraphicsEvents.OnPreRenderEvent += Events.GraphicsEvents.InvokeOnPreRenderEvent;
            Farmhand.Events.GraphicsEvents.OnPreRenderGuiEvent += Events.GraphicsEvents.InvokeOnPreRenderGuiEvent;
            Farmhand.Events.GraphicsEvents.OnPostRenderGuiEvent += Events.GraphicsEvents.InvokeOnPostRenderGuiEvent;
            Farmhand.Events.GraphicsEvents.OnPreRenderHudEvent += Events.GraphicsEvents.InvokeOnPreRenderHudEvent;
            Farmhand.Events.GraphicsEvents.OnPostRenderHudEvent += Events.GraphicsEvents.InvokeOnPostRenderHudEvent;
            Farmhand.Events.GraphicsEvents.OnPostRenderEvent += Events.GraphicsEvents.InvokeOnPostRenderEvent;
            Farmhand.Events.GraphicsEvents.OnPreRenderGuiEventNoCheck += Events.GraphicsEvents.InvokeOnPreRenderGuiEventNoCheck;
            Farmhand.Events.GraphicsEvents.OnPostRenderGuiEventNoCheck += Events.GraphicsEvents.InvokeOnPostRenderGuiEventNoCheck;
            Farmhand.Events.GraphicsEvents.OnPreRenderHudEventNoCheck += Events.GraphicsEvents.InvokeOnPreRenderHudEventNoCheck;
            Farmhand.Events.GraphicsEvents.OnPostRenderHudEventNoCheck += Events.GraphicsEvents.InvokeOnPostRenderHudEventNoCheck;
            Farmhand.Events.GraphicsEvents.OnResize += Events.GraphicsEvents.InvokeResize;
            Farmhand.Events.GraphicsEvents.OnAfterDraw += Events.GraphicsEvents.InvokeDrawTick;
#pragma warning disable CS0618
            Farmhand.Events.GraphicsEvents.OnDrawInRenderTick += Events.GraphicsEvents.InvokeDrawInRenderTargetTick;
#pragma warning restore CS0618

            Farmhand.Events.LocationEvents.OnLocationsChanged += Events.LocationEvents.InvokeLocationsChanged;
            Farmhand.Events.LocationEvents.OnLocationsChanged += Events.PlayerEvents.InvokeLoadedGame;
            Farmhand.Events.LocationEvents.OnLocationObjectsChanged += Events.LocationEvents.InvokeOnNewLocationObject;
            Farmhand.Events.LocationEvents.OnCurrentLocationChanged += Events.LocationEvents.InvokeCurrentLocationChanged;

            Farmhand.Events.MenuEvents.OnMenuChanged += Events.MenuEvents.InvokeMenuChanged;

            Farmhand.Events.PlayerEvents.OnFarmerChanged += Events.PlayerEvents.InvokeFarmerChanged;
            Farmhand.Events.PlayerEvents.OnItemAddedToInventory += Events.PlayerEvents.InvokeInventoryChanged;
            Farmhand.Events.PlayerEvents.OnLevelUp += Events.PlayerEvents.InvokeLeveledUp;

            Farmhand.Events.TimeEvents.OnAfterTimeChanged += Events.TimeEvents.InvokeTimeOfDayChanged;
            Farmhand.Events.TimeEvents.OnAfterDayChanged += Events.TimeEvents.InvokeDayOfMonthChanged;
            Farmhand.Events.TimeEvents.OnAfterSeasonChanged += Events.TimeEvents.InvokeSeasonOfYearChanged;
            Farmhand.Events.TimeEvents.OnAfterYearChanged += Events.TimeEvents.InvokeYearOfGameChanged;
            
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
                    instance.Manifest = new Manifest().InitializeConfig(System.IO.Path.Combine(manifest.ModDirectory, "manifest.json"));
                    instance.PathOnDisk = manifest.ModDirectory;
                    instance.Entry();
                }
            }
            catch (Exception ex)
            {
                Farmhand.Logging.Log.Exception("Error in Entry on SMAPI mod", ex);
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
