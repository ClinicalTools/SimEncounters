using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class SectionFactory : ISerializationFactory<Section>
    {
        protected virtual ISerializationFactory<Tab> TabFactory { get; }
        public SectionFactory(ISerializationFactory<Tab> tabFactory)
        {
            TabFactory = tabFactory;
        }

        protected virtual NodeInfo NameInfo { get; set; } = new NodeInfo("name");
        protected virtual NodeInfo IconKeyInfo { get; } = new NodeInfo("icon");
        protected virtual NodeInfo ColorInfo { get; } = new NodeInfo("color");
        protected virtual NodeInfo ConditionsInfo { get; } = new NodeInfo("conditions");
        protected virtual CollectionInfo TabsInfo { get; } = new CollectionInfo("tabs", "tab");


        public virtual bool ShouldSerialize(Section value) => value != null;

        public virtual void Serialize(XmlSerializer serializer, Section value)
        {
            serializer.AddString(NameInfo, value.Name);
            serializer.AddString(IconKeyInfo, value.IconKey);
            serializer.AddColor(ColorInfo, value.Color);
            serializer.AddKeyValuePairs(TabsInfo, value.Tabs, TabFactory);
        }

        public virtual Section Deserialize(XmlDeserializer deserializer)
        {
            var section = CreateSection(deserializer);

            SetTabs(deserializer, section);

            return section;
        }

        protected virtual string GetName(XmlDeserializer deserializer)
            => deserializer.GetString(NameInfo);
        protected virtual string GetIconKey(XmlDeserializer deserializer)
            => deserializer.GetString(IconKeyInfo);
        protected virtual Color GetColor(XmlDeserializer deserializer)
            => deserializer.GetColor(ColorInfo);
        protected virtual Section CreateSection(XmlDeserializer deserializer)
        {
            var name = GetName(deserializer);
            var iconKey = GetIconKey(deserializer);
            var color = GetColor(deserializer);

            return new Section(name, iconKey, color);
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
    }
}