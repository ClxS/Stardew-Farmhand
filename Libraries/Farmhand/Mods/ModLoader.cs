namespace Farmhand
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Farmhand.Attributes;
    using Farmhand.Events;
    using Farmhand.Events.Arguments;
    using Farmhand.Extensibility;
    using Farmhand.Helpers;
    using Farmhand.Logging;
    using Farmhand.Registries;
    using Farmhand.Registries.Containers;

    using Newtonsoft.Json;

    using StardewValley;

    /// <summary>
    ///     Handles loading mods
    /// </summary>
    public static class ModLoader
    {
        /// <summary>
        ///     Gets the valid mod search directories
        /// </summary>
        public static List<string> ModPaths { get; internal set; } = new List<string> { Constants.DefaultModPath };

        internal static EventManager ModEventManager { get; } = new EventManager();

        [Hook(HookType.Entry, "StardewValley.Game1", ".ctor")]
        internal static void LoadMods()
        {
            Log.Info($"Stardew Valley v{Game1.version}");
            Log.Info($"Stardew Farmhand v{Constants.Version}");

            ApiEvents.OnModError += ApiEvents_OnModError;
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;

            Log.Success("Initializing Mappings");
            GlobalRouteManager.InitialiseMappings();
            Log.Success("Mappings Initialized");

            Log.Info("Loading Mods...");
            try
            {
                Log.Verbose("Loading Farmhand Mods");
                Log.Verbose("Loading Mod Manifests");
                LoadModManifests();
                Log.Verbose("Validating Mod Manifests");
                ValidateModManifests();
                Log.Verbose("Resolving Mod Dependencies");
                ResolveDependencies();
                Log.Verbose("Importing Mod DLLs, Settings, and Content");
                LoadFinalMods();

                var modsPath = ModPaths.First();
                ExtensibilityManager.LoadMods(modsPath);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
            }

            var numModsLoaded = ModRegistry.GetRegisteredItems().Count(n => n.ModState == ModState.Loaded);
            Log.Info($"{numModsLoaded} Mods Loaded!");

            Game1.version += $"Stardew Farmhand v{Constants.Version}: {numModsLoaded} mods loaded";
        }

        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("Stardew Valley"))
            {
                return Assembly.GetExecutingAssembly();
            }

            if (args.Name.StartsWith("StardewModdingAPI"))
            {
                return Assembly.GetExecutingAssembly();
            }

            if (args.Name.StartsWith("Stardew Farmhand.int1"))
            {
                // Problematic injection - never got Assembly corrected from a intermediate assembly
                return Assembly.GetExecutingAssembly();
            }

            if (args.Name.StartsWith("Newtonsoft.Json"))
            {
                return Assembly.GetExecutingAssembly();
            }

            if (args.Name.StartsWith("Mono.Cecil"))
            {
                return Assembly.GetExecutingAssembly();
            }

            foreach (var extension in ExtensibilityManager.Extensions)
            {
                if (extension?.Manifest?.AssemblyRedirect != null
                    && extension.Manifest.AssemblyRedirect.Contains(args.Name))
                {
                    return extension.OwnAssembly;
                }
            }

            return null;
        }

        private static void ApiEvents_OnModError(object sender, EventArgsOnModError e)
        {
            var mod =
                (ModManifest)
                ModRegistry.GetRegisteredItems()
                    .FirstOrDefault(n => n.IsFarmhandMod && ((ModManifest)n).ModAssembly == e.Assembly);
            if (mod != null)
            {
                Log.Exception($"Exception thrown by mod: {mod.Name} - {mod.Author}", e.Exception);
                DeactivateMod(mod, ModState.Errored, e.Exception);
            }
            else
            {
                Log.Exception($"Exception thrown by unknown mod with assembly: {e.Assembly.FullName}", e.Exception);
                DetachAssemblyDelegates(e.Assembly);
            }
        }

        private static void ValidateModManifests()
        {
            var registeredMods = ModRegistry.GetRegisteredItems();
            foreach (var mod in
                registeredMods.Where(n => n.IsFarmhandMod && n.ModState == ModState.Unloaded).Cast<ModManifest>())
            {
                try
                {
                    if (mod.UniqueId == null)
                    {
                        mod.UniqueId = new UniqueId<string>(Guid.NewGuid().ToString());
                    }

                    if (!mod.UniqueId.ThisId.Contains("\\")
                        && (!mod.HasContent || mod.Content.Textures == null
                            || !mod.Content.Textures.Any(n => n.Id.Contains("\\"))))
                    {
                        continue;
                    }

                    Log.Error(
                        $"Error - {mod.Name} by {mod.Author} manifest is invalid. UniqueIDs cannot contain \"\\\"");
                    mod.ModState = ModState.InvalidManifest;
                }
                catch (Exception ex)
                {
                    Log.Error($"Error validating mod {mod.Name} by {mod.Author}\n\t-{ex.Message}");
                }
            }
        }

        private static void LoadFinalMods()
        {
            Func<ModManifest[], List<ModManifest>, List<ModManifest>, List<ModManifest>, ModManifest[]> getModsForThisPass = (mods, modsLoaded, modsErrored, modsProcessed) =>
                    {
                        Func<ModDependency, bool> isDependencyLoaded = dependency =>
                            {
                                if (dependency.IsRequired)
                                {
                                    return modsLoaded.Any(ld => ld.UniqueId.Equals(dependency.UniqueId));
                                }

                                var dependentMod = mods.FirstOrDefault(n => n.UniqueId.Equals(dependency.UniqueId));
                                return dependentMod?.ModState != ModState.Unloaded;
                            };

                        return
                            mods.Where(
                                    n =>
                                        n.ModState == ModState.Unloaded
                                        && (n.Dependencies == null || n.Dependencies.All(d => isDependencyLoaded(d))))
                                .ToArray();
                    };

            var modsToLoad = ModRegistry.GetRegisteredItems().Where(n => n.IsFarmhandMod).Cast<ModManifest>().ToArray();
            var loadedMods = new List<ModManifest>();
            var erroredMods = new List<ModManifest>();
            var processedMods = new List<ModManifest>();

            var modsThisPass = getModsForThisPass(modsToLoad, loadedMods, erroredMods, processedMods);

            while (modsThisPass.Any())
            {
                foreach (var mod in modsThisPass)
                {
                    processedMods.Add(mod);
                    if (mod.ModState == ModState.DependencyLoadError)
                    {
                        erroredMods.Add(mod);
                        continue;
                    }

                    Log.Verbose($"Loading mod: {mod.Name} by {mod.Author}");
                    try
                    {
                        ApiEvents.InvokeModPreLoad(mod);
                        mod.OnBeforeLoaded();
                        if (mod.HasContent)
                        {
                            mod.LoadContent();
                        }

                        if (mod.HasDll)
                        {
                            if (!mod.LoadModDll())
                            {
                                mod.ModState = ModState.Errored;
                                ApiEvents.InvokeModLoadError(mod);
                                erroredMods.Add(mod);
                                BubbleDependencyLoadError(mod);
                                continue;
                            }
                        }

                        mod.ModState = ModState.Loaded;
                        mod.OnAfterLoaded();
                        Log.Success($"Loaded Mod: {mod.Name} v{mod.Version} by {mod.Author}");
                        ApiEvents.InvokeModPostLoad(mod);
                        loadedMods.Add(mod);
                    }
                    catch (Exception ex)
                    {
                        mod.ModState = ModState.Errored;
                        Log.Exception($"Error loading mod {mod.Name} by {mod.Author}", ex);
                        ApiEvents.InvokeModLoadError(mod);
                        erroredMods.Add(mod);
                        BubbleDependencyLoadError(mod);
                        break;
                    }
                }

                modsThisPass = getModsForThisPass(modsToLoad, loadedMods, erroredMods, processedMods);
            }

            // See ReferenceFix.Data.BuildXnaTypeCache()
            // Since mod loading is done we don't need this anymore.
            // There are a lot of types, so might as well save the memory.
            ReferenceHelper.XnaTypes.Clear();
        }

        private static void BubbleDependencyLoadError(ModManifest erroredMod)
        {
            Log.Error($"Marking {erroredMod.Name} dependency hierarchy as failed");

            var problemMods = new Stack<IModManifest>();

            // We use this one to avoid circular resolution issues
            var resolvedMods = new List<IModManifest>();

            problemMods.Push(erroredMod);

            do
            {
                var mod = problemMods.Pop();
                resolvedMods.Add(mod);
                var dependants =
                    ModRegistry.GetRegisteredItems()
                        .Where(n => n.IsFarmhandMod)
                        .Cast<ModManifest>()
                        .Where(
                            n =>
                                n.Dependencies != null
                                && n.Dependencies.Any(d => d.IsRequired && mod.UniqueId.Equals(d.UniqueId)));

                foreach (var dependant in dependants)
                {
                    dependant.ModState = ModState.DependencyLoadError;
                    if (!resolvedMods.Contains(dependant))
                    {
                        Log.Verbose(
                            $"\tDisabling {dependant.Name} due to {mod.Name} failure."
                            + (mod == erroredMod ? string.Empty : $" (Cascaded failure loading {erroredMod.Name}"));
                        problemMods.Push(dependant);
                    }
                }
            }
            while (problemMods.Count > 0);

            Log.Verbose($"{erroredMod.Name} all marked failed");
        }

        private static void ResolveDependencies()
        {
            var registeredMods = ModRegistry.GetRegisteredItems().Where(n => n.IsFarmhandMod).Cast<ModManifest>();

            // Loop to verify every dependent mod is available. 
            bool stateChange;
            var modInfos = registeredMods as ModManifest[] ?? registeredMods.ToArray();
            do
            {
                stateChange = false;
                foreach (var mod in modInfos)
                {
                    if (mod.ModState == ModState.MissingDependency || mod.Dependencies == null)
                    {
                        continue;
                    }

                    foreach (var dependency in mod.Dependencies.Where(n => n.IsRequired))
                    {
                        var dependencyMatch = modInfos.FirstOrDefault(n => n.UniqueId.Equals(dependency.UniqueId));
                        dependency.DependencyState = DependencyState.Ok;
                        if (dependencyMatch == null)
                        {
                            mod.ModState = ModState.MissingDependency;
                            dependency.DependencyState = DependencyState.Missing;
                            stateChange = true;
                            Log.Error($"Failed to load {mod.Name} due to missing dependency: {dependency.UniqueId}");
                        }
                        else if (dependencyMatch.ModState == ModState.MissingDependency)
                        {
                            mod.ModState = ModState.MissingDependency;
                            dependency.DependencyState = DependencyState.ParentMissing;
                            stateChange = true;
                            Log.Error(
                                $"Failed to load {mod.Name} due to missing dependency missing dependency: {dependency.UniqueId}");
                        }
                        else
                        {
                            var dependencyVersion = dependencyMatch.Version;
                            if (dependencyVersion == null)
                            {
                                continue;
                            }

                            if (dependency.MinimumVersion != null && dependency.MinimumVersion > dependencyVersion)
                            {
                                mod.ModState = ModState.MissingDependency;
                                dependency.DependencyState = DependencyState.TooLowVersion;
                                stateChange = true;
                                Log.Error(
                                    $"Failed to load {mod.Name} due to minimum version incompatibility with {dependency.UniqueId}: "
                                    + $"v.{dependencyMatch.Version} < v.{dependency.MinimumVersion}");
                            }
                            else if (dependency.MaximumVersion != null && dependency.MaximumVersion < dependencyVersion)
                            {
                                mod.ModState = ModState.MissingDependency;
                                dependency.DependencyState = DependencyState.TooHighVersion;
                                stateChange = true;
                                Log.Error(
                                    $"Failed to load {mod.Name} due to maximum version incompatibility with {dependency.UniqueId}: "
                                    + $"v.{dependencyMatch.Version} > v.{dependency.MaximumVersion}");
                            }
                        }
                    }
                }
            }
            while (stateChange);
        }

        private static void LoadModManifests()
        {
            foreach (var modPath in ModPaths)
            {
                foreach (var perModPath in Directory.GetDirectories(modPath))
                {
                    var modJsonFiles = Directory.GetFiles(perModPath, "manifest.json");
                    foreach (var file in modJsonFiles)
                    {
                        using (var r = new StreamReader(file))
                        {
                            var json = r.ReadToEnd();
                            var modInfo = JsonConvert.DeserializeObject<ModManifest>(json, new VersionConverter());

                            modInfo.ModDirectory = perModPath;
                            ModRegistry.RegisterItem(
                                modInfo.UniqueId ?? new UniqueId<string>(Guid.NewGuid().ToString()),
                                modInfo);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Forcibly deactivates a mod by detaching it's event listeners.
        /// </summary>
        /// <param name="mod">The mod to deactive</param>
        /// <param name="state">The new state of this mod. Defaults to ModState.Deactivated</param>
        /// <param name="error">The exception encountered causing the mod to be unloaded. Defaults to null</param>
        public static void DeactivateMod(Mod mod, ModState state = ModState.Deactivated, Exception error = null)
        {
            DeactivateMod(mod.ModSettings);
        }

        /// <summary>
        ///     Forcibly deactivates a mod by detaching it's event listeners.
        /// </summary>
        /// <param name="mod">The manifest of the mod to deactive</param>
        /// <param name="state">The new state of this mod. Defaults to ModState.Deactivated</param>
        /// <param name="error">The exception encountered causing the mod to be unloaded. Defaults to null</param>
        public static void DeactivateMod(ModManifest mod, ModState state = ModState.Deactivated, Exception error = null)
        {
            try
            {
                DetachAssemblyDelegates(mod.ModAssembly);
                mod.ModState = state;
                mod.LastException = error;
                Log.Success($"Successfully unloaded mod {mod.Name} by {mod.Author}");
            }
            catch (Exception)
            {
                // Ignored
            }
        }

        /// <summary>
        ///     Forcibly detaches event delegates associated with a particular assembly
        /// </summary>
        /// <param name="assembly">The assembly the detach</param>
        public static void DetachAssemblyDelegates(Assembly assembly)
        {
            if (assembly != null)
            {
                ModEventManager.DetachDelegates(assembly);
            }
        }

        /// <summary>
        ///     Reattaches disabled delegates for previously disabled mods
        /// </summary>
        /// <param name="mod">The mod to reactivate</param>
        public static void ReactivateMod(Mod mod)
        {
            ReactivateMod(mod.ModSettings);
        }

        /// <summary>
        ///     Reattaches disabled delegates for previously disabled mods
        /// </summary>
        /// <param name="mod">The manifest of the mod to reactivate</param>
        public static void ReactivateMod(ModManifest mod)
        {
            if (mod.ModAssembly != null)
            {
                ModEventManager.ReattachDelegates(mod.ModAssembly);
            }
        }

        /// <summary>
        ///     Forces the game to reload mod configurations which use save specific settings.
        /// </summary>
        public static void ReloadConfigurations()
        {
            var mods = ModRegistry.GetRegisteredItems().Where(n => n.IsFarmhandMod).Cast<ModManifest>();
            foreach (var mod in mods)
            {
                var config = mod.Instance?.ConfigurationSettings;
                if (config != null)
                {
                    if (config.UseSaveSpecificConfiguration)
                    {
                        config.Load();
                    }
                }
            }
        }
    }
}