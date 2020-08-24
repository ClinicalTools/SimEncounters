using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IMetadataReader
    {
        WaitableResult<EncounterMetadata> GetMetadata(User user, EncounterMetadata metadata);
    }
}
