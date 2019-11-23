using System;
using System.Collections.Generic;
using System.Xml;

namespace SimEncounters
{
    public class TabCollection : OrderedCollection<Tab>
    {
        public override string CollectionNodeName => "tabs";

        protected override string ValueNodeName => "tab";

        public TabCollection() : base() { }

        public TabCollection(XmlNode sectionsNode) : base(sectionsNode) { }


        // Legacy versions stored the tabs directly in the section node
        protected override XmlNode GetLegacyCollectionNode(XmlNode sectionNode)
        {
            return sectionNode;
        }

        protected override string GetLegacyKey(XmlNode valueNode)
        {
            return valueNode["customTabName"]?.InnerText;
        }

        // Old section nodes were named "<name>Tab"
        protected override bool ValidLegacyNodeName(string valueNodeName)
        {
            return valueNodeName.EndsWith(ValueNodeName, StringComparison.OrdinalIgnoreCase);
        }

        protected override Tab ReadValueNode(XmlNode valueNode)
        {
            var type = valueNode["type"]?.InnerText;
            // Legacy method of getting type
            if (type == null)
                type = valueNode.Name;

            var name = valueNode["name"]?.InnerText;
            // Legacy method of getting name
            if (string.IsNullOrWhiteSpace(name))
                name = valueNode["customTabName"]?.InnerText;
            
            var persists = name.Equals("Personal Info") || name.Equals("Office Visit");
            var data = valueNode["data"];

            List<string> conditions = null;
            var conditionalsNode = valueNode["conditionals"];
            if (conditionalsNode != null)
                conditions = GetConditionalKeys(conditionalsNode);

            return new Tab(type, name, persists, data, conditions);
        }

        protected virtual List<string> GetConditionalKeys(XmlNode conditionalsNode)
        {
            var condKeys = new List<string>();
            foreach (XmlNode cond in conditionalsNode.ChildNodes) {
                if (cond.Name == "cond") {
                    condKeys.Add(cond.Value);
                }
            }

            return condKeys;
        }
        
        // Write the XML
        protected override void WriteValueElem(XmlElement valueElement, Tab value)
        {
            throw new NotImplementedException();
        }
    }
}