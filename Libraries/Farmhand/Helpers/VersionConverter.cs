using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Farmhand.Helpers
{
    public class VersionConverter : JsonConverter
    {
        public class SmapiVersion
        {
            public int MajorVersion = 0;
            public int MinorVersion = 0;
            public int PatchVersion = 0;
            public string Build = "";
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
            JObject jObject = null;
            if (reader.TokenType == JsonToken.StartObject)
            {
                jObject = JObject.Load(reader);
            }

            object retValue = null;
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    break;
                case JsonToken.EndObject:
                    var tmp = jObject.ToObject<SmapiVersion>();
                    retValue = new Version(tmp.MajorVersion, tmp.MinorVersion, tmp.PatchVersion);
                    break;
                case JsonToken.String:
                    retValue = new Version((string)reader.Value);
                    break;
                default:
                    break;
            }

            if (retValue == null)
            {
                retValue = new Version(0, 0, 0);
            }

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
