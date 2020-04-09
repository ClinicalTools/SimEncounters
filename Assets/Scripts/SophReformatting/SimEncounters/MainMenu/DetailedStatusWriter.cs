using ClinicalTools.SimEncounters.Data;


namespace ClinicalTools.SimEncounters.MainMenu
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

        public void DoStuff(User user, FullEncounter encounter)
        {
            ServerDetailedStatusWriter.DoStuff(user, encounter);
            FileDetailedStatusWriter.DoStuff(user, encounter);
        }
    }
}
