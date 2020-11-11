namespace ClinicalTools.SimEncounters
{
    public interface IUserTabSelector : IUserTabDrawer
    {
        event UserTabSelectedHandler TabSelected;
    }
}