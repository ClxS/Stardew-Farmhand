// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
namespace StardewModdingAPI
{
    public class Mod
    {
        /// <summary>
        /// The name of your mod.
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// The name of the mod's authour.
        /// </summary>
        public virtual string Authour { get; protected set; }

        /// <summary>
        /// The version of the mod.
        /// </summary>
        public virtual string Version { get; protected set; }

        /// <summary>
        /// A description of the mod.
        /// </summary>
        public virtual string Description { get; protected set; }

        /// <summary>
        /// A basic method that is the entry-point of your mod. It will always be called once when the mod loads.
        /// </summary>
        public virtual void Entry(params object[] objects)
        {

        }
    }
}
