using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public class CEEncounterReader : EncounterReader
    {
        public CEEncounterReader(IEncounterDataReaderSelector dataReaderSelector)
            : base(dataReaderSelector) { }

        protected override void ProcessResults(WaitableResult<Encounter> result,
            IEncounterMetadata metadata,
            WaitableResult<EncounterData> data)
        {
            if (data.Result.ImageData is CEEncounterImageData ceImageData)
                UpdateLegacySections(data.Result.Content, ceImageData);

            var encounterData = new Encounter(metadata, data.Result.Content, data.Result.ImageData);
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