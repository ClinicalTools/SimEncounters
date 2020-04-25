using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterDataReader
    {
        WaitableResult<EncounterData> GetEncounterData(User user, EncounterMetadata metadata);
    }

    public class CEEncounterDataReader : EncounterDataReader
    {
        public CEEncounterDataReader(IEncounterContentReader contentReader, IImageDataReader imageDataReader) 
            : base(contentReader, imageDataReader) { }


        protected override void ProcessResults(WaitableResult<EncounterData> result,
            WaitableResult<EncounterContent> content,
            WaitableResult<EncounterImageData> imageData)
        {
            if (result.IsCompleted || !content.IsCompleted || !imageData.IsCompleted)
                return;

            if (imageData.Result is CEEncounterImageData ceImageData)
                UpdateLegacySections(content.Result, ceImageData);

            var encounterData = new EncounterData(content.Result, imageData.Result);
            result.SetResult(encounterData);
        }

        protected virtual void UpdateLegacySections(EncounterContent content, CEEncounterImageData imageData)
        {
            foreach (var section in content.Sections) {
                if (section.Value is CESection ceSection)
                    ceSection.InitializeLegacyData(imageData.LegacyIconsInfo);
            }
        }
    }

    public class EncounterDataReader : IEncounterDataReader
    {
        private readonly IEncounterContentReader contentReader;
        private readonly IImageDataReader imageDataReader;
        public EncounterDataReader(IEncounterContentReader contentReader, IImageDataReader imageDataReader)
        {
            this.contentReader = contentReader;
            this.imageDataReader = imageDataReader;
        }

        public virtual WaitableResult<EncounterData> GetEncounterData(User user, EncounterMetadata metadata)
        {
            var content = contentReader.GetEncounterContent(user, metadata);
            var imageData = imageDataReader.GetImageData(user, metadata);

            var encounterData = new WaitableResult<EncounterData>();
            void processResults() => ProcessResults(encounterData, content, imageData);
            content.AddOnCompletedListener((result) => processResults());
            imageData.AddOnCompletedListener((result) => processResults());

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