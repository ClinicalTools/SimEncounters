using System.Collections.Generic;
using System.Xml;

namespace ClinicalTools.SimEncounters
{
    public class StringListXmlHandler : XmlSerializer<List<string>>
    {
        public override string NodeName { get; }
        public string ElementNodeName { get; }

        public StringListXmlHandler(string listNodeName, string elementNodeName) : base()
        {
            NodeName = listNodeName;
            ElementNodeName = elementNodeName;
        }

        public override List<string> Deserialize(XmlNode valueNode)
        {
            var list = new List<string>();
            foreach (XmlNode cond in valueNode.ChildNodes) {
                if (cond.Name == NodeName) {
                    list.Add(cond.Value);
                }
            }

            return list;
        }

        protected override void SerializeValues(XmlElement valueElement, List<string> value)
        {
            throw new System.NotImplementedException();
        }
    }
}