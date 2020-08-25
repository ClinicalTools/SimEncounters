namespace ClinicalTools.SimEncounters
{
    public interface IEncounterWriter
    {
        WaitableResult Save(User user, Encounter encounter);
    }
}
