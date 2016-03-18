using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution
{
    public abstract class Mod
    {
        public virtual string UniqueModId { get; protected set; }

        /// <summary>
        /// The name of your mod.
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// The name of the mod's authour.
        /// </summary>
        public virtual string Author { get; protected set; }

        /// <summary>
        /// The version of the mod.
        /// </summary>
        public virtual string Version { get; protected set; }

        /// <summary>
        /// A description of the mod.
        /// </summary>
        public virtual string Description { get; protected set; }

        public abstract void Entry();
    }
}
