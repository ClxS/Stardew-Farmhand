namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Exposes internal properties to other assemblies.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class HookExposeInternal : FarmhandHook
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HookExposeInternal" /> class.
        /// </summary>
        /// <param name="assemblyName">
        ///     The assembly name.
        /// </param>
        /// <param name="finalExpose">
        ///     A value indicating whether access is provided in final assembly
        /// </param>
        public HookExposeInternal(string assemblyName, bool finalExpose = false)
        {
            this.AssemblyName = assemblyName;
        }

        /// <summary>
        ///     Gets the assembly name to allow access.
        /// </summary>
        public string AssemblyName { get; }
    }
}