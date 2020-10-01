namespace ClinicalTools.SimEncounters
{
    public interface IEncounterPanelField : IPanelField
    {
        void Initialize(Encounter encounter);
        void Initialize(Encounter encounter, string value);
    }
}