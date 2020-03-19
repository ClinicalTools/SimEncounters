using System;


namespace ClinicalTools.SimEncounters.MainMenu
{
    public class DetailedStatusReader : IDetailedStatusReader
    {
        public event Action<EncounterDetailedStatus> Completed;
        public bool IsDone { get; protected set; }
        public EncounterDetailedStatus DetailedStatus { get; protected set; }

        protected IDetailedStatusReader ServerDetailedStatusReader { get; }
        protected IDetailedStatusReader LocalDetailedStatusReader { get; }

        public DetailedStatusReader(ServerDetailedStatusReader serverDetailedStatusReader, LocalDetailedStatusReader localDetailedStatusReader)
        {
            ServerDetailedStatusReader = serverDetailedStatusReader;
            LocalDetailedStatusReader = localDetailedStatusReader;
        }


        public void DoStuff(User user, EncounterInfo info)
        {
            ServerDetailedStatusReader.Completed += DoStuff;
            LocalDetailedStatusReader.Completed += DoStuff;
            ServerDetailedStatusReader.DoStuff(user, info);
            LocalDetailedStatusReader.DoStuff(user, info);
        }

        private void DoStuff(EncounterDetailedStatus obj)
        {
            if (!ServerDetailedStatusReader.IsDone || !LocalDetailedStatusReader.IsDone)
                return;

            if (ServerDetailedStatusReader.DetailedStatus == null || LocalDetailedStatusReader.DetailedStatus?.Timestamp >= ServerDetailedStatusReader.DetailedStatus.Timestamp)
                DetailedStatus = LocalDetailedStatusReader.DetailedStatus;
            else
                DetailedStatus = ServerDetailedStatusReader.DetailedStatus;
            IsDone = true;
            Completed?.Invoke(DetailedStatus);
        }
    }
}
