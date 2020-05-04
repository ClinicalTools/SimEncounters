using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Reader
{
    public interface IUserSectionSelector
    {
        event SectionSelectedHandler SectionSelected;
        void SelectSection(UserSection userSection);
    }
}