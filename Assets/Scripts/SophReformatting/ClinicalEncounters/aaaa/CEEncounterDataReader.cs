using ClinicalTools.SimEncounters;

namespace ClinicalTools.ClinicalEncounters
{
    public class CEEncounterDataReader : EncounterDataReader
    {
        public CEEncounterDataReader(INonImageContentReader contentReader, IImageContentReader imageDataReader) 
            : base(contentReader, imageDataReader) { }

        protected override void ProcessResults(WaitableResult<EncounterContent> result,
            WaitableResult<EncounterNonImageContent> content,
            WaitableResult<EncounterImageContent> imageData)
        {
            if (result.IsCompleted() || !content.IsCompleted() || !imageData.IsCompleted())
                return;

            if (content.Result.IsError()) {
                result.SetError(content.Result.Exception);
                return;
            }
            if (imageData.Result.IsError()) {
                result.SetError(imageData.Result.Exception);
                return;
            }

            if (imageData.Result.Value is CEEncounterImageData ceImageData)
                UpdateLegacySections(content.Result.Value, ceImageData);

            var encounterData = new EncounterContent(content.Result.Value, imageData.Result.Value);
            result.SetResult(encounterData);
        }

        protected virtual void UpdateLegacySections(EncounterNonImageContent content, CEEncounterImageData imageData)
        {
            foreach (var section in content.Sections) {
                if (section.Value is CESection ceSection)
                    ceSection.InitializeLegacyData(imageData.LegacyIconsInfo);
            }
        }
    }
}