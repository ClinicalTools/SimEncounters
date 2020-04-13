using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterReaderSelector
    {
        IFullEncounterReader GetEncounterReader(SaveType saveType);
    }
    public class EncounterReaderSelector : IEncounterReaderSelector
    {
        protected virtual Dictionary<SaveType, IFullEncounterReader> EncounterReaders { get; } = new Dictionary<SaveType, IFullEncounterReader>();
        public EncounterReaderSelector(
            [Inject(Id = SaveType.Autosave)] IFullEncounterReader autosaveReader,
            [Inject(Id = SaveType.Demo)] IFullEncounterReader demoReader,
            [Inject(Id = SaveType.Local)] IFullEncounterReader localReader,
            [Inject(Id = SaveType.Server)] IFullEncounterReader serverReader)
        {
            EncounterReaders.Add(SaveType.Autosave, autosaveReader);
            EncounterReaders.Add(SaveType.Demo, demoReader);
            EncounterReaders.Add(SaveType.Local, localReader);
            EncounterReaders.Add(SaveType.Server, serverReader);
        }

        public IFullEncounterReader GetEncounterReader(SaveType saveType) => EncounterReaders[saveType];
    }

    public interface IFullEncounterReader
    {
        WaitableResult<FullEncounter> GetFullEncounter(User user, EncounterMetadata metadata, EncounterBasicStatus basicStatus);
    }

    public class FullEncounterReader : IFullEncounterReader
    {
        private readonly IEncounterDataReader dataReader;
        private readonly IDetailedStatusReader detailedStatusReader;
        public FullEncounterReader(IEncounterDataReader dataReader, IDetailedStatusReader detailedStatusReader)
        {
            this.dataReader = dataReader;
            this.detailedStatusReader = detailedStatusReader;
        }

        public WaitableResult<FullEncounter> GetFullEncounter(User user, EncounterMetadata metadata, EncounterBasicStatus basicStatus)
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