using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterContentReader
    {
        WaitableResult<EncounterContent> GetEncounterContent(User user, EncounterMetadata metadata);
    }
    public class LocalContentDataReader : IEncounterContentReader
    {
        private readonly IFileManager fileManager;
        private readonly IParser<EncounterContent> parser;
        public LocalContentDataReader(IFileManager fileManager, IParser<EncounterContent> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableResult<EncounterContent> GetEncounterContent(User user, EncounterMetadata metadata)
        {
            var content = new WaitableResult<EncounterContent>();

            var fileText = fileManager.GetFileText(user, FileType.Data, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(content, result));

            return content;
        }

        private void ProcessResults(WaitableResult<EncounterContent> result, string fileText)
        {
            result.SetResult(parser.Parse(fileText));
        }
    }
}