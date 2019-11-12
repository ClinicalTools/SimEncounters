using System.Xml;
using UnityEngine;

namespace SimEncounters
{
    public class XmlHelper
    {
        private const bool NODE_NAME_FIRST_LETTER_CAPITALIZED = false;
        private static readonly XmlDocument xmlDoc = new XmlDocument();

        public static XmlElement CreateElement(string name, XmlElement parent = null)
        {
            if (NODE_NAME_FIRST_LETTER_CAPITALIZED && !char.IsUpper(name[0]))
                Debug.LogWarning($"XML element name should be capitalized: {name}");
            if (!NODE_NAME_FIRST_LETTER_CAPITALIZED && char.IsUpper(name[0]))
                Debug.LogWarning($"XML element name should not be capitalized: {name}");

            var node = xmlDoc.CreateElement(name);

            if (parent != null)
                parent.AppendChild(node);

            return node;
        }

        public static XmlElement CreateElement(string name, string value, XmlElement parent = null)
        {
            var node = CreateElement(name, parent);
            node.InnerText = value;

            return node;
        }
    }
}
