namespace DependencyTest
{
    using System;

    using Farmhand;
    using Farmhand.API.Debug;
    using Farmhand.Events;
    using Farmhand.Registries;

    /// <summary>
    /// A dependency test mod.
    /// </summary>
    public class DependencyMod : Mod
    {
        /// <summary>
        /// The mod entry method.
        /// </summary>
        public override void Entry()
        {
            GameEvents.AfterGameInitialised += GameEvents_OnAfterGameInitialised;
        }

        private void GameEvents_OnAfterGameInitialised(object sender, Farmhand.Events.Arguments.GameEvents.GameInitialisedEventArgs e)
        {
            var baseFail = ModRegistry.GetItem("DependencyBaseEntryFail");
            var baseSuccess = ModRegistry.GetItem("DependencyBaseSuccess");
            var failLoad = ModRegistry.GetItem("DependencyFailLoad");
            var oldVersion = ModRegistry.GetItem("DependencyOldVersion");
            var success = ModRegistry.GetItem("DependencySuccess");
            var successMaxV = ModRegistry.GetItem("DependencySuccessMaxVersion");
            var successMinV = ModRegistry.GetItem("DependencySuccessMinVersion");
            var successNoRequired = ModRegistry.GetItem("DependencySuccessNotRequired");
            var failTooRecent = ModRegistry.GetItem("DependencyTooRecentVersion");
            var cascadeFail = ModRegistry.GetItem("DependencyCascadeFail");

            System.Diagnostics.Debug.Assert(baseFail.ModState == ModState.Errored, "DependencyBaseEntryFail ModState Invalid");
            System.Diagnostics.Debug.Assert(baseSuccess.ModState == ModState.Loaded, "DependencyBaseSuccess ModState Invalid");
            System.Diagnostics.Debug.Assert(failLoad.ModState == ModState.DependencyLoadError, "DependencyFailLoad ModState Invalid");
            System.Diagnostics.Debug.Assert(oldVersion.ModState == ModState.MissingDependency, "DependencyOldVersion ModState Invalid");
            System.Diagnostics.Debug.Assert(success.ModState == ModState.Loaded, "DependencySuccess ModState Invalid");
            System.Diagnostics.Debug.Assert(successMaxV.ModState == ModState.Loaded, "DependencySuccessMaxVersion ModState Invalid");
            System.Diagnostics.Debug.Assert(successMinV.ModState == ModState.Loaded, "DependencySuccessMinVersion ModState Invalid");
            System.Diagnostics.Debug.Assert(successNoRequired.ModState == ModState.Loaded, "DependencySuccessNotRequired ModState Invalid");
            System.Diagnostics.Debug.Assert(failTooRecent.ModState == ModState.MissingDependency, "DependencyTooRecentVersion ModState Invalid");
            System.Diagnostics.Debug.Assert(cascadeFail.ModState == ModState.DependencyLoadError, "DependencyCascadeFail ModState Invalid");
        }
    }
}