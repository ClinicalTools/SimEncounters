using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class OptionsRetriever : BaseOptionsRetriever
    {
        public string[] Options { get => options; set => options = value; }
        [SerializeField] private string[] options;
        protected virtual string[] Options2 { get; } = new string[] {
            "Complete Blood Count", "Comprehensive Metabolic Panel", "Lipid Panel"
        };
        public override IEnumerable<string> GetOptions() => Options;
    }
}