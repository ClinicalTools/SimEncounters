using SimEncounters.Data;
using System.Collections.Generic;

namespace SimEncounters.Xml
{
    public class SectionFactory : ISerializationFactory<Section>
    {
        protected virtual TabFactory TabFactory { get; } = new TabFactory();

        public virtual NodeInfo NameInfo { get; set; } = new NodeInfo("name");
        public virtual NodeInfo IconKeyInfo { get; } = new NodeInfo("icon");
        public virtual CollectionXmlInfo TabsInfo { get; } = new CollectionXmlInfo("tabs", "tab");
        protected virtual CollectionXmlInfo ConditionsInfo { get; } = new CollectionXmlInfo("conditions", "condition");

        public virtual void Serialize(XmlSerializer serializer, Section value)
        {
            serializer.AddString(NameInfo, value.Name);
            // As much as I'd love to store the icon directly in the section,  legacy data stores it in a seperate file.
            // I'd like to read both files concurrently, and I don't want to handle modern data too differently from legacy.
            serializer.AddString(IconKeyInfo, value.IconKey);
            serializer.AddList(TabsInfo, value.Tabs, TabFactory);
            serializer.AddStringList(ConditionsInfo, value.Conditions);
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

        protected virtual List<Tab> GetTabs(XmlDeserializer deserializer)
            => deserializer.GetList(TabsInfo, TabFactory);
        protected virtual void SetTabs(XmlDeserializer deserializer, Section section)
        {
            var tabs = GetTabs(deserializer);
            if (tabs != null) {
                foreach (var tab in tabs)
                    section.Tabs.Add(tab);
            }
        }

        protected virtual List<string> GetConditions(XmlDeserializer deserializer)
            => deserializer.GetStringList(ConditionsInfo);
        protected virtual void AddConditions(XmlDeserializer deserializer, Section section)
        {
            var conditions = GetConditions(deserializer);
            if (conditions != null)
                section.Conditions = conditions;
        }
    }
}