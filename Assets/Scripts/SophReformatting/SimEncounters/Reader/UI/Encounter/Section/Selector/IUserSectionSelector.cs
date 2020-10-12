namespace ClinicalTools.SimEncounters
{
    public interface IUserSectionSelector : IUserSectionDrawer
    {
        event UserSectionSelectedHandler SectionSelected;
    }
}