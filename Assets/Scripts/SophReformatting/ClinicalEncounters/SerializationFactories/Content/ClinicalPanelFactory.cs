using System.Collections.Generic;
using System.Linq;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;

namespace ClinicalTools.ClinicalEncounters.SerializationFactories
{
    public class ClinicalPanelFactory : PanelFactory
    {
        protected override PinDataFactory PinDataFactory { get; }
        public ClinicalPanelFactory(ConditionalDataFactory conditionalDataFactory) : base(conditionalDataFactory) { 
            PinDataFactory = new ClinicalPinDataFactory(this);
        }

        protected NodeInfo LegacyTypeInfo { get; } = new NodeInfo("PanelType");
        protected override string GetType(XmlDeserializer deserializer)
        {
            var type = base.GetType(deserializer);

            if (!string.IsNullOrWhiteSpace(type))
                return type;

            type = deserializer.GetString(LegacyTypeInfo);
            if (type == "OtherPastTestEntryPanel")
                return "Other Past Medical Tests";

            return type;
        }

        protected CollectionInfo LegacyDataInfo { get; } =
            new CollectionInfo(
                new NodeInfo("PanelData"),
                new NodeInfo("data", TagComparison.NameNotEqualTo)
            );
        protected KeyValuePair<NodeInfo, NodeInfo> LegacyDataKeyValueInfo { get; } =
            new KeyValuePair<NodeInfo, NodeInfo>(NodeInfo.RootName, NodeInfo.RootValue);
        protected override List<KeyValuePair<string, string>> GetDataPairs(XmlDeserializer deserializer)
        {
            var dataPairs = base.GetDataPairs(deserializer);
            if (dataPairs != null && dataPairs.Count > 0)
                return dataPairs;


            dataPairs = deserializer.GetStringKeyValuePairs(LegacyDataInfo, LegacyDataKeyValueInfo);
            if (dataPairs == null)
                return null;

            // Pins were previously stored with the data, so they have to be removed
            var legacyData = new List<KeyValuePair<string, string>>(dataPairs.Count);
            foreach (var pair in dataPairs.Where(pair => !pair.Key.EndsWith("Pin")))
                legacyData.Add(pair);


            return legacyData;
        }

        protected CollectionInfo LegacyChildPanelsInfo { get; } =
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

        protected NodeInfo LegacyPinsInfo { get; } = new NodeInfo("PanelData");
        protected override PinData GetPins(XmlDeserializer deserializer)
        {
            var pins = base.GetPins(deserializer);
            if (pins == null)
                pins = deserializer.GetValue(LegacyPinsInfo, PinDataFactory);
            
            return pins;
        }
    }
}