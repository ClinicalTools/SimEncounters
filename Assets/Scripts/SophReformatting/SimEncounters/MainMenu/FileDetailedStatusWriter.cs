using ClinicalTools.SimEncounters.Data;
using System.IO;

namespace ClinicalTools.SimEncounters
{
    public class FileDetailedStatusWriter : IDetailedStatusWriter
    {
        protected IFileManager FileManager { get; }
        protected EncounterStatusSerializer StatusSerializer { get; }
        public FileDetailedStatusWriter(IFileManager fileManager, EncounterStatusSerializer statusSerializer)
        {
            FileManager = fileManager;
            StatusSerializer = statusSerializer;
        }
        public void WriteStatus(UserEncounter encounter)
        {
            var basicStatusString = $"{encounter.Metadata.RecordNumber}::{(encounter.Status.BasicStatus.Completed ? 1 : 0)}::{encounter.Status.BasicStatus.Rating}";
            FileManager.SetFileText(encounter.User, FileType.BasicStatus, encounter.Metadata, basicStatusString);

            var statusString = StatusSerializer.Serialize(encounter.Status.ContentStatus);
            FileManager.SetFileText(encounter.User, FileType.DetailedStatus, encounter.Metadata, statusString);
        }
    }
}