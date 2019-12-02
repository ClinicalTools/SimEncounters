using SimEncounters.Data;
using System.Collections.Generic;

namespace SimEncounters.Xml
{
    public class PanelFactory : ISerializationFactory<Panel>
    {
        protected virtual PanelFactory ChildPanelFactory => this;

        protected virtual NodeInfo TypeInfo { get; } = new NodeInfo("type");

        protected virtual CollectionXmlInfo DataInfo { get; } = new CollectionXmlInfo("values", "value");
        protected virtual CollectionXmlInfo ChildPanelsInfo { get; } = new CollectionXmlInfo("panels", "panel");

        public void Serialize(XmlSerializer serializer, Panel panel)
        {
            serializer.AddString(TypeInfo, panel.Type);
            serializer.AddStringKeyValuePairs(DataInfo, panel.Data);
            serializer.AddList(ChildPanelsInfo, panel.ChildPanels, ChildPanelFactory);
        }

        public virtual Panel Deserialize(XmlDeserializer deserializer)
        {
            var panel = CreatePanel(deserializer);

            AddData(deserializer, panel);

            AddChildPanels(deserializer, panel);

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

        protected virtual List<Panel> GetChildPanels(XmlDeserializer deserializer) 
            => deserializer.GetList(ChildPanelsInfo, ChildPanelFactory);
        protected virtual void AddChildPanels(XmlDeserializer deserializer, Panel panel)
        {
            var childPanels = GetChildPanels(deserializer);
            if (childPanels != null) {
                foreach (var childPanel in childPanels)
                    panel.ChildPanels.Add(childPanel);
            }
        }
    }
}