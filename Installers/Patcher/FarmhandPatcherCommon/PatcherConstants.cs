namespace Farmhand.Installers.Patcher
{
    using System.IO;
    using System.Reflection;

    /// <summary>
    ///     Constants for the Patch tool.
    /// </summary>
    public static class PatcherConstants
    {
        /// <summary>
        ///     The games executable name.
        /// </summary>
        public static string StardewExe => "Stardew Valley.exe";

        /// <summary>
        ///     The default farmhand executable path.
        /// </summary>
        public static string FarmhandExe => $@"{CurrentAssemblyDirectory}..\..\WorkingDirectory\Stardew Farmhand.exe";

        /// <summary>
        ///     The farmhand DLL name.
        /// </summary>
        public static string FarmhandDll => "Farmhand.dll";

        /// <summary>
        ///     The FarmhandUI DLL name.
        /// </summary>
        public static string FarmhandUiDll => "FarmhandUI.dll";

        /// <summary>
        ///     The FarmhandGame DLL name.
        /// </summary>
        public static string FarmhandGameDll => "FarmhandGame.dll";

        /// <summary>
        ///     The FarmhandCharacter DLL name.
        /// </summary>
        public static string FarmhandCharacterDll => "FarmhandCharacter.dll";

        /// <summary>
        ///     The first-pass package result assembly name.
        /// </summary>
        public static string PassOnePackageResult => "Stardew Farmhand.int1-package.dll";

        /// <summary>
        ///     The second-pass package result assembly name.
        /// </summary>
        public static string PassTwoPackageResult => "Stardew Farmhand.dll";

        /// <summary>
        ///     The first-pass patching result assembly name.
        /// </summary>
        public static string PassOneFarmhandExe => "Stardew Farmhand.int1.dll";

        /// <summary>
        ///     The second-pass patching result assembly name.
        /// </summary>
        public static string PassTwoFarmhandExe => FarmhandExe;

        /// <summary>
        ///     The JSON library assembly name.
        /// </summary>
        public static string JsonLibrary => "Newtonsoft.Json.dll";

        /// <summary>
        ///     The Mono.Cecil library assembly name.
        /// </summary>
        public static string MonoCecilLibrary => "Mono.Cecil.dll";

        /// <summary>
        ///     The Mono.Cecil.Rocks library assembly name.
        /// </summary>
        public static string MonoCecilRocksLibrary => "Mono.Cecil.Rocks.dll";

        /// <summary>
        ///     Gets the current executing assembly location.
        /// </summary>
        public static string CurrentAssemblyPath => Assembly.GetExecutingAssembly().Location;

        /// <summary>
        ///     Gets the current executing assembly directory.
        /// </summary>
        public static string CurrentAssemblyDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}