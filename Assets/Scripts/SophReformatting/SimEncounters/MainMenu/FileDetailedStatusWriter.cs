using ClinicalTools.SimEncounters.Data;
using System.IO;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class FileDetailedStatusWriter : IDetailedStatusWriter
    {
        protected IFilePathManager FilePathManager { get; }
        public FileDetailedStatusWriter(IFilePathManager filePathManager)
        {
            FilePathManager = filePathManager;
        }
        public void DoStuff(User user, Encounter encounter)
        {
            /*
            if (user.IsGuest)
                return;*/

            var directory = FilePathManager.GetLocalSavesFolder(user);
            if (!Directory.Exists(directory)) {
                return;
            }

            var encounterFilePath = FilePathManager.EncounterFilePath(user, encounter.Info);

            var basicStatusFilePath = FilePathManager.StatusFilePath(encounterFilePath);
            var basicStatusString = $"{encounter.Info.MetaGroup.RecordNumber}::{(encounter.Info.UserStatus.Completed ? 1 : 0)}::{encounter.Info.UserStatus.Rating}";
            File.WriteAllText(basicStatusFilePath, basicStatusString);

            var detailedStatusFilePath = FilePathManager.DetailedStatusFilePath(encounterFilePath);
            var detailedStatusString = $"{string.Join(":", encounter.Status.ReadTabs)}";
            File.WriteAllText(detailedStatusFilePath, detailedStatusString);
        }
    }
}