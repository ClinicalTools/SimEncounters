using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class FileEncountersInfoReader : IEncountersInfoReader
    {
        public event Action<List<EncounterInfoGroup>> Completed;
        public List<EncounterInfoGroup> Results { get; protected set; }
        public bool IsDone { get; protected set; }

        public EncounterInfoParser EncounterInfoParser { get; }
        public FileEncountersInfoReader()
        {
            EncounterInfoParser = new EncounterInfoParser();
        }


        string directory = @"C:\Users\tanner\AppData\LocalLow\Clinical Tools Inc\Clinical Encounters_ Learner\LocalSaves\594728bc39";
        string menuSearchTerm = "*menu.txt";
        public virtual void GetEncounterInfos(User user)
        {
            List<EncounterInfoGroup> encounters = new List<EncounterInfoGroup>();

            if (!Directory.Exists(directory))
                Complete(null);

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
}