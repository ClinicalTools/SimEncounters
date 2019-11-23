using System;
using System.Collections.Generic;
using System.Xml;

namespace SimEncounters
{
    public class SectionXmlHandler : XmlSerializer<Section>
    {
        public override string NodeName => "section";

        // Old section nodes were named "<name>Section"
        public override bool ValidNodeName(string name)
        {
            return base.ValidNodeName(name)
                || name.EndsWith(NodeName, StringComparison.OrdinalIgnoreCase);
        }

        public override Section Deserialize(XmlNode valueNode)
        {
            var name = valueNode["name"]?.InnerText;

            // Legacy way of handling section display name
            if (name == null)
                name = valueNode["sectionName"]?.InnerText;
            if (name == null)
                name = ConvertNameFromXML(valueNode.Name);

            List<string> conditions = null;
            var conditionalsNode = valueNode["conditionals"];
            if (conditionalsNode != null)
                conditions = GetConditionalKeys(conditionalsNode);

            var tabs = new TabCollection(valueNode);

            return new Section(name, conditions, tabs);
        }


        /// <summary>
        /// Converts a formatted XML name into regular text.
        /// </summary>
        /// <param name="name">Name of the xml tag</param>
        /// <returns>An unformatted version of the name.</returns>
        protected virtual string ConvertNameFromXML(string name)
        {
            if (name.StartsWith("_")) {
                name = name.Substring(1); //Remove starting underscore
            }

            for (int pos = 0; pos < name.Length; pos++) {
                string currentChar = name.ToCharArray()[pos] + "";
                if (currentChar.Equals(".")) {
                    string character = char.ConvertFromUtf32(Convert.ToInt32(name.Substring(pos + 1, 2), 16));
                    name = name.Replace(name.Substring(pos, 4), character);
                }
            }
            return name;
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

        protected override void SerializeValues(XmlElement valueElement, Section value)
        {
            throw new System.NotImplementedException();
        }
    }
}
