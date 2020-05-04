using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IDetailedStatusReader
    {
        WaitableResult<EncounterStatus> GetDetailedStatus(User user, EncounterMetadata metadata, EncounterBasicStatus basicStatus);
    }

    public class LocalDetailedStatusReader : IDetailedStatusReader
    {
        private readonly IFileManager fileManager;
        private readonly IParser<EncounterContentStatus> parser;
        public LocalDetailedStatusReader(IFileManager fileManager, IParser<EncounterContentStatus> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableResult<EncounterStatus> GetDetailedStatus(User user, 
            EncounterMetadata metadata, EncounterBasicStatus basicStatus)
        {
            var detailedStatus = new WaitableResult<EncounterStatus>();

            var fileText = fileManager.GetFileText(user, FileType.DetailedStatus, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(detailedStatus, result, basicStatus));

            return detailedStatus;
        }

        private void ProcessResults(WaitableResult<EncounterStatus> result, 
            string fileText, EncounterBasicStatus basicStatus)
        {
            var detailedStatus = parser.Parse(fileText);
            var status = new EncounterStatus(basicStatus, detailedStatus);
            result.SetResult(status);
        }
    }
}