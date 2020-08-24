using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterContentReader
    {
        WaitableResult<EncounterNonImageContent> GetEncounterContent(User user, EncounterMetadata metadata);
    }
}