namespace ClinicalTools.SimEncounters.Data
{
    public class Encounter
    {
        public EncounterData Data { get; }
        public EncounterInfo Info { get; }
        public EncounterDetailedStatus Status { get; }
        public EncounterMetadata Metadata { get; }

        public Encounter(EncounterData data, EncounterInfo info, EncounterDetailedStatus status, EncounterMetadata metadata)
        {
            Data = data;
            Info = info;
            Status = status;
            Metadata = metadata;
        }
    }
}