namespace ClinicalTools.SimEncounters
{
    public class LocalImageContentReader : IImageContentReader
    {
        private readonly IFileManager fileManager;
        private readonly IParser<EncounterImageContent> parser;
        public LocalImageContentReader(IFileManager fileManager, IParser<EncounterImageContent> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableResult<EncounterImageContent> GetImageData(User user, EncounterMetadata metadata)
        {
            var imageData = new WaitableResult<EncounterImageContent>();

            var fileText = fileManager.GetFileText(user, FileType.Image, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(imageData, result));

            return imageData;
        }

        private void ProcessResults(WaitableResult<EncounterImageContent> result, WaitedResult<string> fileText)
        {
            if (fileText.IsError())
                result.SetError(fileText.Exception);
            else
                result.SetResult(parser.Parse(fileText.Value));
        }
    }
}