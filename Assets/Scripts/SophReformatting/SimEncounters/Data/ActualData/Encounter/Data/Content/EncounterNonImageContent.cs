using ClinicalTools.SimEncounters.Collections;
using System;

namespace ClinicalTools.SimEncounters.Data
{
    public class EncounterNonImageContent
    {
        public virtual int CurrentSectionIndex { get; set; }
        public virtual string GetCurrentSectionKey() => Sections[CurrentSectionIndex].Key;
        public virtual void SetCurrentSection(Section section)
        {
            if (!Sections.Contains(section))
                throw new Exception($"Passed section is not contained in the collection of sections.");
            CurrentSectionIndex = Sections.IndexOf(section);
        }

        public virtual OrderedCollection<Section> Sections { get; } = new OrderedCollection<Section>();


        public int MoveToNextSection()
        {
            if (CurrentSectionIndex >= Sections.Count - 1)
                return CurrentSectionIndex;

            CurrentSectionIndex++;
            var section = Sections[CurrentSectionIndex].Value;
            section.CurrentTabIndex = 0;

            return CurrentSectionIndex;
        }
        public int MoveToPreviousSection()
        {
            if (CurrentSectionIndex <= 0)
                return CurrentSectionIndex;

            CurrentSectionIndex--;
            var section = Sections[CurrentSectionIndex].Value;
            section.CurrentTabIndex = section.Tabs.Count - 1;

            return CurrentSectionIndex;
        }

        public int GetCurrentTabNumber()
        {
            var tabNumber = 1;
            for (int i = 0; i < CurrentSectionIndex; i++)
                tabNumber += Sections[i].Value.Tabs.Count;
            tabNumber += Sections[CurrentSectionIndex].Value.CurrentTabIndex;
            return tabNumber;
        }
    }
}