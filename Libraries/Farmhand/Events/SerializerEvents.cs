namespace Farmhand.Events
{
    using System;
    using System.Xml.Serialization;

    internal static class SerializerEvents
    {
        public static event EventHandler<XmlElementEventArgs> UnknownElement = delegate { };

        public static event EventHandler<XmlAttributeEventArgs> UnknownAttribute = delegate { };

        public static event EventHandler<XmlNodeEventArgs> UnknownNode = delegate { };

        public static event EventHandler<UnreferencedObjectEventArgs> UnreferencedObject = delegate { };

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