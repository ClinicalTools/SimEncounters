using System.Collections.Generic;
using ClinicalTools.SimEncounters;
using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;

namespace ClinicalTools.ClinicalEncounters.SerializationFactories
{
    public class ClinicalSectionFactory : SectionFactory
    {
        protected virtual IKeyGenerator KeyGenerator { get; }

        public ClinicalSectionFactory(IKeyGenerator keyGenerator, 
            ISerializationFactory<Tab> tabFactory)
            : base(tabFactory)
        {
            KeyGenerator = keyGenerator;
        }

        protected override Section CreateSection(XmlDeserializer deserializer)
        {
            var name = GetName(deserializer);
            var iconKey = GetIconKey(deserializer);
            if (string.IsNullOrEmpty(iconKey)) {
                var legacyIconKey = GetLegacyIconKey(deserializer);
                return new CESection(name, legacyIconKey);
            }

            var color = GetColor(deserializer);
            return new Section(name, iconKey, color);
        }

        protected NodeInfo LegacyIconKeyFinder { get; } = NodeInfo.RootName;
        protected virtual string GetLegacyIconKey(XmlDeserializer deserializer)
        {
            var iconKey = deserializer.GetString(LegacyIconKeyFinder);
            if (iconKey == null || iconKey.Length == 0)
                return null;

            if (iconKey[0] == '_')
                iconKey = iconKey.Substring(1);
            return iconKey
                .Replace(".2D.", "-")
                .Replace(".2C.", ",");
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
                       .Replace(".2C.", ",")
                       .Trim();
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
                tabs[i] = new KeyValuePair<string, Tab>(KeyGenerator.Generate(pair.Key), pair.Value);
            }

            return tabs;
        }
    }
}