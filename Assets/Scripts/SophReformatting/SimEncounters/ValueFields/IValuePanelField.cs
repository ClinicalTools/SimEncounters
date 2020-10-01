namespace ClinicalTools.SimEncounters
{
    public interface IValuePanelField : IPanelField
    {
        void Initialize();
        void Initialize(string value);
    }
}