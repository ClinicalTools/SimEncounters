
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class LocalMetadatasReader : IMetadatasReader
    {
        private readonly IFileManager fileManager;
        private readonly IParser<EncounterMetadata> parser;
        public LocalMetadatasReader(IFileManager fileManager, IParser<EncounterMetadata> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableResult<List<EncounterMetadata>> GetMetadatas(User user)
        {
            var metadatas = new WaitableResult<List<EncounterMetadata>>();

            var fileTexts = fileManager.GetFilesText(user, FileType.Metadata);
            fileTexts.AddOnCompletedListener((result) => ProcessResults(metadatas, result));

            return metadatas;
        }

        private void ProcessResults(WaitableResult<List<EncounterMetadata>> result, WaitedResult<string[]> fileTexts)
        {
            if (fileTexts == null) {
                result.SetError(null);
                return;
            }

            var metadatas = new List<EncounterMetadata>();
            foreach (var fileText in fileTexts.Value) {
                var metadata = parser.Parse(fileText);
                if (metadata != null)
                    metadatas.Add(metadata);
            }

            result.SetResult(metadatas);
        }
    }
}
