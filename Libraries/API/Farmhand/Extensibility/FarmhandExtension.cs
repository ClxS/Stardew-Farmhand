namespace Farmhand.Extensibility
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using StardewValley;

    /// <summary>
    ///     The base class for a Farmhand extension, using the Extensibility framework.
    /// </summary>
    public abstract class FarmhandExtension
    {
        /// <summary>
        ///     Gets the current assembly for this extension
        /// </summary>
        public Assembly OwnAssembly { get; internal set; }

        /// <summary>
        ///     Gets the root directory, where the Stardew Farmhand executable is located
        /// </summary>
        public string RootDirectory { get; internal set; }

        /// <summary>
        ///     Gets the information about this extension
        /// </summary>
        public ExtensionManifest Manifest { get; internal set; }

        /// <summary>
        ///     Gets or sets a property which specifies for a particular mod folder to be ignored. This
        ///     could be used for extensions which have their own manifest.json formats
        ///     to stop the default mod loader trying (and failing) to read those files,
        ///     and allows the extension to just handle it instead
        /// </summary>
        public virtual string ModSubdirectory { get; set; }

        /// <summary>
        ///     Gets a type used to override the default Game1 class. Only one Farmhand extension can specify
        ///     one of these.
        /// </summary>
        public abstract Type GameOverrideClass { get; }

        /// <summary>
        ///     Gets or sets the game instance. Is used by Farmhand to notify the game when the Game1 instance has changed
        /// </summary>
        public abstract Game1 GameInstance { get; set; }

        /// <summary>
        ///     Perform initial setup steps for this extension. Called after input argument and configuration handling,
        ///     but before any mod-loading functionality.
        /// </summary>
        public abstract void Initialise();

        /// <summary>
        ///     Instructs the extension to load any mods it is responsible for handling.
        /// </summary>
        /// <param name="modsDirectory">
        ///     The primary mod directory. This is the directory which
        ///     should contain a "ModSubdirectory" folder, containing mods handled by this extension
        /// </param>
        public abstract void LoadMods(string modsDirectory);

        /// <summary>
        ///     Returns an IEnumerable containing event wrapper types. This is used to detect events which should
        ///     be handled in mod exception situations in EventManager.DetachDelegates
        /// </summary>
        /// <returns>
        ///     An <see cref="IEnumerable{Type}" /> of types which contain events to be detached in the event of an exception.
        /// </returns>
        public abstract IEnumerable<Type> GetEventClasses();
    }
}