using ClinicalTools.SimEncounters.Data;
using System;

namespace ClinicalTools.SimEncounters
{
    public class EncounterReader : IEncounterReader
    {
        public bool IsDone { get; protected set; }

        public Encounter Encounter { get; protected set; }

        public event Action<Encounter> Completed;

        protected IEncounterDataReader DataReader { get; }
        protected IDetailedStatusReader StatusReader { get; }

        public EncounterReader(IEncounterDataReader dataReader, IDetailedStatusReader statusReader)
        {
            DataReader = dataReader;
            StatusReader = statusReader;
        }

        private EncounterInfo info;
        private EncounterMetadata metadata;
        public void DoStuff(User user, EncounterInfo info, EncounterMetadata metadata)
        {
            this.info = info;
            this.metadata = metadata;

            DataReader.Completed += (data) => Something();
            StatusReader.Completed += (status) => Something();
            DataReader.DoStuff(user, info);
            StatusReader.DoStuff(user, info);
        }

        private void Something()
        {
            if (!DataReader.IsDone || !StatusReader.IsDone)
                return;

            Encounter = new Encounter(DataReader.EncounterData, info, StatusReader.DetailedStatus, metadata);
            IsDone = true;
            Completed?.Invoke(Encounter);
        }
    }
}