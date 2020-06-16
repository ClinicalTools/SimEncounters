using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
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
        protected IEncounterContentReader ContentReader { get; }
        protected IImageDataReader ImageDataReader { get; }
        public EncounterDataReader(IEncounterContentReader contentReader, IImageDataReader imageDataReader)
        {
            ContentReader = contentReader;
            ImageDataReader = imageDataReader;
        }

        public virtual WaitableResult<EncounterData> GetEncounterData(User user, IEncounterMetadata metadata)
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