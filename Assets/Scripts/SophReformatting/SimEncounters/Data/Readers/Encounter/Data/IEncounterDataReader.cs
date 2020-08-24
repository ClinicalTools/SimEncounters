namespace ClinicalTools.SimEncounters
{
    public interface IEncounterDataReader
    {
        WaitableResult<EncounterContent> GetEncounterData(User user, EncounterMetadata metadata);
    }
}