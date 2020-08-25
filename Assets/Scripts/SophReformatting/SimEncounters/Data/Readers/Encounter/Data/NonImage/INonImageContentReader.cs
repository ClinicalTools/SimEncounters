namespace ClinicalTools.SimEncounters
{
    public interface INonImageContentReader
    {
        WaitableResult<EncounterNonImageContent> GetNonImageContent(User user, EncounterMetadata metadata);
    }
}