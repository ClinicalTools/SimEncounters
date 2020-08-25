namespace ClinicalTools.SimEncounters
{
    public interface IUserTabSelector
    {
        event UserTabSelectedHandler TabSelected;
        void SelectTab(UserTab userTab);
    }
}