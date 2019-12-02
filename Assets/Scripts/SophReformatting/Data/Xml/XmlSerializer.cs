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
        protected virtual XmlElement CreateElement(string name, XmlNode parent)
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

        protected virtual XmlElement CreateElement(string name, string value, XmlNode parent)
        {
            var node = CreateElement(name, parent);
            node.InnerText = UnityWebRequest.EscapeURL(value);

            return node;
        }

        public virtual void AddString(NodeInfo nodeData, string value)
        {
            CreateElement(nodeData.Name, value, Node);
        }
        public virtual void AddBool(NodeInfo nodeData, bool value)
        {
            CreateElement(nodeData.Name, value.ToString(), Node);
        }
        public virtual void AddInt(NodeInfo nodeData, int value)
        {
            CreateElement(nodeData.Name, value.ToString(), Node);
        }
        public virtual void AddColor(NodeInfo nodeData, Color value)
        {
            CreateElement(nodeData.Name, $"{value.r},{value.g},{value.b},{value.a}", Node);
        }

        public virtual void AddValue<T>(NodeInfo nodeData, T value, ISerializationFactory<T> serializationFactory)
        {
            var element = CreateElement(nodeData.Name, Node);
            var serializer = new XmlSerializer(element);
            serializationFactory.Serialize(serializer, value);
        }

        public virtual void AddStringList(CollectionXmlInfo collectionInfo, IEnumerable<string> list)
        {
            var listNode = CreateElement(collectionInfo.CollectionNode.Name, Node);
            foreach (var value in list)
                CreateElement(collectionInfo.ElementNode.Name, value, listNode);
        }

        public virtual void AddList<T>(CollectionXmlInfo collectionInfo, IEnumerable<T> list,
            ISerializationFactory<T> serializationFactory)
        {
            var listNode = CreateElement(collectionInfo.CollectionNode.Name, Node);
            foreach (var value in list) {
                var childNode = CreateElement(collectionInfo.ElementNode.Name, listNode);
                var serializer = new XmlSerializer(childNode);
                serializationFactory.Serialize(serializer, value);
            }
        }

        protected virtual string KeyAttributeName { get; } = "id";
        public virtual void AddStringKeyValuePairs(CollectionXmlInfo collectionInfo,
            IEnumerable<KeyValuePair<string, string>> list)
        {
            var listNode = CreateElement(collectionInfo.CollectionNode.Name, Node);
            foreach (var pair in list) {
                var childNode = CreateElement(collectionInfo.ElementNode.Name, pair.Value, listNode);
                childNode.SetAttribute(KeyAttributeName, pair.Key);
            }
        }

        public virtual void AddKeyValuePairs<T>(CollectionXmlInfo collectionInfo,
            IEnumerable<KeyValuePair<string, T>> list, ISerializationFactory<T> serializationFactory)
            where T : IXmlSerializable
        {
            var listNode = CreateElement(collectionInfo.CollectionNode.Name, Node);
            foreach (var pair in list) {
                var childNode = CreateElement(collectionInfo.ElementNode.Name, listNode);
                childNode.SetAttribute(KeyAttributeName, pair.Key);
                var deserializer = new XmlSerializer(childNode);
                pair.Value.GetObjectData(deserializer);
            }
        }
    }
}