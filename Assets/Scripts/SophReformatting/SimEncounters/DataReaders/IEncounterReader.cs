using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterReader
    {
        WaitableResult<Encounter> GetEncounter(User user, IEncounterMetadata metadata, SaveType saveType);
    }
}