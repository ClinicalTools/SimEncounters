using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class FileEncountersInfoReader : IEncountersInfoReader
    {
        public event Action<List<EncounterDetail>> Completed;
        public List<EncounterDetail> Result { get; protected set; }
        public bool IsDone { get; protected set; }

        protected IFilePathManager FilePathManager { get; }
        protected IParser<EncounterDetail> EncounterDetailParser { get; }
        public FileEncountersInfoReader(IFilePathManager filePathManager)
        {
            FilePathManager = filePathManager;
            EncounterDetailParser = new EncounterDetailParser(new EncounterLocalInfoSetter());
        }

        private const string menuSearchTerm = "*menu.txt";
        public virtual void GetEncounterInfos(User user)
        {
            List<EncounterDetail> encounters = new List<EncounterDetail>();

            var directory = FilePathManager.GetLocalSavesFolder(user);
            if (!Directory.Exists(directory)) {
                Complete(null);
                return;
            }

            var files = Directory.GetFiles(directory, menuSearchTerm);
            foreach (var file in files) {
                var fileText = File.ReadAllText(file);

                var encounter = EncounterDetailParser.Parse(fileText);
                if (encounter != null)
                    encounters.Add(encounter);
            }

            Complete(encounters);
        }

        protected virtual void Complete(List<EncounterDetail> result)
        {
            Result = result;
            IsDone = true;
            Completed?.Invoke(Result);
        }
    }
}