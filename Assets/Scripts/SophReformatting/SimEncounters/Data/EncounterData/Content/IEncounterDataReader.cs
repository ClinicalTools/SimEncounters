using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterDataReader
    {
        WaitableResult<EncounterData> GetEncounterData(User user, EncounterMetadata metadata);
    }
}