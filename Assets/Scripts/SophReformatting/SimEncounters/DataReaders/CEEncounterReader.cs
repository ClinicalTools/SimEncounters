using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public class CEEncounterReader : EncounterReader
    {
        public CEEncounterReader(IEncounterContentReader contentReader, IImageDataReader imageDataReader) 
            : base(contentReader, imageDataReader) { }

        protected override void ProcessResults(WaitableResult<Encounter> result,
            EncounterMetadata metadata,
            WaitableResult<EncounterContent> content,
            WaitableResult<EncounterImageData> imageData)
        {
            if (result.IsCompleted || !content.IsCompleted || !imageData.IsCompleted)
                return;

            if (imageData.Result is CEEncounterImageData ceImageData)
                UpdateLegacySections(content.Result, ceImageData);

            var encounterData = new Encounter(metadata, content.Result, imageData.Result);
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
}