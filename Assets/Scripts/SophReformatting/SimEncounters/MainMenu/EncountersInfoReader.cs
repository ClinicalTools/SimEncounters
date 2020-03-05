using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class EncountersInfoReader : IEncountersInfoReader
    {
        public event Action<List<EncounterInfoGroup>> Completed;
        public List<EncounterInfoGroup> Results { get; protected set; }
        public bool IsDone { get; protected set; }

        protected virtual IEncountersInfoReader FileReader { get; }
        protected virtual IEncountersInfoReader ServerReader { get; }
        public EncountersInfoReader()
        {
            FileReader = new FileEncountersInfoReader();
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

            var encounters = ServerReader.Results;
            if (encounters == null)
                encounters = new List<EncounterInfoGroup>();
            if (FileReader.Results != null) {
                foreach (var localEncounter in FileReader.Results)
                    AddLocalEncounter(encounters, localEncounter);
            }

            Results = encounters;
            IsDone = true;
            Completed?.Invoke(Results);
        }

        protected virtual void AddLocalEncounter(List<EncounterInfoGroup> encounters, EncounterInfoGroup localEncounter)
        {
            if (string.IsNullOrWhiteSpace(localEncounter.RecordNumber))
                encounters.Add(localEncounter);

            foreach (var listEncounter in encounters) {
                if (listEncounter.RecordNumber != localEncounter.RecordNumber)
                    continue;
                listEncounter.LocalInfo = localEncounter.LocalInfo;
                if (localEncounter.Filename != listEncounter.Filename)
                    UpdateFilePath(localEncounter.Filename, listEncounter);

                return;
            }

            encounters.Add(localEncounter);
        }

        protected virtual void UpdateFilePath(string oldPath, EncounterInfoGroup encounterInfo)
        {

        }
    }
    public class EncounterStatusesReader : IEncounterStatusesReader
    {
        public event Action<List<UserEncounterStatus>> Completed;
        public List<UserEncounterStatus> Results { get; protected set; }
        public bool IsDone { get; protected set; }

        protected virtual IEncountersInfoReader FileReader { get; }
        protected virtual IEncountersInfoReader ServerReader { get; }
        public EncounterStatusesReader()
        {
            FileReader = new FileEncountersInfoReader();
            ServerReader = new ServerCasesInfoReader(new WebAddress());
        }

        public void GetEncounterStatuses(User user)
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

            var encounters = ServerReader.Results;
            if (encounters == null)
                encounters = new List<EncounterInfoGroup>();
            if (FileReader.Results != null) {
                foreach (var localEncounter in FileReader.Results)
                    AddLocalEncounter(encounters, localEncounter);
            }

            Results = encounters;
            IsDone = true;
            Completed?.Invoke(Results);
        }

        protected virtual void AddLocalEncounter(List<EncounterInfoGroup> encounters, EncounterInfoGroup localEncounter)
        {
            if (string.IsNullOrWhiteSpace(localEncounter.RecordNumber))
                encounters.Add(localEncounter);

            foreach (var listEncounter in encounters) {
                if (listEncounter.RecordNumber != localEncounter.RecordNumber)
                    continue;
                listEncounter.LocalInfo = localEncounter.LocalInfo;
                if (localEncounter.Filename != listEncounter.Filename)
                    UpdateFilePath(localEncounter.Filename, listEncounter);

                return;
            }

            encounters.Add(localEncounter);
        }

        protected virtual void UpdateFilePath(string oldPath, EncounterInfoGroup encounterInfo)
        {

        }
    }
}