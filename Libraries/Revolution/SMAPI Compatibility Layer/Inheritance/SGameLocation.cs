using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
namespace StardewModdingAPI.Inheritance
{
    public class SGameLocation : GameLocation
    {
        public GameLocation BaseGameLocation { get; private set; }

        public SerializableDictionary<Vector2, SObject> ModObjects { get; set; }

        public static SGameLocation ConstructFromBaseClass(GameLocation baseClass, bool copyAllData = false)
        {
            SGameLocation s = new SGameLocation
            {
                BaseGameLocation = baseClass,
                name = baseClass.name
            };

            Log.Debug("CONSTRUCTED: " + s.name);

            if (!copyAllData) return s;

            foreach (var v in baseClass.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    var fi = s.GetType().GetField(v.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (fi != null && !fi.IsStatic)
                    {
                        fi.SetValue(s, v.GetValue(baseClass));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }

            return s;
        }

        public static List<SGameLocation> ConstructFromBaseClasses(List<GameLocation> baseGameLocations, bool copyAllData = false)
        {
            return baseGameLocations.Select(gl => ConstructFromBaseClass(gl, copyAllData)).ToList();
        }

        public virtual void update(GameTime gameTime)
        {
        }

        public override void draw(SpriteBatch b)
        {
            foreach (var v in ModObjects)
            {
                v.Value.draw(b, (int)v.Key.X, (int)v.Key.Y, 0.999f, 1);
            }
        }

        public SGameLocation()
        {
            ModObjects = new SerializableDictionary<Vector2, SObject>();
        }
    }
}
