namespace ClinicalTools.SimEncounters
{
    public interface IUserSectionSelector
    {
        event UserSectionSelectedHandler SectionSelected;
        void SelectSection(UserSection userSection);
    }
}