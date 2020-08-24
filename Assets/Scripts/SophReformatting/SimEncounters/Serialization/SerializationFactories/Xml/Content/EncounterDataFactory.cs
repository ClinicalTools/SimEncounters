
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class EncounterDataFactory : ISerializationFactory<EncounterNonImageContent>
    {
        protected virtual ISerializationFactory<Section> SectionFactory { get; }

        public EncounterDataFactory(ISerializationFactory<Section> sectionFactory)
        {
            SectionFactory = sectionFactory;
        }

        protected virtual CollectionInfo SectionsInfo { get; } = new CollectionInfo("sections", "section");


        public virtual bool ShouldSerialize(EncounterNonImageContent value) => value != null;

        public void Serialize(XmlSerializer serializer, EncounterNonImageContent value)
        {
            serializer.AddKeyValuePairs(SectionsInfo, value.Sections, SectionFactory);
        }

        public EncounterNonImageContent Deserialize(XmlDeserializer deserializer)
        {
            var encounterData = CreateEncounterData(deserializer);

            AddSections(deserializer, encounterData);

            return encounterData;
        }

        protected virtual EncounterNonImageContent CreateEncounterData(XmlDeserializer deserializer)
        {
            return new EncounterNonImageContent();
        }

        protected virtual List<KeyValuePair<string, Section>> GetSections(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(SectionsInfo, SectionFactory);
        protected virtual void AddSections(XmlDeserializer deserializer, EncounterNonImageContent encounterData)
        {
            var sectionPairs = GetSections(deserializer);
            if (sectionPairs == null)
                return;

            foreach (var sectionPair in sectionPairs)
                encounterData.Sections.Add(sectionPair);
        }
    }
}
