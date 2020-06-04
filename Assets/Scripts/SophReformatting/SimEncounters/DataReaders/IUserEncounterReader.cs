using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IUserEncounterReader
    {
        WaitableResult<UserEncounter> GetUserEncounter(User user, EncounterMetadata metadata, EncounterBasicStatus basicStatus, SaveType saveType);
    }
}