using ClinicalTools.SimEncounters.Collections;

namespace ClinicalTools.SimEncounters.Data
{
    public class SectionsData
    {
        public virtual int CurrentSectionIndex { get; set; }

        public virtual OrderedCollection<Section> Sections { get; } = new OrderedCollection<Section>();
        public virtual VariableData Variables { get; }

        public SectionsData()
        {
            Variables = new VariableData();
        }

        public SectionsData(VariableData variables)
        {
            Variables = variables;
        }

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
    }
}