namespace ClinicalTools.SimEncounters
{
    public interface IEncounterRemover
    {
        public WaitableResult Delete(User user, EncounterMetadata encounterMetadata);
    }
}
