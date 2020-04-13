using System.Collections.Generic;
using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;
using UnityEngine;

namespace ClinicalTools.ClinicalEncounters.SerializationFactories
{
    public class ClinicalSectionFactory : SectionFactory
    {
        public ClinicalSectionFactory(ISerializationFactory<Tab> tabFactory, ISerializationFactory<ConditionalData> conditionalsFactory) 
            : base(tabFactory, conditionalsFactory) { }

        private readonly Color defaultColor = new Color(.0784f, .694f, .639f);
        public override Section Deserialize(XmlDeserializer deserializer)
        {
            var section = base.Deserialize(deserializer);
            if (section.Color == Color.clear)
                section.Color = defaultColor;

            return section;
        }

        protected NodeInfo LegacyNameFinder { get; } = new NodeInfo("sectionName");
        protected override string GetName(XmlDeserializer deserializer)
        {
            var name = base.GetName(deserializer);
            if (!string.IsNullOrWhiteSpace(name))
                return name;

            name = deserializer.GetString(LegacyNameFinder);
            if (!string.IsNullOrWhiteSpace(name))
                return name;

            // Some legacy names were stored in the tag name using the following format:
            // <_Name_Of_The_SectionSection>
            name = deserializer.GetName();
            return name.Remove(name.Length - "Section".Length)
                       .Replace('_', ' ')
                       .Replace(".2D.", " ")
                       .Trim();
        }

        protected NodeInfo LegacyIconKeyFinder { get; } = NodeInfo.RootName;
        protected override string GetIconKey(XmlDeserializer deserializer)
        {
            var iconKey = base.GetIconKey(deserializer);
            if (iconKey != null)
                return iconKey;
            iconKey = deserializer.GetString(LegacyIconKeyFinder);
            if (iconKey == null || iconKey.Length == 0)
                return null;

            if (iconKey[0] == '_')
                iconKey = iconKey.Substring(1);
            return iconKey;
        }

        protected CollectionInfo LegacyTabsInfo { get; } =
            new CollectionInfo(
                NodeInfo.RootName,
                new NodeInfo("Tab", TagComparison.NameEndsWith)
            );
        protected KeyValuePair<NodeInfo, NodeInfo> LegacyTabsKeyValueInfo { get; } =
            new KeyValuePair<NodeInfo, NodeInfo>(
                new NodeInfo("customTabName"),
                NodeInfo.RootValue
            );
        protected override List<KeyValuePair<string, Tab>> GetTabs(XmlDeserializer deserializer)
        {
            var tabs = base.GetTabs(deserializer);
            if (tabs != null && tabs.Count != 0)
                return tabs;

            tabs = deserializer.GetKeyValuePairs(LegacyTabsInfo, LegacyTabsKeyValueInfo, TabFactory);
            for (int i = 0; i < tabs.Count; i++) {
                var pair = tabs[i];
                if (!KeyGenerator.Instance.IsValidKey(pair.Key))
                    tabs[i] = new KeyValuePair<string, Tab>(KeyGenerator.Instance.Generate(pair.Key), pair.Value);
            }

            return tabs;
        }
    }
}