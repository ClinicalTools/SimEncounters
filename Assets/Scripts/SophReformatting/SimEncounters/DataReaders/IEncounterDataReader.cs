using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterDataReaderNew
    {
        WaitableResult<EncounterData> GetEncounterData(User user, EncounterMetadata metadata);
    }

    public class EncounterDataReaderNew : IEncounterDataReaderNew
    {
        private readonly IEncounterContentReader contentReader;
        private readonly IImageDataReader imageDataReader;
        public EncounterDataReaderNew(IEncounterContentReader contentReader, IImageDataReader imageDataReader)
        {
            this.contentReader = contentReader;
            this.imageDataReader = imageDataReader;
        }

        public WaitableResult<EncounterData> GetEncounterData(User user, EncounterMetadata metadata)
        {
            var content = contentReader.GetEncounterContent(user, metadata);
            var imageData = imageDataReader.GetImageData(user, metadata);

            var encounterData = new WaitableResult<EncounterData>();
            void processResults() => ProcessResults(encounterData, content, imageData);
            content.AddOnCompletedListener((result) => processResults());
            imageData.AddOnCompletedListener((result) => processResults());

            return encounterData;
        }

        protected void ProcessResults(WaitableResult<EncounterData> result,
            WaitableResult<EncounterContent> content,
            WaitableResult<EncounterImageData> imageData)
        {
            if (result.IsCompleted || !content.IsCompleted || !imageData.IsCompleted)
                return;

            var encounterData = new EncounterData(content.Result, imageData.Result);
            result.SetResult(encounterData);
        }
    }
}