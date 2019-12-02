using System.Collections.Generic;
using SimEncounters.Data;
using SimEncounters.Xml;

namespace HealthImpactStudio.Xml
{
    public class ClinicalPanelFactory : PanelFactory
    {
        protected virtual NodeInfo LegacyTypeInfo { get; } = new NodeInfo("PanelType");
        protected override string GetType(XmlDeserializer deserializer)
        {
            var type = base.GetType(deserializer);

            if (string.IsNullOrWhiteSpace(type))
                type = deserializer.GetString(LegacyTypeInfo);

            return type;
        }

        protected virtual CollectionXmlInfo LegacyDataInfo { get; } =
            new CollectionXmlInfo(
                new NodeInfo("PanelData"),
                new NodeInfo("data", TagComparison.NameNotEqualTo)
            );
        protected virtual KeyValuePair<NodeInfo, NodeInfo> LegacyDataKeyValueInfo { get; } = 
            new KeyValuePair<NodeInfo, NodeInfo>(NodeInfo.RootName, NodeInfo.RootValue);
        protected override List<KeyValuePair<string, string>> GetDataPairs(XmlDeserializer deserializer)
        {
            var dataPairs = base.GetDataPairs(deserializer);
            if (dataPairs == null || dataPairs.Count == 0)
                dataPairs = deserializer.GetStringKeyValuePairs(LegacyDataInfo, LegacyDataKeyValueInfo);
            return dataPairs;
        }

        protected virtual CollectionXmlInfo LegacyPanelsInfo { get; } =
            new CollectionXmlInfo(
                new NodeInfo("PanelData", TagComparison.NameEquals,
                    new NodeInfo("data", TagComparison.NameEquals, new NodeInfo("EntryData"))),
                new NodeInfo("Entry", TagComparison.NameStartsWith)
            );
        protected override List<Panel> GetChildPanels(XmlDeserializer deserializer)
        {

            var childPanels = base.GetChildPanels(deserializer);
            if (childPanels == null || childPanels.Count == 0)
                childPanels = deserializer.GetList(LegacyPanelsInfo, ChildPanelFactory);
            return childPanels;
        }

    }
}