namespace Farmhand.Helpers
{
    using System;

    using Farmhand.Logging;

    using Newtonsoft.Json;

    internal class UniqueIdConverter : JsonConverter
    {
        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var isSet = true;
            UniqueId<string> output = null;
            var id = (string)reader.Value;

            do
            {
                try
                {
                    output = new UniqueId<string>(id);
                }
                catch (Exception ex)
                {
                    isSet = false;
                    id = Guid.NewGuid().ToString();
                    Log.Exception(
                        $"Failed to set unique ID. This may indicate multiple copies of a mod, using random unique id instead: {id}",
                        ex);
                }
            }
            while (!isSet);

            return output;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}