namespace ClinicalTools.SimEncounters.Writer
{
    public interface ITabSelector
    {
        event TabSelectedHandler TabSelected;
        void SelectTab(Tab tab);
    }
}