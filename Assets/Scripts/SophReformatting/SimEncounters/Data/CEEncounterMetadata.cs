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
            set => throw new Exception("Cannot set title directly for CE Encounter.");
        }

        public CEEncounterMetadata() : base() { }
        public CEEncounterMetadata(EncounterMetadata baseEncounterInfo) : base(baseEncounterInfo) { }

        public int GetCaseType()
        {
            return 0;
        }
    }
}
