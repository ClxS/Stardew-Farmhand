using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StardewModdingAPI
{
    public static class Extensions
    {
        public static Random Random = new Random();

        public static bool IsKeyDown(this Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }

        public static Color RandomColour()
        {
            return new Color(Random.Next(0, 255), Random.Next(0, 255), Random.Next(0, 255));
        }

        [Obsolete("The usage of ToSingular has changed. Please update your call to use ToSingular<T>")]
        public static string ToSingular(this IEnumerable ienum, string split = ", ")
        {
            Log.AsyncR("The usage of ToSingular has changed. Please update your call to use ToSingular<T>");
            return "";
        }

        public static string ToSingular<T>(this IEnumerable<T> ienum, string split = ", ") // where T : class
        {
            //Apparently Keys[] won't split normally :l
            if (typeof (T) == typeof (Keys))
            {
                return string.Join(split, ienum.ToArray());
            }
            return string.Join(split, ienum);
        }

        /*public static string ToSingular<T>(this IEnumerable<T> ienum, string split = ", ")
        {
            return string.Join(split, ienum);
        }*/

        public static bool IsInt32(this object o)
        {
            int i;
            return int.TryParse(o.ToString(), out i);
        }

        public static int AsInt32(this object o)
        {
            return int.Parse(o.ToString());
        }

        public static bool IsBool(this object o)
        {
            bool b;
            return bool.TryParse(o.ToString(), out b);
        }

        public static bool AsBool(this object o)
        {
            return bool.Parse(o.ToString());
        }

        public static int GetHash(this IEnumerable enumerable)
        {
            var hash = 0;
            foreach (var v in enumerable)
            {
                hash ^= v.GetHashCode();
            }
            return hash;
        }

        public static T Cast<T>(this object o) where T : class
        {
            return o as T;
        }

        public static FieldInfo[] GetPrivateFields(this object o)
        {
            return o.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static FieldInfo GetBaseFieldInfo(this Type t, string name)
        {
            return t.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static T GetBaseFieldValue<T>(this Type t, object o, string name) where T : class
        {
            return t.GetBaseFieldInfo(name).GetValue(o) as T;
        }

        public static void SetBaseFieldValue<T>(this Type t, object o, string name, object newValue) where T : class
        {
            t.GetBaseFieldInfo(name).SetValue(o, newValue as T);
        }

        /*
        public static T GetBaseFieldValue<T>(this object o, string name) where T : class
        {
            return o.GetType().GetBaseFieldInfo(name).GetValue(o) as T;
        }*/

        /*
        public static object GetBaseFieldValue(this object o, string name)
        {
            return o.GetType().GetBaseFieldInfo(name).GetValue(o);
        }

        public static void SetBaseFieldValue (this object o, string name, object newValue)
        {
            o.GetType().GetBaseFieldInfo(name).SetValue(o, newValue);
        }
        */

        public static string RemoveNumerics(this string st)
        {
            var s = st;
            foreach (var c in s)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    s = s.Replace(c.ToString(), "");
                }
            }
            return s;
        }
    }
}