namespace Farmhand
{
    using System;
    using System.IO;
    using System.Reflection;

    using Farmhand.Helpers;
    using Farmhand.Helpers.Randomizer;

    /// <summary>
    ///     Various useful constants used by the API
    /// </summary>
    public static class Constants
    {
        /// <summary>
        ///     Gets the default mod path.
        /// </summary>
        public static string DefaultModPath { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                                                       + "\\Mods";

        /// <summary>
        ///     Gets an instance of <see cref="Randomizer" />.
        /// </summary>
        public static Randomizer Randomizer { get; } = new Randomizer();

        /// <summary>
        ///     Gets the directory name under which mods store their mod specific content
        /// </summary>
        public static string ModContentDirectory => "Content";

        /// <summary>
        ///     Gets the extensions directory.
        /// </summary>
        public static string ExtensionsDirectory => "Extensions";

        /// <summary>
        ///     Gets the current Farmhand version.
        /// </summary>
        public static Version Version { get; } = new Version(0, 1, 0, 1);

        /// <summary>
        ///     Gets the current executing assembly.
        /// </summary>
        public static Assembly Assembly => Assembly.GetExecutingAssembly();

        /// <summary>
        ///     Gets whether we are overriding the game's draw method.
        /// </summary>
        public static bool OverrideGameDraw => true;
    }
}