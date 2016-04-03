using Revolution.Events.Arguments;
using Revolution.Logging;
using System;
using Revolution;
using StardewValley;

namespace LoggingMod
{
    internal class Logging : Mod
    {
        public ModConfig Configuration { get; set; }

        public override void Entry()
        {
            //Configuration = ModConfiguration.LoadConfig<ModConfig>(ModSettings.ConfigurationFile);

            //if (Configuration != null)
            //{
            //    Log.IsVerbose = Configuration.UseVerboseLogging;
            //}

            Revolution.Events.GameEvents.OnBeforeGameInitialised += OnGameInitialising;
            Revolution.Events.GameEvents.OnAfterGameInitialised += OnGameInitialised;
            Revolution.Events.GameEvents.OnBeforeLoadContent += GameEvents_OnBeforeLoadContent;
            Revolution.Events.GameEvents.OnAfterLoadedContent += GameEvents_OnAfterLoadedContent;
            Revolution.Events.GameEvents.OnBeforeUpdateTick += GameEvents_OnBeforeUpdateTick;
            Revolution.Events.GameEvents.OnAfterUpdateTick += GameEvents_OnAfterUpdateTick;
            Revolution.Events.GraphicsEvents.OnResize += GraphicsEvents_OnResize;
            Revolution.Events.GraphicsEvents.OnBeforeDraw += GraphicsEvents_OnBeforeDraw;
            Revolution.Events.GraphicsEvents.OnAfterDraw += GraphicsEvents_OnAfterDraw;
            Revolution.Events.LocationEvents.OnLocationsChanged += LocationEvents_OnLocationsChanged;
            Revolution.Events.LocationEvents.OnCurrentLocationChanged += LocationEvents_OnCurrentLocationChanged;
            Revolution.Events.LocationEvents.OnLocationObjectsChanged += LocationEvents_OnLocationObjectsChanged;
            Revolution.Events.PlayerEvents.OnBeforePlayerTakesDamage += PlayerEvents_OnBeforePlayerTakesDamage;
            Revolution.Events.UiEvents.OnAfterIClickableMenuInitialized += UiEvents_OnAfterIClickableMenuInitialized;

            Log.Info("test");
        }

        private void UiEvents_OnAfterIClickableMenuInitialized(object sender, EventArgs e)
        {
            Game1.player.gainExperience(1, 2000);
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
            //Log.Verbose("GraphicsEvents_OnAfterDraw");
        }

        private void GraphicsEvents_OnBeforeDraw(object sender, EventArgs e)
        {
            //Log.Verbose("GraphicsEvents_OnBeforeDraw");
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
