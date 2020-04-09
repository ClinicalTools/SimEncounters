using System;
using System.Collections.Generic;
using System.IO;

namespace ClinicalTools.SimEncounters.Data
{
    public class FullEncounter
    {
        public EncounterMetadata Metadata { get; }
        public EncounterDetailedStatus Status { get; }
        public EncounterData Data { get; }

        public FullEncounter(EncounterMetadata metadata, EncounterData data, EncounterDetailedStatus status)
        {
            Metadata = metadata;
            Data = data;
            Status = status;
        }
    }
}