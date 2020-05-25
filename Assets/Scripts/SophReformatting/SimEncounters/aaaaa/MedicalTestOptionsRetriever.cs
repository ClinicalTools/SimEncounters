using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class MedicalTestOptionsRetriever : BaseOptionsRetriever
    {
        protected static IEnumerable<string> Options { get; set; }

        public override IEnumerable<string> GetOptions()
        {
            if (Options == null) Options = InitializeOptions();
            return Options;
        }

        protected virtual string Folder { get; } = "Medical Panels";
        protected virtual string[] Filenames { get; } = new string[] {
            "Complete Blood Count.csv",
            "Comprehensive Metabolic Panel.csv",
            "Lipid Panel.csv"
        };

        protected virtual IEnumerable<string> InitializeOptions()
        {
            var options = new List<string>();
            foreach (var filename in Filenames)
                options.AddRange(GetOptions(GetFilepath(filename)));
            options.Sort();
            return options;
        }

        protected virtual string GetFilepath(string filename)
            => Path.Combine(Application.streamingAssetsPath, Folder, filename);

        protected virtual IEnumerable<string> GetOptions(string filePath)
        {
            var options = new List<string>();
            var reader = new StreamReader(filePath);
            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                if (line == null || line.Length == 0)
                    continue;

                var parts = line.Split(',');
                options.Add(parts[0]);
            }

            return options;
        }
    }
}