using System;
using System.Collections.Generic;
using System.Xml;

namespace SimEncounters.Xml
{
    public class XmlDeserializer
    {
        protected virtual XmlNode Node { get; }

        public XmlDeserializer(XmlNode xmlNode)
        {
            Node = xmlNode;
        }

        public virtual string GetName()
        {
            return Node.Name;
        }

        public virtual string GetString(string valueName)
            => GetString(new NodeFinder(valueName));
        public virtual string GetString(NodeFinder valueFinder)
        {
            var valueNode = valueFinder.GetNode(Node);
            var value = valueFinder.GetText(valueNode);
            if (string.IsNullOrWhiteSpace(value))
                return null;

            return value;
        }

        public virtual T GetValue<T>(string valueName) where T : IXmlSerializable
            => GetValue<T>(new NodeFinder(valueName));
        public virtual T GetValue<T>(NodeFinder valueFinder)
            where T : IXmlSerializable
        {
            var valueNode = valueFinder.GetNode(Node);
            if (valueNode == null)
                return default;

            var serializationInfo = new XmlDeserializer(valueNode);
            return (T)Activator.CreateInstance(typeof(T), serializationInfo);
        }

        protected virtual IEnumerable<XmlNode> GetElementNodes(NodeFinder valueFinder,
            NodeFinder elementFinder)
        {
            var valueNode = valueFinder.GetNode(Node);
            if (valueNode == null)
                return null;

            return elementFinder.GetNodeList(valueNode);
        }

        public virtual List<string> GetStringList(string valueName, string elementName)
            => GetStringList(new NodeFinder(valueName), new NodeFinder(elementName));
        public virtual List<string> GetStringList(NodeFinder valueFinder, NodeFinder elementFinder)

        {
            var nodes = GetElementNodes(valueFinder, elementFinder);
            if (nodes == null)
                return null;

            var list = new List<string>();
            foreach (var node in nodes) {
                var value = elementFinder.GetText(node);
                if (value == null)
                    list.Add(value);
            }

            return list;
        }

        public virtual List<T> GetList<T>(string valueName, string elementName)
            where T : IXmlSerializable
            => GetList<T>(new NodeFinder(valueName), new NodeFinder(elementName));
        public virtual List<T> GetList<T>(NodeFinder valueFinder, NodeFinder elementFinder)
            where T : IXmlSerializable
        {
            var nodes = GetElementNodes(valueFinder, elementFinder);
            if (nodes == null)
                return null;

            var list = new List<T>();
            foreach (var node in nodes) {
                var childSerializationInfo = new XmlSerializationInfo(node);
                var value = (T)Activator.CreateInstance(typeof(T), childSerializationInfo);
                if (value != null)
                    list.Add(value);
            }

            return list;
        }

        protected readonly NodeFinder DefaultKeyFinder = new NodeFinder("key", TagComparison.AttributeNameEquals);
        public virtual List<KeyValuePair<string, T>> GetKeyValuePairs<T>(string valueName, string elementName)
            where T : IXmlSerializable
            => GetKeyValuePairs<T>(new NodeFinder(valueName), new NodeFinder(elementName),
                DefaultKeyFinder);
        public virtual List<KeyValuePair<string, T>> GetKeyValuePairs<T>(
            NodeFinder valueFinder, NodeFinder elementFinder,
            NodeFinder keyFinder)
            where T : IXmlSerializable
        {
            var elementNodes = GetElementNodes(valueFinder, elementFinder);
            if (elementNodes == null)
                return null;

            var pairs = new List<KeyValuePair<string, T>>();
            foreach (var node in elementNodes) {
                var keyNode = keyFinder.GetNode(node);
                var key = keyFinder.GetText(keyNode);
                if (string.IsNullOrWhiteSpace(key))
                    continue;

                var childSerializationInfo = new XmlDeserializer(node);
                var value = (T)Activator.CreateInstance(typeof(T), childSerializationInfo);
                if (value == null)
                    continue;

                pairs.Add(new KeyValuePair<string, T>(key, value));
            }

            return pairs;
        }

    }
}