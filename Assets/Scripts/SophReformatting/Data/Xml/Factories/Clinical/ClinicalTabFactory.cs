using System.Collections.Generic;
using SimEncounters.Data;
using SimEncounters.Xml;

namespace HealthImpactStudio.Xml
{
    public class ClinicalTabFactory : TabFactory
    {
        protected virtual NodeInfo LegacyTypeFinder { get; } = NodeInfo.RootName;
        protected override string GetType(XmlDeserializer deserializer)
        {
            var type = base.GetType(deserializer);

            if (string.IsNullOrWhiteSpace(type))
                type = deserializer.GetString(LegacyTypeFinder);

            return type;
        }

        protected virtual NodeInfo LegacyNameFinder { get; } = new NodeInfo("customTabName");
        protected override string GetName(XmlDeserializer deserializer)
        {
            var type = base.GetName(deserializer);

            if (string.IsNullOrWhiteSpace(type))
                type = deserializer.GetString(LegacyNameFinder);

            return type;
        }

        protected override PanelFactory PanelFactory => new ClinicalPanelFactory();

        protected virtual CollectionXmlInfo LegacyPanelsInfo { get; } = 
            new CollectionXmlInfo(
                new NodeInfo("data", TagComparison.NameEquals, new NodeInfo("EntryData")),
                new NodeInfo("Entry", TagComparison.NameStartsWith)
            );
        protected override List<Panel> GetPanels(XmlDeserializer deserializer)
        {
            var panels = base.GetPanels(deserializer);

            if (panels == null || panels.Count == 0)
                panels = deserializer.GetList(LegacyPanelsInfo, PanelFactory);

            return panels;
        }
    }
}