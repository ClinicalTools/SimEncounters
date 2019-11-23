using System;
using System.Collections.Generic;
using System.Xml;

namespace SimEncounters
{
    public class TabXmlHandler : XmlSerializer<Tab>
    {
        public override string NodeName => "tab";

        protected virtual StringListXmlHandler ConditionalKeysSerializer { get; }

        public TabXmlHandler(StringListXmlHandler conditionalKeysSerializer)
        {
            ConditionalKeysSerializer = conditionalKeysSerializer;
        }

        // Old section nodes were named "<name>Tab"
        public override bool ValidNodeName(string name)
        {
            return base.ValidNodeName(name)
                || name.EndsWith(NodeName, StringComparison.OrdinalIgnoreCase);
        }

        public override Tab Deserialize(XmlNode valueNode)
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
            var conditionalsNode = valueNode[ConditionalKeysSerializer.NodeName];
            if (conditionalsNode != null)
                conditions = ConditionalKeysSerializer.Deserialize(conditionalsNode);

            return new Tab(type, name, persists, data, conditions);
        }

        protected override void SerializeValues(XmlElement valueElement, Tab value)
        {
            throw new NotImplementedException();
        }
    }
}