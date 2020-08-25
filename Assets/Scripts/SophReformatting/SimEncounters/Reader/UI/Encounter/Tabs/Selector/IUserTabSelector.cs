namespace ClinicalTools.SimEncounters.Reader
{
    public interface IUserTabSelector
    {
        event UserTabSelectedHandler TabSelected;
        void SelectTab(UserTab userTab);
    }
}