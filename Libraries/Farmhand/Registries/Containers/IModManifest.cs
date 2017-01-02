using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmhand.Helpers;

namespace Farmhand.Registries.Containers
{
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
