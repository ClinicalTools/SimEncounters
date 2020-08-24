using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public class LocalMetadataReader : IMetadataReader
    {
        private readonly IFileManager fileManager;
        private readonly IParser<EncounterMetadata> parser;
        public LocalMetadataReader(IFileManager fileManager, IParser<EncounterMetadata> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableResult<EncounterMetadata> GetMetadata(User user, EncounterMetadata metadata)
        {
            var metadataResult = new WaitableResult<EncounterMetadata>();

            var fileText = fileManager.GetFileText(user, FileType.Metadata, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(metadataResult, result));

            return metadataResult;
        }

        private void ProcessResults(WaitableResult<EncounterMetadata> result, WaitedResult<string> fileText)
        {
            if (fileText.Value == null) {
                result.SetError(null);
                return;
            }

            var metadata = parser.Parse(fileText.Value);
            result.SetResult(metadata);
        }
    }
}
