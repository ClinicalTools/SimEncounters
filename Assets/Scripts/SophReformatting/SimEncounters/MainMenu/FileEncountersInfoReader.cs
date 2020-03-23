using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class FileEncountersInfoReader : IEncountersInfoReader
    {
        public event Action<List<EncounterInfo>> Completed;
        public List<EncounterInfo> Result { get; protected set; }
        public bool IsDone { get; protected set; }

        protected IFilePathManager FilePathManager { get; }
        protected IParser<EncounterInfo> EncounterDetailParser { get; }
        public FileEncountersInfoReader(IFilePathManager filePathManager, IParser<EncounterInfo> encounterInfoParser)
        {
            FilePathManager = filePathManager;
            EncounterDetailParser = encounterInfoParser;
        }

        private const string menuSearchTerm = "*menu.txt";
        public virtual void GetEncounterInfos(User user)
        {
            List<EncounterInfo> encounters = new List<EncounterInfo>();

            var directory = FilePathManager.GetLocalSavesFolder(user);

            Debug.Log($"sophDirectoryA: {directory}");
            if (!Directory.Exists(directory)) {
                Debug.Log($"sophDirectoryNotExists");
                Complete(null);
                return;
            }

            var files = Directory.GetFiles(directory, menuSearchTerm);
            Debug.Log($"sophFileCount: {files.Length}");
            foreach (var file in files) {
                var fileText = File.ReadAllText(file);

                Debug.Log($"sophFileText: {fileText}");
                var encounter = EncounterDetailParser.Parse(fileText);
                if (encounter != null)
                    encounters.Add(encounter);
            }

            Complete(encounters);
        }

        protected virtual void Complete(List<EncounterInfo> result)
        {
            Result = result;
            IsDone = true;
            Completed?.Invoke(Result);
        }
    }
}