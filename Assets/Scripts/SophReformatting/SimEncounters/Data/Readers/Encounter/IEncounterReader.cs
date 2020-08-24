

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterReader
    {
        WaitableResult<Encounter> GetEncounter(User user, EncounterMetadata metadata, SaveType saveType);
    }
}