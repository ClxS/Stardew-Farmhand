using Farmhand.Attributes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Farmhand.Overrides.Game
{
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", ".ctor", typeof(FhSerializableDictionary<Vector2, StardewValley.Object>), typeof(SerializableOverride<Vector2, StardewValley.Object>))]
    class FhSerializableDictionary<TKey, TValue> : Farmhand.Overrides.SerializableOverride<TKey, TValue>
    {
        FhSerializableDictionary()
        {
            Farmhand.Logging.Log.Error("Using overwrited serialized dictionary");
        }

        public override void ReadXml(XmlReader reader)
        {
            XmlSerializer xmlSerializer1 = new XmlSerializer(typeof(TKey), API.Serializer.InjectedTypes.ToArray());
            XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(TValue), API.Serializer.InjectedTypes.ToArray());
            bool isEmptyElement = reader.IsEmptyElement;
            reader.Read();
            if (isEmptyElement)
                return;
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");
                TKey key = (TKey)xmlSerializer1.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("value");
                TValue obj = (TValue)xmlSerializer2.Deserialize(reader);
                reader.ReadEndElement();
                this.Add(key, obj);
                reader.ReadEndElement();
                int num = (int)reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public override void WriteXml(XmlWriter writer)
        {
            XmlSerializer xmlSerializer1 = new XmlSerializer(typeof(TKey), API.Serializer.InjectedTypes.ToArray());
            XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(TValue), API.Serializer.InjectedTypes.ToArray());
            foreach (TKey index in this.Keys)
            {
                writer.WriteStartElement("item");
                writer.WriteStartElement("key");
                xmlSerializer1.Serialize(writer, (object)index);
                writer.WriteEndElement();
                writer.WriteStartElement("value");
                TValue obj = this[index];
                xmlSerializer2.Serialize(writer, (object)obj);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }
    }
}
