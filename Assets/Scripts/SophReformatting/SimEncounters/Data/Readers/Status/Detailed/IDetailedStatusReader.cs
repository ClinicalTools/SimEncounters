using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IDetailedStatusReader
    {
        WaitableResult<EncounterStatus> GetDetailedStatus(User user, EncounterMetadata metadata, EncounterBasicStatus basicStatus);
    }
}