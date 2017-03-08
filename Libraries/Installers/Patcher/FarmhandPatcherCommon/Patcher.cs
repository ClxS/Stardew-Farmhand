namespace Farmhand.Installers.Patcher
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Farmhand.Installers.Patcher.Cecil;
    using Farmhand.Installers.Patcher.Helpers;
    using Farmhand.Installers.Patcher.Injection;

    using ILRepacking;

    /// <summary>
    ///     Used by the installers to modify the game executable
    /// </summary>
    public abstract class Patcher
    {
        private CompositionContainer container;

        /// <summary>
        ///     Gets or sets the farmhand assemblies.
        /// </summary>
        protected List<Assembly> FarmhandAssemblies { get; set; } = new List<Assembly>();
        
        /// <summary>
        ///     Patches the games executable, injecting our libraries and making changes found via attributes.
        /// </summary>
        /// <param name="path">
        ///     Exe Path (Pass 1), or Farmhand Output Path (Pass 2)
        /// </param>
        public abstract void PatchStardew(string path = null);

        /// <summary>
        ///     Initializes MEF using the provided catalog.
        /// </summary>
        /// <param name="catalog">
        ///     The MEF type catalog.
        /// </param>
        protected void InitialiseContainer(ComposablePartCatalog catalog)
        {
            // Create the CompositionContainer with the parts in the catalog
            this.container = new CompositionContainer(catalog);

            // Fill the imports of this object
            try
            {
                this.container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }

        /// <summary>
        ///     Gets the assembly path. If an AssemblyDirectory is defined, it will use that as the root assembly directory,
        ///     otherwise it returns the same value as was input.
        /// </summary>
        /// <param name="assembly">
        ///     The assembly to get the path to
        /// </param>
        /// <returns>
        ///     The path to the assembly..
        /// </returns>
        public string GetAssemblyPath(string assembly)
        {
            return string.IsNullOrEmpty(PatcherOptions.AssemblyDirectory)
                       ? assembly
                       : Path.Combine(PatcherOptions.AssemblyDirectory, assembly);
        }
    }
}