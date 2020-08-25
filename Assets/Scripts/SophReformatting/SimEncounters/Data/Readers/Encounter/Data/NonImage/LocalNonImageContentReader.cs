namespace ClinicalTools.SimEncounters
{
    public class LocalNonImageContentReader : INonImageContentReader
    {
        private readonly IFileManager fileManager;
        private readonly IParser<EncounterNonImageContent> parser;
        public LocalNonImageContentReader(IFileManager fileManager, IParser<EncounterNonImageContent> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableResult<EncounterNonImageContent> GetNonImageContent(User user, EncounterMetadata metadata)
        {
            var content = new WaitableResult<EncounterNonImageContent>();

            var fileText = fileManager.GetFileText(user, FileType.Data, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(content, result));

            return content;
        }

        private void ProcessResults(WaitableResult<EncounterNonImageContent> result, WaitedResult<string> fileText)
        {
            if (fileText.IsError())
                result.SetError(fileText.Exception);
            else
                result.SetResult(parser.Parse(fileText.Value));
        }
    }
}