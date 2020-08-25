namespace ClinicalTools.SimEncounters.Writer
{
    public interface ISectionSelector
    {
        event SectionSelectedHandler SectionSelected;
        void SelectSection(Section section);
    }
}