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
    [HookRedirectConstructorFromBase("StardewValley.AnimalHouse", ".ctor", typeof(long), typeof(StardewValley.FarmAnimal))]
    [HookRedirectConstructorFromBase("StardewValley.AnimalHouse", "System.Void StardewValley.AnimalHouse::.ctor(xTile.Map,System.String)", typeof(long), typeof(StardewValley.FarmAnimal))]
    [HookRedirectConstructorFromBase("StardewValley.Buildings.Building", "getIndoors", typeof(Vector2), typeof(StardewValley.TerrainFeatures.TerrainFeature))]
    [HookRedirectConstructorFromBase("StardewValley.Buildings.Building", "load", typeof(Vector2), typeof(StardewValley.TerrainFeatures.TerrainFeature))]
    [HookRedirectConstructorFromBase("StardewValley.Farm", ".ctor", typeof(long), typeof(StardewValley.FarmAnimal))]
    [HookRedirectConstructorFromBase("StardewValley.Farm", "System.Void StardewValley.Farm::.ctor(xTile.Map,System.String)", typeof(long), typeof(StardewValley.FarmAnimal))]
    [HookRedirectConstructorFromBase("StardewValley.Farmer", ".ctor", typeof(string), typeof(int))]
    [HookRedirectConstructorFromBase("StardewValley.Farmer", "System.Void StardewValley.Farmer::.ctor(StardewValley.FarmerSprite,Microsoft.Xna.Framework.Vector2,System.Int32,System.String,System.Collections.Generic.List`1<StardewValley.Item>,System.Boolean)", typeof(string), typeof(int))]
    [HookRedirectConstructorFromBase("StardewValley.Farmer", "System.Void StardewValley.Farmer::.ctor(StardewValley.FarmerSprite,Microsoft.Xna.Framework.Vector2,System.Int32,System.String,System.Collections.Generic.List`1<StardewValley.Item>,System.Boolean)", typeof(int), typeof(int))]
    [HookRedirectConstructorFromBase("StardewValley.Farmer", "System.Void StardewValley.Farmer::.ctor(StardewValley.FarmerSprite,Microsoft.Xna.Framework.Vector2,System.Int32,System.String,System.Collections.Generic.List`1<StardewValley.Item>,System.Boolean)", typeof(int), typeof(int[]))]
    [HookRedirectConstructorFromBase("StardewValley.Farmer", "System.Void StardewValley.Farmer::.ctor(StardewValley.FarmerSprite,Microsoft.Xna.Framework.Vector2,System.Int32,System.String,System.Collections.Generic.List`1<StardewValley.Item>,System.Boolean)", typeof(string), typeof(int[]))]
    [HookRedirectConstructorFromBase("StardewValley.Farmer", "caughtFish", typeof(int), typeof(int[]))]
    [HookRedirectConstructorFromBase("StardewValley.Farmer", "cookedRecipe", typeof(int), typeof(int))]
    [HookRedirectConstructorFromBase("StardewValley.Farmer", "foundArtifact", typeof(int), typeof(int[]))]
    [HookRedirectConstructorFromBase("StardewValley.Farmer", "foundMineral", typeof(int), typeof(int))]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "shiftObjects", typeof(Vector2), typeof(StardewValley.Object))]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", ".ctor", typeof(Vector2), typeof(StardewValley.Object))]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", ".ctor", typeof(Vector2), typeof(StardewValley.TerrainFeatures.TerrainFeature))]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "System.Void StardewValley.GameLocation::.ctor(xTile.Map,System.String)", typeof(Vector2), typeof(StardewValley.Object))]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "System.Void StardewValley.GameLocation::.ctor(xTile.Map,System.String)", typeof(Vector2), typeof(StardewValley.TerrainFeatures.TerrainFeature))]
    [HookRedirectConstructorFromBase("StardewValley.Locations.CommunityCenter", "System.Void StardewValley.Locations.CommunityCenter::.ctor(System.String)", typeof(int), typeof(bool))]
    [HookRedirectConstructorFromBase("StardewValley.Locations.CommunityCenter", "System.Void StardewValley.Locations.CommunityCenter::.ctor(System.String)", typeof(int), typeof(bool[]))]
    [HookRedirectConstructorFromBase("StardewValley.Locations.FarmHouse", "shiftObjects", typeof(Vector2), typeof(StardewValley.TerrainFeatures.TerrainFeature))]
    [HookRedirectConstructorFromBase("StardewValley.Locations.LibraryMuseum", "System.Void StardewValley.Locations.LibraryMuseum::.ctor(xTile.Map,System.String)", typeof(Vector2), typeof(int))]
    [HookRedirectConstructorFromBase("StardewValley.Locations.MineShaft", ".ctor", typeof(int), typeof(StardewValley.Locations.MineInfo))]
    [HookRedirectConstructorFromBase("StardewValley.Menus.CraftingPage", ".ctor", typeof(string), typeof(int))]
    [HookRedirectConstructorFromBase("StardewValley.Stats", ".ctor", typeof(string), typeof(int))]
    public class FhSerializableDictionary<TKey, TValue> : SerializableDictionary<TKey, TValue>
    {
        public FhSerializableDictionary()
        {
            
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
