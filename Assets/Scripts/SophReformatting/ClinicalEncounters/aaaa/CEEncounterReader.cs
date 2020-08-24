using ClinicalTools.SimEncounters;

namespace ClinicalTools.ClinicalEncounters
{
    public class CEEncounterReader : EncounterReader
    {
        public CEEncounterReader(IEncounterDataReaderSelector dataReaderSelector)
            : base(dataReaderSelector) { }

        protected override void ProcessResults(WaitableResult<Encounter> result,
            EncounterMetadata metadata,
            WaitedResult<EncounterContent> data)
        {
            if (data.Value.ImageContent is CEEncounterImageData ceImageData)
                UpdateLegacySections(data.Value.NonImageContent, ceImageData);

            var encounterData = new Encounter(metadata, data.Value);
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