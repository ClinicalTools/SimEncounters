using ClinicalTools.SimEncounters.MainMenu;
using System;
using System.IO;

namespace ClinicalTools.SimEncounters
{
    public class LocalDetailedStatusReader : IDetailedStatusReader
    {
        public bool IsDone { get; protected set; }

        public EncounterDetailedStatus DetailedStatus { get; protected set; }

        public event Action<EncounterDetailedStatus> Completed;

        protected IFilePathManager FilePathManager { get; }
        protected IParser<EncounterDetailedStatus> StatusReader { get; }
        public LocalDetailedStatusReader(IFilePathManager filePathManager)
        {
            FilePathManager = filePathManager;
            StatusReader = new DetailedStatusParser();
        }

        public void DoStuff(User user, EncounterInfo info)
        {
            var filePath = FilePathManager.EncounterFilePath(user, info);
            var statusFilePath = FilePathManager.DetailedStatusFilePath(filePath);
            if (!File.Exists(statusFilePath)) {
                Complete();
                return;
            }

            var text = File.ReadAllText(statusFilePath);
            DetailedStatus = StatusReader.Parse(text);
            Complete();
        }

        protected void Complete()
        {
            IsDone = true;
            Completed?.Invoke(DetailedStatus);
        }
    }
}