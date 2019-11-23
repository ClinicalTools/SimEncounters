using System;
using System.Collections.Generic;
using System.Xml;

namespace SimEncounters
{
    public class SectionCollection : OrderedCollection<Section>
    {
        public override string CollectionNodeName => "sections";

        protected override string ValueNodeName => "section";

        public SectionCollection() : base()
        {
            CreateDefaultSections();
        }

        public SectionCollection(XmlNode sectionsNode) : base(sectionsNode) { }

        // Adds default sections
        // This should be moved to the CE specific
        protected virtual void CreateDefaultSections()
        {
            DefaultDataScript dds = new DefaultDataScript();
            Section sds = new Section(dds.defaultTab);
            //sds.AddPersistingData(dds.defaultTab, "<data></data>");//.Replace(" ", "_") + "Tab", null); //Personal Info will always be saved
            //sds.AddPersistingData("Office Visit", "<data><EntryData><Parent></Parent><Entry0><PanelType>OfficeVisitPanel</PanelType><PanelData></PanelData></Entry0></EntryData></data>"); //office visit may never be visited, so construct null data
            //sds.SetCurrentTab(dds.defaultTab);
            Collection.Add(dds.defaultSection, sds);
        }

        // Legacy versions stored the section collections with 
        // "Sections" beginning with a capital 'S'
        protected override XmlNode GetLegacyCollectionNode(XmlNode encounterNode)
        {
            return encounterNode["Sections"];
        }

        protected override string GetLegacyKey(XmlNode valueNode)
        {
            return valueNode.Name;
        }

        protected override bool ValidLegacyNodeName(string valueNodeName)
        {
            return valueNodeName.EndsWith(ValueNodeName, StringComparison.OrdinalIgnoreCase);
        }

        protected override Section ReadValueNode(XmlNode valueNode)
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
        protected override void WriteValueElem(XmlElement valueElement, Section value)
        {
            throw new NotImplementedException();
        }
    }
}