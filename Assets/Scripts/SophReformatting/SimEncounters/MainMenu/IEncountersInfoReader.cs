using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IEncountersInfoReader
    {
        bool IsDone { get; }
        List<EncounterDetail> Result { get; }

        event Action<List<EncounterDetail>> Completed;

        void GetEncounterInfos(User user);
    }
    public interface IEncounterStatusesReader
    {
        bool IsDone { get; }
        Dictionary<int, UserEncounterStatus> Result { get; }

        event Action<Dictionary<int, UserEncounterStatus>> Completed;

        void GetEncounterStatuses(User user);
    }
}