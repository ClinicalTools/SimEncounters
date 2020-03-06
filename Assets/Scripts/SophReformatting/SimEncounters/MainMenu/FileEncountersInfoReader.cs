using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class FileEncountersInfoReader : IEncountersInfoReader
    {
        public event Action<List<EncounterInfoGroup>> Completed;
        public List<EncounterInfoGroup> Results { get; protected set; }
        public bool IsDone { get; protected set; }

        protected IFilePathManager FilePathManager { get; }
        protected EncounterInfoParser EncounterInfoParser { get; }
        public FileEncountersInfoReader(IFilePathManager filePathManager)
        {
            FilePathManager = filePathManager;
            EncounterInfoParser = new EncounterInfoParser();
        }

        private const string menuSearchTerm = "*menu.txt";
        public virtual void GetEncounterInfos(User user)
        {
            List<EncounterInfoGroup> encounters = new List<EncounterInfoGroup>();

            var directory = FilePathManager.GetLocalSavesFolder(user);
            if (!Directory.Exists(directory)) {
                Complete(null);
                return;
            }

            var files = Directory.GetFiles(directory, menuSearchTerm);
            foreach (var file in files) {
                var fileText = File.ReadAllText(file);

                var encounter = EncounterInfoParser.GetLocalEncounter(fileText);
                if (encounter != null)
                    encounters.Add(encounter);
            }

            Complete(encounters);
        }

        protected virtual void Complete(List<EncounterInfoGroup> results)
        {
            Results = results;
            IsDone = true;
            Completed?.Invoke(Results);
        }
    }

    public class FileEncounterStatusesReader : IEncounterStatusesReader
    {
        public event Action<List<UserEncounterStatus>> Completed;
        public List<UserEncounterStatus> Results { get; protected set; }
        public bool IsDone { get; protected set; }

        public EncounterStatusParser EncounterStatusParser { get; }
        public FileEncounterStatusesReader()
        {
            EncounterStatusParser = new EncounterStatusParser();
        }

        protected string GetDirectory(User user)
            => $"{Application.persistentDataPath}/";
        string directory = @"C:\Users\Nehipasta\AppData\LocalLow\Clinical Tools Inc\Clinical Encounters_ Learner\";
        string menuSearchTerm = "*menu.txt";
        public virtual void GetEncounterStatuses(User user)
        {
            List<UserEncounterStatus> encounters = new List<UserEncounterStatus>();

            var directory = GetDirectory(user);
            if (!Directory.Exists(directory)) {
                Complete(null);
                return;
            }

            var files = Directory.GetFiles(directory, menuSearchTerm);
            foreach (var file in files) {
                var fileText = File.ReadAllText(file);

                var encounter = EncounterStatusParser.GetEncounterStatus(fileText);
                if (encounter != null)
                    encounters.Add(encounter);
            }

            Complete(encounters);
        }

        protected virtual void Complete(List<UserEncounterStatus> results)
        {
            Results = results;
            IsDone = true;
            Completed?.Invoke(Results);
        }
    }
}