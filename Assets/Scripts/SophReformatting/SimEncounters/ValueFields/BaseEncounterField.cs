namespace ClinicalTools.SimEncounters
{
    public abstract class BaseEncounterField : BaseField
    {
        public abstract void Initialize(Encounter encounter);
        public abstract void Initialize(Encounter encounter, string value);
    }
}