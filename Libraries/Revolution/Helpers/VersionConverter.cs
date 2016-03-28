using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Revolution.Helpers
{
    public class VersionConverter : JsonConverter
    {
        private class SmapiVersion
        {
            public int MajorVersion;
            public int MinorVersion;
            public int PatchVersion;
            public string Build;
        }
        private readonly Type[] _types;

        public VersionConverter(params Type[] types)
        {
            _types = types;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanWrite is false. The type will skip the converter.");
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Logging.Log.Success("1");
            // Load JObject from stream
            JObject jObject = null;
            if (reader.TokenType == JsonToken.StartObject)
            {
                jObject = JObject.Load(reader);
                Logging.Log.Success("2" + jObject.Count);
            }

            object retValue = null;
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    Logging.Log.Success("3");
                    Logging.Log.Error("Parsing SMAPI version");
                    break;
                case JsonToken.EndObject:
                    Logging.Log.Success("3");
                    Logging.Log.Error("Parsing SMAPI version");
                    var tmp = jObject.ToObject<SmapiVersion>();
                    retValue = new Version(tmp.MajorVersion, tmp.MinorVersion, tmp.PatchVersion);
                    break;
                case JsonToken.String:
                    Logging.Log.Success("4");
                    Logging.Log.Success((string)reader.Value);
                    retValue = new Version((string)reader.Value);
                    break;
                default:
                    Logging.Log.Success("5");
                    Logging.Log.Error("Unknown type");
                    break;
            }

            if (retValue == null)
            {
                retValue = new Version(0, 0, 0);
            }
            Logging.Log.Success($"{retValue}");

            return retValue;
        }

        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return typeof(System.Version) == objectType;
        }
    }
}
