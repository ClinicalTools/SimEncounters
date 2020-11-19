using System;

namespace ClinicalTools.SimEncounters
{
    public enum SelectionType
    {
        NewSelection,
        Edited
    }

    public delegate void SectionSelectedHandler(object sender, SectionSelectedEventArgs e);
    public class SectionSelectedEventArgs : EventArgs
    {
        public Section SelectedSection { get; }
        public SelectionType SelectionType { get; }
        public SectionSelectedEventArgs(Section selectedSection)
        {
            SelectedSection = selectedSection;
        }
    }
}