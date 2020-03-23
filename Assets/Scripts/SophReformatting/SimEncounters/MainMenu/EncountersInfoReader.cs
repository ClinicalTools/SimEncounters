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
            UnityEngine.Debug.LogWarning("sophDebug1");
            if (!FileReader.IsDone || 
                //!ServerReader.IsDone || 
                !StatusesReader.IsDone)
                return;

            UnityEngine.Debug.LogWarning("sophDebug2");
            var encounters = FileReader.Result;
            /*
            if (encounters == null)
                encounters = new List<EncounterInfo>();
            if (FileReader.Result != null) {
                foreach (var localEncounter in FileReader.Result)
                    AddLocalEncounter(encounters, localEncounter);
            }*/

            UnityEngine.Debug.LogWarning("sophDebug3");
            UnityEngine.Debug.LogWarning("sophDebugaaaa");
            var statuses = StatusesReader.Result;
            UnityEngine.Debug.LogWarning("sophDebugZ");
            if (statuses != null) {
                foreach (var encounter in encounters) {
                    if (encounter == null) {
                        UnityEngine.Debug.LogWarning("sophDebugA");

                        continue;
                    }
                    UnityEngine.Debug.LogWarning("sophDebugB");
                    if (statuses.ContainsKey(encounter.RecordNumber)) {
                        UnityEngine.Debug.LogWarning("sophDebugC");
                        encounter.UserStatus = statuses[encounter.RecordNumber];
                    }
                    UnityEngine.Debug.LogWarning("sophDebugD");
                }
            }


            UnityEngine.Debug.LogWarning("sophDebug4");
            Result = encounters;
            IsDone = true;
            Completed?.Invoke(Result);
            UnityEngine.Debug.LogWarning("sophDebug5");
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