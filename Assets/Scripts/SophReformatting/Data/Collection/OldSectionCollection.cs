using System;
using System.Collections.Generic;
using System.Xml;

namespace SimEncounters
{
    public class OldSectionCollection : OrderedCollection<SectionDataScript>
    {
        public override string CollectionNodeName => "sections";

        protected override string ValueNodeName => "section";

        public OldSectionCollection() : base() {
            CreateDefaultSections();
        }

        public OldSectionCollection(XmlNode sectionsNode) : base(sectionsNode) { }

        // Adds default sections
        // This should be moved to the CE specific
        protected virtual void CreateDefaultSections()
        {
            DefaultDataScript dds = new DefaultDataScript();
            SectionDataScript sds = new SectionDataScript();
            sds.SetPosition(0);
            sds.AddPersistingData(dds.defaultTab, "<data></data>");//.Replace(" ", "_") + "Tab", null); //Personal Info will always be saved
            sds.AddPersistingData("Office Visit", "<data><EntryData><Parent></Parent><Entry0><PanelType>OfficeVisitPanel</PanelType><PanelData></PanelData></Entry0></EntryData></data>"); //office visit may never be visited, so construct null data
            sds.SetCurrentTab(dds.defaultTab);
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
            return ConvertNameFromXML(valueNode.Name);
        }

        // Old section nodes were named "<name>Section"
        protected override bool ValidLegacyNodeName(string valueNodeName)
        {
            return valueNodeName.EndsWith(ValueNodeName, StringComparison.OrdinalIgnoreCase);
        }

        protected override SectionDataScript ReadValueNode(XmlNode valueNode)
        {
            SectionDataScript section = new SectionDataScript();
            section.Initiate();
            // TODO: stop using position like this
            section.SetPosition(Collection.Count);

            // Legacy way of handling section name
            section.Name = ConvertNameFromXML(valueNode.Name);
            string displayName = null;

            foreach (XmlNode child in valueNode) {
                if (child.Name == "name") {
                    section.Name = child.Value;
                } else if (child.Name == "displayName") {
                    displayName = child.Value;
                } else if (child.Name == "conditionals") {
                    section.Conditions = GetConditionalKeys(child);
                } else if (child.Name == "tabs") {
                    foreach (var tabNode in child) {
                        if (child.Name == "tab")
                            AddTab(section, child);
                    }

               // Legacy data
                } else if (child.Name == "sectionName") {
                    displayName = child.Value;
                } else if (child.Name.EndsWith("Tab", StringComparison.OrdinalIgnoreCase)) {
                    AddTab(section, child);
                }
            }

            var tabs = new TabCollection(valueNode);

            if (displayName == null)
                displayName = section.Name;
            section.SetSectionDisplayName(displayName);

            return section;
        }

        protected virtual void AddTab(SectionDataScript section, XmlNode tabNode)
        {
            string tabName = tabNode.Name;
            tabName = tabName.Substring(0, tabName.Length - 3);
            tabName = tabName.Replace('_', ' ');

            string customTabName = tabNode["customTabName"]?.InnerText;
            if (customTabName == null)
                customTabName = tabName;

            var data = tabNode["data"]?.InnerText;
            if (data == null)
                return;

            List<string> conditions = null;
            var conditionalsNode = tabNode["conditionals"];
            if (conditionalsNode != null)
                conditions = GetConditionalKeys(conditionalsNode);

            var persists = tabName.Equals("Personal Info") || tabName.Equals("Office Visit");
            if (persists)
                section.AddPersistingData(tabName, customTabName, tabNode.OuterXml, conditions);
            else
                section.AddData(tabName, customTabName, tabNode.OuterXml, conditions);
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


        // Write the XML
        protected override void WriteValueElem(XmlElement valueElement, SectionDataScript value)
        {
            throw new NotImplementedException();
        }
    }
}