namespace Farmhand
{
    using System;
    using System.IO;
    using System.Reflection;

    using Farmhand.API;
    using Farmhand.Logging;
    using Farmhand.Registries.Containers;

    using Newtonsoft.Json;

    /// <summary>
    ///     A settings configuration class for use with Farmhand. This integrates with the inbuilt settings manager
    ///     and so is the recommended way for mods to provide customizable settings.
    /// </summary>
    public abstract class ModSettings
    {
        /// <summary>
        ///     Gets whether this mod should use save specific configurations
        /// </summary>
        [JsonIgnore]
        public virtual bool UseSaveSpecificConfiguration => false;

        /// <summary>
        ///     Gets the manifest for this mod.
        /// </summary>
        [JsonIgnore]
        public ModManifest Manifest { get; internal set; }

        /// <summary>
        ///     Saves the current mod settings into the correct location
        /// </summary>
        public virtual void Save()
        {
            var outputFile = this.DetermineSaveLocation();
            var outputDirectory = Path.GetDirectoryName(outputFile);
            if (outputDirectory == null)
            {
                throw new Exception("Path.GetDirectoryName(outputFile) returned null!");
            }

            Directory.CreateDirectory(outputDirectory);

            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(outputFile, json);
        }

        /// <summary>
        ///     Checks whether a configuration file already exists.
        /// </summary>
        /// <returns>
        ///     Returns true if the file exists.
        /// </returns>
        public bool DoesConfigurationFileExist()
        {
            return this.DetermineSaveLocation(true) != null;
        }

        /// <summary>
        ///     Loads the current mod settings from the correct location
        /// </summary>
        public virtual void Load()
        {
            var saveFile = this.DetermineSaveLocation(true);
            if (saveFile == null)
            {
                Log.Verbose($"No configuration file located for {this.Manifest.Name}. Using default values.");
            }
            else
            {
                Log.Verbose($"Loading configuration settings for {this.Manifest.Name} from {saveFile}");
                var json = File.ReadAllText(saveFile);
                var newSettings = JsonConvert.DeserializeObject(
                    json,
                    this.GetType(),
                    new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore });
                this.MemberwiseAssign(this, newSettings);
            }
        }

        /// <summary>
        ///     Performs a member-wise assignment from the temporary settings to the primary settings instance
        /// </summary>
        /// <param name="modSettings">
        ///     The primary settings.
        /// </param>
        /// <param name="newSettings">
        ///     The temporary settings.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     ArgumentException is thrown if modSettings and newSettings are not of the same type.
        /// </exception>
        protected void MemberwiseAssign(object modSettings, object newSettings)
        {
            var type = modSettings.GetType();
            if (type != newSettings.GetType())
            {
                throw new ArgumentException("Object1 and Object2 must have the same type for MemberwiseAssign");
            }

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in properties)
            {
                if (prop.GetSetMethod() == null)
                {
                    continue;
                }

                prop.SetValue(modSettings, prop.GetValue(newSettings, null), null);
            }
        }

        /// <summary>
        ///     Determines the correct configuration location based on whether this is a per save config, and if
        ///     a save has been loaded.
        /// </summary>
        /// <param name="ensureExists">
        ///     Ensures the file exists, if it doesn't try the global one, or return null if the global settings config does not
        ///     exist.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected string DetermineSaveLocation(bool ensureExists = false)
        {
            string file;
            if (this.UseSaveSpecificConfiguration && Game.HasLoadedGame && Game.Player != null)
            {
                var str = Game.Player.Name;
                foreach (var c in str)
                {
                    if (!char.IsLetterOrDigit(c))
                    {
                        str = str.Replace(string.Concat(c), string.Empty);
                    }
                }

                var saveFileName = string.Concat(new[] { str, "_", (object)Game.GameUniqueId });

                file = Path.Combine(this.Manifest.ConfigurationPath, saveFileName, "Config.json");
                if (!ensureExists || File.Exists(file))
                {
                    return file;
                }
            }

            file = Path.Combine(this.Manifest.ConfigurationPath, "Config.json");
            if (!ensureExists || File.Exists(file))
            {
                return file;
            }

            return null;
        }
    }
}