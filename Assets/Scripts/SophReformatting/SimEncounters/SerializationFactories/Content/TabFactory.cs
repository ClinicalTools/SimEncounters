using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class TabFactory : ISerializationFactory<Tab>
    {
        protected virtual PanelFactory PanelFactory { get; } = new PanelFactory();
        protected virtual ConditionalDataFactory ConditionalDataFactory { get; } = new ConditionalDataFactory();

        protected virtual NodeInfo TypeInfo { get; } = new NodeInfo("type");
        protected virtual NodeInfo NameInfo { get; } = new NodeInfo("name");
        protected virtual NodeInfo ConditionsInfo { get; } = new NodeInfo("conditions");

        protected virtual CollectionInfo PanelsInfo { get; } = new CollectionInfo("panels", "panel");

        public virtual bool ShouldSerialize(Tab value) => value != null;

        public void Serialize(XmlSerializer serializer, Tab value)
        {
            serializer.AddString(TypeInfo, value.Type);
            serializer.AddString(NameInfo, value.Name);
            serializer.AddKeyValuePairs(PanelsInfo, value.Panels, PanelFactory);
            serializer.AddValue(ConditionsInfo, value.Conditions, ConditionalDataFactory);
        }

        public virtual Tab Deserialize(XmlDeserializer deserializer)
        {
            var tab = CreateTab(deserializer);

            AddPanels(deserializer, tab);
            AddConditions(deserializer, tab);

            return tab;
        }

        protected virtual string GetType(XmlDeserializer deserializer)
            => deserializer.GetString(TypeInfo);
        protected virtual string GetName(XmlDeserializer deserializer)
            => deserializer.GetString(NameInfo);
        protected virtual Tab CreateTab(XmlDeserializer deserializer)
        {
            var type = GetType(deserializer);
            var name = GetName(deserializer);

            return new Tab(type, name);
        }

        protected virtual List<KeyValuePair<string, Panel>> GetPanels(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(PanelsInfo, PanelFactory);
        protected virtual void AddPanels(XmlDeserializer deserializer, Tab tab)
        {
            var panels = GetPanels(deserializer);
            if (panels == null)
                return;

            foreach (var panel in panels)
                tab.Panels.Add(panel);
        }

        protected virtual ConditionalData GetConditions(XmlDeserializer deserializer)
            => deserializer.GetValue(ConditionsInfo, ConditionalDataFactory);
        protected virtual void AddConditions(XmlDeserializer deserializer, Tab tab)
            => tab.Conditions = GetConditions(deserializer);
    }
}