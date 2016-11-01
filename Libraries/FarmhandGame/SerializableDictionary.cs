using Farmhand.Attributes;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Farmhand.Overrides.Game
{
    public static class SerializableDictionaryOverrides
    {
        private static Dictionary<Type, XmlSerializer> SerializerCache = new Dictionary<Type, XmlSerializer>();

        [Hook(HookType.Entry, "StardewValley.SerializableDictionary`2", "ReadXml")]
        internal static bool ReadXmlOverride<TKey, TValue>([ThisBind] object @this,
            [InputBind(typeof(XmlReader), "reader")] XmlReader reader)
        {
            return ReadXmlGeneric<TKey, TValue>(@this, reader);
        }

        [Hook(HookType.Entry, "StardewValley.SerializableDictionary`2", "WriteXml")]
        internal static bool WriteXmlOverride<TKey, TValue>([ThisBind] object @this,
            [InputBind(typeof(XmlWriter), "writer")] XmlWriter writer)
        {
            return WriteXmlGeneric<TKey, TValue>(@this, writer);
        }

        private static XmlSerializer GetSerializerForType(Type type)
        {
            if (SerializerCache.ContainsKey(type))
            {
                return SerializerCache[type];
            }

            var serializer = new XmlSerializer(type, API.Serializer.InjectedTypes.ToArray());
            SerializerCache[type] = serializer;
            return serializer;
        }

        private static bool ReadXmlGeneric<TKey, TValue>(object @this, XmlReader reader)
        {
            var serializableDictionary = @this as SerializableDictionary<TKey, TValue>;

            if (serializableDictionary == null) return false;

            var dictionary = serializableDictionary;
            var xmlSerializer1 = GetSerializerForType(typeof(TKey));
            var xmlSerializer2 = GetSerializerForType(typeof(TValue));
            var isEmptyElement = reader.IsEmptyElement;
            reader.Read();

            if (isEmptyElement)
            {
                return true;
            }
            
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");
                var key = (TKey)xmlSerializer1.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("value");
                var obj = (TValue)xmlSerializer2.Deserialize(reader);
                reader.ReadEndElement();
                dictionary.Add(key, obj);
                reader.ReadEndElement();
                var num = (int)reader.MoveToContent();
            }
            reader.ReadEndElement();
            
            return true;
        }

        private static bool WriteXmlGeneric<TKey, TValue>(object @this, XmlWriter writer)
        {
            var serializableDictionary = @this as SerializableDictionary<TKey, TValue>;

            if (serializableDictionary == null) return false;
            
            var serializer = GetSerializerForType(typeof(TKey));
            var serializer2 = GetSerializerForType(typeof(TValue));
            Dictionary<TKey, TValue>.KeyCollection.Enumerator enumerator = serializableDictionary.Keys.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    TKey current = enumerator.Current;
                    writer.WriteStartElement("item");
                    writer.WriteStartElement("key");
                    serializer.Serialize(writer, current);
                    writer.WriteEndElement();
                    writer.WriteStartElement("value");
                    TValue o = serializableDictionary[current];
                    serializer2.Serialize(writer, o);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }
            finally
            {
                enumerator.Dispose();
            }
            return true;
        }
    }
}
