using Farmhand.Attributes;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Farmhand.Events;
using Farmhand.Events.Arguments;
using Object = StardewValley.Object;

namespace Farmhand.Overrides.Game
{
    public static class SerializableDictionaryOverrides
    {
        [Hook(HookType.Entry, "StardewValley.SerializableDictionary`2", "ReadXml")]
        internal static bool ReadXmlOverride([ThisBind] object @this,
            [InputBind(typeof(XmlReader), "reader")] XmlReader reader)
        {
            var method = typeof(SerializableDictionaryOverrides).GetMethod("ReadXmlGeneric", BindingFlags.NonPublic | BindingFlags.Static);
            var methodRef = method.MakeGenericMethod(@this.GetType().GetGenericArguments());
            return (bool)methodRef.Invoke(null, new object[] { @this, reader });
        }

        [Hook(HookType.Entry, "StardewValley.SerializableDictionary`2", "WriteXml")]
        internal static bool WriteXmlOverride([ThisBind] object @this,
            [InputBind(typeof(XmlWriter), "writer")] XmlWriter writer)
        {
            var method = typeof(SerializableDictionaryOverrides).GetMethod("WriteXmlGeneric", BindingFlags.NonPublic | BindingFlags.Static);
            var methodRef = method.MakeGenericMethod(@this.GetType().GetGenericArguments());
            return (bool)methodRef.Invoke(null, new object[] { @this, writer });
        }

        private static bool ReadXmlGeneric<TKey, TValue>(object @this, XmlReader reader)
        {
            var serializableDictionary = @this as SerializableDictionary<TKey, TValue>;

            if (serializableDictionary == null) return false;

            var dictionary = serializableDictionary;
            var xmlSerializer1 = new XmlSerializer(typeof(TKey), API.Serializer.InjectedTypes.ToArray());
            var xmlSerializer2 = new XmlSerializer(typeof(TValue), API.Serializer.InjectedTypes.ToArray());
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

            XmlSerializer serializer = new XmlSerializer(typeof(TKey), API.Serializer.InjectedTypes.ToArray());
            XmlSerializer serializer2 = new XmlSerializer(typeof(TValue), API.Serializer.InjectedTypes.ToArray());
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
