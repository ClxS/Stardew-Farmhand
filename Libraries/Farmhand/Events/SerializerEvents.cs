using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Farmhand.Events
{
    public static class SerializerEvents
    {
        public static event EventHandler<System.Xml.Serialization.XmlElementEventArgs> UnknownElement = delegate { };
        public static event EventHandler<System.Xml.Serialization.XmlAttributeEventArgs> UnknownAttribute = delegate { };
        public static event EventHandler<System.Xml.Serialization.XmlNodeEventArgs> UnknownNode = delegate { };
        public static event EventHandler<System.Xml.Serialization.UnreferencedObjectEventArgs> UnreferencedObject = delegate { };
        
        internal static void OnUnknownElement(object sender, XmlElementEventArgs e)
        {
            EventCommon.SafeInvoke(UnknownElement, sender, e);
        }

        internal static void OnUnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            EventCommon.SafeInvoke(UnknownAttribute, sender, e);
        }

        internal static void OnUnknownNode(object sender, XmlNodeEventArgs e)
        {
            EventCommon.SafeInvoke(UnknownNode, sender, e);
        }

        internal static void OnUnreferencedObject(object sender, UnreferencedObjectEventArgs e)
        {
            EventCommon.SafeInvoke(UnreferencedObject, sender, e);
        }
    }
}
