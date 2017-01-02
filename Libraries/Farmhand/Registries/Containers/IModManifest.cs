namespace Farmhand.Registries.Containers
{
    using System;

    using Farmhand.Helpers;

    public interface IModManifest
    {
        UniqueId<string> UniqueId { get; }

        bool IsFarmhandMod { get; }

        string Name { get; }

        string Author { get; }

        string Description { get; }

        ModState ModState { get; }

        Version Version { get; }
    }
}