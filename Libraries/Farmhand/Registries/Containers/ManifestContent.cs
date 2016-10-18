using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmhand.Registries.Containers
{
    public class ManifestContent
    {
        public bool HasContent => (Textures != null && Textures.Any()) ||
                                  (Xnb != null && Xnb.Any()) ||
                                  (Maps != null && Maps.Any());


        public List<DiskTexture> Textures { get; set; }
        public List<ModXnb> Xnb { get; set; }
        public List<ModMap> Maps { get; set; }
    }
}
