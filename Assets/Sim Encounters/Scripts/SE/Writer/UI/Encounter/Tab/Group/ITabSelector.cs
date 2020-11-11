namespace ClinicalTools.SimEncounters
{
    public interface ITabSelector
    {
        event TabSelectedHandler TabSelected;
        void SelectTab(Tab tab);
    }
}