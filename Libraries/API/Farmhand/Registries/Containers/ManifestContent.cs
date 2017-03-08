namespace Farmhand.Registries.Containers
{
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    /// <summary>
    ///     Defines manifest content.
    /// </summary>
    public class ManifestContent
    {
        /// <summary>
        ///     Gets whether this manifest has content.
        /// </summary>
        public bool HasContent
            =>
                (this.Textures != null && this.Textures.Any()) ||
                (this.Xnb != null && this.Xnb.Any()) ||
                (this.Maps != null && this.Maps.Any());

        /// <summary>
        ///     Gets the textures for this manifest.
        /// </summary>
        [JsonProperty]
        public List<DiskTexture> Textures { get; internal set; }

        /// <summary>
        ///     Gets the XNBs for this manifest.
        /// </summary>
        [JsonProperty]
        public List<ModXnb> Xnb { get; internal set; }

        /// <summary>
        ///     Gets the maps for this manifest.
        /// </summary>
        [JsonProperty]
        public List<ModMap> Maps { get; internal set; }
    }
}