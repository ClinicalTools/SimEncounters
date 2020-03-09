using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.MainMenu
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
        Dictionary<string, UserEncounterStatus> Result { get; }

        event Action<Dictionary<string, UserEncounterStatus>> Completed;

        void GetEncounterStatuses(User user);
    }
}