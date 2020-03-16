using System.Collections.Generic;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;

namespace ClinicalTools.ClinicalEncounters.SerializationFactories
{
    public class ClinicalEncounterDataFactory : EncounterDataFactory
    {
        protected override SectionFactory SectionFactory { get; } 
        public ClinicalEncounterDataFactory(EncounterImageData images) : base()
        {
            SectionFactory = new ClinicalSectionFactory(images);
        }

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
            if (sections != null && sections.Count != 0)
                return sections;

            sections = deserializer.GetKeyValuePairs(LegacySectionsInfo, LegacySectionKeyValueInfo, SectionFactory);
            if (sections == null)
                return null;
            for (int i = 0; i < sections.Count; i++) {
                var key = GetClinicalSectionKey(sections[i].Key);
                sections[i] = new KeyValuePair<string, Section>(key, sections[i].Value);
            }

            return sections;
        }

        protected string GetClinicalSectionKey(string key)
        {
            if (key == null || key.Length == 0)
                return key;
            else if (key[0] == '_')
                return key.Substring(1);
            else
                return key;
        }
    }
}