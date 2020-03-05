using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public interface IEncountersInfoReader
    {
        bool IsDone { get; }
        List<EncounterInfoGroup> Results { get; }

        event Action<List<EncounterInfoGroup>> Completed;

        void GetEncounterInfos(User user);
    }
    public interface IEncounterStatusesReader
    {
        bool IsDone { get; }
        List<UserEncounterStatus> Results { get; }

        event Action<List<UserEncounterStatus>> Completed;

        void GetEncounterStatuses(User user);
    }
}