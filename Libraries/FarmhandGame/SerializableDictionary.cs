using Farmhand.Attributes;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Farmhand.Overrides.Game
{
    //[HookRedirectConstructorFromBase("StardewValley.GameLocation", "shiftObjects", typeof(Vector2), typeof(StardewValley.Object))]
    //[HookRedirectConstructorFromBase("StardewValley.GameLocation", ".ctor", typeof(Vector2), typeof(StardewValley.Object))]
    //[HookRedirectConstructorFromBase("StardewValley.GameLocation", "System.Void StardewValley.GameLocation::.ctor(xTile.Map,System.String)", typeof(Vector2), typeof(StardewValley.Object))]
    public class FhSerializableDictionary<TKey, TValue> : SerializableDictionary<TKey, TValue>
    {
        public FhSerializableDictionary()
        {
            Farmhand.Logging.Log.Error("Using overwritten serialized dictionary");
        }

        public new void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);
            return;
            var xmlSerializer1 = new XmlSerializer(typeof(TKey));
            var xmlSerializer2 = new XmlSerializer(typeof(TValue));
            var isEmptyElement = reader.IsEmptyElement;
            reader.Read();
            if (isEmptyElement)
                return;
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");
                var key = (TKey)xmlSerializer1.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("value");
                var obj = (TValue)xmlSerializer2.Deserialize(reader);
                reader.ReadEndElement();
                this.Add(key, obj);
                reader.ReadEndElement();
                var num = (int)reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public new void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            return;
            var xmlSerializer1 = new XmlSerializer(typeof(TKey), API.Serializer.InjectedTypes.ToArray());
            var xmlSerializer2 = new XmlSerializer(typeof(TValue), API.Serializer.InjectedTypes.ToArray());
            foreach (var index in this.Keys)
            {
                writer.WriteStartElement("item");
                writer.WriteStartElement("key");
                xmlSerializer1.Serialize(writer, (object)index);
                writer.WriteEndElement();
                writer.WriteStartElement("value");
                var obj = this[index];
                xmlSerializer2.Serialize(writer, (object)obj);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }
    }
}
