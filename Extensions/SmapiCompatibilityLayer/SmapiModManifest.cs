using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farmhand;
using Farmhand.Helpers;
using Farmhand.Registries.Containers;

namespace SmapiCompatibilityLayer
{
    class SmapiModManifest : IModManifest
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
