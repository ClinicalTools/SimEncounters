using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class LocalEncounterRemover : IEncounterRemover
    {
        protected IFileManager LocalFileManager { get; }
        protected IFileManager AutosaveFileManager { get; }
        public LocalEncounterRemover(
            [Inject(Id = SaveType.Local)] IFileManager localFileManager,
            [Inject(Id = SaveType.Autosave)] IFileManager autosaveFileManager)
        {
            LocalFileManager = localFileManager;
            AutosaveFileManager = autosaveFileManager;
        }

        public WaitableResult Delete(User user, EncounterMetadata encounterMetadata)
        {
            LocalFileManager.DeleteFiles(user, encounterMetadata);
            AutosaveFileManager.DeleteFiles(user, encounterMetadata);
            return WaitableResult.CompletedResult;
        }
    }
}
