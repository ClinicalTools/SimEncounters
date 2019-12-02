using System.Collections.Generic;
using UnityEngine;

namespace SimEncounters.Data
{
    public class Section
    {
        public Tab CurrentTab { get; protected set; }

        public virtual string Name { get; set; }

        public virtual string IconKey { get; }
        public virtual Icon Icon { get; }
        public virtual OrderedDictionary<Tab> Tabs { get; } = new OrderedDictionary<Tab>();
        public virtual List<string> Conditions { get; set; }

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