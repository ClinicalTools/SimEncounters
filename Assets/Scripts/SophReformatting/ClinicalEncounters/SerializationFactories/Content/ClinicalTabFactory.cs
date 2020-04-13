using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.XmlSerialization;
using ClinicalTools.SimEncounters.SerializationFactories;

namespace ClinicalTools.ClinicalEncounters.SerializationFactories
{
    public class ClinicalTabFactory : TabFactory
    {
        public ClinicalTabFactory(ISerializationFactory<ConditionalData> conditionalsFactory, ISerializationFactory<Panel> panelFactory) 
            : base(conditionalsFactory, panelFactory) { }

        protected NodeInfo LegacyTypeFinder { get; } = NodeInfo.RootName;
        protected override string GetType(XmlDeserializer deserializer)
        {
            var type = base.GetType(deserializer);
            if (!string.IsNullOrWhiteSpace(type))
                return type;

            type = deserializer.GetString(LegacyTypeFinder);
            var suffix = "Tab";
            if (type.EndsWith(suffix))
                type = type.Substring(0, type.Length - suffix.Length);
            type = type.Replace('_', ' ');

            return type;
        }

        protected NodeInfo LegacyNameFinder { get; } = new NodeInfo("customTabName");
        protected override string GetName(XmlDeserializer deserializer)
        {
            var type = base.GetName(deserializer);

            if (string.IsNullOrWhiteSpace(type))
                type = deserializer.GetString(LegacyNameFinder);

            return type;
        }

        protected CollectionInfo LegacyPanelsInfo { get; } =
            new CollectionInfo(
                new NodeInfo("data", TagComparison.NameEquals, new NodeInfo("EntryData")),
                new NodeInfo("Entry", TagComparison.NameStartsWith)
            );
        protected override void AddPanels(XmlDeserializer deserializer, Tab tab)
        {
            base.AddPanels(deserializer, tab);
            if (tab.Panels.Count != 0)
                return;

            var panels = deserializer.GetList(LegacyPanelsInfo, PanelFactory);
            if (panels == null)
                return;

            foreach (var panel in panels)
                tab.Panels.Add(panel);
        }
    }
}