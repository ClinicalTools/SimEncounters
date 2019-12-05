using ClinicalTools.SimEncounters.XmlSerialization;
using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class PanelFactory : ISerializationFactory<Panel>
    {
        protected virtual PanelFactory ChildPanelFactory => this;
        protected virtual ConditionalDataFactory ConditionalDataFactory { get; } = new ConditionalDataFactory();


        protected virtual NodeInfo TypeInfo { get; } = new NodeInfo("type");
        protected virtual NodeInfo ConditionsInfo { get; } = new NodeInfo("conditions");

        protected virtual CollectionInfo DataInfo { get; } = new CollectionInfo("values", "value");
        protected virtual CollectionInfo ChildPanelsInfo { get; } = new CollectionInfo("panels", "panel");

        public virtual bool ShouldSerialize(Panel value) => value != null;

        public void Serialize(XmlSerializer serializer, Panel value)
        {
            serializer.AddString(TypeInfo, value.Type);
            serializer.AddStringKeyValuePairs(DataInfo, value.Data);
            serializer.AddKeyValuePairs(ChildPanelsInfo, value.ChildPanels, ChildPanelFactory);
            serializer.AddValue(ConditionsInfo, value.Conditions, ConditionalDataFactory);
        }

        public virtual Panel Deserialize(XmlDeserializer deserializer)
        {
            var panel = CreatePanel(deserializer);

            AddData(deserializer, panel);
            AddChildPanels(deserializer, panel);
            AddConditions(deserializer, panel);

            return panel;
        }

        protected virtual string GetType(XmlDeserializer deserializer)
            => deserializer.GetString(TypeInfo);
        protected virtual Panel CreatePanel(XmlDeserializer deserializer)
        {
            var type = GetType(deserializer);

            return new Panel(type);
        }

        protected virtual List<KeyValuePair<string, string>> GetDataPairs(XmlDeserializer deserializer)
            => deserializer.GetStringKeyValuePairs(DataInfo);
        protected virtual void AddData(XmlDeserializer deserializer, Panel panel)
        {
            var dataPairs = GetDataPairs(deserializer);
            if (dataPairs != null) {
                foreach (var pair in dataPairs)
                    panel.Data.Add(pair);
            }
        }

        protected virtual List<KeyValuePair<string, Panel>> GetChildPanels(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(ChildPanelsInfo, ChildPanelFactory);
        protected virtual void AddChildPanels(XmlDeserializer deserializer, Panel panel)
        {
            var childPanels = GetChildPanels(deserializer);
            if (childPanels != null) {
                foreach (var childPanel in childPanels)
                    panel.ChildPanels.Add(childPanel);
            }
        }

        protected virtual ConditionalData GetConditions(XmlDeserializer deserializer)
            => deserializer.GetValue(ConditionsInfo, ConditionalDataFactory);
        protected virtual void AddConditions(XmlDeserializer deserializer, Panel panel)
            => panel.Conditions = GetConditions(deserializer);
    }
}