using ClinicalTools.SimEncounters.Collections;
using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Data
{
    public class Section
    {
        public int CurrentTabIndex { get; set; }
        public virtual string GetCurrentTabKey() => Tabs[CurrentTabIndex].Key;
        public virtual void SetCurrentTab(Tab tab)
        {
            if (!Tabs.Contains(tab))
                throw new Exception($"Passed section is not contained in the collection of sections.");
            CurrentTabIndex = Tabs.IndexOf(tab);
        }

        public virtual string Name { get; set; }
        public virtual string IconKey { get; set; }
        public virtual Color Color { get; set; } 

        public virtual OrderedCollection<Tab> Tabs { get; } = new OrderedCollection<Tab>();
        public virtual ConditionalData Conditions { get; set; }

        public Section(string name, string iconKey, Color color)
        {
            Name = name;
            IconKey = iconKey;
            Color = color;
        }

        public int MoveToNextTab()
        {
            if (CurrentTabIndex < Tabs.Count - 1)
                CurrentTabIndex++;
            return CurrentTabIndex;
        }
        public int MoveToPreviousTab()
        {
            if (CurrentTabIndex > 0)
                CurrentTabIndex--;
            return CurrentTabIndex;
        }
    }
}