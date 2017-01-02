namespace SmapiCompatibilityLayer
{
    using System;

    using Farmhand;
    using Farmhand.Helpers;
    using Farmhand.Registries.Containers;

    internal class SmapiModManifest : IModManifest
    {
        public UniqueId<string> UniqueId { get; set; }

        public bool IsFarmhandMod => false;

        public string Name { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public ModState ModState { get; set; }

        public Version Version { get; set; }
    }
}
