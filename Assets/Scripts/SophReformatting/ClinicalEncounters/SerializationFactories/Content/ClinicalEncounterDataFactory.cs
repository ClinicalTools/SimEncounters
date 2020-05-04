using System.Collections.Generic;
using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;

namespace ClinicalTools.ClinicalEncounters.SerializationFactories
{
    public class ClinicalEncounterDataFactory : EncounterDataFactory
    {
        protected virtual IKeyGenerator KeyGenerator { get; }

        public ClinicalEncounterDataFactory(IKeyGenerator keyGenerator, 
            ISerializationFactory<Section> sectionFactory, ISerializationFactory<VariableData> variablesFactory)
            : base(sectionFactory, variablesFactory)
        {
            KeyGenerator = keyGenerator;
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
                sections[i] = new KeyValuePair<string, Section>(KeyGenerator.Generate(sections[i].Key), sections[i].Value);
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