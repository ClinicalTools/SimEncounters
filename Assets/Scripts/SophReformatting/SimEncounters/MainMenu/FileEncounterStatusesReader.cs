using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class FileEncounterStatusesReader : IEncounterStatusesReader
    {
        public event Action<Dictionary<int, EncounterBasicStatus>> Completed;
        public Dictionary<int, EncounterBasicStatus> Result { get; protected set; }
        public bool IsDone { get; protected set; }

        public EncounterStatusParser EncounterStatusParser { get; }
        public FileEncounterStatusesReader()
        {
            EncounterStatusParser = new EncounterStatusParser();
        }

        protected string GetDirectory(User user)
            => $"{Application.persistentDataPath}/";
        string menuSearchTerm = "*menu.txt";
        public virtual void GetEncounterStatuses(User user)
        {
            Dictionary<int, EncounterBasicStatus> encounters = new Dictionary<int, EncounterBasicStatus>();

            var directory = GetDirectory(user);
            if (!Directory.Exists(directory)) {
                Complete(null);
                return;
            }

            var files = Directory.GetFiles(directory, menuSearchTerm);
            foreach (var file in files) {
                var fileText = File.ReadAllText(file);

                var keyedEncounter = EncounterStatusParser.Parse(fileText);
                if (keyedEncounter.Value != null && !encounters.ContainsKey(keyedEncounter.Key))
                    encounters.Add(keyedEncounter.Key, keyedEncounter.Value);
            }

            Complete(encounters);
        }

        protected virtual void Complete(Dictionary<int, EncounterBasicStatus> result)
        {
            Result = result;
            IsDone = true;
            Completed?.Invoke(Result);
        }
    }
}