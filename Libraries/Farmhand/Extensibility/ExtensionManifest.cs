namespace Farmhand.Extensibility
{
    using System.Collections.Generic;

    using Farmhand.Helpers;

    using Newtonsoft.Json;

    /// <summary>
    ///     Contains information collected from the extensions manifest.json file
    /// </summary>
    public class ExtensionManifest
    {
        /// <summary>
        ///     Gets A unique identifier for this extension
        /// </summary>
        [JsonConverter(typeof(UniqueIdConverter))]
        [JsonProperty]
        public UniqueId<string> UniqueId { get; internal set; }

        /// <summary>
        ///     Gets the display name for this extension
        /// </summary>
        [JsonProperty]
        public string Name { get; internal set; }

        /// <summary>
        ///     Gets the author of this extension
        /// </summary>
        [JsonProperty]
        public string Author { get; internal set; }

        /// <summary>
        ///     Gets the primary DLL for this extension
        /// </summary>
        [JsonProperty]
        public string ExtensionDll { get; internal set; }

        /// <summary>
        ///     Gets the extension's description
        /// </summary>
        [JsonProperty]
        public string Description { get; internal set; }

        /// <summary>
        ///     Gets a value indicating whether the extension DLL will not be loaded if there are no
        ///     mod manifests located in it's ModsFolder
        /// </summary>
        [JsonProperty]
        public bool LoadOnlyIfModsPresent { get; internal set; }

        /// <summary>
        ///     Gets the subdirectory in the mods folder which contains the mods to be handled by this extension
        /// </summary>
        [JsonProperty]
        public string ModsFolder { get; internal set; }

        /// <summary>
        ///     Gets any assemblies which should have their references redirected to this assembly.
        /// </summary>
        [JsonProperty]
        public List<string> AssemblyRedirect { get; internal set; }
    }
}