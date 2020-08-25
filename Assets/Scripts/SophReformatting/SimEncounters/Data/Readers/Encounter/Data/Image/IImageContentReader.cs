namespace ClinicalTools.SimEncounters
{
    public interface IImageContentReader
    {
        WaitableResult<EncounterImageContent> GetImageData(User user, EncounterMetadata metadata);
    }
}