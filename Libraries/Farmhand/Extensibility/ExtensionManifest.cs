using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmhand.Helpers;
using Newtonsoft.Json;

namespace Farmhand.Extensibility
{
    public class ExtensionManifest
    {
        /// <summary>
        /// A unique identifier for this extension
        /// </summary>
        [JsonConverter(typeof(UniqueIdConverter))]
        public UniqueId<string> UniqueId { get; set; }

        /// <summary>
        /// The display name for this extension
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The author of this extension
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The primary DLL for this extension
        /// </summary>
        public string ExtensionDll { get; set; }

        /// <summary>
        /// The extension's description
        /// </summary>
        public string Descrption { get; set; }

        /// <summary>
        /// If set to true, the extension DLL will not be loaded if there are no
        /// mod manifests located in it's ModsFolder
        /// </summary>
        public bool LoadOnlyIfModsPresent { get; set; }

        /// <summary>
        /// The subdirectory in the mods folder which contains the mods to be handled by this extension
        /// </summary>
        public string ModsFolder { get; set; }

        /// <summary>
        /// Any assemblies which should have their references redirected to this assembly.
        /// </summary>
        public List<string> AssemblyRedirect { get; set; }
    }
}
