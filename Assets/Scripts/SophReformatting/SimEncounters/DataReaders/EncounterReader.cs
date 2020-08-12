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

        public virtual WaitableResult<Encounter> GetEncounter(User user, EncounterMetadata metadata, SaveType saveType)
        { 
            var dataReader = dataReaderSelector.GetEncounterDataReader(saveType);

            var data = dataReader.GetEncounterData(user, metadata);

            var encounterData = new WaitableResult<Encounter>();
            data.AddOnCompletedListener((result) => ProcessResults(encounterData, metadata, result));

            return encounterData;
        }

        protected virtual void ProcessResults(WaitableResult<Encounter> result,
            EncounterMetadata metadata,
            WaitedResult<EncounterData> data)
        {
            if (data.IsError()) {
                result.SetError(data.Exception);
                return;
            }
            var encounterData = new Encounter(metadata, data.Value.Content, data.Value.ImageData);
            result.SetResult(encounterData);
        }
    }
}