using System.Collections.Generic;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;

namespace ClinicalTools.ClinicalEncounters.SerializationFactories
{
    public class ClinicalSectionFactory : SectionFactory
    {
        protected override TabFactory TabFactory { get; }
        public ClinicalSectionFactory()
        {
            TabFactory = new ClinicalTabFactory(ConditionalDataFactory);
        }

        protected NodeInfo LegacyNameFinder { get; } = new NodeInfo("sectionName");
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
            name = deserializer.GetName();
            return name.Remove(name.Length - "Section".Length)
                       .Replace('_', ' ')
                       .Trim();
        }

        protected CollectionInfo LegacyTabsInfo { get; } =
            new CollectionInfo(
                NodeInfo.RootName,
                new NodeInfo("Tab", TagComparison.NameEndsWith)
            );
        protected KeyValuePair<NodeInfo, NodeInfo> LegacyTabsKeyValueInfo { get; } =
            new KeyValuePair<NodeInfo, NodeInfo>(
                new NodeInfo("customTabName"),
                NodeInfo.RootValue
            );
        protected override List<KeyValuePair<string, Tab>> GetTabs(XmlDeserializer deserializer)
        {
            var tabs = base.GetTabs(deserializer);
            if (tabs == null || tabs.Count == 0)
                tabs = deserializer.GetKeyValuePairs(LegacyTabsInfo, LegacyTabsKeyValueInfo, TabFactory);
            return tabs;
        }
    }
}