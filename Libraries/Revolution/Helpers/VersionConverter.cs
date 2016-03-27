using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Revolution.Helpers
{
    public class VersionConverter : JsonConverter
    {
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
            return null;
            //// Load JObject from stream
            //JObject jObject = JObject.Load(reader);
            //Logging.Log.Success("2");

            //object retValue = null;
            //switch (reader.TokenType)
            //{
            //    case JsonToken.StartObject:
            //Logging.Log.Success("3");
            //        Logging.Log.Error("Parsing SMAPI version");
            //        break;
            //    case JsonToken.String:
            //Logging.Log.Success("4");
            //        retValue = serializer.Deserialize(reader, typeof(Version));
            //        break;
            //    default:
            //Logging.Log.Success("5");
            //        Logging.Log.Error("Unknown type");
            //        break;
            //}
            //Logging.Log.Success("6");

            //return retValue;
        }

        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return _types.Any(t => t == objectType);
        }
    }
}
