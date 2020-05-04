using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public class EncounterReader : IEncounterReader
    {
        private readonly IEncounterContentReader contentReader;
        private readonly IImageDataReader imageDataReader;
        public EncounterReader(IEncounterContentReader contentReader, IImageDataReader imageDataReader)
        {
            this.contentReader = contentReader;
            this.imageDataReader = imageDataReader;
        }

        public virtual WaitableResult<Encounter> GetEncounter(User user, EncounterMetadata metadata)
        {
            var content = contentReader.GetEncounterContent(user, metadata);
            var imageData = imageDataReader.GetImageData(user, metadata);

            var encounterData = new WaitableResult<Encounter>();
            void processResults() => ProcessResults(encounterData, metadata, content, imageData);
            content.AddOnCompletedListener((result) => processResults());
            imageData.AddOnCompletedListener((result) => processResults());

            return encounterData;
        }

        protected virtual void ProcessResults(WaitableResult<Encounter> result,
            EncounterMetadata metadata,
            WaitableResult<EncounterContent> content,
            WaitableResult<EncounterImageData> imageData)
        {
            if (result.IsCompleted || !content.IsCompleted || !imageData.IsCompleted)
                return;

            var encounterData = new Encounter(metadata, content.Result, imageData.Result);
            result.SetResult(encounterData);
        }
    }
}