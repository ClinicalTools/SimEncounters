using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IEncountersInfoReader
    {
        bool IsDone { get; }
        List<EncounterInfo> Result { get; }

        event Action<List<EncounterInfo>> Completed;

        void GetEncounterInfos(User user);
    }
    public interface IEncounterStatusesReader
    {
        bool IsDone { get; }
        Dictionary<int, EncounterBasicStatus> Result { get; }

        event Action<Dictionary<int, EncounterBasicStatus>> Completed;

        void GetEncounterStatuses(User user);
    }
}