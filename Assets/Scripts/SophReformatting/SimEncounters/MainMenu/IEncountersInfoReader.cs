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
}