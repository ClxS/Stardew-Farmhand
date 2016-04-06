using Farmhand.Events.Arguments;
using Farmhand.Logging;
using System;
using Farmhand;
using StardewValley;

namespace LoggingMod
{
    internal class Logging : Mod
    {
        public ModConfig Configuration { get; set; }

        public override void Entry()
        {
            Configuration = ModConfiguration.Load<ModConfig>(ModSettings.ConfigurationFile);

            if (Configuration != null)
            {
                Log.IsVerbose = Configuration.UseVerboseLogging;
            }

            Farmhand.Events.GameEvents.OnBeforeGameInitialised += OnGameInitialising;
            Farmhand.Events.GameEvents.OnAfterGameInitialised += OnGameInitialised;
            Farmhand.Events.GameEvents.OnBeforeLoadContent += GameEvents_OnBeforeLoadContent;
            Farmhand.Events.GameEvents.OnAfterLoadedContent += GameEvents_OnAfterLoadedContent;
            Farmhand.Events.GameEvents.OnBeforeUpdateTick += GameEvents_OnBeforeUpdateTick;
            Farmhand.Events.GameEvents.OnAfterUpdateTick += GameEvents_OnAfterUpdateTick;
            Farmhand.Events.GraphicsEvents.OnResize += GraphicsEvents_OnResize;
            Farmhand.Events.GraphicsEvents.OnBeforeDraw += GraphicsEvents_OnBeforeDraw;
            Farmhand.Events.GraphicsEvents.OnAfterDraw += GraphicsEvents_OnAfterDraw;
            Farmhand.Events.LocationEvents.OnLocationsChanged += LocationEvents_OnLocationsChanged;
            Farmhand.Events.LocationEvents.OnCurrentLocationChanged += LocationEvents_OnCurrentLocationChanged;
            Farmhand.Events.LocationEvents.OnLocationObjectsChanged += LocationEvents_OnLocationObjectsChanged;
            Farmhand.Events.PlayerEvents.OnBeforePlayerTakesDamage += PlayerEvents_OnBeforePlayerTakesDamage;
            Farmhand.Events.UiEvents.OnAfterIClickableMenuInitialized += UiEvents_OnAfterIClickableMenuInitialized;
            Farmhand.Events.LocationEvents.OnBeforeLocationLoadObjects += LocationEvents_OnBeforeLocationLoadObjects;
            Farmhand.Events.LocationEvents.OnAfterLocationLoadObjects += LocationEvents_OnAfterLocationLoadObjects;
            Farmhand.Events.SaveEvents.OnBeforeSave += SaveEvents_OnBeforeSave;
            Farmhand.Events.SaveEvents.OnAfterSave += SaveEvents_OnAfterSave;
            Farmhand.Events.SaveEvents.OnBeforeLoad += SaveEvents_OnBeforeLoad;
            Farmhand.Events.SaveEvents.OnAfterLoad += SaveEvents_OnAfterLoad;
        }

        private void SaveEvents_OnAfterLoad(object sender, Farmhand.Events.Arguments.SaveEvents.EventArgsOnAfterLoad e)
        {
            Log.Success($"SaveEvents_OnAfterLoad {e.Filename}");
        }

        private void SaveEvents_OnBeforeLoad(object sender, Farmhand.Events.Arguments.SaveEvents.EventArgsOnBeforeLoad e)
        {
            Log.Success($"SaveEvents_OnBeforeLoad {e.Filename}");
        }

        private void SaveEvents_OnAfterSave(object sender, EventArgs e)
        {
            Log.Success($"SaveEvents_OnAfterSave");
        }

        private void SaveEvents_OnBeforeSave(object sender, EventArgs e)
        {
            Log.Success($"SaveEvents_OnBeforeSave");
        }

        private void LocationEvents_OnAfterLocationLoadObjects(object sender, EventArgs e)
        {
            //Log.Info("LocationEvents_OnAfterLocationLoadObjects");
        }

        private void LocationEvents_OnBeforeLocationLoadObjects(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Log.Info("LocationEvents_OnBeforeLocationLoadObjects");
        }

        private void UiEvents_OnAfterIClickableMenuInitialized(object sender, EventArgs e)
        {

        }

        private void PlayerEvents_OnBeforePlayerTakesDamage(object sender, EventArgsOnBeforePlayerTakesDamage e)
        {
            //Log.Info("PlayerEvents_OnBeforePlayerTakesDamage");
        }

        private void LocationEvents_OnLocationObjectsChanged(object sender, EventArgs e)
        {
            //Log.Verbose("LocationEvents_OnLocationObjectsChanged");
        }

        private void LocationEvents_OnCurrentLocationChanged(object sender, EventArgs e)
        {
            //Log.Verbose("LocationEvents_OnCurrentLocationChanged");
        }

        private void LocationEvents_OnLocationsChanged(object sender, EventArgs e)
        {
            //Log.Verbose("LocationEvents_OnLocationsChanged");
        }

        private void GraphicsEvents_OnAfterDraw(object sender, EventArgs e)
        {
            //Log.Error("GraphicsEvents_OnAfterDraw");
        }

        private void GraphicsEvents_OnBeforeDraw(object sender, EventArgs e)
        {
            //Log.Success("GraphicsEvents_OnBeforeDraw");
        }

        private void GraphicsEvents_OnResize(object sender, EventArgs e)
        {
            Log.Verbose("GraphicsEvents_OnResize");
        }

        private void GameEvents_OnAfterUpdateTick(object sender, EventArgs e)
        {
           // Log.Verbose("GameEvents_OnAfterUpdateTick");
        }

        private void GameEvents_OnBeforeUpdateTick(object sender, EventArgs e)
        {
            //Log.Verbose("GameEvents_OnBeforeUpdateTick");
        }

        private void GameEvents_OnAfterLoadedContent(object sender, EventArgs e)
        {
            //Log.Verbose("GameEvents_OnAfterLoadedContent");
        }

        private void GameEvents_OnBeforeLoadContent(object sender, EventArgs e)
        {
            //Log.Verbose("GameEvents_OnBeforeLoadContent");
        }

        private void OnGameInitialising(object sender, EventArgs e)
        {
            //Log.Verbose("OnGameInitialising");
        }

        private void OnGameInitialised(object sender, EventArgs e)
        {
            //Log.Verbose("OnGameInitialised");
        }
    }
}
