using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IFullEncounterReader
    {
        WaitableResult<FullEncounter> GetEncounter(User user, EncounterMetadata metadata, EncounterBasicStatus basicStatus);
    }

    public class FullEncounterReader : IFullEncounterReader
    {
        private readonly IEncounterDataReaderNew dataReader;
        private readonly IDetailedStatusReader detailedStatusReader;
        public FullEncounterReader(IEncounterDataReaderNew dataReader, IDetailedStatusReader detailedStatusReader)
        {
            this.dataReader = dataReader;
            this.detailedStatusReader = detailedStatusReader;
        }

        public WaitableResult<FullEncounter> GetEncounter(User user, EncounterMetadata metadata, EncounterBasicStatus basicStatus)
        {
            var encounterData = dataReader.GetEncounterData(user, metadata);
            var detailedStatus = detailedStatusReader.GetDetailedStatus(user, metadata, basicStatus);

            var encounter = new WaitableResult<FullEncounter>();
            void processResults() => ProcessResults(encounter, metadata, encounterData, detailedStatus);
            encounterData.AddOnCompletedListener((result) => processResults());
            detailedStatus.AddOnCompletedListener((result) => processResults());

            return encounter;
        }

        protected void ProcessResults(WaitableResult<FullEncounter> result,
            EncounterMetadata metadata,
            WaitableResult<EncounterData> encounterData,
            WaitableResult<EncounterDetailedStatus> detailedStatus)
        {
            if (result.IsCompleted || !encounterData.IsCompleted || !detailedStatus.IsCompleted)
                return;

            var encounter = new FullEncounter(metadata, encounterData.Result, detailedStatus.Result);
            result.SetResult(encounter);
        }
    }
}