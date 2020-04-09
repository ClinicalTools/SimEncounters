using ClinicalTools.SimEncounters.Data;
using System.IO;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class FileDetailedStatusWriter : IDetailedStatusWriter
    {
        protected IFileManager FileManager { get; }
        public FileDetailedStatusWriter(IFileManager fileManager)
        {
            FileManager = fileManager;
        }
        public void DoStuff(User user, FullEncounter encounter)
        {
            var basicStatusString = $"{encounter.Metadata.RecordNumber}::{(encounter.Status.BasicStatus.Completed ? 1 : 0)}::{encounter.Status.BasicStatus.Rating}";
            FileManager.SetFileText(user, FileType.BasicStatus, encounter.Metadata, basicStatusString);

            var detailedStatusString = $"{string.Join(":", encounter.Status.ReadTabs)}";
            FileManager.SetFileText(user, FileType.DetailedStatus, encounter.Metadata, detailedStatusString);
        }
    }
}