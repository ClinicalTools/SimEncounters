using System;

namespace ClinicalTools.SimEncounters.Writer
{
    public delegate void SectionSelectedHandler(object sender, SectionSelectedEventArgs e);
    public class SectionSelectedEventArgs : EventArgs
    {
        public Section SelectedSection { get; }
        public SectionSelectedEventArgs(Section selectedSection)
        {
            SelectedSection = selectedSection;
        }
    }
}