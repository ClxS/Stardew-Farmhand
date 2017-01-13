using Farmhand.Events.Arguments;
using Farmhand.Logging;
using System;
using System.Xml.Serialization;
using Farmhand;
using StardewValley;

namespace LoggingMod
{
    internal class Logging : Mod
    {
        public override void Entry()
        {
            Farmhand.Events.GameEvents.BeforeGameInitialised += OnGameInitialising;
            Farmhand.Events.GameEvents.AfterGameInitialised += OnGameInitialised;
            Farmhand.Events.GameEvents.BeforeLoadContent += GameEvents_OnBeforeLoadContent;
            Farmhand.Events.GameEvents.AfterLoadedContent += GameEvents_OnAfterLoadedContent;
            Farmhand.Events.GameEvents.BeforeUpdateTick += GameEvents_OnBeforeUpdateTick;
            Farmhand.Events.GameEvents.AfterUpdateTick += GameEvents_OnAfterUpdateTick;
            Farmhand.Events.GraphicsEvents.Resize += GraphicsEvents_OnResize;
            Farmhand.Events.GraphicsEvents.BeforeDraw += GraphicsEvents_OnBeforeDraw;
            Farmhand.Events.GraphicsEvents.AfterDraw += GraphicsEvents_OnAfterDraw;
            Farmhand.Events.LocationEvents.LocationsChanged += LocationEvents_OnLocationsChanged;
            Farmhand.Events.LocationEvents.BeforeWarp += LocationEvents_OnBeforeWarp;
            Farmhand.Events.LocationEvents.CurrentLocationChanged += LocationEvents_OnCurrentLocationChanged;
            Farmhand.Events.LocationEvents.LocationObjectsChanged += LocationEvents_OnLocationObjectsChanged;
            Farmhand.Events.PlayerEvents.BeforePlayerTakesDamage += PlayerEvents_OnBeforePlayerTakesDamage;
            Farmhand.Events.UiEvents.AfterIClickableMenuInitialized += UiEvents_OnAfterIClickableMenuInitialized;
            Farmhand.Events.LocationEvents.BeforeLocationLoadObjects += LocationEvents_OnBeforeLocationLoadObjects;
            Farmhand.Events.LocationEvents.AfterLocationLoadObjects += LocationEvents_OnAfterLocationLoadObjects;
            Farmhand.Events.SaveEvents.BeforeSave += SaveEvents_OnBeforeSave;
            Farmhand.Events.SaveEvents.AfterSave += SaveEvents_OnAfterSave;
            Farmhand.Events.SaveEvents.BeforeLoad += SaveEvents_OnBeforeLoad;
            Farmhand.Events.SaveEvents.AfterLoad += SaveEvents_OnAfterLoad;
        }

        private void SerializerEvents_UnreferencedObject(object sender, UnreferencedObjectEventArgs e)
        {
        }

        private void SerializerEvents_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
        }

        private void SerializerEvents_UnknownElement(object sender, XmlElementEventArgs e)
        {
        }

        private void SerializerEvents_UnknownNode(object sender, XmlNodeEventArgs e)
        {
        }

        private void SaveEvents_OnAfterLoad(object sender, Farmhand.Events.Arguments.SaveEvents.EventArgsOnAfterLoad e)
        {
            Log.Success($"SaveEvents_OnAfterLoad");
        }

        private void SaveEvents_OnBeforeLoad(object sender, Farmhand.Events.Arguments.SaveEvents.EventArgsOnBeforeLoad e)
        {
            Log.Success($"SaveEvents_OnBeforeLoad {e.Filename}");
        }

        private void SaveEvents_OnAfterSave(object sender, Farmhand.Events.Arguments.SaveEvents.EventArgsOnAfterSave e)
        {
            Log.Success("SaveEvents_OnAfterSave");
        }

        private void SaveEvents_OnBeforeSave(object sender, EventArgs e)
        {
            Log.Success("SaveEvents_OnBeforeSave");
        }

        private void LocationEvents_OnAfterLocationLoadObjects(object sender, EventArgs e)
        {
            Log.Info("LocationEvents_OnAfterLocationLoadObjects");
        }

        private void LocationEvents_OnBeforeLocationLoadObjects(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.Info("LocationEvents_OnBeforeLocationLoadObjects");
        }

        private void UiEvents_OnAfterIClickableMenuInitialized(object sender, EventArgs e)
        {

        }

        private void PlayerEvents_OnBeforePlayerTakesDamage(object sender, EventArgsOnBeforePlayerTakesDamage e)
        {
            Log.Info("PlayerEvents_OnBeforePlayerTakesDamage");
        }

        private void LocationEvents_OnLocationObjectsChanged(object sender, EventArgs e)
        {
            Log.Verbose("LocationEvents_OnLocationObjectsChanged");
        }

        private void LocationEvents_OnBeforeWarp(object sender, EventArgs e)
        {
            Log.Verbose("LocationEvents_OnBeforeWarp");
        }

        private void LocationEvents_OnCurrentLocationChanged(object sender, EventArgs e)
        {
            Log.Verbose("LocationEvents_OnCurrentLocationChanged");
        }

        private void LocationEvents_OnLocationsChanged(object sender, EventArgs e)
        {
            Log.Verbose("LocationEvents_OnLocationsChanged");
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
            //Log.Verbose("GameEvents_OnAfterUpdateTick");
            //Log.Verbose($"X: {Game1.player.position.X / Game1.tileSize}, Y:  {Game1.player.position.Y / Game1.tileSize}");
        }

        private void GameEvents_OnBeforeUpdateTick(object sender, EventArgs e)
        {
            //Log.Verbose("GameEvents_OnBeforeUpdateTick");
        }

        private void GameEvents_OnAfterLoadedContent(object sender, EventArgs e)
        {
            Log.Verbose("GameEvents_OnAfterLoadedContent");
        }

        private void GameEvents_OnBeforeLoadContent(object sender, EventArgs e)
        {
            Log.Verbose("GameEvents_OnBeforeLoadContent");
        }

        private void OnGameInitialising(object sender, EventArgs e)
        {
            Log.Verbose("OnGameInitialising");
        }

        private void OnGameInitialised(object sender, EventArgs e)
        {
            Log.Verbose("OnGameInitialised");
        }
    }
}
