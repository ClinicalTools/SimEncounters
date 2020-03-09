using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class EncounterStatusesReader : IEncounterStatusesReader
    {
        public event Action<Dictionary<int, UserEncounterStatus>> Completed;
        public Dictionary<int, UserEncounterStatus> Result { get; protected set; }
        public bool IsDone { get; protected set; }

        protected virtual IEncounterStatusesReader FileReader { get; }
        protected virtual IEncounterStatusesReader ServerReader { get; }
        public EncounterStatusesReader()
        {
            FileReader = new FileEncounterStatusesReader();
            ServerReader = new ServerEncounterStatusesReader(new WebAddress());
        }

        public void GetEncounterStatuses(User user)
        {
            ServerReader.Completed += (results) => ProcessResults();
            FileReader.Completed += (results) => ProcessResults();
            ServerReader.GetEncounterStatuses(user);
            FileReader.GetEncounterStatuses(user);
        }

        protected virtual void ProcessResults()
        {
            if (!FileReader.IsDone || !ServerReader.IsDone)
                return;

            var encounters = ServerReader.Result;
            if (encounters == null)
                encounters = new Dictionary<int, UserEncounterStatus>();
            if (FileReader.Result != null)
                foreach (var encounterPair in FileReader.Result)
                    AddEncounter(encounters, encounterPair);

            Result = encounters;
            IsDone = true;
            Completed?.Invoke(Result);
        }

        private void AddEncounter(Dictionary<int, UserEncounterStatus> encounters, KeyValuePair<int, UserEncounterStatus> encounterPair)
        {
            if (!encounters.ContainsKey(encounterPair.Key))
                encounters.Add(encounterPair.Key, encounterPair.Value);
            else if (encounters[encounterPair.Key].Timestamp < encounterPair.Value.Timestamp)
                encounters[encounterPair.Key] = encounterPair.Value;
        }
    }
}