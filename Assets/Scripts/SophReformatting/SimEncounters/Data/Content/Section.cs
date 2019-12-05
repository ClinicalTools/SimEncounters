using UnityEngine;

namespace ClinicalTools.SimEncounters.Data
{
    public class Section
    {
        public Tab CurrentTab { get; protected set; }

        public virtual string Name { get; set; }

        public virtual string IconKey { get; }
        public virtual Icon Icon { get; }
        public virtual OrderedCollection<Tab> Tabs { get; } = new OrderedCollection<Tab>();
        public virtual ConditionalData Conditions { get; set; }

        public Section(string name, string iconKey)
        {
            Name = name;
            IconKey = iconKey;
        }

        /**
         * Updates the current tab
         */
        public void SetCurrentTab(Tab tab)
        {
            if (Tabs.Contains(tab))
                CurrentTab = tab;
            else
                Debug.LogError("Could not find " + tab.Name + " in section tab list");
        }

    }
}