using System;
using System.Collections.Generic;
using System.Reflection;
using StardewValley;

namespace Farmhand.Extensibility
{
    public abstract class CompatibilityLayer
    {
        /// <summary>
        /// The current assembly for this extension
        /// </summary>
        public Assembly OwnAssembly { get; set; }

        /// <summary>
        /// The root directory, where the Stardew Farmhand executable is located
        /// </summary>
        public string RootDirectory { get; set; }

        /// <summary>
        /// A property which specifies for a particular mod folder to be ignored. This
        /// could be used for extensions which have their own manifest.json formats 
        /// to stop the default mod loader trying (and failing) to read those files, 
        /// and allows the extension to just handle it instead
        /// </summary>
        public virtual string ModSubdirectory { get; set; }

        /// <summary>
        /// Returns a type used to override the default Game1 class. Only one Farmhand extension can specify
        /// one of these.
        /// </summary>
        public abstract Type GameOverrideClass { get; }

        /// <summary>
        /// Is used by Farmhand to notify the game when the Game1 instance has changed
        /// </summary>
        public abstract Game1 GameInstance { get; set; }

        /// <summary>
        /// Perform initial setup steps for this extension. Called after input argument and configuration handling,
        /// but before any mod-loading functionality.
        /// </summary>
        public abstract void Initialise();
        
        /// <summary>
        /// Instructs the extension to load any mods it is responsible for handling.
        /// </summary>
        /// <param name="modsDirectory">The primary mod directory. This is the directory which
        /// should contain a "ModSubdirectory" folder, containing mods handled by this extension</param>
        public abstract void LoadMods(string modsDirectory);

        /// <summary>
        /// Returns an IEnumerable containing event wrapper types. This is used to detect events which should
        /// be handled in mod exception situations in EventManager.DetachDelegates
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<Type> GetEventClasses();
    }
}
