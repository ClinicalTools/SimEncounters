using System.Collections.Generic;
using UnityEngine;
using SimEncounters.Xml;

namespace SimEncounters
{
    public class Section : IXmlSerializable
    {
        public Tab CurrentTab { get; protected set; }

        public virtual string Name { get; set; }
        public virtual List<string> Conditions { get; set; }
        public virtual SpriteHolderScript SpriteHolder { get; }
        public virtual OrderedDictionary<Tab> Tabs { get; }

        public Section(string name) {
            Name = name;
            Tabs = new OrderedDictionary<Tab>();
        }
        public Section(string name, List<string> conditions, OrderedDictionary<Tab> tabs)
        {
            Name = name;
            Conditions = conditions;
            Tabs = tabs;
        }

        public Section(XmlDeserializer deserializer)
        {
            Name = deserializer.GetString("name");
            Conditions = deserializer.GetStringList("conditions", "condition");
            var tabs = deserializer.GetKeyValuePairs<Tab>("tabs", "tab");
            if (tabs == null)
                Tabs = new OrderedDictionary<Tab>();
            else
                Tabs = new OrderedDictionary<Tab>(tabs);
        }

        public void GetObjectData(XmlSerializer serializer)
        {
            serializer.AddString("name", Name);
            serializer.AddStringList("conditions", "condition", Conditions);
            serializer.AddKeyValuePairs("tabs", "tab", Tabs);
        }

        /**
         * Updates the current tab
         */
        public void SetCurrentTab(string tabName)
        {
            if (Tabs.ContainsKey(tabName))
                CurrentTab = Tabs[tabName];
            else
                Debug.LogError("Could not find " + tabName + " in Dict");
        }

    }
}