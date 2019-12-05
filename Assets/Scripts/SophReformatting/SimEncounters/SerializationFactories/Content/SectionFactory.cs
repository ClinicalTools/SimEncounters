using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class SectionFactory : ISerializationFactory<Section>
    {
        protected virtual TabFactory TabFactory { get; } = new TabFactory();
        protected virtual ConditionalDataFactory ConditionalDataFactory { get; } = new ConditionalDataFactory();

        protected virtual NodeInfo NameInfo { get; set; } = new NodeInfo("name");
        protected virtual NodeInfo IconKeyInfo { get; } = new NodeInfo("icon");
        protected virtual NodeInfo ConditionsInfo { get; } = new NodeInfo("conditions");
        protected virtual CollectionInfo TabsInfo { get; } = new CollectionInfo("tabs", "tab");

        public virtual bool ShouldSerialize(Section value) => value != null;

        public virtual void Serialize(XmlSerializer serializer, Section value)
        {
            serializer.AddString(NameInfo, value.Name);
            // As much as I'd love to store the icon directly in the section,  legacy data stores it in a seperate file.
            // I'd like to read both files concurrently, and I don't want to handle modern data too differently from legacy.
            serializer.AddString(IconKeyInfo, value.IconKey);
            serializer.AddKeyValuePairs(TabsInfo, value.Tabs, TabFactory);
            serializer.AddValue(ConditionsInfo, value.Conditions, ConditionalDataFactory);
        }

        public virtual Section Deserialize(XmlDeserializer deserializer)
        {
            var section = CreateSection(deserializer);

            SetTabs(deserializer, section);
            AddConditions(deserializer, section);

            return section;
        }

        protected virtual string GetName(XmlDeserializer deserializer)
            => deserializer.GetString(NameInfo);
        protected virtual string GetIconKey(XmlDeserializer deserializer)
            => deserializer.GetString(IconKeyInfo);
        protected virtual Section CreateSection(XmlDeserializer deserializer)
        {
            var name = GetName(deserializer);
            var iconKey = GetIconKey(deserializer);

            return new Section(name, iconKey);
        }

        protected virtual List<KeyValuePair<string, Tab>> GetTabs(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(TabsInfo, TabFactory);
        protected virtual void SetTabs(XmlDeserializer deserializer, Section section)
        {
            var tabs = GetTabs(deserializer);
            if (tabs == null)
                return;

            foreach (var tab in tabs)
                section.Tabs.Add(tab);
        }

        protected virtual ConditionalData GetConditions(XmlDeserializer deserializer)
            => deserializer.GetValue(ConditionsInfo, ConditionalDataFactory);
        protected virtual void AddConditions(XmlDeserializer deserializer, Section section)
            => section.Conditions = GetConditions(deserializer);
    }
}