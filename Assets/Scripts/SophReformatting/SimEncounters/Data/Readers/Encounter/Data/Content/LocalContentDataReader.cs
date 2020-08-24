namespace ClinicalTools.SimEncounters
{
    public class LocalContentDataReader : IEncounterContentReader
    {
        private readonly IFileManager fileManager;
        private readonly IParser<EncounterNonImageContent> parser;
        public LocalContentDataReader(IFileManager fileManager, IParser<EncounterNonImageContent> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableResult<EncounterNonImageContent> GetEncounterContent(User user, EncounterMetadata metadata)
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