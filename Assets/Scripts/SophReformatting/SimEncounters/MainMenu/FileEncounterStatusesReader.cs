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

        protected IFilePathManager FilePathManager { get; }
        public KeyedEncounterStatusParser EncounterStatusParser { get; }
        public FileEncounterStatusesReader(IFilePathManager filePathManager)
        {
            FilePathManager = filePathManager;
            EncounterStatusParser = new KeyedEncounterStatusParser();
        }

        private readonly string statusSearchTerm = "*.ces";
        public virtual void GetEncounterStatuses(User user)
        {
            Dictionary<int, EncounterBasicStatus> encounters = new Dictionary<int, EncounterBasicStatus>();

            var directory = FilePathManager.GetLocalSavesFolder(user);
            var files = Directory.GetFiles(directory, statusSearchTerm);
            foreach (var file in files) {
                var fileText = File.ReadAllText(file);

                var keyedEncounter = EncounterStatusParser.Parse(fileText);
                if (keyedEncounter.Value != null && !encounters.ContainsKey(keyedEncounter.Key)) {
                    var modifiedTime = File.GetLastWriteTimeUtc(file);
                    DateTimeOffset dateTimeOffset = new DateTimeOffset(modifiedTime);
                    keyedEncounter.Value.Timestamp = dateTimeOffset.ToUnixTimeSeconds();
                    encounters.Add(keyedEncounter.Key, keyedEncounter.Value);
                }
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