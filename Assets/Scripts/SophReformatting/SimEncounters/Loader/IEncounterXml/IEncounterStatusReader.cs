using System;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterStatusReader
    {
        bool IsDone { get; }
        EncounterDetailedStatus DetailedStatus { get; }

        event Action<EncounterDetailedStatus> Completed;

        void DoStuff(User user, EncounterInfo info);
    }

    public class LocalEncounterStatusReader : IEncounterStatusReader
    {
        public bool IsDone { get; protected set; }

        public EncounterDetailedStatus DetailedStatus { get; protected set; }

        public event Action<EncounterDetailedStatus> Completed;

        public void DoStuff(User user, EncounterInfo info)
        {
            DetailedStatus = new EncounterDetailedStatus();
            IsDone = true;
            Completed?.Invoke(DetailedStatus);
        }
    }
}