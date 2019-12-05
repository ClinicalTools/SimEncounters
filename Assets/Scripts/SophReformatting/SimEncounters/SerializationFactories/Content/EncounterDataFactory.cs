﻿using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class EncounterDataFactory : ISerializationFactory<EncounterData>
    {
        protected virtual SectionFactory SectionFactory { get; } = new SectionFactory();
        protected virtual VariableDataFactory VariablesFactory { get; } = new VariableDataFactory();

        protected virtual NodeInfo VariablesInfo { get; } = new NodeInfo("variables");

        protected virtual CollectionInfo SectionsInfo { get; } = new CollectionInfo("sections", "section");


        public virtual bool ShouldSerialize(EncounterData value) => value != null;

        public void Serialize(XmlSerializer serializer, EncounterData value)
        {
            serializer.AddValue(VariablesInfo, value.Variables, VariablesFactory);
            serializer.AddKeyValuePairs(SectionsInfo, value.Sections, SectionFactory);
        }

        public EncounterData Deserialize(XmlDeserializer deserializer)
        {
            var encounterData = CreateEncounterData(deserializer);

            AddSections(deserializer, encounterData);

            return encounterData;
        }

        protected virtual VariableData GetVariables(XmlDeserializer deserializer)
            => deserializer.GetValue(VariablesInfo, VariablesFactory);
        protected virtual EncounterData CreateEncounterData(XmlDeserializer deserializer)
        {
            var variables = GetVariables(deserializer);

            return new EncounterData(variables);
        }

        protected virtual List<KeyValuePair<string, Section>> GetSections(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(SectionsInfo, SectionFactory);
        protected virtual void AddSections(XmlDeserializer deserializer, EncounterData encounterData)
        {
            var sectionPairs = GetSections(deserializer);
            if (sectionPairs == null)
                return;

            foreach (var sectionPair in sectionPairs)
                encounterData.Sections.Add(sectionPair);
        }
    }
}
