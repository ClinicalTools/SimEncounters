using ClinicalTools.SimEncounters.Collections;
using System;

namespace ClinicalTools.SimEncounters.Data
{
    public class EncounterData
    {
        public EncounterContent Content { get; }
        public EncounterImageData ImageData { get; }

        public EncounterData(EncounterContent content, EncounterImageData imageData)
        {
            Content = content;
            ImageData = imageData;
        }
    }

    public class EncounterContent
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
        public virtual VariableData Variables { get; }

        public EncounterContent()
        {
            Variables = new VariableData();
        }

        public EncounterContent(VariableData variables)
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