using ClinicalTools.SimEncounters.Data;
using System;

namespace ClinicalTools.ClinicalEncounters
{
    public class CEEncounterMetadata : EncounterMetadata
    {
        public Name Name { get; set; } = new Name();
        public string Url { get; set; }
        public string CompletionCode { get; set; }
        public override string Title {
            get => Name.ToString();
            // I'd like to throw an error, but it'd make using the default JSON serialization harder
            set => base.Title = value;
        }

        public CEEncounterMetadata() : base() { }
        public CEEncounterMetadata(EncounterMetadata baseEncounterInfo) : base(baseEncounterInfo) { }
    }
}
