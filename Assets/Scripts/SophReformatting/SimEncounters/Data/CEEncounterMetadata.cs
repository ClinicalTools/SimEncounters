using ClinicalTools.SimEncounters.Data;
using System;

namespace ClinicalTools.ClinicalEncounters
{
    public class CEEncounterMetadata : EncounterMetadata
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Url { get; set; }
        public string CompletionCode { get; set; }
        public override string Title {
            get => $"{FirstName} {LastName}";
            set { throw new Exception("Cannot set title directly for CE Encounter."); }
        }


        public CEEncounterMetadata() : base() { }
        public CEEncounterMetadata(EncounterMetadata baseEncounterInfo) : base(baseEncounterInfo) { }
    }
}
