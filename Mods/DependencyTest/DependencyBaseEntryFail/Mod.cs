namespace DependencyTest
{
    using System;

    using Farmhand;

    /// <summary>
    /// A dependency test mod.
    /// </summary>
    public class DependencyMod : Mod
    {
        /// <summary>
        /// The mod entry method.
        /// </summary>
        public override void Entry()
        {
            throw new Exception("Expected failure in entry" + "");
        }
    }
}