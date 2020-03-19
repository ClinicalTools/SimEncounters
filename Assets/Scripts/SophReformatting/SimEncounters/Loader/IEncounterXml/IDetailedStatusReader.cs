using System;

namespace ClinicalTools.SimEncounters
{
    public interface IDetailedStatusReader
    {
        bool IsDone { get; }
        EncounterDetailedStatus DetailedStatus { get; }

        event Action<EncounterDetailedStatus> Completed;

        void DoStuff(User user, EncounterInfo info);
    }
}