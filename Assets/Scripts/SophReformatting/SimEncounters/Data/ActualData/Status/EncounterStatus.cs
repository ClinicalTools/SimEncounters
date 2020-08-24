using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class EncounterStatus
    {
        public EncounterBasicStatus BasicStatus { get; }
        public EncounterContentStatus ContentStatus { get; }
        public long Timestamp { get; set; }

        public EncounterStatus(EncounterBasicStatus basicStatus, EncounterContentStatus contentStatus) {
            BasicStatus = basicStatus;
            ContentStatus = contentStatus;
        }
    }
}