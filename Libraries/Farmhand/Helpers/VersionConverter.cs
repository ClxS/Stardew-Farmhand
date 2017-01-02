namespace Farmhand.Helpers
{
    using System;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal class VersionConverter : JsonConverter
    {
        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(
                "Unnecessary because CanWrite is false. The type will skip the converter.");
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            JObject jsonObject = null;
            if (reader.TokenType == JsonToken.StartObject)
            {
                jsonObject = JObject.Load(reader);
            }

            object retValue = null;
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    break;
                case JsonToken.EndObject:
                    if (jsonObject != null)
                    {
                        var tmp = jsonObject.ToObject<SimpleVersion>();
                        retValue = new Version(tmp.MajorVersion, tmp.MinorVersion, tmp.PatchVersion);
                    }

                    break;
                case JsonToken.String:
                    retValue = new Version((string)reader.Value);
                    break;
            }

            return retValue ?? (retValue = new Version(0, 0, 0));
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Version) == objectType;
        }

        #region Nested type: SimpleVersion

        internal class SimpleVersion
        {
            public string Build { get; set; } = string.Empty;

            public int MajorVersion { get; set; } = 0;

            public int MinorVersion { get; set; } = 0;

            public int PatchVersion { get; set; } = 0;
        }

        #endregion
    }
}