using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public class EncounterReader : IEncounterReader
    {
        private readonly IEncounterDataReaderSelector dataReaderSelector;
        public EncounterReader(IEncounterDataReaderSelector dataReaderSelector)
        {
            this.dataReaderSelector = dataReaderSelector;
        }

        public virtual WaitableResult<Encounter> GetEncounter(User user, IEncounterMetadata metadata, SaveType saveType)
        { 
            var dataReader = dataReaderSelector.GetEncounterDataReader(saveType);

            var data = dataReader.GetEncounterData(user, metadata);

            var encounterData = new WaitableResult<Encounter>();
            data.AddOnCompletedListener((result) => ProcessResults(encounterData, metadata, data));

            return encounterData;
        }

        protected virtual void ProcessResults(WaitableResult<Encounter> result,
            IEncounterMetadata metadata,
            WaitableResult<EncounterData> data)
        {
            var encounterData = new Encounter(metadata, data.Result.Content, data.Result.ImageData);
            result.SetResult(encounterData);
        }
    }
}