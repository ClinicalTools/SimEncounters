﻿using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class CasesInfoReader : ICasesInfoReader
    {
        public event Action<List<EncounterInfoGroup>> Completed;
        public List<EncounterInfoGroup> Results { get; protected set; }
        public bool IsDone { get; protected set; }

        protected virtual ICasesInfoReader FileReader { get; }
        protected virtual ICasesInfoReader ServerReader { get; }
        public CasesInfoReader()
        {
            FileReader = new FileCasesInfoReader();
            ServerReader = new ServerCasesInfoReader();
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
            foreach (var localEncounter in FileReader.Results)
                AddLocalEncounter(encounters, localEncounter);

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