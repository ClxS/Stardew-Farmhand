using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using StardewModdingAPI.Inheritance;
using Object = StardewValley.Object;

namespace StardewModdingAPI
{
    internal class JsonResolver : DefaultContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            if (objectType == typeof (Rectangle) || objectType == typeof (Rectangle?))
            {
                Console.WriteLine("FOUND A RECT");
                JsonContract contract = CreateObjectContract(objectType);
                contract.Converter = new RectangleConverter();
                return contract;
            }
            if (objectType == typeof (Object))
            {
                Log.AsyncY("FOUND AN OBJECT");
                JsonContract contract = CreateObjectContract(objectType);
                contract.Converter = new ObjectConverter();
                return contract;
            }
            return base.CreateContract(objectType);
        }
    }

    public class ObjectConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Log.AsyncY("TRYING TO WRITE");
            var obj = (Object) value;
            Log.AsyncY("TRYING TO WRITE");

            var jObject = GetObject(obj);
            Log.AsyncY("TRYING TO WRITE");

            try
            {
                Log.AsyncY(jObject.ToString());
            }
            catch (Exception ex)
            {
                Log.AsyncR(ex);
            }

            Console.ReadKey();

            jObject.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            return GetObject(jObject);
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        protected static JObject GetObject(Object o)
        {
            try
            {
                var parentSheetIndex = o.parentSheetIndex;
                var stack = o.stack;
                var isRecipe = o.isRecipe;
                var price = o.price;
                var quality = o.quality;

                var oo = new SBareObject(parentSheetIndex, stack, isRecipe, price, quality);
                Log.AsyncG(JsonConvert.SerializeObject(oo));
                return JObject.FromObject(oo);
            }
            catch (Exception ex)
            {
                Log.AsyncR(ex);
                Console.ReadKey();
            }
            return null;
        }

        protected static Object GetObject(JObject jObject)
        {
            var parentSheetIndex = GetTokenValue<object>(jObject, "parentSheetIndex") as int?;
            var stack = GetTokenValue<object>(jObject, "parentSheetIndex") as int?;
            var isRecipe = GetTokenValue<object>(jObject, "parentSheetIndex") as bool?;
            var price = GetTokenValue<object>(jObject, "parentSheetIndex") as int?;
            var quality = GetTokenValue<object>(jObject, "parentSheetIndex") as int?;

            return new Object(parentSheetIndex ?? 0, stack ?? 0, isRecipe ?? false, price ?? -1, quality ?? 0);
        }

        protected static Object GetObject(JToken jToken)
        {
            var jObject = JObject.FromObject(jToken);

            return GetObject(jObject);
        }

        protected static T GetTokenValue<T>(JObject jObject, string tokenName) where T : class
        {
            JToken jToken;
            jObject.TryGetValue(tokenName, StringComparison.InvariantCultureIgnoreCase, out jToken);
            return jToken as T;
        }
    }

    public class RectangleConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var rectangle = (Rectangle) value;

            var jObject = GetObject(rectangle);

            jObject.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Console.WriteLine(reader.ReadAsString());
            var jObject = JObject.Load(reader);

            return GetRectangle(jObject);
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        protected static JObject GetObject(Rectangle rectangle)
        {
            var x = rectangle.X;
            var y = rectangle.Y;
            var width = rectangle.Width;
            var height = rectangle.Height;

            return JObject.FromObject(new {x, y, width, height});
        }

        protected static Rectangle GetRectangle(JObject jObject)
        {
            var x = GetTokenValue(jObject, "x") ?? 0;
            var y = GetTokenValue(jObject, "y") ?? 0;
            var width = GetTokenValue(jObject, "width") ?? 0;
            var height = GetTokenValue(jObject, "height") ?? 0;

            return new Rectangle(x, y, width, height);
        }

        protected static Rectangle GetRectangle(JToken jToken)
        {
            var jObject = JObject.FromObject(jToken);

            return GetRectangle(jObject);
        }

        protected static int? GetTokenValue(JObject jObject, string tokenName)
        {
            JToken jToken;
            return jObject.TryGetValue(tokenName, StringComparison.InvariantCultureIgnoreCase, out jToken) ? (int) jToken : (int?) null;
        }
    }

    public class RectangleListConverter : RectangleConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var rectangleList = (IList<Rectangle>) value;

            var jArray = new JArray();

            foreach (var rectangle in rectangleList)
            {
                jArray.Add(GetObject(rectangle));
            }

            jArray.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var rectangleList = new List<Rectangle>();

            var jArray = JArray.Load(reader);

            foreach (var jToken in jArray)
            {
                rectangleList.Add(GetRectangle(jToken));
            }

            return rectangleList;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}