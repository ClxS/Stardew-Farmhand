using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmhand.Helpers;

namespace Farmhand.Registries.Containers
{
    public interface IModManifest
    {
        UniqueId<string> UniqueId { get; set; }

        bool IsFarmhandMod { get; }

        string Name { get; set; }

        string Author { get; set; }

        string Description { get; set; }

        ModState ModState { get; set; }

        Version Version { get; set; }
    }
}
