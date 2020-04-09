using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IImageDataReader
    {
        WaitableResult<EncounterImageData> GetImageData(User user, EncounterMetadata metadata);
    }
    public class LocalImageDataReader : IImageDataReader
    {
        private readonly IFileManager fileManager;
        private readonly IParser<EncounterImageData> parser;
        public LocalImageDataReader(IFileManager fileManager, IParser<EncounterImageData> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableResult<EncounterImageData> GetImageData(User user, EncounterMetadata metadata)
        {
            var imageData = new WaitableResult<EncounterImageData>();

            var fileText = fileManager.GetFileText(user, FileType.Image, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(imageData, result));

            return imageData;
        }

        private void ProcessResults(WaitableResult<EncounterImageData> result, string fileText)
        {
            result.SetResult(parser.Parse(fileText));
        }
    }
}