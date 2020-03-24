using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class EncountersInfoReader : IEncountersInfoReader
    {
        public event Action<List<EncounterInfo>> Completed;
        public List<EncounterInfo> Result { get; protected set; }
        public bool IsDone { get; protected set; }

        protected virtual IEncountersInfoReader FileReader { get; }
        protected virtual IEncountersInfoReader ServerReader { get; }
        protected virtual IEncounterStatusesReader StatusesReader { get; }
        public EncountersInfoReader()
        {
            //FileReader = new FileEncountersInfoReader(new DemoPathManager(), new EncounterDetailParser(new EncounterDemoInfoSetter()));
            FileReader = new DemoCasesInfoReader(new DemoPathManager());
            //FileReader = new FileEncountersInfoReader(new FilePathManager(), new EncounterDetailParser(new EncounterLocalInfoSetter()));
            ServerReader = new ServerCasesInfoReader(new WebAddress());
            StatusesReader = new FileEncounterStatusesReader(new FilePathManager()); //new EncounterStatusesReader();
        }

        public void GetEncounterInfos(User user)
        {
            //ServerReader.Completed += (results) => ProcessResults();
            FileReader.Completed += (results) => ProcessResults();
            StatusesReader.Completed += (results) => ProcessResults();
            //ServerReader.GetEncounterInfos(user);
            StatusesReader.GetEncounterStatuses(user);
            FileReader.GetEncounterInfos(user);
        }

        protected virtual void ProcessResults()
        {
            if (!FileReader.IsDone ||
                //!ServerReader.IsDone || 
                !StatusesReader.IsDone)
                return;

            var encounters = FileReader.Result;
            /*
            if (encounters == null)
                encounters = new List<EncounterInfo>();
            if (FileReader.Result != null) {
                foreach (var localEncounter in FileReader.Result)
                    AddLocalEncounter(encounters, localEncounter);
            }*/

            var statuses = StatusesReader.Result;
            if (statuses != null) {
                foreach (var encounter in encounters) {
                    if (encounter != null && statuses.ContainsKey(encounter.RecordNumber))
                        encounter.UserStatus = statuses[encounter.RecordNumber];
                }
            }

            Result = encounters;
            IsDone = true;
            Completed?.Invoke(Result);
        }

        protected virtual void AddLocalEncounter(List<EncounterInfo> encounters, EncounterInfo localEncounter)
        {
            if (localEncounter.RecordNumber < 0)
                encounters.Add(localEncounter);

            foreach (var listEncounter in encounters) {
                if (listEncounter.RecordNumber != localEncounter.RecordNumber)
                    continue;
                listEncounter.MetaGroup.LocalInfo = localEncounter.MetaGroup.LocalInfo;

                return;
            }

            encounters.Add(localEncounter);
        }

        protected virtual void UpdateFilePath(string oldPath, EncounterMetaGroup encounterInfo)
        {

        }
    }
}