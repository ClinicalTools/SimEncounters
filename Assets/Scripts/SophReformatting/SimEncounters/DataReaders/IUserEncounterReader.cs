using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IUserEncounterReader
    {
        WaitableResult<UserEncounter> GetUserEncounter(User user, IEncounterMetadata metadata, EncounterBasicStatus basicStatus, SaveType saveType);
    }
}