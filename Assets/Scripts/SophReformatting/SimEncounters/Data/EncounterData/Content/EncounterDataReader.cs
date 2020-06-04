using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public class EncounterDataReader : IEncounterDataReader
    {
        protected IEncounterContentReader ContentReader { get; }
        protected IImageDataReader ImageDataReader { get; }
        public EncounterDataReader(IEncounterContentReader contentReader, IImageDataReader imageDataReader)
        {
            ContentReader = contentReader;
            ImageDataReader = imageDataReader;
        }

        public virtual WaitableResult<EncounterData> GetEncounterData(User user, EncounterMetadata metadata)
        {
            var encounterData = new WaitableResult<EncounterData>();
            var content = ContentReader.GetEncounterContent(user, metadata);
            var imageData = ImageDataReader.GetImageData(user, metadata);

            content.AddOnCompletedListener((result) => ProcessResults(encounterData, content, imageData));
            imageData.AddOnCompletedListener((result) => ProcessResults(encounterData, content, imageData));

            return encounterData;
        }

        protected virtual void ProcessResults(WaitableResult<EncounterData> result,
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