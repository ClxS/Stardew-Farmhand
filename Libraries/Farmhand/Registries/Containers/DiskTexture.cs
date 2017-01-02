namespace Farmhand.Registries.Containers
{
    using System.IO;

    using Microsoft.Xna.Framework.Graphics;

    using Newtonsoft.Json;

    using StardewValley;

    /// <summary>
    ///     Contains a texture located on disk.
    /// </summary>
    public class DiskTexture
    {
        [JsonIgnore]
        private Texture2D texture;

        /// <summary>
        ///     Gets or sets the file for this texture.
        /// </summary>
        public string File { get; set; }

        /// <summary>
        ///     Gets or sets the ID for this texture.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the absolute file path to this texture.
        /// </summary>
        [JsonIgnore]
        public string AbsoluteFilePath { get; set; }

        /// <summary>
        ///     Gets the <see cref="Texture2D" />.
        /// </summary>
        [JsonIgnore]
        public Texture2D Texture
        {
            get
            {
                if (this.texture == null && this.Exists())
                {
                    this.texture = Texture2D.FromStream(
                        Game1.graphics.GraphicsDevice,
                        new FileStream(this.AbsoluteFilePath, FileMode.Open));
                }

                return this.texture;
            }

            internal set
            {
                this.texture = value;
            }
        }

        /// <summary>
        ///     Gets whether this texture exists on disk.
        /// </summary>
        /// <returns>
        ///     Whether the texture exists.
        /// </returns>
        public bool Exists()
        {
            return !string.IsNullOrEmpty(this.AbsoluteFilePath) && System.IO.File.Exists(this.AbsoluteFilePath);
        }
    }
}