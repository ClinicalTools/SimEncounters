using SimEncounters.Data;
using System.Collections.Generic;

namespace SimEncounters.Xml
{
    public class TabFactory : ISerializationFactory<Tab>
    {
        protected virtual PanelFactory PanelFactory { get; } = new PanelFactory();

        protected virtual NodeInfo TypeInfo { get; } = new NodeInfo("type");
        protected virtual NodeInfo NameInfo { get; } = new NodeInfo("name");

        protected virtual CollectionXmlInfo PanelsInfo { get; } = new CollectionXmlInfo("panels", "panel");
        protected virtual CollectionXmlInfo ConditionsInfo { get; } = new CollectionXmlInfo("conditions", "condition");


        public void Serialize(XmlSerializer serializer, Tab tab)
        {
            serializer.AddString(TypeInfo, tab.Type);
            serializer.AddString(NameInfo, tab.Name);
            serializer.AddKeyValuePairs(PanelsInfo, tab.Panels, PanelFactory);
            serializer.AddStringList(ConditionsInfo, tab.Conditions);
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

            return new Tab(type, name, false);
        }

        protected virtual List<Panel> GetPanels(XmlDeserializer deserializer) 
            => deserializer.GetList(PanelsInfo, PanelFactory);
        protected virtual void AddPanels(XmlDeserializer deserializer, Tab tab)
        {
            var panels = GetPanels(deserializer);
            if (panels != null) {
                foreach (var panel in panels)
                    tab.Panels.Add(panel);
            }
        }

        protected virtual List<string> GetConditions(XmlDeserializer deserializer)
            => deserializer.GetStringList(ConditionsInfo);
        protected virtual void AddConditions(XmlDeserializer deserializer, Tab tab)
        {
            var conditions = GetConditions(deserializer);
            if (conditions != null) 
                tab.Conditions = conditions;
        }


    }
}