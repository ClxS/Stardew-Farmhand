namespace Farmhand.Registries.Containers
{
    using Microsoft.Xna.Framework;

    using Newtonsoft.Json;

    /// <summary>
    ///     Defines a Mods XNB replacement.
    /// </summary>
    public class ModXnb
    {
        /// <summary>
        ///     Gets or sets the original XNB file.
        /// </summary>
        public string Original { get; set; }

        /// <summary>
        ///     Gets or sets the new XNB file.
        /// </summary>
        public string File { get; set; }

        /// <summary>
        ///     Gets or sets the new texture.
        /// </summary>
        public string Texture { get; set; }

        /// <summary>
        ///     Gets or sets the texture source location.
        /// </summary>
        public TextureRect Source { get; set; }

        /// <summary>
        ///     Gets or sets the texture destination location.
        /// </summary>
        public TextureRect Destination { get; set; }

        /// <summary>
        ///     Gets or sets the owning mod.
        /// </summary>
        [JsonIgnore]
        public ModManifest OwningMod { get; set; }

        /// <summary>
        ///     Gets or sets the absolute file path.
        /// </summary>
        [JsonIgnore]
        public string AbsoluteFilePath { get; set; }

        /// <summary>
        ///     Gets whether this is a texture-only XNB replacement.
        /// </summary>
        [JsonIgnore]
        public bool IsTexture => !string.IsNullOrEmpty(this.Texture);

        /// <summary>
        ///     Gets whether this is a full XNB file replacement.
        /// </summary>
        [JsonIgnore]
        public bool IsXnb => !string.IsNullOrEmpty(this.File);

        /// <summary>
        ///     Gets or sets a value indicating whether this XNB is dirty.
        /// </summary>
        [JsonIgnore]
        public bool IsDirty { get; set; } = false;

        /// <summary>
        ///     Gets whether the file for this replacement exists.
        /// </summary>
        /// <param name="mod">
        ///     The owning mod.
        /// </param>
        /// <returns>
        ///     Whether the file exists.
        /// </returns>
        public bool Exists(ModManifest mod)
        {
            if (this.IsXnb)
            {
                return System.IO.File.Exists(this.AbsoluteFilePath + ".xnb");
            }

            if (this.IsTexture)
            {
                return TextureRegistry.GetItem(this.Texture, mod) != null;
            }

            return false;
        }

        #region Nested type: TextureRect

        /// <summary>
        ///     A simple class defining a rectangle.
        /// </summary>
        public class TextureRect
        {
            /// <summary>
            ///     Gets or sets the height.
            /// </summary>
            public int Height { get; set; }

            /// <summary>
            ///     Gets or sets the width.
            /// </summary>
            public int Width { get; set; }

            /// <summary>
            ///     Gets or sets the X position.
            /// </summary>
            public int X { get; set; }

            /// <summary>
            ///     Gets or sets the Y position.
            /// </summary>
            public int Y { get; set; }

            /// <summary>
            ///     Implicit conversion to <see cref="Rectangle" />
            /// </summary>
            /// <param name="rect">
            ///     The <see cref="TextureRect" /> to convert.
            /// </param>
            /// <returns>
            /// </returns>
            public static implicit operator Rectangle(TextureRect rect)
            {
                return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
            }

            /// <summary>
            ///     Converts <see cref="TextureRect" /> to <see cref="string" />.
            /// </summary>
            /// <returns>
            ///     The <see cref="TextureRect" /> as a <see cref="string" />.
            /// </returns>
            public override string ToString()
            {
                return $"X:{this.X}, Y:{this.Y}, H:{this.Height}, W{this.Width}";
            }
        }

        #endregion
    }
}