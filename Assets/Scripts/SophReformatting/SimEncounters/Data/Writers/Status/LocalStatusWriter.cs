namespace ClinicalTools.SimEncounters
{
    public class LocalStatusWriter : IStatusWriter
    {
        protected IFileManager FileManager { get; }
        protected EncounterContentStatusSerializer StatusSerializer { get; }
        public LocalStatusWriter(IFileManager fileManager, EncounterContentStatusSerializer statusSerializer)
        {
            FileManager = fileManager;
            StatusSerializer = statusSerializer;
        }

        public WaitableTask WriteStatus(UserEncounter encounter)
        {
            var basicStatusString = $"{encounter.Data.Metadata.RecordNumber}::{(encounter.Status.BasicStatus.Completed ? 1 : 0)}::{encounter.Status.BasicStatus.Rating}";
            FileManager.SetFileText(encounter.User, FileType.BasicStatus, encounter.Data.Metadata, basicStatusString);

            var statusString = StatusSerializer.Serialize(encounter.Status.ContentStatus);
            FileManager.SetFileText(encounter.User, FileType.DetailedStatus, encounter.Data.Metadata, statusString);

            return WaitableTask.CompletedTask;
        }
    }
}