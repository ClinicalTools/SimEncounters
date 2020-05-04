using ClinicalTools.SimEncounters.Data;


namespace ClinicalTools.SimEncounters
{
    public class DetailedStatusWriter : IDetailedStatusWriter
    {
        protected IDetailedStatusWriter ServerDetailedStatusWriter { get; }
        protected IDetailedStatusWriter FileDetailedStatusWriter { get; }
        public DetailedStatusWriter(ServerDetailedStatusWriter serverDetailedStatusWriter, FileDetailedStatusWriter fileDetailedStatusWriter)
        {
            ServerDetailedStatusWriter = serverDetailedStatusWriter;
            FileDetailedStatusWriter = fileDetailedStatusWriter;
        }

        public void WriteStatus(UserEncounter encounter)
        {
            ServerDetailedStatusWriter.WriteStatus(encounter);
            FileDetailedStatusWriter.WriteStatus(encounter);
        }
    }
}
