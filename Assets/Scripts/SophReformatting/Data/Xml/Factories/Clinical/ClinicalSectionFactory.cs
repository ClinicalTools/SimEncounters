using System.Collections.Generic;
using SimEncounters.Data;
using SimEncounters.Xml;

namespace HealthImpactStudio.Xml
{
    public class ClinicalSectionFactory : SectionFactory
    {
        protected static NodeInfo LegacyNameFinder { get; } = new NodeInfo("sectionName");
        protected override string GetName(XmlDeserializer deserializer)
        {
            var name = base.GetName(deserializer);
            if (!string.IsNullOrWhiteSpace(name))
                return name;

            name = deserializer.GetString(LegacyNameFinder);
            if (!string.IsNullOrWhiteSpace(name))
                return name;


            // Some legacy names were stored in the tag name using the following format:
            // <_Name_Of_The_SectionSection>
            name = deserializer.GetName()
                        .Remove(name.Length - "Section".Length)
                        .Replace('_', ' ')
                        .Trim();

            return name;
        }

        protected virtual CollectionXmlInfo LegacyTabsInfo { get; } = 
            new CollectionXmlInfo(
                NodeInfo.RootName,
                new NodeInfo("Tab", TagComparison.NameEndsWith)
            );
        protected override List<KeyValuePair<string, Tab>> GetTabs(XmlDeserializer deserializer)
        {
            var tabs = base.GetTabs(deserializer);
            if (tabs == null || tabs.Count == 0)
                tabs = deserializer.GetKeyValuePairs(LegacyTabsInfo, TabFactory);
            return tabs;
        }
    }
}