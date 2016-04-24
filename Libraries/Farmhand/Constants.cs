using System;
using System.Reflection;

namespace Farmhand
{
    /// <summary>
    /// Various useful constants used by the API
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The directory name under which mods store their mod specific content
        /// </summary>
        public static string ModContentDirectory => "Content";

        public static Version Version { get; } = new Version(0, 1, 0, 1);

        public static Assembly Assembly => Assembly.GetExecutingAssembly();

        public static bool OverrideGameDraw => true;
    }
}
