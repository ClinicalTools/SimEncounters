using System.Collections.Generic;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;

namespace ClinicalTools.ClinicalEncounters.SerializationFactories
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

        protected virtual CollectionInfo LegacyDataInfo { get; } =
            new CollectionInfo(
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

        protected virtual CollectionInfo LegacyChildPanelsInfo { get; } =
            new CollectionInfo(
                new NodeInfo("PanelData", TagComparison.NameEquals,
                    new NodeInfo("data", TagComparison.NameEquals, new NodeInfo("EntryData"))),
                new NodeInfo("Entry", TagComparison.NameStartsWith)
            );
        protected override void AddChildPanels(XmlDeserializer deserializer, Panel panel)
        {
            base.AddChildPanels(deserializer, panel);
            if (panel.ChildPanels.Count != 0)
                return;

            var childPanels = deserializer.GetList(LegacyChildPanelsInfo, ChildPanelFactory);
            if (childPanels == null)
                return;

            foreach (var childPanel in childPanels)
                panel.ChildPanels.Add(childPanel);
        }
    }
}