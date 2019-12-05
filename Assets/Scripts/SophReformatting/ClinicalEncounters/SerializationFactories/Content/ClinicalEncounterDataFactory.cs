using System.Collections.Generic;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;

namespace ClinicalTools.ClinicalEncounters.SerializationFactories
{
    public class ClinicalEncounterDataFactory : EncounterDataFactory
    {
        protected override SectionFactory SectionFactory { get; } = new ClinicalSectionFactory();

        protected virtual CollectionInfo LegacySectionsInfo { get; } =
            new CollectionInfo(
                new NodeInfo("Sections"),
                new NodeInfo("Section", TagComparison.NameEndsWith)
            );
        protected virtual KeyValuePair<NodeInfo, NodeInfo> LegacySectionKeyValueInfo { get; } =
            new KeyValuePair<NodeInfo, NodeInfo>(NodeInfo.RootName, NodeInfo.RootValue);
        protected override List<KeyValuePair<string, Section>> GetSections(XmlDeserializer deserializer)
        {
            var sections = base.GetSections(deserializer);
            if (sections == null || sections.Count == 0)
                sections = deserializer.GetKeyValuePairs(LegacySectionsInfo, LegacySectionKeyValueInfo, SectionFactory);

            return sections;
        }
    }
}