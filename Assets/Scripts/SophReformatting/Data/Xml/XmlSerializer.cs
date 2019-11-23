using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

namespace SimEncounters.Xml
{
    public class XmlSerializer
    {

        protected virtual XmlNode Node { get; }

        public const string DEFAULT_BASE_TAG_NAME = "data";
        public XmlSerializer(XmlDocument xmlDocument)
        {
            Node = xmlDocument.CreateElement(DEFAULT_BASE_TAG_NAME);
            xmlDocument.AppendChild(Node);
        }

        public XmlSerializer(XmlNode node)
        {
            Node = node;
        }

        private const bool NODE_NAME_FIRST_LETTER_CAPITALIZED = false;
        public virtual XmlElement CreateElement(string name, XmlNode parent)
        {
            if (NODE_NAME_FIRST_LETTER_CAPITALIZED && !char.IsUpper(name[0]))
                Debug.LogWarning($"XML element name should be capitalized: {name}");
            if (!NODE_NAME_FIRST_LETTER_CAPITALIZED && char.IsUpper(name[0]))
                Debug.LogWarning($"XML element name should not be capitalized: {name}");

            var node = parent.OwnerDocument.CreateElement(name);

            if (parent != null)
                parent.AppendChild(node);

            return node;
        }

        public virtual XmlElement CreateElement(string name, string value, XmlNode parent)
        {
            var node = CreateElement(name, parent);
            node.InnerText = value;

            return node;
        }

        public virtual void AddString(string name, string value)
        {
            XmlHelper.CreateElement(name, UnityWebRequest.EscapeURL(value), Node);
        }

        public virtual void AddValue<T>(string name, T value)
            where T : IXmlSerializable
        {
            var element = XmlHelper.CreateElement(name, Node);
            var deserializer = new XmlSerializer(element);
            value.GetObjectData(deserializer);
        }

        public virtual void AddStringList(string name, string elementName, IEnumerable<string> list)
        {
            var listNode = XmlHelper.CreateElement(name, Node);
            foreach (var value in list)
                XmlHelper.CreateElement(elementName, UnityWebRequest.EscapeURL(value), listNode);
        }

        public virtual void AddList<T>(string name, string elementName, IEnumerable<T> list)
            where T : IXmlSerializable
        {
            var listNode = XmlHelper.CreateElement(name, Node);
            foreach (var value in list) {
                var childNode = XmlHelper.CreateElement(elementName, listNode);
                var deserializer = new XmlSerializer(childNode);
                value.GetObjectData(deserializer);
            }
        }
        public virtual void AddKeyValuePairs<T>(string name, string elementName,
            IEnumerable<KeyValuePair<string, T>> list)
             where T : IXmlSerializable
        {
            var listNode = XmlHelper.CreateElement(name, Node);
            foreach (var pair in list) {
                var childNode = XmlHelper.CreateElement(elementName, listNode);
                childNode.SetAttribute("key", pair.Key);
                var deserializer = new XmlSerializer(childNode);
                pair.Value.GetObjectData(deserializer);
            }
        }
    }
}