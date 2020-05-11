using ClinicalTools.SimEncounters.Data;
using System;

namespace ClinicalTools.SimEncounters.Writer
{
    public class SectionSelectedEventArgs : EventArgs
    {
        public Section SelectedSection { get; }
        public SectionSelectedEventArgs(Section selectedSection)
        {
            SelectedSection = selectedSection;
        }
    }
    public delegate void SectionSelectedHandler(object sender, SectionSelectedEventArgs e);
    public interface ISectionSelector
    {
        event SectionSelectedHandler SectionSelected;
        void SelectSection(Section section);
    }
}