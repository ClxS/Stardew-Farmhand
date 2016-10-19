using Newtonsoft.Json;
using xTile.Format;

namespace Farmhand.Registries.Containers
{
    public class ModMap
    {
        public string File { get; set; }
        public string Id { get; set; }

        [JsonIgnore]
        public string AbsoluteFilePath { get; set; }

        [JsonIgnore]
        private xTile.Map _map;

        [JsonIgnore]
        public xTile.Map Map
        {
            get
            {
                if (_map == null && Exists())
                {
                    _map = FormatManager.Instance.LoadMap(AbsoluteFilePath);
                }
                return _map;
            }
            internal set { _map = value; }
        }

        public bool Exists()
        {
            return !string.IsNullOrEmpty(AbsoluteFilePath) && System.IO.File.Exists(AbsoluteFilePath);
        }
    }
}
