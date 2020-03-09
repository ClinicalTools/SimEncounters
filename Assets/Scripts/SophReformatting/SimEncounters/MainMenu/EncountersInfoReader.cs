using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class EncountersInfoReader : IEncountersInfoReader
    {
        public event Action<List<EncounterDetail>> Completed;
        public List<EncounterDetail> Result { get; protected set; }
        public bool IsDone { get; protected set; }

        protected virtual IEncountersInfoReader FileReader { get; }
        protected virtual IEncountersInfoReader ServerReader { get; }
        public EncountersInfoReader()
        {
            FileReader = new FileEncountersInfoReader(new FilePathManager());
            ServerReader = new ServerCasesInfoReader(new WebAddress());
        }

        public void GetEncounterInfos(User user)
        {
            ServerReader.Completed += (results) => ProcessResults();
            FileReader.Completed += (results) => ProcessResults();
            ServerReader.GetEncounterInfos(user);
            FileReader.GetEncounterInfos(user);
        }

        protected virtual void ProcessResults()
        {
            if (!FileReader.IsDone || !ServerReader.IsDone)
                return;

            var encounters = ServerReader.Result;
            if (encounters == null)
                encounters = new List<EncounterDetail>();
            if (FileReader.Result != null) {
                foreach (var localEncounter in FileReader.Result)
                    AddLocalEncounter(encounters, localEncounter);
            }

            Result = encounters;
            IsDone = true;
            Completed?.Invoke(Result);
        }

        protected virtual void AddLocalEncounter(List<EncounterDetail> encounters, EncounterDetail localEncounter)
        {
            if (localEncounter.RecordNumber < 0)
                encounters.Add(localEncounter);

            foreach (var listEncounter in encounters) {
                if (listEncounter.RecordNumber != localEncounter.RecordNumber)
                    continue;
                listEncounter.InfoGroup.LocalInfo = localEncounter.InfoGroup.LocalInfo;

                return;
            }

            encounters.Add(localEncounter);
        }

        protected virtual void UpdateFilePath(string oldPath, EncounterInfoGroup encounterInfo)
        {

        }
    }
}